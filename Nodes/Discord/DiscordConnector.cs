using Discord.WebSocket;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Messaging.Nodes.Discord
{
    [NodeDefinition("DiscordConnector", "Discord Connector", NodeTypeEnum.Connector, "Discord")]
    [NodeGraphDescription("Discord Connector, allow you to receive and send messages on discord guilds")]
    public class DiscordConnector : Node
    {
        public DiscordConnector(string id, BlockGraph graph)
              : base(id, graph, typeof(DiscordConnector).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("token", new NodeParameter(this, "token", typeof(string), true));

            this.OutParameters.Add("discord", new NodeParameter(this, "discord", typeof(DiscordConnector), false));
        }

        [JsonIgnore]
        public DiscordSocketClient DiscordClient { get; set; }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.DiscordClient = new DiscordSocketClient();
            this.DiscordClient.LoginAsync(global::Discord.TokenType.Bot, this.InParameters["token"].GetValue().ToString()).Wait();
            this.DiscordClient.StartAsync().Wait();
            Task.Delay(2500).Wait();

            this.Next();
        }

        public override void OnStop()
        {
            if(this.DiscordClient != null)
                this.DiscordClient.Dispose();
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "discord")
            {
                return this;
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
