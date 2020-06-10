using Game.Base.Packets;
using Game.Server.HotSpringRooms;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.HOTSPRING_CMD,"礼堂数据")]
    public class HotSpringCmdDataHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            packet = packet.Clone();
            packet.ClearContext();
            client.Player.CurrentHotSpringRoom = new HotSpringRoom(new HotSpringRoomInfo(), new TankHotSpringLogicProcessor());
            client.Player.CurrentHotSpringRoom.ProcessData(client.Player, packet);
            return 0;
        }

    }
}
