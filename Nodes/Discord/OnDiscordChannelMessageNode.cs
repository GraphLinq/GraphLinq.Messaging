using Discord;
using Discord.WebSocket;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Messaging.Nodes.Discord
{
    [NodeDefinition("OnDiscordChannelMessage", "On Discord Channel Message", NodeTypeEnum.Event, "Discord")]
    [NodeGraphDescription("Trigger a event when receive a message in channel on Discord")]
    public class OnDiscordChannelMessageNode : Node
    {
        public OnDiscordChannelMessageNode(string id, BlockGraph graph)
               : base(id, graph, typeof(OnDiscordChannelMessageNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("discord", new NodeParameter(this, "discord", typeof(DiscordConnector), true));
            this.InParameters.Add("channelId", new NodeParameter(this, "channelId", typeof(ulong), true));

            this.OutParameters.Add("guildId", new NodeParameter(this, "guildId", typeof(ulong), true));
            this.OutParameters.Add("channel", new NodeParameter(this, "channel", typeof(ulong), true));
            this.OutParameters.Add("message", new NodeParameter(this, "message", typeof(string), true));

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
            var discordClient = (this.InParameters["discord"].GetValue() as DiscordConnector).DiscordClient;

            var id = ulong.Parse(this.InParameters["channelId"].GetValue().ToString());
            if (arg.Channel is IMessageChannel && arg.Channel.Id ==  id && arg.Author.Id != discordClient.CurrentUser.Id)
            {
                var parameters = this.InstanciatedParametersForCycle();
                var guild = arg.Channel as SocketGuildChannel;
                parameters["guildId"].SetValue(guild.Guild.Id);
                parameters["channel"].SetValue(arg.Channel.Id);
                parameters["message"].SetValue(arg.Content);
                this.Graph.AddCycle(this, parameters);
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
