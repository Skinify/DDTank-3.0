using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    public class CNM1177 : AMissionControl
    {
        private SimpleBoss m_king = null;
        private int m_kill = 0;
        private int IsSay = 0;
        private int bossID = 0x453;
        private int npcID = 0x450;
        private static string[] KillChat = new string[] { "灭亡是你唯一的归宿！", "太不堪一击了！" };
        private static string[] ShootedChat = new string[] { "哎哟～你打的我好疼啊！<br/>啊哈哈哈哈！", "你们就只有这点本事？！", "哼～有点意思了" };

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
            if (!this.m_king.IsLiving)
            {
                this.m_kill++;
                return true;
            }
            return base.Game.TurnIndex > (base.Game.MissionInfo.TotalTurn - 1);
        }

        public override void DoOther()
        {
            base.DoOther();
            int index = base.Game.Random.Next(0, KillChat.Length);
            this.m_king.Say(KillChat[index], 0, 0);
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            this.IsSay = 0;
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
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] npcIds = new int[] { this.npcID, this.bossID };
            base.Game.LoadResources(npcIds);
            base.Game.LoadNpcGameOverResources(npcIds);
            base.Game.SetMap(0x434);
            base.Game.IsBossWar = "炸弹人王";
        }

        public override void OnShooted()
        {
            if (this.m_king.IsLiving && (this.IsSay == 0))
            {
                int index = base.Game.Random.Next(0, ShootedChat.Length);
                this.m_king.Say(ShootedChat[index], 0, 0x5dc);
                this.IsSay = 1;
            }
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            this.m_king = base.Game.CreateBoss(this.bossID, 750, 510, -1, 0);
            this.m_king.SetRelateDemagemRect(-41, -187, 0x53, 140);
            this.m_king.Say("你们知道的太多了，我不能让你们继续活着！", 0, 0xbb8);
            this.m_king.AddDelay(0x10);
        }

        public override int UpdateUIData() { 
            return this.m_kill;
        }
            
    }
}

