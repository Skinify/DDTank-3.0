using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{

    public class CHM1271 : AMissionControl
    {
        private List<SimpleNpc> SomeNpc = new List<SimpleNpc>();
        private int redTotalCount = 0;
        private int dieRedCount = 0;
        private int redNpcID = 0x4b1;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 600)
            {
                return 3;
            }
            if (score > 520)
            {
                return 2;
            }
            if (score > 450)
            {
                return 1;
            }
            return 0;
        }

        public override bool CanGameOver()
        {
            bool flag = true;
            base.CanGameOver();
            this.dieRedCount = 0;
            foreach (SimpleNpc npc in this.SomeNpc)
            {
                if (npc.IsLiving)
                {
                    flag = false;
                }
                else
                {
                    this.dieRedCount++;
                }
            }
            if (flag && (this.dieRedCount == 15))
            {
                base.Game.IsWin = true;
                return true;
            }
            return base.Game.TurnIndex > (base.Game.MissionInfo.TotalTurn - 1);
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (base.Game.GetLivedLivings().Count == 0)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo> {
                new LoadingFileInfo(2, "image/map/2", "")
            };
            base.Game.SendLoadResource(loadingFileInfos);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            if (base.Game.GetLivedLivings().Count == 0)
            {
                base.Game.PveGameDelay = 0;
            }
            if ((base.Game.TurnIndex > 1) && (base.Game.CurrentPlayer.Delay > base.Game.PveGameDelay))
            {
                for (int i = 0; i < 4; i++)
                {
                    if (this.redTotalCount < 15)
                    {
                        this.redTotalCount++;
                        if (i < 1)
                        {
                            this.SomeNpc.Add(base.Game.CreateNpc(this.redNpcID, 900 + ((i + 1) * 100), 0x1f9, 1));
                        }
                        else if (i < 3)
                        {
                            this.SomeNpc.Add(base.Game.CreateNpc(this.redNpcID, 920 + ((i + 1) * 100), 0x1f9, 1));
                        }
                        else
                        {
                            this.SomeNpc.Add(base.Game.CreateNpc(this.redNpcID, 0x3e8 + ((i + 1) * 100), 0x203, 1));
                        }
                    }
                }
                if (this.redTotalCount < 15)
                {
                    this.redTotalCount++;
                    this.SomeNpc.Add(base.Game.CreateNpc(this.redNpcID, 0x5bb, 0x1ef, 1));
                }
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] npcIds = new int[] { this.redNpcID };
            base.Game.LoadResources(npcIds);
            base.Game.LoadNpcGameOverResources(npcIds);
            base.Game.SetMap(0x430);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            if (base.Game.GetLivedLivings().Count == 0)
            {
                base.Game.PveGameDelay = 0;
            }
            for (int i = 0; i < 4; i++)
            {
                this.redTotalCount++;
                if (i < 1)
                {
                    this.SomeNpc.Add(base.Game.CreateNpc(this.redNpcID, 900 + ((i + 1) * 100), 0x1f9, 1));
                }
                else if (i < 3)
                {
                    this.SomeNpc.Add(base.Game.CreateNpc(this.redNpcID, 920 + ((i + 1) * 100), 0x1f9, 1));
                }
                else
                {
                    this.SomeNpc.Add(base.Game.CreateNpc(this.redNpcID, 0x3e8 + ((i + 1) * 100), 0x203, 1));
                }
            }
            this.redTotalCount++;
            this.SomeNpc.Add(base.Game.CreateNpc(this.redNpcID, 0x5bb, 0x1ef, 1));
        }

        public override int UpdateUIData() {
            return base.Game.TotalKillCount;
        }
           
    }
}

