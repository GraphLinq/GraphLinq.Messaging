using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Net;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace NodeBlock.Plugin.Messaging.Nodes.Discord
{
    [NodeDefinition("OnDiscordCommandNode", "On Discord Command", NodeTypeEnum.Event, "Discord")]
    [NodeGraphDescription("Trigger a event when receive a command in channel on Discord")]
    public class OnDiscordChannelCommandNode : Node
    {


        public OnDiscordChannelCommandNode(string id, BlockGraph graph)
               : base(id, graph, typeof(OnDiscordChannelCommandNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("discord", new NodeParameter(this, "discord", typeof(DiscordConnector), true));
            this.InParameters.Add("channelId", new NodeParameter(this, "channelId", typeof(ulong), true));
            this.InParameters.Add("prefix", new NodeParameter(this, "prefix", typeof(char), true));
            this.InParameters.Add("command", new NodeParameter(this, "command", typeof(string), true));

            this.OutParameters.Add("guildId", new NodeParameter(this, "guildId", typeof(ulong), true));
            this.OutParameters.Add("channel", new NodeParameter(this, "channel", typeof(ulong), true));
            this.OutParameters.Add("params", new NodeParameter(this, "params", typeof(List<object>), true));

        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            var discordClient = (this.InParameters["discord"].GetValue() as DiscordConnector).DiscordClient;
            discordClient.MessageReceived += DiscordClient_MessageReceived;
        }

        private System.Threading.Tasks.Task DiscordClient_MessageReceived(global::Discord.WebSocket.SocketMessage arg)
        {

            var message = arg as SocketUserMessage;
            if (message == null) return Task.CompletedTask;

           
            var discordClient = (this.InParameters["discord"].GetValue() as DiscordConnector).DiscordClient;
           
            var id = ulong.Parse(this.InParameters["channelId"].GetValue().ToString());
            if (arg.Channel is IMessageChannel && arg.Channel.Id == id && arg.Author.Id != discordClient.CurrentUser.Id)
            {

                int argPos = 0;
                if (!(message.HasCharPrefix(char.Parse(this.InParameters["prefix"].GetValue().ToString()), ref argPos) ||
                  message.HasMentionPrefix(discordClient.CurrentUser, ref argPos)) ||
                  message.Author.IsBot)
                            return Task.CompletedTask;

                var dispatch = message.Content.Substring(argPos).Split(' ');
                if(dispatch.Length > 0  && dispatch[0].Equals(this.InParameters["command"].GetValue().ToString()))
                {
                    var @params = new List<object>();
                    foreach(var param in dispatch.Skip(1).ToArray())
                    {
                        @params.Add(param);
                    }

                    var parameters = this.InstanciateParametersForCycle();
                    var guild = arg.Channel as SocketGuildChannel;
                    parameters["guildId"].SetValue(guild.Guild.Id);
                    parameters["channel"].SetValue(arg.Channel.Id);
                    parameters["params"].SetValue(@params);

                    this.Graph.AddCycle(this, parameters);
                }
        
            }

            return Task.CompletedTask;
        }

        public override void OnStop()
        {
            var discordClient = (this.InParameters["discord"].GetValue() as DiscordConnector).DiscordClient;
            discordClient.MessageReceived -= DiscordClient_MessageReceived;
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
