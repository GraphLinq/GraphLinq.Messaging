using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.Telegram
{
    [NodeDefinition("OnMessageTelegramBotNode", "On Telegram Message", NodeTypeEnum.Event, "Telegram")]
    [NodeGraphDescription("This event allow you to handle a message from Telegram in your graph")]
    public class OnMessageTelegramBotNode : Node
    {
        public OnMessageTelegramBotNode(string id, BlockGraph graph)
        : base(id, graph, typeof(OnMessageTelegramBotNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("telegramBot", new NodeParameter(this, "telegramBot", typeof(string), true));

            this.OutParameters.Add("message", new NodeParameter(this, "message", typeof(global::Telegram.Bot.Types.Message), false));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            TelegramBotInstanceNode telegramBotInstance = this.InParameters["telegramBot"].GetValue() as TelegramBotInstanceNode;
            this.setupDefaultBehavior();
        }

        private void setupDefaultBehavior()
        {
            TelegramBotInstanceNode telegramBotInstance = this.InParameters["telegramBot"].GetValue() as TelegramBotInstanceNode;
            telegramBotInstance.Bot.OnMessage += Bot_OnMessage;
        }

        private void Bot_OnMessage(object sender, global::Telegram.Bot.Args.MessageEventArgs e)
        {
            TelegramBotInstanceNode telegramBotInstance = this.InParameters["telegramBot"].GetValue() as TelegramBotInstanceNode;
            if (e.Message.Text == "id")
            {
                telegramBotInstance.Bot.SendTextMessageAsync(e.Message.Chat.Id, "The chat ID is : " + e.Message.Chat.Id).Wait();
            }
        }
    }
}
