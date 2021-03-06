﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Server.GameObjects;
using Game.Server.Packets;
using Game.Base.Packets;
using Bussiness;
using SqlDataProvider.Data;
using System.Collections;
using log4net;
using System.Reflection;
using Game.Server.Managers;
using System.Drawing;
using Game.Server.HotSpringRooms.TankHandle;

namespace Game.Server.HotSpringRooms
{
    [HotSpringProcessor(9, "礼堂逻辑")]
    public class TankHotSpringLogicProcessor : AbstractHotSpringProcessor
    {

        private HotSpringCommandMgr _commandMgr;
        private ThreadSafeRandom random = new ThreadSafeRandom();
        public  readonly int TIMEOUT = 1 * 60 * 1000;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TankHotSpringLogicProcessor()
        {
            _commandMgr = new HotSpringCommandMgr();
        }

        public override void OnTick(HotSpringRoom room)
        {
            try 
            {
                if(room != null)
                {
                    room.KickAllPlayer();
                    GSPacketIn pkg = new GSPacketIn((short)ePackageType.MARRY_ROOM_DISPOSE);
                    pkg.WriteInt(room.Info.ID);
                    WorldMgr.MarryScene.SendToALL(pkg);
                    room.StopTimer();   
                }
            }
            catch(Exception ex)
            {
                if (log.IsErrorEnabled)
                    log.Error("OnTick",ex);
            }
        }

        public override void OnGameData(HotSpringRoom room, GamePlayer player, GSPacketIn packet)
        {
            Console.WriteLine(packet.ReadByte());
            HotSpringCmdType type = HotSpringCmdType.TARGET_POINT;

            try
            {
                if (Enum.IsDefined(typeof(HotSpringCmdType), (int)packet.ReadByte()))
                {
                    type = (HotSpringCmdType)packet.ReadByte();
                }
            }
            catch { }


            try
            {
                IHotSpringCommandHandler handleCommand = _commandMgr.LoadCommandHandler((int)type);
                if (handleCommand != null)
                {
                    handleCommand.HandleCommand(this, player, packet);
                }
                else
                {
                    log.Error(string.Format("IP: {0}", player.Client.TcpEndpoint));
                }
            }
            catch(Exception e)
            {
                log.Error(string.Format("IP:{1}, OnGameData is Error: {0}", e.ToString(),player.Client.TcpEndpoint));
            }
        }
    }

}
