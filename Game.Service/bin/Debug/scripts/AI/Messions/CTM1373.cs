﻿using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
    public class CTM1373 : AMissionControl
    {
        private SimpleBoss m_boss;
        private PhysicalObj m_moive;
        private PhysicalObj m_front;
        private int bossID = 0x517;
        private int npcID = 0x51d;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 0x604)
            {
                return 3;
            }
            if (score > 0x582)
            {
                return 2;
            }
            if (score > 0x505)
            {
                return 1;
            }
            return 0;
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();
            return !this.m_boss.IsLiving;
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (base.Game.TurnIndex > 1)
            {
                if (this.m_moive != null)
                {
                    base.Game.RemovePhysicalObj(this.m_moive, true);
                    this.m_moive = null;
                }
                if (this.m_front != null)
                {
                    base.Game.RemovePhysicalObj(this.m_front, true);
                    this.m_front = null;
                }
            }
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (!this.m_boss.IsLiving)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo> {
                new LoadingFileInfo(2, "image/map/show4.jpg", "")
            };
            base.Game.SendLoadResource(loadingFileInfos);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            base.Game.AddLoadingFile(1, "bombs/61.swf", "tank.resource.bombs.Bomb61");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.boguoLeaderAsset");
            int[] npcIds = new int[] { this.bossID, this.npcID };
            base.Game.LoadResources(npcIds);
            int[] numArray2 = new int[] { this.bossID };
            base.Game.LoadNpcGameOverResources(numArray2);
            base.Game.SetMap(0x431);
            base.Game.IsBossWar = LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1373.msg1", new object[0]);
        }

        public override void OnStartGame()
        {
            this.m_moive = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 1);
            this.m_front = base.Game.Createlayer(680, 330, "font", "game.asset.living.boguoLeaderAsset", "out", 1, 1);
            this.m_boss = base.Game.CreateBoss(this.bossID, 770, -1500, -1, 1);
            this.m_boss.FallFrom(770, 0x12d, "fall", 0, 2, 0x3e8);
            this.m_boss.SetRelateDemagemRect(0x22, -35, 11, 0x12);
            this.m_boss.AddDelay(10);
            this.m_boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1373.msg2", new object[0]), 0, 0x1770);
            this.m_boss.PlayMovie("call", 0x170c, 0);
            this.m_moive.PlayMovie("in", 0x2328, 0);
            this.m_boss.PlayMovie("weakness", 0x2710, 0x1388);
            this.m_front.PlayMovie("in", 0x2328, 0);
            this.m_moive.PlayMovie("out", 0x3a98, 0);
            base.Game.BossCardCount = 1;
            base.OnStartGame();
        }

        public override int UpdateUIData()
        {
            if (this.m_boss == null)
            {
                return 0;
            }
            if (!this.m_boss.IsLiving)
            {
                return 1;
            }
            return base.UpdateUIData();
        }
    }
}

