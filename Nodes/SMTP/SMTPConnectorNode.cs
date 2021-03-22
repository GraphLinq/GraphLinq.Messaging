using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.SMTP
{
    [NodeDefinition("SMTPConnectorNode", "SMTP Connector", NodeTypeEnum.Connector, "SMTP")]
    [NodeGraphDescription("SMTP Connector, allow you to send email from your graph")]
    public class SMTPConnectorNode : Node
    {
        public SMTPConnectorNode(string id, BlockGraph graph)
            : base(id, graph, typeof(SMTPConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("host", new NodeParameter(this, "host", typeof(string), true));
            this.InParameters.Add("port", new NodeParameter(this, "port", typeof(int), true));
            this.InParameters.Add("useSSL", new NodeParameter(this, "useSSL", typeof(bool), true));

            this.OutParameters.Add("smtp", new NodeParameter(this, "smtp", typeof(SMTPConnectorNode), false));
        }

        public SmtpClient Client { get; set; }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.Client = new SmtpClient();
            this.Client.Host = this.InParameters["host"].GetValue().ToString();
            this.Client.Port = int.Parse(this.InParameters["port"].GetValue().ToString());
            this.Client.EnableSsl = bool.Parse(this.InParameters["useSSL"].GetValue().ToString());
            this.Client.DeliveryMethod = SmtpDeliveryMethod.Network;
            this.Next();
        }

        public override void OnStop()
        {

        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "smtp")
            {
                return this;
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
