using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
    //[PacketHandler((byte)ePackageType.GAME_PLAYER_EXIT, "关闭房间位置")]
    public class GameUserCloseHandler:IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            /*if (client.Player.CurrentRoom != null && client.Player == client.Player.CurrentRoom.Host)
            {

                    byte index = packet.ReadByte();
                    client.Player.CurrentRoom.OpenState[index] = false;
                    client.Player.CurrentRoom.KickPlayerIndex(client.Player, index);
                    client.Player.CurrentRoom.SendToAll(packet);
                
            }
            return 0;*/
            return 0;
        }
    }
}
