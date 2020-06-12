using Game.Base.Packets;
using Game.Server;


namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.PROP_UPDATE, "防沉迷系统开关")]
    internal class UpdatePropertyHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            client.Player.UpdateProperty();
            return 0;
        }
    }
}

