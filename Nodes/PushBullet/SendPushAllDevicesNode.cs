using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using PushbulletSharp.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.PushBullet
{
    [NodeDefinition("SendPushAllDevicesNode", "Send Push All Devices", NodeTypeEnum.Function, "PushBullet")]
    [NodeGraphDescription("Send a push via PushBullet on all your devices")]
    public class SendPushAllDevicesNode : Node
    {
        public SendPushAllDevicesNode(string id, BlockGraph graph)
            : base(id, graph, typeof(SendPushAllDevicesNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "pushBullet", new NodeParameter(this, "pushBullet", typeof(PushBulletConnectorNode), true) },
                { "title", new NodeParameter(this, "title", typeof(string), true) },
                { "message", new NodeParameter(this, "message", typeof(string), true) }
            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            PushBulletConnectorNode pushBulletConnector = this.InParameters["pushBullet"].GetValue() as PushBulletConnectorNode;
            var title = this.InParameters["title"].GetValue().ToString();
            var message = this.InParameters["message"].GetValue().ToString();
            var devices = pushBulletConnector.Client.CurrentUsersDevices();
            foreach(var device in devices.Devices)
            {
                var request = new PushNoteRequest
                {
                    DeviceIden = device.Iden,
                    Title = title,
                    Body = message
                };
                pushBulletConnector.Client.PushNote(request);
            }
            return true;
        }
    }
}
