using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Streaming;

namespace NodeBlock.Plugin.Messaging.Nodes.Twitter
{
    [NodeDefinition("OnUserTweetNode", "On User Tweet", NodeTypeEnum.Event, "Twitter")]
    [NodeGraphDescription("This event allow you to track tweets from a user")]
    public class OnUserTweetNode : Node
    {
        private IFilteredStream stream;

        public OnUserTweetNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OnUserTweetNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("twitter", new NodeParameter(this, "twitter", typeof(TwitterConnector), true));
            this.InParameters.Add("user", new NodeParameter(this, "user", typeof(string), true));

            this.OutParameters.Add("message", new NodeParameter(this, "message", typeof(string), false));
            this.OutParameters.Add("link", new NodeParameter(this, "link", typeof(string), false));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            TwitterConnector twitterConnector = this.InParameters["twitter"].GetValue() as TwitterConnector;
            this.stream = twitterConnector.Client.Streams.CreateFilteredStream();
            stream.MatchingTweetReceived += Stream_MatchingTweetReceived;

            var twitterUser = twitterConnector.Client.Users.GetUserAsync(this.InParameters["user"].GetValue().ToString());
            twitterUser.Wait();
            stream.AddFollow(twitterUser.Result);

            stream.StartMatchingAllConditionsAsync();
        }

        public override void OnStop()
        {
            this.stream.Stop();
        }

        private void Stream_MatchingTweetReceived(object sender, Tweetinvi.Events.MatchedTweetReceivedEventArgs e)
        {
            if (e.Tweet.CreatedBy.ScreenName != this.InParameters["user"].GetValue().ToString()) return;
            var instanciatedParameters = this.InstanciateParametersForCycle();
            instanciatedParameters["message"].SetValue(e.Tweet.FullText);
            instanciatedParameters["link"].SetValue(e.Tweet.Url);
            this.Graph.AddCycle(this, instanciatedParameters);
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
