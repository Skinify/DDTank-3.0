using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;
using System.Drawing;

namespace GameServerScript.AI.Messions
{
    public class ExplorationMission : AMissionControl
    {
        public Dictionary<int, int> ballIds;
        public int[] remoteIds;
        public int[] livingIds;
        private int remoteCount = 0;
        private int livingCount = 0;
        private int currentTotalLivings = 0;
        private int currentTotalRemotIds = 0;
        private int turnCreateRemoteNum = 1;
        private int turnCreateLivingNum = 4;
        private int samePingMaxRemoteNum = 0;
        private int samePingMaxLivingNum = 0;
        public NpcCreateParam npcCreateParamSimple = null;
        public NpcCreateParam npcCreateParamNormal = null;
        public NpcCreateParam npcCreateParamHard = null;
        public NpcCreateParam npcCreateParamTerror = null;
        public List<Living> livings = new List<Living>();
        public List<SimpleBoss> remoteNpc = new List<SimpleBoss>();

        public override int CalculateScoreGrade(int score)
        {
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
            if (base.Game.TurnIndex > 0x63)
            {
                return true;
            }
            foreach (SimpleBoss boss in this.remoteNpc)
            {
                if (boss.IsLiving)
                {
                    flag = false;
                    break;
                }
            }
            foreach (Living living in this.livings)
            {
                if (living.IsLiving)
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                base.Game.IsWin = true;
            }
            return flag;
        }

        private void CreateLiving()
        {
            if (this.livingIds.Length != 0)
            {
                int count = base.Game.GetLivedLivings().Count;
                if ((this.livingCount > 0) && (count < this.samePingMaxLivingNum) && (this.currentTotalLivings < this.livingCount))
                {
                    for (int i = 0; i < this.turnCreateLivingNum; i++)
                    {
                        if ((this.currentTotalLivings >= this.livingCount) || (count >= this.samePingMaxLivingNum))
                        {
                            break;
                        }
                        Point npcBornPos = this.GetNpcBornPos();
                        int randomNpcId = this.GetRandomNpcId(this.livingIds);
                        this.livings.Add(base.Game.CreateNpc(randomNpcId, npcBornPos.X, npcBornPos.Y, 0));
                        this.currentTotalLivings++;
                        count = base.Game.GetLivedLivings().Count;
                    }
                }
            }
        }

        private void CreateRemote()
        {
            if (this.remoteIds.Length != 0)
            {
                int livedRemoteCount = this.GetLivedRemoteCount();
                if ((this.remoteCount > 0) && (livedRemoteCount < this.samePingMaxRemoteNum) && (this.currentTotalRemotIds < this.remoteCount))
                {
                    for (int i = 0; i < this.turnCreateRemoteNum; i++)
                    {
                        if ((this.currentTotalRemotIds >= this.remoteCount) || (livedRemoteCount >= this.samePingMaxRemoteNum))
                        {
                            break;
                        }
                        Point npcBornPos = this.GetNpcBornPos();
                        int randomNpcId = this.GetRandomNpcId(this.remoteIds);
                        this.remoteNpc.Add(base.Game.CreateBoss(randomNpcId, npcBornPos.X, npcBornPos.Y, -1, 0));
                        this.remoteNpc[this.currentTotalRemotIds].NpcInfo.CurrentBallId = this.ballIds[randomNpcId];
                        this.currentTotalRemotIds++;
                        livedRemoteCount = this.GetLivedRemoteCount();
                    }
                }
            }
        }

        private int GetLivedRemoteCount()
        {
            int num = 0;
            foreach (TurnedLiving living in base.Game.TurnQueue)
            {
                if ((living is SimpleBoss) && living.IsLiving)
                {
                    num++;
                }
            }
            return num;
        }

        public int GetMapId(int[] mapIds, int defaultMapId)
        {
            for (int i = 0; i < 100; i++)
            {
                int index = base.Game.Random.Next(0, mapIds.Length);
                int item = mapIds[index];
                if (!base.Game.MapHistoryIds.Contains(item))
                {
                    base.Game.MapHistoryIds.Add(item);
                    return item;
                }
            }
            return defaultMapId;
        }

