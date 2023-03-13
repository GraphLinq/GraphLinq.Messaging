using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.OpenAI
{
    [NodeDefinition("OpenAIConnector", "OpenAI Connector", NodeTypeEnum.Connector, "OpenAI")]
    [NodeGraphDescription("OpenAI connector, it allow you to interact with OpenAI API")]
    [NodeSpecialActionAttribute("Open OpenAI Developer", "open_url", "https://platform.openai.com/docs/quickstart")]
    public class OpenAIConnectorNode : Node
    {
        public OpenAIConnectorNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OpenAIConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("openAIKey", new NodeParameter(this, "openAIKey", typeof(string), true));

            this.OutParameters.Add("openAI", new NodeParameter(this, "openAI", typeof(OpenAIConnectorNode), true));
        }

        [JsonIgnore]
        public OpenAI_API.OpenAIAPI Client;

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.Client = new OpenAI_API.OpenAIAPI(this.InParameters["openAIKey"].GetValue().ToString());
            this.Next();
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "openAI")
            {
                return this;
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
