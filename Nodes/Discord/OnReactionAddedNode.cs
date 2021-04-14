using Discord;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Messaging.Nodes.Discord
{
    [NodeDefinition("OnReactionAddedNode", "On Reaction Added Message", NodeTypeEnum.Event, "Discord")]
    [NodeGraphDescription("Trigger a event when added a reaction in message on Discord")]
    public class OnReactionAddedNode : Node
    {
        public OnReactionAddedNode(string id, BlockGraph graph)
               : base(id, graph, typeof(OnReactionAddedNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("discord", new NodeParameter(this, "discord", typeof(DiscordConnector), true));
            this.InParameters.Add("channelId", new NodeParameter(this, "channelId", typeof(ulong), true));
            this.OutParameters.Add("guildId", new NodeParameter(this, "guildId", typeof(ulong), true));
            this.OutParameters.Add("messageId", new NodeParameter(this, "message", typeof(ulong), true));

            this.OutParameters.Add("userId", new NodeParameter(this, "guildId", typeof(ulong), true));

        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            var discordClient = (this.InParameters["discord"].GetValue() as DiscordConnector).DiscordClient;

            discordClient.ReactionAdded += DiscordClient_ReactionAdded;
        }

        private Task DiscordClient_ReactionAdded(Cacheable<IUserMessage, ulong> arg1, global::Discord.WebSocket.ISocketMessageChannel arg2, global::Discord.WebSocket.SocketReaction arg3)
        {
            var parameters = this.InstanciateParametersForCycle();
            parameters["userId"].SetValue(arg1.Id);
            this.Graph.AddCycle(this, parameters);
            return Task.CompletedTask;
        }



        public override void OnStop()
        {
            var discordClient = (this.InParameters["discord"].GetValue() as DiscordConnector).DiscordClient;
            discordClient.ReactionAdded -= DiscordClient_ReactionAdded;
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
