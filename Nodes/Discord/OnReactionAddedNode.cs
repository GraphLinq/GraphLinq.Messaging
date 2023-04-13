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
            this.InParameters.Add("messageId", new NodeParameter(this, "messageId", typeof(ulong), true));
            this.InParameters.Add("emoteName", new NodeParameter(this, "emoteName", typeof(string), true));
            this.OutParameters.Add("userId", new NodeParameter(this, "userId", typeof(ulong), true));



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
            if(arg1.Id == ulong.Parse(this.InParameters["messageId"].GetValue().ToString()))
            {
                var emote = new Emoji(this.InParameters["emoteName"].GetValue().ToString());
                if(emote.Name == arg3.Emote.Name)
                {
                    var parameters = this.InstanciatedParametersForCycle();
                    parameters["userId"].SetValue(arg3.UserId);
                    this.Graph.AddCycle(this, parameters);
                }        
            }
            
            return Task.CompletedTask;
        }


        public static string DecodeFromUtf8(string utf8String)
        {
            // copy the string as UTF-8 bytes.
            byte[] utf8Bytes = new byte[utf8String.Length];
            for (int i = 0; i < utf8String.Length; ++i)
            {
                //Debug.Assert( 0 <= utf8String[i] && utf8String[i] <= 255, "the char must be in byte's range");
                utf8Bytes[i] = (byte)utf8String[i];
            }

            return Encoding.UTF8.GetString(utf8Bytes, 0, utf8Bytes.Length);
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
