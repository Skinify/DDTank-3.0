using Game.Server;
using Game.Base;

namespace GameServerScript.Commands
{
    [Cmd(
        "&version",
        ePrivLevel.Player,
        "Get the version of the GameServer",
        "/version")]
    public class VersionCommandHandler : AbstractCommandHandler, ICommandHandler
    {
        public bool OnCommand(BaseClient client, string[] args)
        {
            DisplayMessage(client,"Servion Id:{0},Version:{1}",GameServer.Instance.Configuration.ServerID,GameServer.Edition);

            return true;
        }
    }
}
