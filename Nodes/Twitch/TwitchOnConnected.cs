using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.Twitch
{
    [NodeDefinition("TwitchOnConnected", "On Twitch Connected", NodeTypeEnum.Event, "Twitch")]
    [NodeGraphDescription("This event allow you to do things after a twitch connection")]
    public class TwitchOnConnected : Node
    {
        public TwitchOnConnected(string id, BlockGraph graph)
       : base(id, graph, typeof(TwitchOnConnected).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("twitch", new NodeParameter(this, "twitch", typeof(object), true));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            TwitchConnector twitchInstance = this.InParameters["twitch"].GetValue() as TwitchConnector;
            this.setupDefaultBehavior();
        }

        public override void OnStop()
        {
            TwitchConnector twitchInstance = this.InParameters["twitch"].GetValue() as TwitchConnector;
            twitchInstance.Client.OnConnected -= Client_OnConnected;
        }

        private void setupDefaultBehavior()
        {
            TwitchConnector twitchInstance = this.InParameters["twitch"].GetValue() as TwitchConnector;
            twitchInstance.Client.OnConnected += Client_OnConnected;
        }

        private void Client_OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
        {
            var instanciatedParameters = this.InstanciateParametersForCycle();
            this.Graph.AddCycle(this, instanciatedParameters);
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
