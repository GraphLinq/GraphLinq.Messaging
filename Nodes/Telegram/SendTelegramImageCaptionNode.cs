using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.Telegram
{
    [NodeDefinition("SendTelegramImageCaptionNode", "Send Telegram Photo With Caption", NodeTypeEnum.Function, "Telegram")]
    [NodeGraphDescription("Send a photo on Telegram with a caption")]
    public class SendTelegramImageCaptionNode : Node
    {
        public SendTelegramImageCaptionNode(string id, BlockGraph graph)
            : base(id, graph, typeof(SendTelegramImageCaptionNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "telegramBot", new NodeParameter(this, "telegramBot", typeof(TelegramBotInstanceNode), true) },
                { "chatId", new NodeParameter(this, "chatId", typeof(string), true) },
                { "photo", new NodeParameter(this, "photo", typeof(string), true) },
                { "caption", new NodeParameter(this, "caption", typeof (string), true) }
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {

            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            TelegramBotInstanceNode telegramBotInstance = this.InParameters["telegramBot"].GetValue() as TelegramBotInstanceNode;
            telegramBotInstance.Bot.SendPhotoAsync(long.Parse(this.InParameters["chatId"].GetValue().ToString()), this.InParameters["photo"].GetValue().ToString(), this.InParameters["caption"].GetValue().ToString());

            return true;
        }
    }
}
