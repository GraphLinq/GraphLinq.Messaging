using Discord;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Messaging.Nodes.Discord
{
    [NodeDefinition("OnReactionRemovedNode", "On Reaction Removed Message", NodeTypeEnum.Event, "Discord")]
    [NodeGraphDescription("Trigger a event when remove a reaction in message on Discord")]
    public class OnReactionRemovedNode : Node
    {
        public OnReactionRemovedNode(string id, BlockGraph graph)
               : base(id, graph, typeof(OnReactionRemovedNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("discord", new NodeParameter(this, "discord", typeof(DiscordConnector), true));
            this.InParameters.Add("channelId", new NodeParameter(this, "channelId", typeof(ulong), true));
            this.InParameters.Add("messageId", new NodeParameter(this, "messageId", typeof(ulong), true));
            this.InParameters.Add("emoteName", new NodeParameter(this, "emoteName", typeof(string), true));
            this.OutParameters.Add("userId", new NodeParameter(this, "userId", typeof(ulong), true));

        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            var discordClient = (this.InParameters["discord"].GetValue() as DiscordConnector).DiscordClient;

            discordClient.ReactionRemoved += DiscordClient_ReactionRemoved;
        }

        private Task DiscordClient_ReactionRemoved(Cacheable<IUserMessage, ulong> arg1, global::Discord.WebSocket.ISocketMessageChannel arg2, global::Discord.WebSocket.SocketReaction arg3)
        {
            if (arg1.Id == ulong.Parse(this.InParameters["messageId"].GetValue().ToString()))
            {
                var emote = new Emoji(this.InParameters["emoteName"].GetValue().ToString());
                if (emote.Name == arg3.Emote.Name)
                {
                    var parameters = this.InstanciatedParametersForCycle();
                    parameters["userId"].SetValue(arg3.UserId);
                    this.Graph.AddCycle(this, parameters);
                }
            }

            return Task.CompletedTask;
        }



        public override void OnStop()
        {
            var discordClient = (this.InParameters["discord"].GetValue() as DiscordConnector).DiscordClient;
            discordClient.ReactionRemoved -= DiscordClient_ReactionRemoved;
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
