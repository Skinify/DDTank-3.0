using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace GameServerScript.AI.NPC
{
    public class MyNpcAi : ABrain
    {
        protected Player m_targer;
        private static Random random = new Random();
        private static string[] listChat = new string[] { "为了荣誉！为了胜利！！", "握紧手中的武器，不要发抖呀～", "为了国王而战！", "敌人就在眼前，大家做好战斗准备！", "感觉最近国王的行为举止越来越反常......", "为了啵咕的胜利！！兄弟们冲啊！", "快消灭敌人！", "大家一起上,人多力量大！", "大家一起速战速决！", "包围敌人，歼灭他们。", "增援！增援！我们需要更多的增援！！", "就算牺牲自己，也不会让你们轻易得逞。", "不要轻视啵咕的力量，否则你会为此付出代价。" };

        public void Beating()
        {
            if (this.m_targer != null)
            {
                int num2;
                Player player = base.Game.FindRandomPlayer();
                switch (random.Next(2, 5))
                {
                    case 2:
                        num2 = 0;
                        while (num2 < 3)
                        {
                            base.Body.ShootPoint(player.X, player.Y, 0x33, 0x3e8, 0x2710, 1, 3f, 0x9f6);
                            num2++;
                        }
                        base.Body.PlayMovie("beatA", 0x3e8, 0x7d0);
                        break;

                    case 3:
                        num2 = 0;
                        while (num2 < 2)
                        {
                            base.Body.ShootPoint(player.X, player.Y, 0x43, 0x3e8, 0x2710, 3, 3f, 0x9f6);
                            num2++;
                        }
                        base.Body.PlayMovie("beatB", 0x3e8, 0x7d0);
                        break;

                    case 4:
                        for (num2 = 0; num2 < 10; num2++)
                        {
                            base.Body.Shoot(0x43, random.Next(0, 500), 5, 20, 0x2d, 1, 0x7d0);
                        }
                        base.Body.PlayMovie("beatC", 0x3e8, 0x7d0);
                        break;

                    case 5:
                        base.Body.PlayMovie("beatD", 0x3e8, 0x7d0);
                        break;

                    case 6:
                        base.Body.PlayMovie("beatF", 0x3e8, 0x7d0);
                        break;
                }
            }
        }

        public void Fall()
        {
            base.Body.FallFrom(base.Body.X, base.Body.Y + 240, null, 0, 0, 12, new LivingCallBack(this.Beating));
        }

        public void FallBeat()
        {
            base.Body.Beat(this.m_targer, "beat", 100, 0, 0x7d0);
        }

        public static string GetOneChat()
        {
            int index = random.Next(0, listChat.Length);
            return listChat[index];
        }

        public void Jump()
        {
            base.Body.Direction = 1;
            base.Body.JumpTo(base.Body.X, base.Body.Y - 240, "Jump", 0, 2, 3, new LivingCallBack(this.Beating));
        }

        public static void LivingSay(List<Living> livings)
        {
            if ((livings != null) && (livings.Count != 0))
            {
                int num = 0;
                int count = livings.Count;
                foreach (Living living in livings)
                {
                    living.IsSay = false;
                }
                if (count <= 5)
                {
                    num = random.Next(0, 2);
                }
                else if ((count > 5) && (count <= 10))
                {
                    num = random.Next(1, 3);
                }
                else
                {
                    num = random.Next(1, 4);
                }
                if (num > 0)
                {
                    int[] numArray = new int[num];
                    int num3 = 0;
                    while (num3 < num)
                    {
                        int num4 = random.Next(0, count);
                        if (!livings[num4].IsSay)
                        {
                            livings[num4].IsSay = true;
                            int delay = random.Next(0, 0x1388);
                            livings[num4].Say(SimpleNpcAi.GetOneChat(), 0, delay);
                            num3++;
                        }
                    }
                }
            }
        }

        public void MoveBeat()
        {
            base.Body.Beat(this.m_targer, "beat", 100, 0, 0);
        }

        public void MoveToPlayer(Player player)
        {
            int num = (int) player.Distance(base.Body.X, base.Body.Y);
            int num2 = base.Game.Random.Next(((SimpleNpc) base.Body).NpcInfo.MoveMin, ((SimpleNpc) base.Body).NpcInfo.MoveMax);
            if (num > 0x61)
            {
                if (num > ((SimpleNpc) base.Body).NpcInfo.MoveMax)
                {
                    num = num2;
                }
                else
                {
                    num -= 90;
                }
                if ((player.Y < 420) && (player.X < 210))
                {
                    if (base.Body.Y > 420)
                    {
                        if ((base.Body.X - num) < 50)
                        {
                            base.Body.MoveTo(0x19, base.Body.Y, "walk", 0x4b0, new LivingCallBack(this.Jump));
                        }
                        else
                        {
                            base.Body.MoveTo(base.Body.X - num, base.Body.Y, "walk", 0x4b0, new LivingCallBack(this.MoveBeat));
                        }
                    }
                    else if (player.X > base.Body.X)
                    {
                        base.Body.MoveTo(base.Body.X + num, base.Body.Y, "walk", 0x4b0, new LivingCallBack(this.MoveBeat));
                    }
                    else
                    {
                        base.Body.MoveTo(base.Body.X - num, base.Body.Y, "walk", 0x4b0, new LivingCallBack(this.MoveBeat));
                    }
                }
                else if (base.Body.Y < 420)
                {
                    if ((base.Body.X + num) > 200)
                    {
                        base.Body.MoveTo(200, base.Body.Y, "walk", 0x4b0, new LivingCallBack(this.Fall));
                    }
                }
                else if (player.X > base.Body.X)
                {
                    base.Body.MoveTo(base.Body.X + num, base.Body.Y, "walk", 0x4b0, new LivingCallBack(this.MoveBeat));
                }
                else
                {
                    base.Body.MoveTo(base.Body.X - num, base.Body.Y, "walk", 0x4b0, new LivingCallBack(this.MoveBeat));
                }
            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            base.m_body.CurrentDamagePlus = 1f;
            base.m_body.CurrentShootMinus = 1f;
            if (base.m_body.IsSay)
            {
                string oneChat = GetOneChat();
                int delay = base.Game.Random.Next(0, 0x1388);
                base.m_body.Say(oneChat, 0, delay);
            }
        }

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnCreated()
        {
            base.OnCreated();
            base.Body.Direction = -1;
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            this.m_targer = base.Game.FindNearestPlayer(base.Body.X, base.Body.Y);
            this.Beating();
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
    }
}

