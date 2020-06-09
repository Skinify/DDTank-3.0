﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.HOTSPRING_ROOM_ENTER_CONFIRM,"礼堂数据")]
    public class HotSpringEnterConfirmHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            var roomId = packet.ReadInt();
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.HOTSPRING_ROOM_ENTER_CONFIRM);
            pkg.WriteInt(roomId);
            client.SendTCP(pkg);
            return 0;
        }

    }
}
