using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.Twitch
{
    [NodeDefinition("JoinTwitchChannelNode", "Join Twitch channel", NodeTypeEnum.Function, "Twitch")]
    [NodeGraphDescription("Join a twitch channel as a bot")]
    [NodeIDEParameters(Hidden = true)]
    public class JoinTwitchChannelNode : Node
    {
        public JoinTwitchChannelNode(string id, BlockGraph graph)
            : base(id, graph, typeof(JoinTwitchChannelNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "twitch", new NodeParameter(this, "twitch", typeof(object), true) },
                { "channel", new NodeParameter(this, "channel", typeof(string), true) }
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
            twitchInstance.Client.JoinChannel(this.InParameters["channel"].GetValue().ToString());
            return true;
        }
    }
}
