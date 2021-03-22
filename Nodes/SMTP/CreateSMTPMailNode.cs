using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.SMTP
{
    [NodeDefinition("CreateSMTPMailNode", "Create SMTP Mail", NodeTypeEnum.Function, "SMTP")]
    [NodeGraphDescription("Create a new smtp mail instance")]
    public class CreateSMTPMailNode : Node
    {
        public CreateSMTPMailNode(string id, BlockGraph graph)
            : base(id, graph, typeof(CreateSMTPMailNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("subject", new NodeParameter(this, "subject", typeof(string), true));
            this.InParameters.Add("body", new NodeParameter(this, "body", typeof(string), true));

            this.OutParameters.Add("mail", new NodeParameter(this, "mail", typeof(CreateSMTPMailNode), false));
        }

        public MailMessage Mail { get; set; }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            Mail = new MailMessage();
            var subject = this.InParameters["subject"].GetValue().ToString();
            var body = this.InParameters["body"].GetValue().ToString();
            Mail.Subject = subject;
            Mail.Body = body;
            return true;
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if(parameter.Name == "mail")
            {
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
