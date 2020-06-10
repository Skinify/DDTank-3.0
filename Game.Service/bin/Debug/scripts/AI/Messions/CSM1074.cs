using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;


using Game.Logic.Effects;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{

    public class CSM1074 : AMissionControl
    {
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
            return base.Game.TurnIndex > (base.Game.TotalTurn - 1);
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            ((Player) base.Game.CurrentLiving).SetBall(3);
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            base.Game.IsWin = true;
            foreach (Player player in base.Game.GetAllFightPlayers())
            {
                SealEffect ofType = (SealEffect) player.EffectList.GetOfType(eEffectType.SealEffect);
                if (ofType != null)
                {
                    ofType.Stop();
                }
            }
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo> {
                new LoadingFileInfo(2, "image/map/show5.jpg", "")
            };
            base.Game.SendLoadResource(loadingFileInfos);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            ((Player) base.Game.CurrentLiving).Seal((Player) base.Game.CurrentLiving, 0, 0);
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            base.Game.SetMap(0x432);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            base.Game.TotalTurn = base.Game.PlayerCount * 3;
            base.Game.SendMissionInfo();
            List<SqlDataProvider.Data.ItemInfo> list = new List<SqlDataProvider.Data.ItemInfo>();
            for (int i = 0; i < 0x18; i++)
            {
                List<SqlDataProvider.Data.ItemInfo> list2 = null;
                DropInventory.SpecialDrop(0x432, 2, ref list2);
                if (list2 != null)
                {
                    foreach (SqlDataProvider.Data.ItemInfo info in list2)
                    {
                        list.Add(info);
                    }
                }
            }
            base.Game.CreateBox(550, 0x44, "2", list[0]);
            base.Game.CreateBox(750, 0x44, "2", list[1]);
            base.Game.CreateBox(0x3a4, 0x44, "2", list[2]);
            base.Game.CreateBox(0x450, 0x44, "2", list[3]);
            base.Game.CreateBox(0x1c3, 0xb8, "1", list[4]);
            base.Game.CreateBox(0x1c3, 0x11d, "1", list[5]);
            base.Game.CreateBox(0x1c3, 0x18a, "1", list[6]);
            base.Game.CreateBox(0x1c3, 0x1f3, "1", list[7]);
            base.Game.CreateBox(0x283, 0xb8, "1", list[8]);
            base.Game.CreateBox(0x283, 0x11d, "1", list[9]);
            base.Game.CreateBox(0x283, 0x18a, "1", list[10]);
            base.Game.CreateBox(0x283, 0x1f3, "1", list[11]);
            base.Game.CreateBox(830, 0xb8, "1", list[12]);
            base.Game.CreateBox(830, 0x11d, "1", list[13]);
            base.Game.CreateBox(830, 0x18a, "1", list[14]);
            base.Game.CreateBox(830, 0x1f3, "1", list[15]);
            base.Game.CreateBox(0x3fe, 0xb8, "1", list[0x10]);
            base.Game.CreateBox(0x3fe, 0x11d, "1", list[0x11]);
            base.Game.CreateBox(0x3fe, 0x18a, "1", list[0x12]);
            base.Game.CreateBox(0x3fe, 0x1f3, "1", list[0x13]);
            base.Game.CreateBox(0x4b1, 0xb8, "1", list[20]);
            base.Game.CreateBox(0x4b1, 0x11d, "1", list[0x15]);
            base.Game.CreateBox(0x4b1, 0x18a, "1", list[0x16]);
            base.Game.CreateBox(0x4b1, 0x1f3, "1", list[0x17]);
        }

        public override int UpdateUIData()
        {
            return base.UpdateUIData();
        }
           
    }
}

