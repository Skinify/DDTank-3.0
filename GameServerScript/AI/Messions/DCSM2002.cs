using Bussiness;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    public class DCSM2002 : AMissionControl
    {
        private SimpleBoss boss = null;
        private int npcID = 0x7d4;
        private int bossID = 0x7d3;
        private int kill = 0;
        private PhysicalObj m_moive;
        private PhysicalObj m_front;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 0x6d6)
            {
                return 3;
            }
            if (score > 0x68b)
            {
                return 2;
            }
            if (score > 0x640)
            {
                return 1;
            }
            return 0;
        }

        public override bool CanGameOver()
        {
            if (!((this.boss == null) || this.boss.IsLiving))
            {
                this.kill++;
                return true;
            }
            return false;
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
            if (!((this.boss == null) || this.boss.IsLiving))
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
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] npcIds = new int[] { this.bossID, this.npcID };
            int[] numArray2 = new int[] { this.bossID };
            base.Game.LoadResources(npcIds);
            base.Game.LoadNpcGameOverResources(numArray2);
            base.Game.AddLoadingFile(1, "bombs/51.swf", "tank.resource.bombs.Bomb51");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.AntQueenAsset");
            base.Game.SetMap(0x461);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            this.m_moive = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 1);
            this.m_front = base.Game.Createlayer(0x46b, 150, "font", "game.asset.living.AntQueenAsset", "out", 1, 1);
            this.boss = base.Game.CreateBoss(this.bossID, 0x524, 0x1bc, -1, 1);
            this.boss.SetRelateDemagemRect(-42, -200, 0x54, 0xc2);
            this.boss.Say(LanguageMgr.GetTranslation("GameServerScript.AI.Messions.DCSM2002.msg1", new object[0]), 0, 200, 0);
            this.m_moive.PlayMovie("in", 0x1770, 0);
            this.m_front.PlayMovie("in", 0x17d4, 0);
            this.m_moive.PlayMovie("out", 0x2710, 0x3e8);
            this.m_front.PlayMovie("out", 0x26ac, 0);
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return this.kill;
        }
    }
}

