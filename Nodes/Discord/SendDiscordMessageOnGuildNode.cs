using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeBlock.Plugin.Messaging.Nodes.Discord
{

    [NodeDefinition("SendDiscordMessageOnGuildNode", "Send Discord Channel Message", NodeTypeEnum.Function, "Discord")]
    [NodeGraphDescription("Send a message on a guild channel")]
    public class SendDiscordMessageOnGuildNode : Node
    {
        public SendDiscordMessageOnGuildNode(string id, BlockGraph graph)
         : base(id, graph, typeof(SendDiscordMessageOnGuildNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("discord", new NodeParameter(this, "discord", typeof(DiscordConnector), true));
            this.InParameters.Add("guildId", new NodeParameter(this, "guildId", typeof(ulong), true));
            this.InParameters.Add("channel", new NodeParameter(this, "channel", typeof(ulong), true));
            this.InParameters.Add("message", new NodeParameter(this, "message", typeof(string), true));

            this.OutParameters.Add("messageId", new NodeParameter(this, "messageId", typeof(ulong), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var discordClient = (this.InParameters["discord"].GetValue() as DiscordConnector).DiscordClient;
            var guilds = discordClient.Guilds;
            var guild = discordClient.GetGuild(ulong.Parse(this.InParameters["guildId"].GetValue().ToString()));
            var channel = guild.GetTextChannel(ulong.Parse(this.InParameters["channel"].GetValue().ToString()));
            if (channel == null) return false;
            var messageTask = channel.SendMessageAsync(this.InParameters["message"].GetValue().ToString());
            messageTask.Wait();
            var messageId = messageTask.Result.Id;
            this.OutParameters["messageId"].SetValue(messageId);
            return true;
        }
    }
}
