using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using OpenAI_API.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Messaging.Nodes.OpenAI.ChatGPT
{
    [NodeDefinition("GeneratePromptGPTNode", "Generate Prompt GPT", NodeTypeEnum.Function, "OpenAI")]
    [NodeGraphDescription("Send a prompt to ChatGPT API")]
    public class GeneratePromptGPTNode : Node
    {
        public GeneratePromptGPTNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GeneratePromptGPTNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "openAI", new NodeParameter(this, "openAI", typeof(object), true) },
                { "conversation", new NodeParameter(this, "conversation", typeof(object), true) },
                { "prompt", new NodeParameter(this, "prompt", typeof(string), true) }
            };


            this.OutParameters.Add("response", new NodeParameter(this, "response", typeof(string), false));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var response = this.AppendInput();
            response.Wait();
            this.OutParameters["response"].SetValue(response.Result);
            return true;
        }

        public async Task<string> AppendInput()
        {
            var conversation = (this.InParameters["conversation"].GetValue() as Conversation);
            conversation.AppendUserInput(this.InParameters["prompt"].GetValue().ToString());
            var response = await conversation.GetResponseFromChatbot();
            return response;
        }
    }
}
