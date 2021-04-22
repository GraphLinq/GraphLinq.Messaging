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
    [NodeDefinition("OnDiscordPrivateMessageNode", "On Discord Private Message", NodeTypeEnum.Event, "Discord")]
    [NodeGraphDescription("Trigger a event when you receive a private message on Discord")]
    public class OnDiscordPrivateMessageNode : Node
    {
        public OnDiscordPrivateMessageNode(string id, BlockGraph graph)
               : base(id, graph, typeof(OnDiscordPrivateMessageNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("discord", new NodeParameter(this, "discord", typeof(DiscordConnector), true));

            this.OutParameters.Add("from", new NodeParameter(this, "from", typeof(string), false));
            this.OutParameters.Add("message", new NodeParameter(this, "message", typeof(string), false));
            this.OutParameters.Add("messageId", new NodeParameter(this, "messageId", typeof(ulong), false));
            this.OutParameters.Add("author", new NodeParameter(this, "author", typeof(SocketUser), false));

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
            if(arg.Channel is IDMChannel)
            {
                var parameters = this.InstanciateParametersForCycle();
                parameters["message"].SetValue(arg.Content);
                parameters["from"].SetValue(arg.Author.Username);
                parameters["messageId"].SetValue(arg.Id);
                parameters["author"].SetValue(arg.Author);
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
