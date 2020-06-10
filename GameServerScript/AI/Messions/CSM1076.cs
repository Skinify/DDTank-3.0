using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
    public class CSM1076 : AMissionControl
    {
        private PhysicalObj m_kingMoive;
        private PhysicalObj m_kingFront;
        private SimpleBoss m_king = null;
        private SimpleBoss m_secondKing = null;
        private PhysicalObj[] m_leftWall = null;
        private PhysicalObj[] m_rightWall = null;
        private int m_kill = 0;
        private int m_state = 0x3ed;
        private int turn = 0;
        private int IsSay = 0;
        private int firstBossID = 0x3ed;
        private int secondBossID = 0x3ee;
        private int npcID = 0x3f2;
        private static string[] KillChat = new string[] { "马迪亚斯不要再控制我！", "这就是挑战我的下场！", "不！！这不是我的意愿… " };
        private static string[] ShootedChat = new string[] { "哎呀~~你们为什么要攻击我？<br/>我在干什么？", "噢~~好痛!我为什么要战斗？<br/>我必须战斗…" };

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
            if (base.Game.TurnIndex > (base.Game.MissionInfo.TotalTurn - 1))
            {
                return true;
            }
            if (!this.m_king.IsLiving && (this.m_state == this.firstBossID))
            {
                this.m_state++;
            }
            if ((this.m_state == this.secondBossID) && (this.m_secondKing == null))
            {
                this.m_kingMoive = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 0);
                this.m_secondKing = base.Game.CreateBoss(this.m_state, this.m_king.X, this.m_king.Y, this.m_king.Direction, 1);
                base.Game.RemoveLiving(this.m_king.Id);
                if (this.m_secondKing.Direction == -1)
                {
                    this.m_secondKing.SetRectBomb(0x18, -159, 0x42, 0x26);
                    this.m_secondKing.SetRelateDemagemRect(0x3a, -142, 5, 3);
                }
                else
                {
                    this.m_secondKing.SetRectBomb(-90, -159, 0x42, 0x26);
                    this.m_secondKing.SetRelateDemagemRect(-63, -142, 5, 3);
                }
                this.m_secondKing.Say("<span class='red'>你们把我激怒了，我饶不了你们！</span>", 0, 0xbb8);
                this.m_kingMoive.PlayMovie("in", 0x1388, 0);
                this.m_secondKing.PlayMovie("weakness", 0x17d4, 0);
                this.m_kingMoive.PlayMovie("out", 0x2ee0, 0);
                List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
                int delay = base.Game.FindRandomPlayer().Delay;
                foreach (Player player in allFightPlayers)
                {
                    if (player.Delay < delay)
                    {
                        delay = player.Delay;
                    }
                }
                this.m_secondKing.AddDelay(delay - 0x7d0);
                this.turn = base.Game.TurnIndex;
            }
            if ((this.m_secondKing != null) && !this.m_secondKing.IsLiving)
            {
                this.m_leftWall = base.Game.FindPhysicalObjByName("wallLeft", false);
                this.m_rightWall = base.Game.FindPhysicalObjByName("wallRight", false);
                if (this.m_leftWall != null)
                {
                    base.Game.RemovePhysicalObj(this.m_leftWall[0], true);
                }
                if (this.m_rightWall != null)
                {
                    base.Game.RemovePhysicalObj(this.m_rightWall[0], true);
                }
                this.m_kill++;
                return true;
            }
            return false;
        }

        public override void DoOther()
        {
            int num;
            base.DoOther();
            if (this.m_king.IsLiving)
            {
                num = base.Game.Random.Next(0, KillChat.Length);
                this.m_king.Say(KillChat[num], 0, 0);
            }
            else
            {
                num = base.Game.Random.Next(0, KillChat.Length);
                this.m_king.Say(KillChat[num], 0, 0);
            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            this.IsSay = 0;
            if (base.Game.TurnIndex > (this.turn + 1))
            {
                if (this.m_kingMoive != null)
                {
                    base.Game.RemovePhysicalObj(this.m_kingMoive, true);
                    this.m_kingMoive = null;
                }
                if (this.m_kingFront != null)
                {
                    base.Game.RemovePhysicalObj(this.m_kingFront, true);
                    this.m_kingFront = null;
                }
            }
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (!((this.m_state != 6) || this.m_secondKing.IsLiving))
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo> {
                new LoadingFileInfo(2, "image/map/show7.jpg", "")
            };
            base.Game.SendLoadResource(loadingFileInfos);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            base.Game.AddLoadingFile(1, "bombs/61.swf", "tank.resource.bombs.Bomb61");
            base.Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_left");
            base.Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_right");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.boguoLeaderAsset");
            int[] npcIds = new int[] { this.firstBossID, this.secondBossID, this.npcID };
            base.Game.LoadResources(npcIds);
            int[] numArray2 = new int[] { this.firstBossID, this.npcID };
            base.Game.LoadNpcGameOverResources(numArray2);
            base.Game.SetMap(0x434);
            base.Game.IsBossWar = "啵咕国王";
        }

        public override void OnShooted()
        {
            if (this.IsSay == 0)
            {
                int num;
                if (this.m_king.IsLiving)
                {
                    num = base.Game.Random.Next(0, ShootedChat.Length);
                    this.m_king.Say(ShootedChat[num], 0, 0x5dc);
                }
                else
                {
                    num = base.Game.Random.Next(0, ShootedChat.Length);
                    this.m_secondKing.Say(ShootedChat[num], 0, 0x5dc);
                }
                this.IsSay = 1;
            }
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            this.m_kingMoive = base.Game.Createlayer(0, 0, "kingmoive", "game.asset.living.BossBgAsset", "out", 1, 0);
            this.m_kingFront = base.Game.Createlayer(610, 380, "font", "game.asset.living.boguoKingAsset", "out", 1, 0);
            this.m_king = base.Game.CreateBoss(this.m_state, 750, 0x29c, -1, 0);
            this.m_king.FallFrom(750, 640, "fall", 0, 2, 0x3e8);
            this.m_king.SetRelateDemagemRect(-21, -79, 0x48, 0x33);
            this.m_king.AddDelay(10);
            this.m_king.Say("你们这些低等的庶民，竟敢来到我的王国放肆！", 0, 0xbb8);
            this.m_kingMoive.PlayMovie("in", 0x2328, 0);
            this.m_kingFront.PlayMovie("in", 0x2328, 0);
            this.m_kingMoive.PlayMovie("out", 0x32c8, 0);
            this.m_kingFront.PlayMovie("out", 0x3458, 0);
            this.turn = base.Game.TurnIndex;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return this.m_kill;
        }
    }
}

