using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Base.Packets;
using Game.Server.HotSpringRooms;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.HOTSPRING_ENTER,"礼堂数据")]
    public class HotSpringEnterHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            HotSpringRoom[] allHotRoom = HotSpringRoomMgr.GetAllHotRoom();
            foreach (HotSpringRoom room in allHotRoom)
            {
                client.Player.Out.SendHotSpringRoomInfo(client.Player, room);
            }
            return 0;
        }

    }
}
