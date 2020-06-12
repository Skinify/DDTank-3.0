﻿using Game.Base.Packets;
using Game.Server.GameObjects;
using log4net;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;


namespace Game.Server.GameUtils
{
    public class Scene
    {
        protected ReaderWriterLock _locker = new ReaderWriterLock();
        protected Dictionary<int, GamePlayer> _players = new Dictionary<int, GamePlayer>();
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Scene(ServerInfo info)
        {
        }

        public bool AddPlayer(GamePlayer player)
        {
            bool flag;
            this._locker.AcquireWriterLock(-1);
            try
            {
                if (this._players.ContainsKey(player.PlayerCharacter.ID))
                {
                    this._players[player.PlayerCharacter.ID] = player;
                    return true;
                }
                this._players.Add(player.PlayerCharacter.ID, player);
                flag = true;
            }
            finally
            {
                this._locker.ReleaseWriterLock();
            }
            return flag;
        }

        public GamePlayer[] GetAllPlayer()
        {
            GamePlayer[] playerArray = null;
            this._locker.AcquireReaderLock(-1);
            try
            {
                playerArray = this._players.Values.ToArray<GamePlayer>();
            }
            finally
            {
                this._locker.ReleaseReaderLock();
            }
            return ((playerArray == null) ? new GamePlayer[0] : playerArray);
        }

        public GamePlayer GetClientFromID(int id)
        {
            try
            {
                if (this._players.Keys.Contains<int>(id))
                {
                    return this._players[id];
                }
            }
            finally
            {
            }
            return null;
        }

        public void RemovePlayer(GamePlayer player)
        {
            this._locker.AcquireWriterLock(-1);
            try
            {
                if (this._players.ContainsKey(player.PlayerCharacter.ID))
                {
                    this._players.Remove(player.PlayerCharacter.ID);
                }
            }
            finally
            {
                this._locker.ReleaseWriterLock();
            }
            GamePlayer[] allPlayer = this.GetAllPlayer();
            GSPacketIn packet = null;
            foreach (GamePlayer player2 in allPlayer)
            {
                if (packet == null)
                {
                    packet = player2.Out.SendSceneRemovePlayer(player);
                }
                else
                {
                    player2.Out.SendTCP(packet);
                }
            }
        }

        public void SendToALL(GSPacketIn pkg)
        {
            this.SendToALL(pkg, null);
        }

        public void SendToALL(GSPacketIn pkg, GamePlayer except)
        {
            GamePlayer[] allPlayer = this.GetAllPlayer();
            foreach (GamePlayer player in allPlayer)
            {
                if (player != except)
                {
                    player.Out.SendTCP(pkg);
                }
            }
        }
    }
}