        private Point GetNpcBornPos()
        {
            List<Point> list = base.Game.MapPos.PosX1;
            int num = base.Game.Random.Next(list.Count);
            return list[num];
        }

        private int GetRandomNpcId(int[] list)
        {
            int index = base.Game.Random.Next(0, list.Length);
            return list[index];
        }

        public override void OnBeginNewTurn()
        {
        }

        public override void OnGameOver()
        {
        }

        public override void OnNewTurnStarted()
        {
            int num = this.remoteCount + this.livingCount;
            int num2 = this.currentTotalRemotIds + this.currentTotalLivings;
            if ((base.Game.TurnIndex >= 2) && (num2 < num))
            {
                this.CreateLiving();
                this.CreateRemote();
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            switch (base.Game.HandLevel)
            {
                case eHardLevel.Simple:
                    this.remoteCount = this.npcCreateParamSimple.RemoteCount;
                    this.livingCount = this.npcCreateParamSimple.LivingCount;
                    this.turnCreateRemoteNum = this.npcCreateParamSimple.TurnCreateRemoteNum;
                    this.turnCreateLivingNum = this.npcCreateParamSimple.TurnCreateLivingNum;
                    this.samePingMaxRemoteNum = this.npcCreateParamSimple.SamePingMaxRemoteNum;
                    this.samePingMaxLivingNum = this.npcCreateParamSimple.SamePingMaxLivingNum;
                    break;

                case eHardLevel.Normal:
                    this.remoteCount = this.npcCreateParamNormal.RemoteCount;
                    this.livingCount = this.npcCreateParamNormal.LivingCount;
                    this.turnCreateRemoteNum = this.npcCreateParamNormal.TurnCreateRemoteNum;
                    this.turnCreateLivingNum = this.npcCreateParamNormal.TurnCreateLivingNum;
                    this.samePingMaxRemoteNum = this.npcCreateParamNormal.SamePingMaxRemoteNum;
                    this.samePingMaxLivingNum = this.npcCreateParamNormal.SamePingMaxLivingNum;
                    break;

                case eHardLevel.Hard:
                    this.remoteCount = this.npcCreateParamHard.RemoteCount;
                    this.livingCount = this.npcCreateParamHard.LivingCount;
                    this.turnCreateRemoteNum = this.npcCreateParamHard.TurnCreateRemoteNum;
                    this.turnCreateLivingNum = this.npcCreateParamHard.TurnCreateLivingNum;
                    this.samePingMaxRemoteNum = this.npcCreateParamHard.SamePingMaxRemoteNum;
                    this.samePingMaxLivingNum = this.npcCreateParamHard.SamePingMaxLivingNum;
                    break;

                case eHardLevel.Terror:
                    this.remoteCount = this.npcCreateParamTerror.RemoteCount;
                    this.livingCount = this.npcCreateParamTerror.LivingCount;
                    this.turnCreateRemoteNum = this.npcCreateParamTerror.TurnCreateRemoteNum;
                    this.turnCreateLivingNum = this.npcCreateParamTerror.TurnCreateLivingNum;
                    this.samePingMaxRemoteNum = this.npcCreateParamTerror.SamePingMaxRemoteNum;
                    this.samePingMaxLivingNum = this.npcCreateParamTerror.SamePingMaxLivingNum;
                    break;

                default:
                    this.remoteCount = this.npcCreateParamSimple.RemoteCount;
                    this.livingCount = this.npcCreateParamSimple.LivingCount;
                    this.turnCreateRemoteNum = this.npcCreateParamSimple.TurnCreateRemoteNum;
                    this.turnCreateLivingNum = this.npcCreateParamSimple.TurnCreateLivingNum;
                    this.samePingMaxRemoteNum = this.npcCreateParamSimple.SamePingMaxRemoteNum;
                    this.samePingMaxLivingNum = this.npcCreateParamSimple.SamePingMaxLivingNum;
                    break;
            }
        }

        public override void OnStartGame()
        {
            this.CreateLiving();
            this.CreateRemote();
            base.Game.TotalCount = this.livingCount + this.remoteCount;
            base.Game.SendMissionInfo();
        }

        public override int UpdateUIData()
        {
            return base.Game.TotalKillCount; 
        }
            
    }
}

