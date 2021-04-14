using Discord.WebSocket;
using Discord;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Net;

namespace NodeBlock.Plugin.Messaging.Nodes.Discord
{

    [NodeDefinition("AddEmojiOnMessageNode", "Add Emoji On Message", NodeTypeEnum.Function, "Discord")]
    [NodeGraphDescription("Add emoji on message")]
    public class AddEmojiOnMessageNode : Node
    {
        public AddEmojiOnMessageNode(string id, BlockGraph graph)
         : base(id, graph, typeof(AddEmojiOnMessageNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("discord", new NodeParameter(this, "discord", typeof(DiscordConnector), true));
            this.InParameters.Add("channelId", new NodeParameter(this, "channelId", typeof(ulong), true));
            this.InParameters.Add("messageId", new NodeParameter(this, "messageId", typeof(ulong), true));
            this.InParameters.Add("guildId", new NodeParameter(this, "guildId", typeof(ulong), true));

        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var discordClient = (this.InParameters["discord"].GetValue() as DiscordConnector).DiscordClient;
            var guilds = discordClient.Guilds;
            var guild = discordClient.GetGuild(ulong.Parse(this.InParameters["guildId"].GetValue().ToString()));
            var channel = guild.GetTextChannel(ulong.Parse(this.InParameters["channelId"].GetValue().ToString()));
            if (channel == null) return false;
            var message = channel.GetMessageAsync(ulong.Parse(this.InParameters["channelId"].GetValue().ToString()));
            message.Wait();
            var reaction  = message.Result.AddReactionAsync(new Emoji(this.InParameters["channelId"].GetValue().ToString()));
            reaction.Wait();
            return true;
        }
    }
}
