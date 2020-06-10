using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC
{
    public class TerrorKingFirst : ABrain
    {
        private int m_attackTurn = 0;
        private static string[] AllAttackChat = new string[] { "要地震喽！！<br/>各位请扶好哦", "把你武器震下来！", "看你们能还经得起几下！！" };
        private static string[] ShootChat = new string[] { "让你知道什么叫百发百中！", "送你一个球~你可要接好啦", "你们这群无知的低等庶民" };
        private static string[] ShootedChat = new string[] { "哎呀~~你们为什么要攻击我？<br/>我在干什么？", "噢~~好痛!我为什么要战斗？<br/>我必须战斗…" };
        private static string[] AddBooldChat = new string[] { "扭啊扭~<br/>扭啊扭~~", "哈利路亚~<br/>路亚路亚~~", "呀呀呀，<br/>好舒服啊！" };
        private static string[] KillAttackChat = new string[] { "君临天下！！" };

        private void AllAttack()
        {
            base.Body.CurrentDamagePlus = 0.5f;
            int index = base.Game.Random.Next(0, AllAttackChat.Length);
            base.Body.Say(AllAttackChat[index], 1, 0);
            base.Body.PlayMovie("beat", 0x3e8, 0);
            base.Body.RangeAttacking(base.Body.X - 0x3e8, base.Body.X + 0x3e8, "cry", 0xbb8, null);
        }

        private void Healing()
        {
            int index = base.Game.Random.Next(0, AddBooldChat.Length);
            base.Body.Say(AddBooldChat[index], 1, 0);
            base.Body.SyncAtTime = true;
            base.Body.AddBlood(0x1388);
            base.Body.PlayMovie("", 0x3e8, 0x1194);
        }

        private void KillAttack(int fx, int tx)
        {
            base.Body.CurrentDamagePlus = 10f;
            int index = base.Game.Random.Next(0, KillAttackChat.Length);
            base.Body.Say(KillAttackChat[index], 1, 0x3e8);
            base.Body.PlayMovie("beat", 0xbb8, 0);
            base.Body.RangeAttacking(fx, tx, "cry", 0xfa0, null);
        }

        private void NextAttack()
        {
            Player player = base.Game.FindRandomPlayer();
            if (player.X > base.Body.Y)
            {
                base.Body.ChangeDirection(1, 800);
            }
            else
            {
                base.Body.ChangeDirection(-1, 800);
            }
            base.Body.CurrentDamagePlus = 0.8f;
            int index = base.Game.Random.Next(0, ShootChat.Length);
            base.Body.Say(ShootChat[index], 1, 0);
            if (player != null)
            {
                int x = base.Game.Random.Next(player.X - 50, player.X + 50);
                if (base.Body.ShootPoint(x, player.Y, 0x3d, 0x3e8, 0x2710, 1, 1f, 0x8fc))
                {
                    base.Body.PlayMovie("beat2", 0x5dc, 0);
                }
                if (base.Body.ShootPoint(x, player.Y, 0x3d, 0x3e8, 0x2710, 1, 1f, 0x1004))
                {
                    base.Body.PlayMovie("beat2", 0xce4, 0);
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
            base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
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
                this.m_attackTurn++;
            }
            else if (this.m_attackTurn == 1)
            {
                this.PersonalAttack();
                this.m_attackTurn++;
            }
            else
            {
                this.Healing();
                this.m_attackTurn = 0;
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        private void PersonalAttack()
        {
            int x = base.Game.Random.Next(670, 880);
            base.Body.MoveTo(x, base.Body.Y, "walk", 0x3e8, new LivingCallBack(this.NextAttack));
        }
    }
}

