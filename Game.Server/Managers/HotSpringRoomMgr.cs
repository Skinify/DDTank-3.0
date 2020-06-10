namespace Game.Server.Managers
{
    using Game.Server.HotSpringRooms;
    using SqlDataProvider.BaseClass;
    using SqlDataProvider.Data;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class HotSpringRoomMgr
    {
        private static List<HotSpringRoom> ROOM = new List<HotSpringRoom>();

        private static void Caprom()
        {
            List<HotSpringRoom> list = new List<HotSpringRoom>();
            TankHotSpringLogicProcessor processor = new TankHotSpringLogicProcessor();
            foreach (HotSpringRoomInfo info in GetHotRoomInfo())
            {
                HotSpringRoom item = new HotSpringRoom(info, processor);
                list.Add(item);
            }
            ROOM = list;
        }

        public static HotSpringRoom[] GetAllHotRoom()
        {
            if (ROOM.Count == 0)
            {
                Caprom();
            }
            return ROOM.ToArray();
        }

        public static HotSpringRoom GetHotRoombyID(int id) =>
            ROOM[id - 1];

        private static HotSpringRoomInfo[] GetHotRoomInfo()
        {
            Sql_DbObject obj2 = new Sql_DbObject("AppConfig", "conString");
            SqlDataReader resultDataReader = null;
            List<HotSpringRoomInfo> list = new List<HotSpringRoomInfo>();
            try
            {
                obj2.GetReader(ref resultDataReader, "SP_Get_HotSpring_Room");
                while (resultDataReader.Read())
                {
                    HotSpringRoomInfo item = new HotSpringRoomInfo
                    {
                        ID = (int)resultDataReader["roomID"],
                        Name = resultDataReader["roomName"].ToString(),
                        PlayerID = 0,
                        PlayerName = "",
                        GroomID = (int)resultDataReader["roomNumber"],
                        GroomName = resultDataReader["roomName"].ToString(),
                        BrideID = 0,
                        BrideName = "",
                        Pwd = resultDataReader["roomPassword"].ToString(),
                        AvailTime = 0,
                        MaxCount = (int)resultDataReader["maxCount"],
                        GuestInvite = true,
                        MapIndex = 0,
                        BeginTime = DateTime.Now.AddYears(-1),
                        BreakTime = DateTime.Now.AddYears(2),
                        RoomIntroduction = resultDataReader["roomIntroduction"].ToString(),
                        ServerID = 1,
                        IsHymeneal = true,
                        IsGunsaluteUsed = true
                    };
                    list.Add(item);
                }
                return list.ToArray();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (!((resultDataReader == null) || resultDataReader.IsClosed))
                {
                    resultDataReader.Close();
                }
            }
            return null;
        }

        public static bool Init()
        {
            Caprom();
            return true;
        }
    }
}
