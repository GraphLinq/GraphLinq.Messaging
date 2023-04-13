using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace NodeBlock.Plugin.Messaging.Nodes.Twitch
{
    [NodeDefinition("TwitchConnector", "Twitch Connector", NodeTypeEnum.Connector, "Twitch")]
    [NodeGraphDescription("Twitch connector, it allow you to create twitch bot easily")]
    [NodeSpecialActionAttribute("Open Twitch Developer", "open_url", "https://dev.twitch.tv/")]
    [NodeIDEParameters(Hidden = true)]
    public class TwitchConnector : Node
    {
        public TwitchConnector(string id, BlockGraph graph)
            : base(id, graph, typeof(TwitchConnector).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("twitchUsername", new NodeParameter(this, "twitchUsername", typeof(string), true));
            this.InParameters.Add("accessToken", new NodeParameter(this, "accessToken", typeof(string), true));

            this.OutParameters.Add("twitch", new NodeParameter(this, "twitch", typeof(TwitchConnector), true));
        }

        [JsonIgnore]
        public TwitchClient Client;

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            ConnectionCredentials credentials = new ConnectionCredentials(this.InParameters["twitchUsername"].GetValue().ToString(),
                this.InParameters["accessToken"].GetValue().ToString());
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            this.Client = new TwitchClient(customClient);

            //this.Client.OnConnected += Client_OnConnected;
            //this.Client.OnJoinedChannel += Client_OnJoinedChannel;
            this.Client.Initialize(credentials, this.InParameters["twitchUsername"].GetValue().ToString());

            var result = this.Client.Connect();

            this.Next();
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "twitch")
            {
                return this;
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
