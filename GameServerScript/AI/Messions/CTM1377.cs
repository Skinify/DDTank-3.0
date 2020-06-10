using Bussiness;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    public class CTM1377 : AMissionControl
    {
        private SimpleBoss m_king = null;
        private int m_kill = 0;
        private int bossID = 0x51b;
        private int npcID = 0x518;
        private PhysicalObj m_kingMoive;
        private PhysicalObj m_front;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 0x532)
            {
                return 3;
            }
            if (score > 0x47e)
            {
                return 2;
            }
            if (score > 970)
            {
                return 1;
            }
            return 0;
        }

        public override bool CanGameOver()
        {
            if (!this.m_king.IsLiving)
            {
                this.m_kill++;
                return true;
            }
            return false;
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (this.m_kingMoive != null)
            {
                base.Game.RemovePhysicalObj(this.m_kingMoive, true);
                this.m_kingMoive = null;
            }
            if (this.m_front != null)
            {
                base.Game.RemovePhysicalObj(this.m_front, true);
                this.m_front = null;
            }
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            bool flag = true;
            foreach (Player player in base.Game.GetAllFightPlayers())
            {
                if (player.IsLiving)
                {
                    flag = false;
                }
            }
            if (!(this.m_king.IsLiving || flag))
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            if (this.m_king.State == 0)
            {
                this.m_king.SetRelateDemagemRect(-41, -187, 0x53, 140);
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] npcIds = new int[] { this.npcID, this.bossID };
            int[] numArray2 = new int[] { this.bossID };
            base.Game.LoadResources(npcIds);
            base.Game.LoadNpcGameOverResources(numArray2);
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.ZhenBombKingAsset");
            base.Game.SetMap(0x434);
            base.Game.IsBossWar = LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1378.msg1", new object[0]);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            this.m_king = base.Game.CreateBoss(this.bossID, 900, 590, -1, 0);
            this.m_kingMoive = base.Game.Createlayer(0, 0, "kingmoive", "game.asset.living.BossBgAsset", "out", 1, 1);
            this.m_front = base.Game.Createlayer(710, 380, "font", "game.asset.living.ZhenBombKingAsset", "out", 1, 1);
            this.m_king.FallFrom(900, 590, "fall", 0, 2, 0x3e8);
            this.m_king.SetRelateDemagemRect(-41, -187, 0x53, 140);
            this.m_kingMoive.PlayMovie("in", 0x3e8, 0);
            this.m_front.PlayMovie("in", 0x7d0, 0x7d0);
            this.m_king.AddDelay(0x10);
            base.Game.BossCardCount = 1;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return this.m_kill;
        }
    }
}

