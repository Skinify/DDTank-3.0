﻿using Game.Base.Packets;
using Game.Server.Rooms;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler((byte)ePackageType.GAME_ROOM_UPDATE_PLACE, "改变房间位置的状态")]
    public class GameUserUpdatePlaceHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client.Player.CurrentRoom != null && client.Player == client.Player.CurrentRoom.Host)
            {
                RoomMgr.UpdateRoomPos(client.Player.CurrentRoom, packet.ReadByte(), packet.ReadBoolean());
            }
            return 0;
        }
    }
}
