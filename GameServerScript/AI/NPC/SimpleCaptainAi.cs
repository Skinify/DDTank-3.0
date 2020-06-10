using System.Collections.Generic;
using Game.Logic.Phy.Object;

using Game.Logic.AI;

namespace GameServerScript.AI.NPC
{
    public class SimpleCaptainAi : ABrain
    {
        private int m_attackTurn = 0;
        public int currentCount = 0;
        public int Dander = 0;
        private int npcID = 0x3f1;
        public List<SimpleNpc> Children = new List<SimpleNpc>();
        private static string[] AllAttackChat = new string[] { "你们这是自寻死路！", "你惹毛我了!", "超级无敌大地震……<br/>震……震…… " };
        private static string[] ShootChat = new string[] { "砸你家玻璃。", "看哥打的可比你们准多了" };
        private static string[] KillPlayerChat = new string[] { "送你回老家！", "就凭你还妄想能够打败我？" };
        private static string[] CallChat = new string[] { "卫兵！ <br/>卫兵！！ ", "啵咕们！！<br/>给我些帮助！" };
        private static string[] ShootedChat = new string[] { "哎呦！很痛…", "我还顶的住…" };
        private static string[] JumpChat = new string[] { "为了你们的胜利，<br/>向我开炮！", "你再往前半步我就把你给杀了！", "高！<br/>实在是高！" };
        private static string[] KillAttackChat = new string[] { "超级肉弹！！" };

        private void AllAttack()
        {
            this.ChangeDirection(3);
            base.Body.CurrentDamagePlus = 0.5f;
            int index = base.Game.Random.Next(0, AllAttackChat.Length);
            base.Body.Say(AllAttackChat[index], 1, 0);
            base.Body.FallFrom(base.Body.X, 0x1fd, null, 0x3e8, 1, 12);
            base.Body.PlayMovie("beat2", 0x3e8, 0);
            base.Body.RangeAttacking(base.Body.X - 0x3e8, base.Body.X + 0x3e8, "cry", 0xfa0, null);
        }

        private void ChangeDirection(int count)
        {
            int direction = base.Body.Direction;
            for (int i = 0; i < count; i++)
            {
                base.Body.ChangeDirection(-direction, (i * 200) + 100);
                base.Body.ChangeDirection(direction, ((i + 1) * 100) + (i * 200));
            }
        }

        public void CreateChild()
        {
            ((SimpleBoss) base.Body).CreateChild(this.npcID, 520, 530, 430, 6);
        }

        private void KillAttack(int fx, int tx)
        {
            this.ChangeDirection(3);
            int index = base.Game.Random.Next(0, KillAttackChat.Length);
            base.Body.Say(KillAttackChat[index], 1, 0x3e8);
            base.Body.CurrentDamagePlus = 10f;
            base.Body.PlayMovie("beat2", 0xbb8, 0);
            base.Body.RangeAttacking(fx, tx, "cry", 0x1388, null);
        }

        private void NextAttack()
        {
            Player player = base.Game.FindRandomPlayer();
            base.Body.SetRect(0, 0, 0, 0);
            if (player.X > base.Body.Y)
            {
                base.Body.ChangeDirection(1, 500);
            }
            else
            {
                base.Body.ChangeDirection(-1, 500);
            }
            base.Body.CurrentDamagePlus = 0.8f;
            if (player != null)
            {
                int x = base.Game.Random.Next(player.X - 50, player.X + 50);
                if (base.Body.ShootPoint(x, player.Y, 0x3d, 0x3e8, 0x2710, 1, 1f, 0x898))
                {
                    base.Body.PlayMovie("beat", 0x6a4, 0);
                }
                if (base.Body.ShootPoint(x, player.Y, 0x3d, 0x3e8, 0x2710, 1, 1f, 0xc80))
                {
                    base.Body.PlayMovie("beat", 0xa8c, 0);
                }
            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            base.Body.CurrentDamagePlus = 1f;
            base.Body.CurrentShootMinus = 1f;
            base.Body.SetRect(((SimpleBoss) base.Body).NpcInfo.X, ((SimpleBoss) base.Body).NpcInfo.Y, ((SimpleBoss) base.Body).NpcInfo.Width, ((SimpleBoss) base.Body).NpcInfo.Height);
            if (base.Body.Direction == -1)
            {
                base.Body.SetRect(((SimpleBoss) base.Body).NpcInfo.X, ((SimpleBoss) base.Body).NpcInfo.Y, ((SimpleBoss) base.Body).NpcInfo.Width, ((SimpleBoss) base.Body).NpcInfo.Height);
            }
            else
            {
                base.Body.SetRect(-((SimpleBoss) base.Body).NpcInfo.X - ((SimpleBoss) base.Body).NpcInfo.Width, ((SimpleBoss) base.Body).NpcInfo.Y, ((SimpleBoss) base.Body).NpcInfo.Width, ((SimpleBoss) base.Body).NpcInfo.Height);
            }
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
            base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
            bool flag = false;
            int num = 0;
            foreach (Player player in base.Game.GetAllFightPlayers())
            {
                if (player.IsLiving && (player.X > 480) && (player.X < 0x3e8))
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
                this.KillAttack(480, 0x3e8);
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
                this.Summon();
                this.m_attackTurn = 0;
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        private void PersonalAttack()
        {
            this.ChangeDirection(3);
            int index = base.Game.Random.Next(0, ShootChat.Length);
            base.Body.Say(ShootChat[index], 1, 0);
            int x = base.Game.Random.Next(670, 880);
            int direction = base.Body.Direction;
            base.Body.MoveTo(x, base.Body.Y, "walk", 0x3e8, new LivingCallBack(this.NextAttack));
            base.Body.ChangeDirection(base.Game.FindlivingbyDir(base.Body), 0x2328);
        }

        private void Summon()
        {
            this.ChangeDirection(3);
            base.Body.JumpTo(base.Body.X, base.Body.Y - 300, "Jump", 0x3e8, 1);
            int index = base.Game.Random.Next(0, CallChat.Length);
            base.Body.Say(CallChat[index], 1, 0xce4);
            base.Body.PlayMovie("call", 0xdac, 0);
            base.Body.CallFuction(new LivingCallBack(this.CreateChild), 0xfa0);
        }
    }
}

