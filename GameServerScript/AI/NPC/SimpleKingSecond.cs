using System.Collections.Generic;
using Game.Logic.Phy.Object;

using Game.Logic.AI;
using Game.Logic;

namespace GameServerScript.AI.NPC
{
    public class SimpleKingSecond : ABrain
    {
        private int m_attackTurn = 0;
        private int m_turn = 0;
        private PhysicalObj m_wallLeft = null;
        private PhysicalObj m_wallRight = null;
        private int IsEixt = 0;
        private static string[] AllAttackChat = new string[] { "要地震喽！！<br/>各位请扶好哦", "把你武器震下来！", "看你们能还经得起几下！！" };
        private static string[] ShootChat = new string[] { "让你知道什么叫百发百中！", "送你一个球~你可要接好啦", "你们这群无知的低等庶民" };
        private static string[] ShootedChat = new string[] { "哎呀~~你们为什么要攻击我？<br/>我在干什么？", "噢~~好痛!我为什么要战斗？<br/>我必须战斗…" };
        private static string[] KillPlayerChat = new string[] { "马迪亚斯不要再控制我！", "这就是挑战我的下场！", "不！！这不是我的意愿… " };
        private static string[] AddBooldChat = new string[] { "扭啊扭~<br/>扭啊扭~~", "哈利路亚~<br/>路亚路亚~~", "呀呀呀，<br/>好舒服啊！" };
        private static string[] KillAttackChat = new string[] { "君临天下！！" };
        private static string[] FrostChat = new string[] { "来尝尝这个吧", "让你冷静一下", "你们激怒了我" };
        private static string[] WallChat = new string[] { "神啊，赐予我力量吧！", "绝望吧，看我的水晶防护墙！" };

        private void AllAttack()
        {
            int num;
            base.Body.CurrentDamagePlus = 0.5f;
            if (this.m_turn == 0)
            {
                num = base.Game.Random.Next(0, AllAttackChat.Length);
                base.Body.Say(AllAttackChat[num], 1, 0x32c8);
                base.Body.PlayMovie("beat1", 0x3a98, 0);
                base.Body.RangeAttacking(base.Body.X - 0x3e8, base.Body.X + 0x3e8, "cry", 0x4268, null);
                this.m_turn++;
            }
            else
            {
                num = base.Game.Random.Next(0, AllAttackChat.Length);
                base.Body.Say(AllAttackChat[num], 1, 0);
                base.Body.PlayMovie("beat1", 0x3e8, 0);
                base.Body.RangeAttacking(base.Body.X - 0x3e8, base.Body.X + 0x3e8, "cry", 0xbb8, null);
            }
        }

        public void CreateChild()
        {
            base.Body.PlayMovie("renew", 100, 0x7d0);
            ((SimpleBoss) base.Body).CreateChild(2, 520, 530, 400, 6);
        }

        private void CriticalStrikes()
        {
            Player frostPlayerRadom = base.Game.GetFrostPlayerRadom();
            List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
            List<Player> players = new List<Player>();
            foreach (Player player2 in allFightPlayers)
            {
                if (!player2.IsFrost)
                {
                    players.Add(player2);
                }
            }
            ((SimpleBoss) base.Body).CurrentDamagePlus = 30f;
            if (players.Count != allFightPlayers.Count)
            {
                if (players.Count != 0)
                {
                    base.Body.PlayMovie("beat1", 0, 0);
                    base.Body.RangeAttacking(base.Body.X - 0x3e8, base.Body.X + 0x3e8, "beat1", 0x5dc, players);
                }
                else
                {
                    base.Body.PlayMovie("beat1", 0, 0);
                    base.Body.RangeAttacking(base.Body.X - 0x3e8, base.Body.X + 0x3e8, "beat1", 0x5dc, null);
                }
            }
            else
            {
                base.Body.Say("小的们给我上，好好教训敌人！", 1, 0xce4);
                base.Body.PlayMovie("renew", 0xdac, 0);
                base.Body.CallFuction(new LivingCallBack(this.CreateChild), 0x1770);
            }
        }

