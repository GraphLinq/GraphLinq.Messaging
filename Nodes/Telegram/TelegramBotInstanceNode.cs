using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace NodeBlock.Plugin.Messaging.Nodes.Telegram
{
    [NodeDefinition("TelegramBotInstanceNode", "Telegram Bot", NodeTypeEnum.Connector, "Telegram")]
    [NodeGraphDescription("Telegram connector, to retrieve your AccessToken talk to BotFather to create a new bot")]
    [NodeSpecialActionAttribute("Talk to @BotFather", "open_url", "https://t.me/BotFather")]
    public class TelegramBotInstanceNode : Node
    {
        public TelegramBotInstanceNode(string id, BlockGraph graph)
           : base(id, graph, typeof(TelegramBotInstanceNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("accessToken", new NodeParameter(this, "accessToken", typeof(string), true));

            this.OutParameters.Add("telegramBot", new NodeParameter(this, "telegramBot", typeof(TelegramBotInstanceNode), true));
        }

        [JsonIgnore]
        public TelegramBotClient Bot { get; set; }
        
        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.Bot = new TelegramBotClient(this.InParameters["accessToken"].GetValue().ToString());
            this.Bot.StartReceiving();
            this.Next();
        }

        public override void OnStop()
        {
            this.Bot.StopReceiving();
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "telegramBot")
            {
                return this;
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
