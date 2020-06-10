using Bussiness.Managers;
using SqlDataProvider.Data;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
    public class NTM1083 : AMissionControl
    {
        private int mapId = 0x45e;
        private int indexOf = 0;
        private bool isDander = false;
        private int redNpcID = 0xc9;
        private int blueNpcID = 0xca;
        private int totalNpcCount = 5;
        private bool isPlayMovie = false;
        private bool isLoadItems = false;
        private PhysicalObj tip = null;
        private List<SimpleNpc> simpleNpcList = new List<SimpleNpc>();

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
            foreach (SimpleNpc npc in this.simpleNpcList)
            {
                if (npc.IsLiving)
                {
                    return false;
                }
            }
            return this.simpleNpcList.Count == this.totalNpcCount;
        }

        private void CreateNpc()
        {
            int[,] numArray = new int[,] { { 0x217, 0x229 }, { 0x27b, 0x22a }, { 0x2df, 0x229 }, { 0x343, 0x227 } };
            for (int i = 0; i < 4; i++)
            {
                this.simpleNpcList.Add(base.Game.CreateNpc(this.redNpcID, numArray[i, 0], numArray[i, 1], 1));
            }
            this.simpleNpcList.Add(base.Game.CreateNpc(this.blueNpcID, 0x2ad, 0x229, 1));
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            foreach (Player player in base.Game.GetAllFightPlayers())
            {
                player.CanGetProp = true;
            }
            if (base.Game.GetLivedLivings().Count == 0)
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
            List<SqlDataProvider.Data.ItemInfo> list = new List<SqlDataProvider.Data.ItemInfo>();
            List<ItemTemplateInfo> list2 = new List<ItemTemplateInfo>();
            if (base.Game.CurrentPlayer.Delay < base.Game.PveGameDelay)
            {
                if (this.tip.CurrentAction == "Empty")
                {
                    this.tip.PlayMovie("tip1", 0, 0xbb8);
                }
                if ((base.Game.TotalKillCount >= 1) && (this.indexOf < 1))
                {
                    this.isLoadItems = true;
                    this.tip.PlayMovie("tip2", 0, 0x7d0);
                    this.indexOf++;
                }
                if (this.isPlayMovie)
                {
                    bool flag = false;
                    this.tip.PlayMovie("tip3", 0, 0x7d0);
                    foreach (Player player in base.Game.GetAllFightPlayers())
                    {
                        if (player.Dander < 200)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        this.tip.PlayMovie("tip4", 0, 0x7d0);
                    }
                }
                if (this.isLoadItems)
                {
                    list2.Add(ItemMgr.FindItemTemplate(0x2711));
                    list2.Add(ItemMgr.FindItemTemplate(0x2713));
                    list2.Add(ItemMgr.FindItemTemplate(0x2722));
                    foreach (ItemTemplateInfo info in list2)
                    {
                        list.Add(SqlDataProvider.Data.ItemInfo.CreateFromTemplate(info, 1, 0x65));
                    }
                    foreach (Player player2 in base.Game.GetAllFightPlayers())
                    {
                        player2.CanGetProp = false;
                        player2.PlayerDetail.ClearFightBag();
                        foreach (SqlDataProvider.Data.ItemInfo info2 in list)
                        {
                            player2.PlayerDetail.AddTemplate(info2, eBageType.FightBag, info2.Count);
                        }
                        if (!this.isDander)
                        {
                            player2.Dander = 200;
                            this.isPlayMovie = true;
                            this.isDander = true;
                        }
                    }
                    this.isLoadItems = false;
                }
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] npcIds = new int[] { this.redNpcID, this.blueNpcID };
            base.Game.LoadResources(npcIds);
            base.Game.LoadNpcGameOverResources(npcIds);
            base.Game.AddLoadingFile(2, "image/map/1118/object/Asset.swf", "com.map.trainer.TankTrainerAssetII");
            base.Game.SetMap(this.mapId);
        }

        public override void OnStartGame()
        {
            this.CreateNpc();
            this.tip = base.Game.CreateTip(390, 120, "firstFront", "com.map.trainer.TankTrainerAssetII", "Empty", 1, 1);
        }

        public override int UpdateUIData()
        {
            return base.Game.TotalKillCount;
        }
            
    }
}