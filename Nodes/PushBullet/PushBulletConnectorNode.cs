using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using PushbulletSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.PushBullet
{
    [NodeDefinition("PushBulletConnectorNode", "PushBullet Connector", NodeTypeEnum.Connector, "PushBullet")]
    [NodeGraphDescription("PushBullet is a service that allow you to send push notification to your smartphone")]
    public class PushBulletConnectorNode : Node
    {
        public PushBulletConnectorNode(string id, BlockGraph graph)
           : base(id, graph, typeof(PushBulletConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));

            this.OutParameters.Add("pushBullet", new NodeParameter(this, "pushBullet", typeof(PushBulletConnectorNode), true));
        }

        public PushbulletClient Client { get; set; }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            Client = new PushbulletClient(this.InParameters["apiKey"].GetValue().ToString());
            this.Next();
        }

        public override void OnStop()
        {

        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "pushBullet")
            {
                return this;
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
