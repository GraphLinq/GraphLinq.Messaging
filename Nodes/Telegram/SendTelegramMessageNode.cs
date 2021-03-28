using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.Telegram
{
    [NodeDefinition("SendTelegramMessageNode", "Send Telegram Message", NodeTypeEnum.Function, "Telegram")]
    [NodeGraphDescription("Send a message on Telegram")]
    public class SendTelegramMessageNode : Node
    {
        public SendTelegramMessageNode(string id, BlockGraph graph)
            : base(id, graph, typeof(SendTelegramMessageNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "telegramBot", new NodeParameter(this, "telegramBot", typeof(TelegramBotInstanceNode), true) },
                { "chatId", new NodeParameter(this, "chatId", typeof(string), true) },
                { "message", new NodeParameter(this, "message", typeof(string), true) }
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
            telegramBotInstance.Bot.SendTextMessageAsync(long.Parse(this.InParameters["chatId"].GetValue().ToString()), this.InParameters["message"].GetValue().ToString());
            return true;
        }
    }
}
