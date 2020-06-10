using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{

    public class CNM1174 : AMissionControl
    {
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 0x7fd)
            {
                return 3;
            }
            if (score > 0x7f3)
            {
                return 2;
            }
            if (score > 0x7e9)
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
            List<SqlDataProvider.Data.ItemInfo> list = new List<SqlDataProvider.Data.ItemInfo>();
            for (int i = 0; i < 0x36; i++)
            {
                List<SqlDataProvider.Data.ItemInfo> list2 = null;
                if (i > 9)
                {
                    DropInventory.SpecialDrop(base.Game.MissionInfo.Id, 1, ref list2);
                }
                else
                {
                    DropInventory.SpecialDrop(base.Game.MissionInfo.Id, 2, ref list2);
                }
                if (list2 != null)
                {
                    foreach (SqlDataProvider.Data.ItemInfo info in list2)
                    {
                        list.Add(info);
                    }
                }
                else
                {
                    list.Add(null);
                }
            }
            base.Game.CreateBox(0x1c7, 0x58, "2", list[0]);
            base.Game.CreateBox(0x22b, 0x58, "2", list[1]);
            base.Game.CreateBox(0x28f, 0x58, "2", list[2]);
            base.Game.CreateBox(0x2f3, 0x58, "2", list[3]);
            base.Game.CreateBox(0x357, 0x58, "2", list[4]);
            base.Game.CreateBox(0x3bb, 0x58, "2", list[5]);
            base.Game.CreateBox(0x41f, 0x58, "2", list[6]);
            base.Game.CreateBox(0x483, 0x58, "2", list[7]);
            base.Game.CreateBox(0x4e7, 0x58, "2", list[8]);
            base.Game.CreateBox(450, 0xb8, "1", list[9]);
            base.Game.CreateBox(450, 0x103, "1", list[10]);
            base.Game.CreateBox(450, 0x14f, "1", list[11]);
            base.Game.CreateBox(450, 420, "1", list[12]);
            base.Game.CreateBox(450, 0x1f8, "1", list[13]);
            base.Game.CreateBox(550, 0xb8, "1", list[14]);
            base.Game.CreateBox(550, 0x103, "1", list[15]);
            base.Game.CreateBox(550, 0x14f, "1", list[0x10]);
            base.Game.CreateBox(550, 420, "1", list[0x11]);
            base.Game.CreateBox(550, 0x1f8, "1", list[0x12]);
            base.Game.CreateBox(650, 0xb8, "1", list[0x13]);
            base.Game.CreateBox(650, 0x103, "1", list[20]);
            base.Game.CreateBox(650, 0x14f, "1", list[0x15]);
            base.Game.CreateBox(650, 420, "1", list[0x16]);
            base.Game.CreateBox(650, 0x1f8, "1", list[0x17]);
            base.Game.CreateBox(750, 0xb8, "1", list[0x18]);
            base.Game.CreateBox(750, 0x103, "1", list[0x19]);
            base.Game.CreateBox(750, 0x14f, "1", list[0x1a]);
            base.Game.CreateBox(750, 420, "1", list[0x1b]);
            base.Game.CreateBox(750, 0x1f8, "1", list[0x1c]);
            base.Game.CreateBox(850, 0xb8, "1", list[0x1d]);
            base.Game.CreateBox(850, 0x103, "1", list[30]);
            base.Game.CreateBox(850, 0x14f, "1", list[0x1f]);
            base.Game.CreateBox(850, 420, "1", list[0x20]);
            base.Game.CreateBox(850, 0x1f8, "1", list[0x21]);
            base.Game.CreateBox(950, 0xb8, "1", list[0x22]);
            base.Game.CreateBox(950, 0x103, "1", list[0x23]);
            base.Game.CreateBox(950, 0x14f, "1", list[0x24]);
            base.Game.CreateBox(950, 420, "1", list[0x25]);
            base.Game.CreateBox(950, 0x1f8, "1", list[0x26]);
            base.Game.CreateBox(0x41a, 0xb8, "1", list[0x27]);
            base.Game.CreateBox(0x41a, 0x103, "1", list[40]);
            base.Game.CreateBox(0x41a, 0x14f, "1", list[0x29]);
            base.Game.CreateBox(0x41a, 420, "1", list[0x2a]);
            base.Game.CreateBox(0x41a, 0x1f8, "1", list[0x2b]);
            base.Game.CreateBox(0x47e, 0xb8, "1", list[0x2c]);
            base.Game.CreateBox(0x47e, 0x103, "1", list[0x2d]);
            base.Game.CreateBox(0x47e, 0x14f, "1", list[0x2e]);
            base.Game.CreateBox(0x47e, 420, "1", list[0x2f]);
            base.Game.CreateBox(0x47e, 0x1f8, "1", list[0x30]);
            base.Game.CreateBox(0x4e2, 0xbd, "1", list[0x31]);
            base.Game.CreateBox(0x4e2, 0x103, "1", list[50]);
            base.Game.CreateBox(0x4e2, 0x14f, "1", list[0x33]);
            base.Game.CreateBox(0x4e2, 420, "1", list[0x34]);
            base.Game.CreateBox(0x4e2, 0x1f8, "1", list[0x35]);
            base.Game.BossCardCount = 1;
        }

        public override int UpdateUIData() {
            return base.UpdateUIData();
        }
            
    }
}

