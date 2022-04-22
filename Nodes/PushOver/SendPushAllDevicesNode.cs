using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using PushoverClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.PushOver
{
    [NodeDefinition("SendPushOverAllDevicesNode", "Send Push All Devices", NodeTypeEnum.Function, "PushOver")]
    [NodeGraphDescription("Send a push via PushOver on all your devices")]
    public class SendPushOverAllDevicesNode : Node
    {
        public SendPushOverAllDevicesNode(string id, BlockGraph graph)
            : base(id, graph, typeof(SendPushOverAllDevicesNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "pushOver", new NodeParameter(this, "pushOver", typeof(PushOverConnectorNode), true) },
                { "title", new NodeParameter(this, "title", typeof(string), true) },
                { "message", new NodeParameter(this, "message", typeof(string), true) }
            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            PushOverConnectorNode pushOverConnector = this.InParameters["pushOver"].GetValue() as PushOverConnectorNode;
            var title = this.InParameters["title"].GetValue().ToString();
            var message = this.InParameters["message"].GetValue().ToString();

            PushResponse response = pushOverConnector.Client.Push(title, message);

            return true;
        }
    }
}
