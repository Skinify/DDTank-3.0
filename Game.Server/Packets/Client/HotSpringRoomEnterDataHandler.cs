using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Game.Base.Packets;
using Game.Server.HotSpringRooms;
using SqlDataProvider.BaseClass;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.HOTSPRING_ROOM_ENTER,"礼堂数据")]
    public class HotSpringRoomEnterDataHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int num = packet.ReadInt();
            try
            {
                TankHotSpringLogicProcessor processor = new TankHotSpringLogicProcessor();
                HotSpringRoomInfo info = new HotSpringRoomInfo();
                info.ID = num;
                client.Player.CurrentHotSpringRoom = new HotSpringRoom(info, processor);
            }
            catch
            {
                Console.WriteLine("LOi");
            }
            Sql_DbObject obj2 = new Sql_DbObject("AppConfig", "conString");
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@RoomID", num), new SqlParameter("@CurCount", 1) };
                obj2.RunProcedure("SP_Update_HotSpring", sqlParameters);
            }
            catch
            {
            }
            string str = packet.ReadString();
            GSPacketIn pkg = new GSPacketIn((short)ePackageType.HOTSPRING_ROOM_ENTER);
            pkg.WriteInt(num);
            pkg.WriteInt(num);
            pkg.WriteString("RoomName");
            pkg.WriteString("");
            pkg.WriteInt(1);
            pkg.WriteInt(1);
            pkg.WriteInt(client.Player.PlayerCharacter.ID);
            pkg.WriteString("abc");
            pkg.WriteDateTime(DateTime.Now.AddDays(-50.0));
            pkg.WriteString("Room Intro");
            pkg.WriteInt(1);
            pkg.WriteInt(10);
            pkg.WriteDateTime(DateTime.Now);
            pkg.WriteInt(10);
            client.SendTCP(pkg);
            return 0;
        }

    }
}
