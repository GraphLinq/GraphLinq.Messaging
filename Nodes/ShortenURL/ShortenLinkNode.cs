﻿using NodeBlock.Engine.Attributes;
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
    [NodeDefinition("ShortenLinkNode", "Shorten a URL", NodeTypeEnum.Function, "Shorten URL")]
    [NodeGraphDescription("Shorten a long URL using glq.link shortener")]
    [NodeSpecialActionAttribute("Get API key/View Stats", "open_url", "https://glq.link")]
    public class ShortenLinkNode: Node
    {
        public ShortenLinkNode(string id, BlockGraph graph)
            : base(id, graph, typeof(ShortenLinkNode).Name)
        {
            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));
            this.InParameters.Add("originalLink", new NodeParameter(this, "originalLink", typeof(string), true));

            this.OutParameters.Add("shortLink", new NodeParameter(this, "shortLink", typeof(string), true));
        }

        private HttpClient client = new HttpClient();

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var link = this.InParameters["originalLink"].GetValue().ToString();
            var key = this.InParameters["apiKey"].GetValue().ToString();

            var response = client.GetAsync($"https://glq.link/api/v2/action/shorten?key={key}&url={link}").Result;
            var result = response.Content.ReadAsStringAsync().Result;

            this.OutParameters["shortLink"].SetValue(result.ToString());

            return true;
        }
    }
}
