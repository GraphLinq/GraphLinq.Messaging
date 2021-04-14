using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace NodeBlock.Plugin.Messaging.Nodes.Discord
{
    [NodeDefinition("RemoveRoleUserNode", "Remove Role User", NodeTypeEnum.Function, "Discord")]
    [NodeGraphDescription("Remove role for some user")]
    public class RemoveRoleUserNode : Node
    {
        public RemoveRoleUserNode(string id, BlockGraph graph)
         : base(id, graph, typeof(RemoveRoleUserNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("discord", new NodeParameter(this, "discord", typeof(DiscordConnector), true));
            this.InParameters.Add("guildId", new NodeParameter(this, "guildId", typeof(ulong), true));
            this.InParameters.Add("userId", new NodeParameter(this, "userId", typeof(ulong), true));
            this.InParameters.Add("roleName", new NodeParameter(this, "roleName", typeof(string), true));

        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var discordClient = (this.InParameters["discord"].GetValue() as DiscordConnector).DiscordClient;
            var guilds = discordClient.Guilds;
            var guild = discordClient.GetGuild(ulong.Parse(this.InParameters["guildId"].GetValue().ToString()));
            var user = guild.GetUser(ulong.Parse(this.InParameters["userId"].GetValue().ToString()));

            var role = user.Roles.FirstOrDefault(x => x.Name == this.InParameters["roleName"].GetValue().ToString());
            if (role == null) return true;
            var roleRequest = user.RemoveRoleAsync(role);
            roleRequest.Wait();

            return true;
        }
    }
}
