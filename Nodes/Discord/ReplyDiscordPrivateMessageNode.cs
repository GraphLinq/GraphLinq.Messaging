using Discord.WebSocket;
using Discord;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.Discord
{
    [NodeDefinition("ReplyDiscordPrivateMessageNode", "Reply Private Discord Message", NodeTypeEnum.Function, "Discord")]
    [NodeGraphDescription("Send a reply to a private message")]
    public class ReplyDiscordPrivateMessageNode : Node
    {
        public ReplyDiscordPrivateMessageNode(string id, BlockGraph graph)
         : base(id, graph, typeof(ReplyDiscordPrivateMessageNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("author", new NodeParameter(this, "author", typeof(SocketUser), true));
            this.InParameters.Add("message", new NodeParameter(this, "message", typeof(string), true));

            this.OutParameters.Add("messageId", new NodeParameter(this, "messageId", typeof(ulong), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var author = (this.InParameters["author"].GetValue() as SocketUser);
            var messageTask = author.SendMessageAsync(this.InParameters["message"].GetValue().ToString());
            messageTask.Wait();
            this.OutParameters["messageId"].SetValue(messageTask.Result.Id);
            return true;
        }
    }
}
