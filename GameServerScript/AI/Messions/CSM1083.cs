using Bussiness.Managers;
using SqlDataProvider.Data;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
    public class CSM1083 : AMissionControl
    {
        private List<SimpleNpc> SomeNpc = new List<SimpleNpc>();
        private PhysicalObj Tip = null;
        private bool result = false;
        private int killCount = 0;
        private int preKillNum = 0;
        private bool canPlayMovie = false;

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
            List<SqlDataProvider.Data.ItemInfo> list2 = new List<SqlDataProvider.Data.ItemInfo>();
            using (List<Player>.Enumerator enumerator = base.Game.GetAllFightPlayers().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    foreach (SimpleNpc npc in base.Game.GetLivedLivings())
                    {
                        Player player = base.Game.CurrentPlayer;
                        if (npc.Distance(player.X, player.Y) <= 100.0)
                        {
                            this.canPlayMovie = true;
                        }
                    }
                }
            }
            if ((base.Game.TurnIndex > 1) && (base.Game.CurrentPlayer.Delay > base.Game.PveGameDelay))
            {
                for (int i = 0; i < 5; i++)
                {
                    if (this.SomeNpc.Count < 15)
                    {
                        this.SomeNpc.Add(base.Game.CreateNpc(0xc9, (i + 1) * 100, 500, 1));
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (base.Game.CurrentPlayer.Delay < base.Game.PveGameDelay)
            {
                if (this.Tip.CurrentAction == "Empty")
                {
                    this.Tip.PlayMovie("tip1", 0, 0xbb8);
                }
                if ((this.preKillNum < base.Game.TotalKillCount) && (this.killCount < 2))
                {
                    this.killCount++;
                }
                if (this.killCount == 2)
                {
                    this.Tip.PlayMovie("tip2", 0, 0x7d0);
                }
                if (this.canPlayMovie)
                {
                    this.Tip.PlayMovie("tip3", 0, 0x7d0);
                }
                list.Add(ItemMgr.FindItemTemplate(0x2711));
                list.Add(ItemMgr.FindItemTemplate(0x2713));
                list.Add(ItemMgr.FindItemTemplate(0x2722));
                foreach (ItemTemplateInfo info in list)
                {
                    list2.Add(SqlDataProvider.Data.ItemInfo.CreateFromTemplate(info, 1, 0x65));
                }
                foreach (Player player in base.Game.GetAllFightPlayers())
                {
                    player.CanGetProp = false;
                    player.PlayerDetail.ClearFightBag();
                    foreach (SqlDataProvider.Data.ItemInfo info2 in list2)
                    {
                        player.PlayerDetail.AddTemplate(info2, eBageType.FightBag, info2.Count);
                    }
                }
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            base.Game.AddLoadingFile(2, "image/map/1086/object/Asset.swf", "com.map.trainer.TankTrainerAssetII");
            int[] npcIds = new int[] { 1, 2 };
            base.Game.LoadResources(npcIds);
            base.Game.LoadNpcGameOverResources(npcIds);
            base.Game.SetMap(0x43e);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            for (int i = 0; i < 4; i++)
            {
                this.SomeNpc.Add(base.Game.CreateNpc(0xc9, (i + 1) * 100, 500, 1));
            }
            this.SomeNpc.Add(base.Game.CreateNpc(0xca, 500, 500, 1));
            this.Tip = base.Game.CreateTip(390, 120, "firstFront", "com.map.trainer.TankTrainerAssetII", "Empty", 1, 0);
        }

        public override int UpdateUIData()
        {
            this.preKillNum = base.Game.TotalKillCount;
            return base.Game.TotalKillCount;
        }
    }
}

