using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using OpenAI_API.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.OpenAI.ChatGPT
{
    [NodeDefinition("CreateConversationGPTNode", "Create Conversation GPT", NodeTypeEnum.Function, "OpenAI")]
    [NodeGraphDescription("Creation a conversation with ChatGPT API")]
    public class CreateConversationGPTNode : Node
    {
        public CreateConversationGPTNode(string id, BlockGraph graph)
            : base(id, graph, typeof(CreateConversationGPTNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "openAI", new NodeParameter(this, "openAI", typeof(object), true) }
            };


            this.OutParameters.Add("conversation", new NodeParameter(this, "conversation", typeof(object), false));
        }

        public override bool CanExecute => true;
        public override bool CanBeExecuted => true;

        public override bool OnExecution()
        {
            var conversation = (this.InParameters["openAI"].GetValue() as OpenAIConnectorNode).Client.Chat.CreateConversation();
            this.OutParameters["conversation"].SetValue(conversation);
            return true;
        }
    }
}
