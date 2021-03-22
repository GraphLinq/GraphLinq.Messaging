using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.SMTP
{
    [NodeDefinition("AttachFileToSMTPMailNode", "Attach File SMTP Mail", NodeTypeEnum.Function, "SMTP")]
    [NodeGraphDescription("Attach a file to smtp mail")]
    public class AttachFileToSMTPMailNode : Node
    {
        public AttachFileToSMTPMailNode(string id, BlockGraph graph)
            : base(id, graph, typeof(AttachFileToSMTPMailNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("mail", new NodeParameter(this, "mail", typeof(CreateSMTPMailNode), true));
            this.InParameters.Add("file", new NodeParameter(this, "file", typeof(System.Byte[]), true));
            this.InParameters.Add("fileName", new NodeParameter(this, "fileName", typeof(string), true));

            this.OutParameters.Add("mailOut", new NodeParameter(this, "mailOut", typeof(CreateSMTPMailNode), false));
        }


        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var mail = (this.InParameters["mail"].GetValue() as CreateSMTPMailNode).Mail;
            var fileName = this.InParameters["fileName"].GetValue().ToString();
            var file = new MemoryStream(this.InParameters["file"].GetValue() as System.Byte[]);

            var attachment = new Attachment(file, fileName);
            mail.Attachments.Add(attachment);

            this.OutParameters["mailOut"].Value = (this.InParameters["mail"].GetValue() as CreateSMTPMailNode);

            return true;
        }
    }
}
