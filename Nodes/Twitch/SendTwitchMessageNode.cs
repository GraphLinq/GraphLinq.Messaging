using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Messaging.Nodes.Twitch;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.Twitch
{
    [NodeDefinition("SendTwitchMessageNode", "Send Twitch Message", NodeTypeEnum.Function, "Twitch")]
    [NodeGraphDescription("Send a message on Twitch on a channel")]
    [NodeIDEParameters(Hidden = true)]
    public class SendTwitchMessageNode : Node
    {
        public SendTwitchMessageNode(string id, BlockGraph graph)
            : base(id, graph, typeof(SendTwitchMessageNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "twitch", new NodeParameter(this, "twitch", typeof(object), true) },
                { "channel", new NodeParameter(this, "channel", typeof(string), true) },
                { "message", new NodeParameter(this, "message", typeof(string), true) }
            };

            this.OutParameters = new Dictionary<string, NodeParameter>()
            {

            };
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            TwitchConnector twitchInstance = this.InParameters["twitch"].GetValue() as TwitchConnector;
            var messages = this.SplitString(this.InParameters["message"].GetValue().ToString());
            foreach(var m in messages)
            {
                twitchInstance.Client.SendMessage(this.InParameters["channel"].GetValue().ToString(), m);
            }
            
            return true;
        }

        public List<string> SplitString(string inputString)
        {
            const int maxLength = 500;
            var outputList = new List<string>();

            for (int i = 0; i < inputString.Length; i += maxLength)
            {
                if (i + maxLength <= inputString.Length)
                    outputList.Add(inputString.Substring(i, maxLength));
                else
                    outputList.Add(inputString.Substring(i));
            }

            return outputList;
        }
    }
}
