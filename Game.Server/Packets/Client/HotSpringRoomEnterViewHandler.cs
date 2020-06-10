using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.HOTSPRING_ROOM_ENTER_VIEW,"礼堂数据")]
    public class HotSpringRoomEnterViewHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            GSPacketIn pkg = new GSPacketIn((short)ePackageType.HOTSPRING_ROOM_PLAYER_ADD);
            
            PlayerInfo player = client.Player.PlayerCharacter;

            pkg.WriteInt(player.ID);
            pkg.WriteInt(player.Grade);
            pkg.WriteInt(player.Hide);
            pkg.WriteInt(player.Repute);
            pkg.WriteString(player.NickName);
            pkg.WriteBoolean(true);
            pkg.WriteInt(5);
            pkg.WriteBoolean(player.Sex);
            pkg.WriteString(player.Style);
            pkg.WriteString(player.Colors);
            pkg.WriteString(player.Skin);
            pkg.WriteInt(300);
            pkg.WriteInt(400);
            pkg.WriteInt(player.FightPower);
            pkg.WriteInt(player.Win);
            pkg.WriteInt(player.Total);
            pkg.WriteInt(1);
            client.SendTCP(pkg);
            return 0;
        }

    }
}
