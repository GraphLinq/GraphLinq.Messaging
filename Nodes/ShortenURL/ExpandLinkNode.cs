using NodeBlock.Engine.Attributes;
using NodeBlock.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using NodeBlock.Plugin.Messaging.Nodes.ShortenURL;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;

namespace NodeBlock.Plugin.Messaging.Nodes.ShortenURL
{
    [NodeDefinition("ExpandLinkNode", "Expand shortened URL", NodeTypeEnum.Function, "Shorten URL")]
    [NodeGraphDescription("Expand a Shortened URL using glq.link shortener")]
    [NodeSpecialActionAttribute("Get API key/View Stats", "open_url", "https://glq.link")]
    public class ExpandLinkNode : Node
    {
        public ExpandLinkNode(string id, BlockGraph graph)
            : base(id, graph, typeof(ExpandLinkNode).Name)
        {
            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));
            this.InParameters.Add("shortLink", new NodeParameter(this, "shortLink", typeof(string), true));

            this.OutParameters.Add("originalLink", new NodeParameter(this, "originalLink", typeof(string), true));
        }

        private HttpClient client = new HttpClient();

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var link = this.InParameters["shortLink"].GetValue().ToString();
            var key = this.InParameters["apiKey"].GetValue().ToString();



            var response = client.GetAsync($"https://glq.link/api/v2/action/lookup?key={key}&url_ending={link}").Result;
            var result = response.Content.ReadAsStringAsync().Result;

            this.OutParameters["originalLink"].SetValue(result.ToString());

            return true;
        }
    }
}
