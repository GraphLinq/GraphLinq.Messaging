using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi;
using Tweetinvi.Models;

namespace NodeBlock.Plugin.Messaging.Nodes.Twitter
{
    [NodeDefinition("TwitterConnector", "Twitter Connector", NodeTypeEnum.Connector, "Twitter")]
    [NodeGraphDescription("Twitter connector, it allow you to track tweets users or tweet some text to create credentials go here https://developer.twitter.com/en/apps/create")]
    [NodeSpecialActionAttribute("Open Twitter Developer", "open_url", "https://developer.twitter.com/en/apps/create")]
    public class TwitterConnector : Node
    {
        public TwitterConnector(string id, BlockGraph graph)
           : base(id, graph, typeof(TwitterConnector).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("consumerKey", new NodeParameter(this, "consumerKey", typeof(string), true));
            this.InParameters.Add("consumerSecret", new NodeParameter(this, "consumerSecret", typeof(string), true));
            this.InParameters.Add("accessToken", new NodeParameter(this, "accessToken", typeof(string), true));
            this.InParameters.Add("accessTokenSecret", new NodeParameter(this, "accessTokenSecret", typeof(string), true));

            this.OutParameters.Add("twitter", new NodeParameter(this, "twitter", typeof(TwitterConnector), true));
        }

        [JsonIgnore]
        public TwitterClient Client { get; set; }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.Client = new TwitterClient(this.InParameters["consumerKey"].GetValue().ToString(), this.InParameters["consumerSecret"].GetValue().ToString(),
                this.InParameters["accessToken"].GetValue().ToString(), this.InParameters["accessTokenSecret"].GetValue().ToString());

            this.Next();
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "twitter")
            {
                return this;
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
