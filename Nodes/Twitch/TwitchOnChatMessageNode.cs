using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.Twitch
{
    [NodeDefinition("TwitchOnChatMessageNode", "On Twitch Chat Message", NodeTypeEnum.Event, "Twitch")]
    [NodeGraphDescription("This event trigger when someone send a message in channel chat")]
    public class TwitchOnChatMessageNode : Node
    {
        public TwitchOnChatMessageNode(string id, BlockGraph graph)
      : base(id, graph, typeof(TwitchOnChatMessageNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("twitch", new NodeParameter(this, "twitch", typeof(object), true));

            this.OutParameters.Add("id", new NodeParameter(this, "id", typeof(string), false));
            this.OutParameters.Add("channel", new NodeParameter(this, "channel", typeof(string), false));
            this.OutParameters.Add("message", new NodeParameter(this, "message", typeof(string), false));
            this.OutParameters.Add("userId", new NodeParameter(this, "userId", typeof(string), false));
            this.OutParameters.Add("displayName", new NodeParameter(this, "displayName", typeof(string), false));
            this.OutParameters.Add("isSubscriber", new NodeParameter(this, "isSubscriber", typeof(bool), false));
            this.OutParameters.Add("isFirstMessage", new NodeParameter(this, "isFirstMessage", typeof(bool), false));
            this.OutParameters.Add("isModerator", new NodeParameter(this, "isModerator", typeof(bool), false));
            this.OutParameters.Add("isVip", new NodeParameter(this, "isVip", typeof(bool), false));
            this.OutParameters.Add("isHighlighted", new NodeParameter(this, "isHighlighted", typeof(bool), false));
            this.OutParameters.Add("bits", new NodeParameter(this, "bits", typeof(int), false));
            this.OutParameters.Add("bitsInDollars", new NodeParameter(this, "bitsInDollars", typeof(double), false));
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
            twitchInstance.Client.OnMessageReceived -= Client_OnMessageReceived;
        }

        private void setupDefaultBehavior()
        {
            TwitchConnector twitchInstance = this.InParameters["twitch"].GetValue() as TwitchConnector;
            twitchInstance.Client.OnMessageReceived += Client_OnMessageReceived;
        }

        private void Client_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.IsMe) return;
            var instanciatedParameters = this.InstanciateParametersForCycle();
            instanciatedParameters["id"].SetValue(e.ChatMessage.Id);
            instanciatedParameters["channel"].SetValue(e.ChatMessage.Channel);
            instanciatedParameters["message"].SetValue(e.ChatMessage.Message);
            instanciatedParameters["userId"].SetValue(e.ChatMessage.UserId);
            instanciatedParameters["displayName"].SetValue(e.ChatMessage.DisplayName);
            instanciatedParameters["isSubscriber"].SetValue(e.ChatMessage.IsSubscriber);
            instanciatedParameters["isFirstMessage"].SetValue(e.ChatMessage.IsFirstMessage);
            instanciatedParameters["isModerator"].SetValue(e.ChatMessage.IsModerator);
            instanciatedParameters["isVip"].SetValue(e.ChatMessage.IsVip);
            instanciatedParameters["isHighlighted"].SetValue(e.ChatMessage.IsHighlighted);
            instanciatedParameters["bits"].SetValue(e.ChatMessage.Bits);
            instanciatedParameters["bitsInDollars"].SetValue(e.ChatMessage.BitsInDollars);
           
            this.Graph.AddCycle(this, instanciatedParameters);
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
