using System.Collections.Generic;
using Game.Logic.Phy.Object;

using Game.Logic.AI;
using Game.Logic.Effects;

namespace GameServerScript.AI.NPC
{
    public class NormalKingZhen : ABrain
    {
        public int attackingTurn = 1;
        public int orchinIndex = 1;
        public int currentCount = 0;
        public int Dander = 0;
        private int npcID = 0x457;
        private static string[] AllAttackChat = new string[] { "看我的绝技！", "这招酷吧，<br/>想学不？", "消失吧！！！<br/>卑微的灰尘！", "你们会为此付出代价的！ " };
        private static string[] ShootChat = new string[] { "你是在给我挠痒痒吗？", "我可不会像刚才那个废物一样被你打败！", "哎哟，你打的我好疼啊，<br/>哈哈哈哈！", "啧啧啧，就这样的攻击力！", "看到我是你们的荣幸！" };
        private static string[] CallChat = new string[] { "来啊，<br/>让他们尝尝炸弹的厉害！" };
        private static string[] AngryChat = new string[] { "是你们逼我使出绝招的！" };
        private static string[] KillAttackChat = new string[] { "你来找死吗？" };
        private static string[] SealChat = new string[] { "异次元放逐！" };
        private static string[] KillPlayerChat = new string[] { "灭亡是你唯一的归宿！", "太不堪一击了！" };
        public List<SimpleNpc> orchins = new List<SimpleNpc>();

        public void Angger()
        {
            int index = base.Game.Random.Next(0, AngryChat.Length);
            base.Body.Say(AngryChat[index], 1, 0);
            base.Body.State = 1;
            this.Dander += 100;
            ((SimpleBoss) base.Body).SetDander(this.Dander);
            if (base.Body.Direction == -1)
            {
                ((SimpleBoss) base.Body).SetRelateDemagemRect(8, -252, 0x4a, 50);
            }
            else
            {
                ((SimpleBoss) base.Body).SetRelateDemagemRect(-82, -252, 0x4a, 50);
            }
        }

        public void CreateChild()
        {
            ((SimpleBoss) base.Body).CreateChild(this.npcID, 520, 530, 400, 6);
        }

        public void GoOnAngger()
        {
            if (base.Body.State == 1)
            {
                base.Body.CurrentDamagePlus = 1000f;
                base.Body.PlayMovie("beatC", 0xdac, 0);
                base.Body.RangeAttacking(base.Body.X - 0x3e8, base.Body.X + 0x3e8, "cry", 0x15e0, null);
                base.Body.Die(0x15e0);
            }
            else
            {
                ((SimpleBoss) base.Body).SetRelateDemagemRect(-41, -187, 0x53, 140);
                base.Body.PlayMovie("mantra", 0, 0x7d0);
                List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
                foreach (Player player in allLivingPlayers)
                {
                    player.AddEffect(new ContinueReduceBloodEffect(2, -50), 0);
                }
            }
        }

        public void HalfAttack()
        {
            base.Body.CurrentDamagePlus = 0.5f;
            int index = base.Game.Random.Next(0, SealChat.Length);
            base.Body.Say(AllAttackChat[index], 1, 500);
            base.Body.PlayMovie("beatB", 0x9c4, 0);
            if (base.Body.Direction == 1)
            {
                base.Body.RangeAttacking(base.Body.X, base.Body.X + 0x3e8, "cry", 0xce4, null);
            }
            else
            {
                base.Body.RangeAttacking(base.Body.X - 0x3e8, base.Body.X, "cry", 0xce4, null);
            }
        }

        public void Healing()
        {
            base.Body.SyncAtTime = true;
            base.Body.AddBlood(0x1388);
            base.Body.Say("哈哈,我又充满力量了!", 1, 0);
        }

        public void KillAttack(int fx, int tx)
        {
            base.Body.CurrentDamagePlus = 10f;
            int index = base.Game.Random.Next(0, KillAttackChat.Length);
            ((SimpleBoss) base.Body).Say(KillAttackChat[index], 1, 500);
            base.Body.PlayMovie("beatB", 0x9c4, 0);
            base.Body.RangeAttacking(fx, tx, "cry", 0xce4, null);
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            base.Body.CurrentDamagePlus = 1f;
            base.Body.CurrentShootMinus = 1f;
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
            else
            {
                if (this.attackingTurn == 1)
                {
                    this.Healing();
                    this.HalfAttack();
                }
                else if (this.attackingTurn == 2)
                {
                    this.Healing();
                    this.Summon();
                }
                else if (this.attackingTurn == 3)
                {
                    this.Healing();
                    this.Seal();
                }
                else if (this.attackingTurn == 4)
                {
                    this.Healing();
                    this.Angger();
                }
                else
                {
                    this.GoOnAngger();
                    this.attackingTurn = 0;
                }
                this.attackingTurn++;
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public void Seal()
        {
            int index = base.Game.Random.Next(0, SealChat.Length);
            ((SimpleBoss) base.Body).Say(SealChat[index], 1, 0);
            Player player = base.Game.FindRandomPlayer();
            base.Body.PlayMovie("mantra", 0x7d0, 0x7d0);
            base.Body.Seal(player, 1, 0xbb8);
        }

        public void Summon()
        {
            int index = base.Game.Random.Next(0, CallChat.Length);
            base.Body.Say(CallChat[index], 1, 0);
            base.Body.PlayMovie("beatA", 100, 0);
            base.Body.CallFuction(new LivingCallBack(this.CreateChild), 0x9c4);
        }
    }
}

