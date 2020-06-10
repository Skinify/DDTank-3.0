using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
    public class CTM1376 : AMissionControl
    {
        private PhysicalObj m_kingMoive;
        private PhysicalObj m_kingFront;
        private SimpleBoss m_king = null;
        private SimpleBoss m_secondKing = null;
        private PhysicalObj[] m_leftWall = null;
        private PhysicalObj[] m_rightWall = null;
        private int m_kill = 0;
        private int m_state = 0x519;
        private int turn = 0;
        private int firstBossID = 0x519;
        private int secondBossID = 0x51a;
        private int npcID = 0x51e;
        private int direction;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 0x47e)
            {
                return 3;
            }
            if (score > 0x39d)
            {
                return 2;
            }
            if (score > 700)
            {
                return 1;
            }
            return 0;
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();
            if (!this.m_king.IsLiving && (this.m_state == this.firstBossID))
            {
                this.m_state++;
            }
            if ((this.m_state == this.secondBossID) && (this.m_secondKing == null))
            {
                this.m_secondKing = base.Game.CreateBoss(this.m_state, this.m_king.X, this.m_king.Y, this.m_king.Direction, 2);
                base.Game.RemoveLiving(this.m_king.Id);
                if (this.m_secondKing.Direction == 1)
                {
                    this.m_secondKing.SetRect(-40, -112, 0x73, 0x60);
                }
                this.m_secondKing.SetRelateDemagemRect(-21, -87, 0x48, 0x3b);
                this.m_secondKing.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1376.msg3", new object[0]), 0, 0xbb8);
                List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
                Player player = base.Game.FindRandomPlayer();
                int delay = 0;
                if (player != null)
                {
                    delay = player.Delay;
                }
                foreach (Player player2 in allFightPlayers)
                {
                    if (player2.Delay < delay)
                    {
                        delay = player2.Delay;
                    }
                }
                this.m_secondKing.AddDelay(delay - 0x7d0);
                this.turn = base.Game.TurnIndex;
            }
            if (!((this.m_secondKing == null) || this.m_secondKing.IsLiving))
            {
                this.direction = this.m_secondKing.Direction;
                this.m_kill++;
                return true;
            }
            return false;
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (base.Game.TurnIndex > (this.turn + 1))
            {
                if (this.m_kingMoive != null)
                {
                    base.Game.RemovePhysicalObj(this.m_kingMoive, true);
                    this.m_kingMoive = null;
                }
                if (this.m_kingFront != null)
                {
                    base.Game.RemovePhysicalObj(this.m_kingFront, true);
                    this.m_kingFront = null;
                }
            }
        }

        public override void OnGameOver()
        {
            int num;
            base.OnGameOver();
            if (!((this.m_state != this.secondBossID) || this.m_secondKing.IsLiving))
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo> {
                new LoadingFileInfo(2, "image/map/show7.jpg", "")
            };
            base.Game.SendLoadResource(loadingFileInfos);
            this.m_leftWall = base.Game.FindPhysicalObjByName("wallLeft");
            this.m_rightWall = base.Game.FindPhysicalObjByName("wallRight");
            for (num = 0; num < this.m_leftWall.Length; num++)
            {
                base.Game.RemovePhysicalObj(this.m_leftWall[num], true);
            }
            for (num = 0; num < this.m_rightWall.Length; num++)
            {
                base.Game.RemovePhysicalObj(this.m_rightWall[num], true);
            }
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            base.Game.AddLoadingFile(1, "bombs/61.swf", "tank.resource.bombs.Bomb61");
            base.Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_left");
            base.Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_right");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.boguoLeaderAsset");
            int[] npcIds = new int[] { this.firstBossID, this.secondBossID, this.npcID };
            base.Game.LoadResources(npcIds);
            int[] numArray2 = new int[] { this.firstBossID };
            base.Game.LoadNpcGameOverResources(numArray2);
            base.Game.SetMap(0x434);
            base.Game.IsBossWar = LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1376.msg1", new object[0]);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            this.m_kingMoive = base.Game.Createlayer(0, 0, "kingmoive", "game.asset.living.BossBgAsset", "out", 1, 1);
            this.m_kingFront = base.Game.Createlayer(720, 0x1ef, "font", "game.asset.living.boguoKingAsset", "out", 1, 1);
            this.m_king = base.Game.CreateBoss(this.m_state, 0x378, 590, -1, 0);
            this.m_king.FallFrom(0x378, 690, "fall", 0, 2, 0x3e8);
            this.m_king.SetRelateDemagemRect(-21, -87, 0x48, 0x3b);
            this.m_king.AddDelay(10);
            this.m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1376.msg2", new object[0]), 0, 0xbb8);
            this.m_kingMoive.PlayMovie("in", 0x2328, 0);
            this.m_kingFront.PlayMovie("in", 0x2328, 0);
            this.m_kingMoive.PlayMovie("out", 0x32c8, 0);
            this.m_kingFront.PlayMovie("out", 0x3458, 0);
            this.turn = base.Game.TurnIndex;
            base.Game.BossCardCount = 1;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return this.m_kill;
        }
    }
}

