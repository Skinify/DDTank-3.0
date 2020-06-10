using Game.Logic.AI;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
    public class CSM3001 : AMissionControl
    {
        private List<SimpleNpc> SomeNpc = new List<SimpleNpc>();
        private SimpleBoss boss = null;
        private PhysicalObj Tip = null;
        private bool result = false;
        private int killCount = 0;
        private int preKillNum = 0;
        private bool canPlayMovie = false;
        public int turnCount;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 900)
            {
                return 3;
            }
            if (score > 0x339)
            {
                return 2;
            }
            if (score > 0x2d5)
            {
                return 1;
            }
            return 0;
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();
            if (base.Game.TurnIndex > 0x63)
            {
                return true;
            }
            this.result = false;
            foreach (SimpleNpc npc in this.SomeNpc)
            {
                if (npc.IsLiving)
                {
                    this.result = true;
                }
            }
            return !(this.result || (this.SomeNpc.Count != 15));
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override void OnGameOver()
        {
            if (!this.result)
            {
                foreach (Player player in base.Game.GetAllFightPlayers())
                {
                    player.CanGetProp = true;
                }
                base.Game.IsWin = true;
            }
        }

        public override void OnNewTurnStarted()
        {
            List<ItemTemplateInfo> list = new List<ItemTemplateInfo>();
            List<ItemInfo> list2 = new List<ItemInfo>();
            if ((base.Game.TurnIndex > 1) && (base.Game.CurrentPlayer.Delay > base.Game.PveGameDelay))
            {
                for (int i = 0; i < 3; i++)
                {
                    if (this.SomeNpc.Count >= 7)
                    {
                        break;
                    }
                    if ((this.turnCount % 2) == 0)
                    {
                        this.SomeNpc.Add(base.Game.CreateNpc(0xbbb, (i + 1) * 50, this.boss.Y - 50, 1));
                    }
                    else
                    {
                        this.SomeNpc.Add(base.Game.CreateNpc(0xbbb, ((i + 1) * 50) + 500, this.boss.Y - 50, 1));
                    }
                    this.turnCount++;
                }
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            base.Game.AddLoadingFile(1, "bombs/51.swf", "tank.resource.bombs.Bomb51");
            base.Game.AddLoadingFile(1, "bombs/17.swf", "tank.resource.bombs.Bomb17");
            base.Game.AddLoadingFile(1, "bombs/18.swf", "tank.resource.bombs.Bomb18");
            base.Game.AddLoadingFile(1, "bombs/19.swf", "tank.resource.bombs.Bomb19");
            base.Game.AddLoadingFile(1, "bombs/67.swf", "tank.resource.bombs.Bomb67");
            int[] npcIds = new int[] { 0xbb9, 0xbbb, 0xbbc, 0xbbd };
            base.Game.LoadResources(npcIds);
            base.Game.LoadNpcGameOverResources(npcIds);
            base.Game.SetMap(0x441);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            this.boss = base.Game.CreateBoss(0xbbd, 0x7d0, 0x4b0, -1, 1);
            this.boss.SetRelateDemagemRect(-42, -200, 0x54, 0xc2);
            this.turnCount = 1;
        }

        public override int UpdateUIData()
        {
            this.preKillNum = base.Game.TotalKillCount;
            return base.Game.TotalKillCount;
        }
    }
}