        private void FrostAttack()
        {
            int x = base.Game.Random.Next(660, 840);
            base.Body.MoveTo(x, base.Body.Y, "walk", 0, new LivingCallBack(this.NextAttack));
        }

        private void KillAttack(int fx, int tx)
        {
            int index = base.Game.Random.Next(0, KillAttackChat.Length);
            if (this.m_turn == 0)
            {
                base.Body.CurrentDamagePlus = 10f;
                base.Body.Say(KillAttackChat[index], 1, 0x32c8);
                base.Body.PlayMovie("beat1", 0x3a98, 0);
                base.Body.RangeAttacking(fx, tx, "cry", 0x4268, null);
                this.m_turn++;
            }
            else
            {
                base.Body.CurrentDamagePlus = 10f;
                base.Body.Say(KillAttackChat[index], 1, 0);
                base.Body.PlayMovie("beat1", 0x7d0, 0);
                base.Body.RangeAttacking(fx, tx, "cry", 0xfa0, null);
            }
        }

        private void NextAttack()
        {
            int num = base.Game.Random.Next(1, 2);
            for (int i = 0; i < num; i++)
            {
                Player player = base.Game.FindRandomPlayer();
                int index = base.Game.Random.Next(0, ShootChat.Length);
                base.Body.Say(ShootChat[index], 1, 0);
                if (player.X > base.Body.X)
                {
                    base.Body.ChangeDirection(1, 500);
                }
                else
                {
                    base.Body.ChangeDirection(-1, 500);
                }
                if ((player != null) && !player.IsFrost && base.Body.ShootPoint(player.X, player.Y, 1, 0x3e8, 0x2710, 1, 1.5f, 0x7d0))
                {
                    base.Body.PlayMovie("beat2", 0x5dc, 0);
                }
            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            base.m_body.CurrentDamagePlus = 1f;
            base.m_body.CurrentShootMinus = 1f;
        }

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            bool flag = false;
            int num = 0;
            foreach (Player player in base.Game.GetAllFightPlayers())
            {
                if (player.IsLiving && (player.X > 390) && (player.X < 0x456))
                {
                    int num2 = (int) base.Body.Distance(player.X, player.Y);
                    if (num2 > num)
                    {
                        num = num2;
                    }
                    flag = true;
                }
            }
            if (flag)
            {
                this.KillAttack(390, 0x456);
            }
            else if (this.m_attackTurn == 0)
            {
                this.AllAttack();
                if (this.IsEixt == 1)
                {
                    this.m_wallLeft.CanPenetrate = true;
                    this.m_wallRight.CanPenetrate = true;
                    base.Game.RemovePhysicalObj(this.m_wallLeft, true);
                    base.Game.RemovePhysicalObj(this.m_wallRight, true);
                    this.IsEixt = 0;
                }
                this.m_attackTurn++;
            }
            else if (this.m_attackTurn == 1)
            {
                this.FrostAttack();
                this.m_attackTurn++;
            }
            else if (this.m_attackTurn == 2)
            {
                this.ProtectingWall();
                this.m_attackTurn++;
            }
            else
            {
                this.CriticalStrikes();
                this.m_attackTurn = 0;
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        private void ProtectingWall()
        {
            if (this.IsEixt == 0)
            {
                this.m_wallLeft = ((PVEGame) base.Game).CreatePhysicalObj(base.Body.X - 0x41, 510, "wallLeft", "com.mapobject.asset.WaveAsset_01_left", "1", 1, 0);
                this.m_wallRight = ((PVEGame) base.Game).CreatePhysicalObj(base.Body.X + 0x41, 510, "wallLeft", "com.mapobject.asset.WaveAsset_01_right", "1", 1, 0);
                this.m_wallLeft.SetRect(-165, -169, 0x2b, 330);
                this.m_wallRight.SetRect(0x80, -165, 0x29, 330);
                this.IsEixt = 1;
            }
            int index = base.Game.Random.Next(0, WallChat.Length);
            base.Body.Say(WallChat[index], 1, 0);
        }
    }
}

