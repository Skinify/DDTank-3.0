using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
    public class DCSM2001 : AMissionControl
    {
        private List<SimpleNpc> someNpc = new List<SimpleNpc>();
        private int dieRedCount = 0;
        private int[] npcIDs = new int[] { 0x7d1, 0x7d2 };
        private int[] birthX = new int[] { 0x34, 0x73, 0xb7, 0xfd, 320, 0x4b6, 0x4fb, 0x53e, 0x582, 0x5c3 };

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 0x74e)
            {
                return 3;
            }
            if (score > 0x721)
            {
                return 2;
            }
            if (score > 0x6f4)
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
            foreach (SimpleNpc npc in this.someNpc)
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
            if (flag && (this.dieRedCount == base.Game.MissionInfo.TotalCount))
            {
                base.Game.IsWin = true;
                return true;
            }
            return false;
        }

        protected int GetNpcCountByID(int Id)
        {
            int num = 0;
            foreach (SimpleNpc npc in this.someNpc)
            {
                if (npc.NpcInfo.ID == Id)
                {
                    num++;
                }
            }
            return num;
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
                new LoadingFileInfo(2, "image/map/2/show2", "")
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
            if ((base.Game.TurnIndex > 1) && (base.Game.CurrentPlayer.Delay > base.Game.PveGameDelay) && (base.Game.GetLivedLivings().Count < 10))
            {
                for (int i = 0; i < (10 - base.Game.GetLivedLivings().Count); i++)
                {
                    if (this.someNpc.Count == base.Game.MissionInfo.TotalCount)
                    {
                        break;
                    }
                    int index = base.Game.Random.Next(0, this.birthX.Length);
                    int x = this.birthX[index];
                    if (x <= 320)
                    {
                    }
                    if ((base.Game.Random.Next(0, this.npcIDs.Length) == 1) && (this.GetNpcCountByID(this.npcIDs[1]) < 10))
                    {
                        this.someNpc.Add(base.Game.CreateNpc(this.npcIDs[1], x, 0x1fa, 1));
                    }
                    else
                    {
                        this.someNpc.Add(base.Game.CreateNpc(this.npcIDs[0], x, 0x1fa, 1));
                    }
                }
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] npcIds = new int[] { this.npcIDs[0], this.npcIDs[1] };
            int[] numArray2 = new int[] { this.npcIDs[1], this.npcIDs[0], this.npcIDs[0], this.npcIDs[0] };
            base.Game.LoadResources(npcIds);
            base.Game.LoadNpcGameOverResources(numArray2);
            base.Game.SetMap(0x460);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            int index = base.Game.Random.Next(0, this.npcIDs.Length);
            this.someNpc.Add(base.Game.CreateNpc(this.npcIDs[index], 0x34, 0xce, 1));
            index = base.Game.Random.Next(0, this.npcIDs.Length);
            this.someNpc.Add(base.Game.CreateNpc(this.npcIDs[index], 100, 0xcf, 1));
            index = base.Game.Random.Next(0, this.npcIDs.Length);
            this.someNpc.Add(base.Game.CreateNpc(this.npcIDs[index], 0x9b, 0xd0, 1));
            index = base.Game.Random.Next(0, this.npcIDs.Length);
            this.someNpc.Add(base.Game.CreateNpc(this.npcIDs[index], 210, 0xcf, 1));
            index = base.Game.Random.Next(0, this.npcIDs.Length);
            this.someNpc.Add(base.Game.CreateNpc(this.npcIDs[index], 0xfd, 0xcf, 1));
            index = base.Game.Random.Next(0, this.npcIDs.Length);
            this.someNpc.Add(base.Game.CreateNpc(this.npcIDs[index], 0x4fb, 0xd0, 1));
            index = base.Game.Random.Next(0, this.npcIDs.Length);
            this.someNpc.Add(base.Game.CreateNpc(this.npcIDs[index], 0x52d, 0xce, 1));
            index = base.Game.Random.Next(0, this.npcIDs.Length);
            this.someNpc.Add(base.Game.CreateNpc(this.npcIDs[index], 0x550, 0xd0, 1));
            index = base.Game.Random.Next(0, this.npcIDs.Length);
            this.someNpc.Add(base.Game.CreateNpc(this.npcIDs[index], 0x582, 0xce, 1));
            index = base.Game.Random.Next(0, this.npcIDs.Length);
            this.someNpc.Add(base.Game.CreateNpc(this.npcIDs[index], 0x5c3, 0xd0, 1));
        }

        public override int UpdateUIData()
        {
            return base.Game.TotalKillCount;
        }
            
    }
}

