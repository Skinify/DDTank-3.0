namespace Game.Server.Packets.Client
{
    using Game.Base.Packets;
    using Game.Server;

    [PacketHandler((int)ePackageType.DAILY_OPEN, "qua")]
    internal class GunnyQua : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            GSPacketIn pkg = packet.Clone();
            pkg.WriteBoolean(true);
            client.SendTCP(pkg);
            return 0;
        }
    }
}
