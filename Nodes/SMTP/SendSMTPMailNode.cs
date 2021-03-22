using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.SMTP
{
    [NodeDefinition("SendSMTPMailNode", "Send SMTP Mail", NodeTypeEnum.Function, "SMTP")]
    [NodeGraphDescription("Send a mail with a smtp connector")]
    public class SendSMTPMailNode : Node
    {
        public SendSMTPMailNode(string id, BlockGraph graph)
            : base(id, graph, typeof(SendSMTPMailNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("smtp", new NodeParameter(this, "smtp", typeof(SMTPConnectorNode), true));
            this.InParameters.Add("mail", new NodeParameter(this, "mail", typeof(CreateSMTPMailNode), true));
            this.InParameters.Add("from", new NodeParameter(this, "from", typeof(string), true));
            this.InParameters.Add("to", new NodeParameter(this, "to", typeof(string), true));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var smtp = (this.InParameters["smtp"].GetValue() as SMTPConnectorNode).Client;
            var mail = (this.InParameters["mail"].GetValue() as CreateSMTPMailNode).Mail;
            mail.To.Clear();
            mail.From = new System.Net.Mail.MailAddress(this.InParameters["from"].GetValue().ToString());
            mail.To.Add(new System.Net.Mail.MailAddress(this.InParameters["to"].GetValue().ToString()));
            smtp.Send(mail);
            return true;
        }
    }
}
