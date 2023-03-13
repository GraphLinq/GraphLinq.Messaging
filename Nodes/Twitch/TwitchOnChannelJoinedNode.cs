using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.Twitch
{
    [NodeDefinition("TwitchOnChannelJoinedNode", "On Twitch Channel Joined", NodeTypeEnum.Event, "Twitch")]
    [NodeGraphDescription("This event trigger when your bot join a twitch channel")]
    public class TwitchOnChannelJoinedNode : Node
    {
        public TwitchOnChannelJoinedNode(string id, BlockGraph graph)
      : base(id, graph, typeof(TwitchOnChannelJoinedNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("twitch", new NodeParameter(this, "twitch", typeof(object), true));

            this.OutParameters.Add("channel", new NodeParameter(this, "channel", typeof(string), false));
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
            twitchInstance.Client.OnJoinedChannel -= Client_OnJoinedChannel;
        }

        private void setupDefaultBehavior()
        {
            TwitchConnector twitchInstance = this.InParameters["twitch"].GetValue() as TwitchConnector;
            twitchInstance.Client.OnJoinedChannel += Client_OnJoinedChannel;
        }

        private void Client_OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
        {
            var instanciatedParameters = this.InstanciateParametersForCycle();
            instanciatedParameters["channel"].SetValue(e.Channel);
            this.Graph.AddCycle(this, instanciatedParameters);
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
