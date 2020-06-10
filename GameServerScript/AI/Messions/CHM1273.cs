using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{

    public class CHM1273 : AMissionControl
    {
        private SimpleBoss m_boss;
        private PhysicalObj m_moive;
        private PhysicalObj m_front;
        private int IsSay = 0;
        private int bossID = 0x4b3;
        private int npcID = 0x4b9;
        private static string[] KillChat = new string[] { "送你回老家！", "就凭你还妄想能够打败我？" };
        private static string[] ShootedChat = new string[] { "哎呦！很痛…", "我还顶的住…" };

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
            return (base.Game.TurnIndex > (base.Game.MissionInfo.TotalTurn - 1)) || !this.m_boss.IsLiving;
        }

        public override void DoOther()
        {
            base.DoOther();
            int index = base.Game.Random.Next(0, KillChat.Length);
            this.m_boss.Say(KillChat[index], 0, 0);
        }

        public static void msg(Living living, Living target, int damageAmount, int criticalAmount)
        {
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
            if (!this.m_boss.IsLiving)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo> {
                new LoadingFileInfo(2, "image/map/show4.jpg", "")
            };
            base.Game.SendLoadResource(loadingFileInfos);
        }

        public void OnKillPlayer(Living living, Living target, int damageAmount, int criticalAmount)
        {
            int index = base.Game.Random.Next(0, KillChat.Length);
            this.m_boss.Say(KillChat[index], 0, 0);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
            foreach (Player player in allFightPlayers)
            {
                player.AfterKilledByLiving += new KillLivingEventHanlde(this.OnKillPlayer);
            }
            this.IsSay = 0;
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            base.Game.AddLoadingFile(1, "bombs/61.swf", "tank.resource.bombs.Bomb61");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.boguoLeaderAsset");
            int[] npcIds = new int[] { this.bossID, this.npcID };
            base.Game.LoadResources(npcIds);
            base.Game.LoadNpcGameOverResources(npcIds);
            base.Game.SetMap(0x431);
            base.Game.IsBossWar = "啵咕队长";
            foreach (Player player in base.Game.GetAllPlayers())
            {
                player.AfterKillingLiving += new KillLivingEventHanlde(CHM1273.msg);
            }
        }

        public override void OnShooted()
        {
            if (this.m_boss.IsLiving && (this.IsSay == 0))
            {
                int index = base.Game.Random.Next(0, ShootedChat.Length);
                this.m_boss.Say(ShootedChat[index], 0, 0x5dc);
                this.IsSay = 1;
            }
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            this.m_moive = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 0);
            this.m_front = base.Game.Createlayer(680, 330, "font", "game.asset.living.boguoLeaderAsset", "out", 1, 0);
            this.m_boss = base.Game.CreateBoss(this.bossID, 770, -1500, -1, 1);
            this.m_boss.FallFrom(770, 0x12d, "fall", 0, 2, 0x3e8);
            this.m_boss.SetRelateDemagemRect(0x22, -35, 11, 0x12);
            this.m_boss.AddDelay(10);
            this.m_boss.Say("你们胆敢闯入我的地盘，准备受死吧！", 0, 0x1770);
            this.m_boss.PlayMovie("call", 0x170c, 0);
            this.m_moive.PlayMovie("in", 0x2328, 0);
            this.m_boss.PlayMovie("weakness", 0x2710, 0x1388);
            this.m_front.PlayMovie("in", 0x2328, 0);
            this.m_moive.PlayMovie("out", 0x3a98, 0);
            base.Game.BossCardCount = 1;
        }

        public override int UpdateUIData()
        {
            if (this.m_boss == null)
            {
                return 0;
            }
            if (!this.m_boss.IsLiving)
            {
                return 1;
            }
            return base.UpdateUIData();
        }
    }
}

