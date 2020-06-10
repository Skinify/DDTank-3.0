using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{

    public class CHM1272 : AMissionControl
    {
        private List<SimpleNpc> redNpc = new List<SimpleNpc>();
        private List<SimpleNpc> blueNpc = new List<SimpleNpc>();
        private int redCount = 0;
        private int blueCount = 0;
        private int redTotalCount = 0;
        private int blueTotalCount = 0;
        private int dieRedCount = 0;
        private int dieBlueCount = 0;
        private int redNpcID = 0x4b1;
        private int blueNpcID = 0x4b2;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 930)
            {
                return 3;
            }
            if (score > 850)
            {
                return 2;
            }
            if (score > 0x307)
            {
                return 1;
            }
            return 0;
        }

        public override bool CanGameOver()
        {
            bool flag = true;
            this.dieRedCount = 0;
            this.dieBlueCount = 0;
            foreach (SimpleNpc npc in this.redNpc)
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
            foreach (SimpleNpc npc2 in this.blueNpc)
            {
                if (npc2.IsLiving)
                {
                    flag = false;
                }
                else
                {
                    this.dieBlueCount++;
                }
            }
            if (flag && (this.redTotalCount == 20) && (this.blueTotalCount == 5))
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
                new LoadingFileInfo(2, "image/map/3", "")
            };
            base.Game.SendLoadResource(loadingFileInfos);
        }

        public override void OnNewTurnStarted()
        {
            this.redCount = this.redTotalCount - this.dieRedCount;
            this.blueCount = this.blueTotalCount - this.dieBlueCount;
            if (base.Game.GetLivedLivings().Count == 0)
            {
                base.Game.PveGameDelay = 0;
            }
            if ((base.Game.TurnIndex > 1) && (base.Game.CurrentPlayer.Delay > base.Game.PveGameDelay) && ((this.blueCount != 3) || (this.redCount != 12)))
            {
                int num;
                if ((this.redTotalCount < 12) && (this.blueTotalCount < 3))
                {
                    for (num = 0; num < 4; num++)
                    {
                        this.redTotalCount++;
                        if (num < 1)
                        {
                            this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 900 + ((num + 1) * 100), 0x1f9, 1));
                        }
                        else if (num < 3)
                        {
                            this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 920 + ((num + 1) * 100), 0x1f9, 1));
                        }
                        else
                        {
                            this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 0x3e8 + ((num + 1) * 100), 0x203, 1));
                        }
                    }
                    this.blueTotalCount++;
                    this.blueNpc.Add(base.Game.CreateNpc(this.blueNpcID, 0x5bb, 0x1ef, 1));
                }
                else if (this.redCount < 12)
                {
                    if ((12 - this.redCount) >= 4)
                    {
                        for (num = 0; num < 4; num++)
                        {
                            if ((this.redTotalCount < 20) && (this.redCount != 12))
                            {
                                this.redTotalCount++;
                                if (num < 1)
                                {
                                    this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 900 + ((num + 1) * 100), 0x1f9, 1));
                                }
                                else if (num < 3)
                                {
                                    this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 920 + ((num + 1) * 100), 0x1f9, 1));
                                }
                                else
                                {
                                    this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 0x3e8 + ((num + 1) * 100), 0x203, 1));
                                }
                            }
                        }
                    }
                    else if ((12 - this.redCount) > 0)
                    {
                        for (num = 0; num < (12 - this.redCount); num++)
                        {
                            if ((this.redTotalCount < 20) && (this.redCount != 12))
                            {
                                this.redTotalCount++;
                                if (num < 1)
                                {
                                    this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 900 + ((num + 1) * 100), 0x1f9, 1));
                                }
                                else if (num < 3)
                                {
                                    this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 920 + ((num + 1) * 100), 0x1f9, 1));
                                }
                                else
                                {
                                    this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 0x3e8 + ((num + 1) * 100), 0x203, 1));
                                }
                            }
                        }
                    }
                    if ((this.blueCount < 3) && (this.blueTotalCount < 5))
                    {
                        this.blueTotalCount++;
                        this.blueNpc.Add(base.Game.CreateNpc(this.blueNpcID, 0x5bb, 0x1ef, 1));
                    }
                }
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] npcIds = new int[] { this.redNpcID, this.blueNpcID };
            base.Game.LoadResources(npcIds);
            base.Game.LoadNpcGameOverResources(npcIds);
            base.Game.SetMap(0x430);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            for (int i = 0; i < 4; i++)
            {
                this.redTotalCount++;
                if (i < 1)
                {
                    this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 900 + ((i + 1) * 100), 0x1f9, 1));
                }
                else if (i < 3)
                {
                    this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 920 + ((i + 1) * 100), 0x1f9, 1));
                }
                else
                {
                    this.redNpc.Add(base.Game.CreateNpc(this.redNpcID, 0x3e8 + ((i + 1) * 100), 0x203, 1));
                }
            }
            this.blueTotalCount++;
            this.blueNpc.Add(base.Game.CreateNpc(this.blueNpcID, 0x5bb, 0x1ef, 1));
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return base.Game.TotalKillCount;
        }
    }
}

