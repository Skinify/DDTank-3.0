using Game.Logic;
using Game.Logic.Actions;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{


    public class CHM1275 : AMissionControl
    {
        private List<PhysicalObj> m_bord = new List<PhysicalObj>();
        private List<PhysicalObj> m_key = new List<PhysicalObj>();
        private PhysicalObj m_door = null;
        private string KeyIndex = null;
        private int m_count = 0;

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
            for (int i = 0; i < 12; i++)
            {
                foreach (Player player in base.Game.GetAllFightPlayers())
                {
                    if ((player.X > (this.m_bord[i].X - 40)) && (player.X < (this.m_bord[i].X + 40)) && (player.Y < this.m_bord[i].Y) && (player.Y > (this.m_bord[i].Y - 40)) && (this.m_bord[i].CurrentAction == "2"))
                    {
                        this.m_bord[i].PlayMovie("3", 0, 0);
                        this.KeyIndex = "Key" + i; 
                        base.Game.RemovePhysicalObj(base.Game.FindPhysicalObjByName(this.KeyIndex)[0], true);
                        this.m_count++;
                    }
                }
            }
            if (this.m_count == base.Game.TotalCount)
            {
                base.Game.SendGameObjectFocus(2, "door", 0, 0x1770);
                base.Game.SendPlaySound("201");
                this.m_door.PlayMovie("end", 0xfa0, 0xbb8);
                base.Game.AddAction(new PlaySoundAction("202", 0x2710));
                base.Game.SendUpdateUiData();
                base.Game.TurnQueue.Clear();
            }
            return ((base.Game.TurnIndex > (base.Game.TotalTurn - 1)) && (this.m_count != base.Game.TotalCount)) || (this.m_door.CurrentAction == "end");
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (base.Game.CurrentLiving != null)
            {
                ((Player) base.Game.CurrentLiving).SetBall(3);
            }
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (this.m_door.CurrentAction == "end")
            {
                foreach (Player player in base.Game.GetAllFightPlayers())
                {
                    SealEffect ofType = (SealEffect) player.EffectList.GetOfType(eEffectType.SealEffect);
                    if (ofType != null)
                    {
                        ofType.Stop();
                    }
                }
                base.Game.AddAllPlayerToTurn();
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo> {
                new LoadingFileInfo(2, "image/map/show6.jpg", "")
            };
            base.Game.SendLoadResource(loadingFileInfos);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            if (base.Game.CurrentLiving != null)
            {
                ((Player) base.Game.CurrentLiving).Seal((Player) base.Game.CurrentLiving, 0, 0);
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            base.Game.AddLoadingFile(2, "image/map/1075/objects/1075Object.swf", "game.crazytank.assetmap.Board001");
            base.Game.AddLoadingFile(2, "image/map/1075/objects/1075Object.swf", "game.crazytank.assetmap.CrystalDoor001");
            base.Game.AddLoadingFile(2, "image/map/1075/objects/1075Object.swf", "game.crazytank.assetmap.Key");
            base.Game.SetMap(0x433);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            base.Game.TotalCount = base.Game.PlayerCount;
            base.Game.TotalTurn = base.Game.PlayerCount * 6;
            base.Game.SendMissionInfo();
            this.m_bord.Add(base.Game.CreatePhysicalObj(0x4c, 0xa7, "board1", "game.crazytank.assetmap.Board001", "1", 1, 0x150));
            this.m_bord.Add(base.Game.CreatePhysicalObj(0x192, 0x9f, "board2", "game.crazytank.assetmap.Board001", "1", 1, 0x17));
            this.m_bord.Add(base.Game.CreatePhysicalObj(0x2bb, 0x9c, "board3", "game.crazytank.assetmap.Board001", "1", 1, 350));
            this.m_bord.Add(base.Game.CreatePhysicalObj(0x3bf, 0x94, "board4", "game.crazytank.assetmap.Board001", "1", 1, 0x145));
            this.m_bord.Add(base.Game.CreatePhysicalObj(0xb1, 0x105, "board5", "game.crazytank.assetmap.Board001", "1", 1, 0x16));
            this.m_bord.Add(base.Game.CreatePhysicalObj(0x202, 0x115, "board6", "game.crazytank.assetmap.Board001", "1", 1, 0x150));
            this.m_bord.Add(base.Game.CreatePhysicalObj(0x30e, 0x11d, "board7", "game.crazytank.assetmap.Board001", "1", 1, 0x17));
            this.m_bord.Add(base.Game.CreatePhysicalObj(0x425, 280, "board8", "game.crazytank.assetmap.Board001", "1", 1, 0x16));
            this.m_bord.Add(base.Game.CreatePhysicalObj(0x111, 0x196, "board9", "game.crazytank.assetmap.Board001", "1", 1, 350));
            this.m_bord.Add(base.Game.CreatePhysicalObj(620, 0x198, "board10", "game.crazytank.assetmap.Board001", "1", 1, 0x17));
            this.m_bord.Add(base.Game.CreatePhysicalObj(0x369, 0x19e, "board11", "game.crazytank.assetmap.Board001", "1", 1, 0x150));
            this.m_bord.Add(base.Game.CreatePhysicalObj(0x483, 0x1ac, "board12", "game.crazytank.assetmap.Board001", "1", 1, 0x150));
            this.m_door = base.Game.CreatePhysicalObj(0x4fb, 0x22c, "door", "game.crazytank.assetmap.CrystalDoor001", "start", 1, 0);
            int[] numArray = new int[] { 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12 };
            for (int i = 0; i < base.Game.TotalCount; i++)
            {
                int index = base.Game.Random.Next(0, 12);
                if (numArray[index] == index)
                {
                    i--;
                }
                else
                {
                    numArray[index] = index;
                    this.m_bord.ToArray()[index].PlayMovie("2", 0, 0);
                    this.KeyIndex = "Key" + index;
                    this.m_key.Add(base.Game.CreatePhysicalObj(this.m_bord.ToArray()[index].X, this.m_bord.ToArray()[index].Y - 8, this.KeyIndex, "game.crazytank.assetmap.Key", "1", 1, 0));
                    base.Game.SendGameObjectFocus(1, this.m_bord.ToArray()[index].Name, 0, 0);
                }
            }
            base.Game.SendGameObjectFocus(1, "door", 0x3e8, 0);
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo> {
                new LoadingFileInfo(2, "sound/Sound201.swf", "Sound201"),
                new LoadingFileInfo(2, "sound/Sound202.swf", "Sound202")
            };
            base.Game.SendLoadResource(loadingFileInfos);
            base.Game.GameOverResources.Add("game.crazytank.assetmap.CrystalDoor001");
        }

        public override int UpdateUIData() {
            return this.m_count;
        }
           
    }
}

