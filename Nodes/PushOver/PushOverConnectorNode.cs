using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using PushoverClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.PushOver
{
    [NodeDefinition("PushOverConnectorNode", "PushOver Connector", NodeTypeEnum.Connector, "PushOver")]
    [NodeGraphDescription("PushOver is a service that allow you to send push notification to your smartphone")]
    public class PushOverConnectorNode : Node
    {
        public PushOverConnectorNode(string id, BlockGraph graph)
           : base(id, graph, typeof(PushOverConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));

            this.OutParameters.Add("pushOver", new NodeParameter(this, "pushOver", typeof(PushOverConnectorNode), true));
        }

        public PushoverClient.Pushover Client { get; set; }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            Client = new Pushover(this.InParameters["apiKey"].GetValue().ToString());
            this.Next();
        }

        public override void OnStop()
        {

        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "pushOver")
            {
                return this;
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
