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

            this.InParameters.Add("telegramBot", new NodeParameter(this, "telegramBot", typeof(object), true));

            this.OutParameters.Add("message", new NodeParameter(this, "message", typeof(global::Telegram.Bot.Types.Message), false));
            this.OutParameters.Add("chatId", new NodeParameter(this, "chatId", typeof(long), false));
            this.OutParameters.Add("fromId", new NodeParameter(this, "fromId", typeof(int), false));
            this.OutParameters.Add("from", new NodeParameter(this, "from", typeof(string), false));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            TelegramBotInstanceNode telegramBotInstance = this.InParameters["telegramBot"].GetValue() as TelegramBotInstanceNode;
            this.setupDefaultBehavior();
        }

        public override void OnStop()
        {
            TelegramBotInstanceNode telegramBotInstance = this.InParameters["telegramBot"].GetValue() as TelegramBotInstanceNode;
            telegramBotInstance.Bot.OnMessage -= Bot_OnMessage;
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
            else
            {
                var instanciatedParameters = this.InstanciateParametersForCycle();
                instanciatedParameters["message"].SetValue(e.Message.Text);
                instanciatedParameters["chatId"].SetValue(e.Message.Chat.Id);
                instanciatedParameters["fromId"].SetValue(e.Message.From.Id);
                instanciatedParameters["from"].SetValue(e.Message.From.Username);
                this.Graph.AddCycle(this, instanciatedParameters);
            }
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
