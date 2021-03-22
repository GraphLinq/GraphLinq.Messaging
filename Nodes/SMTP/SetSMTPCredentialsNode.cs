using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.SMTP
{
    [NodeDefinition("SetSMTPCredentialsNode", "Set SMTP Credentials", NodeTypeEnum.Function, "SMTP")]
    [NodeGraphDescription("Set credentials for a SMTP connector")]
    public class SetSMTPCredentialsNode : Node
    {
        public SetSMTPCredentialsNode(string id, BlockGraph graph)
            : base(id, graph, typeof(SetSMTPCredentialsNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("smtp", new NodeParameter(this, "smtp", typeof(SMTPConnectorNode), true));
            this.InParameters.Add("username", new NodeParameter(this, "username", typeof(string), true));
            this.InParameters.Add("password", new NodeParameter(this, "password", typeof(string), true));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var smtpClient = (this.InParameters["smtp"].GetValue() as SMTPConnectorNode).Client;
            var username = this.InParameters["username"].GetValue().ToString();
            var password = this.InParameters["password"].GetValue().ToString();
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(username, password);
            return true;
        }
    }
}
