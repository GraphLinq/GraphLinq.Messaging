using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.Twitter
{
    [NodeDefinition("TweetMessageNode", "Tweet Message", NodeTypeEnum.Function, "Twitter")]
    [NodeGraphDescription("Tweet a message on Twitter")]
    public class TweetMessageNode : Node
    {
        public TweetMessageNode(string id, BlockGraph graph)
       : base(id, graph, typeof(TweetMessageNode).Name)
        {
            this.InParameters.Add("twitter", new NodeParameter(this, "twitter", typeof(TwitterConnector), true));
            this.InParameters.Add("message", new NodeParameter(this, "message", typeof(string), true));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            try
            {
                TwitterConnector twitterConnector = this.InParameters["twitter"].GetValue() as TwitterConnector;
                twitterConnector.Client.Tweets.PublishTweetAsync(this.InParameters["message"].GetValue().ToString()).Wait();
            }
            catch(Exception ex)
            {
                if(this.CurrentTraceItem != null)
                {
                    this.CurrentTraceItem.ExecutionException = ex;
                }
            }

            return true;
        }
    }
}
