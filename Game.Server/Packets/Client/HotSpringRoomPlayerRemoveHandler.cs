
using System.Data.SqlClient;
using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.BaseClass;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.HOTSPRING_ROOM_PLAYER_REMOVE,"礼堂数据")]
    public class HotSpringRoomPlayerRemoveHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client.Player.CurrentHotSpringRoom != null)
            {
                int iD = client.Player.CurrentHotSpringRoom.Info.ID;
                client.Player.CurrentHotSpringRoom = null;
                Sql_DbObject obj2 = new Sql_DbObject("AppConfig", "conString");
                try
                {
                    SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@RoomID", iD), new SqlParameter("@CurCount", -1) };
                    obj2.RunProcedure("SP_Update_HotSpring", sqlParameters);
                }
                catch
                {
                }
                GSPacketIn pkg = new GSPacketIn((int)ePackageType.HOTSPRING_ROOM_PLAYER_REMOVE);
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("HotSpringRoom.Exited"));
                client.SendTCP(pkg);
            }
            return 0;
        }

    }
}
