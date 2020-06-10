using System.Drawing;
using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC
{
    public class SimpleQueenAntAi : ABrain
    {
        private int m_attackTurn = 0;
        private int npcID = 0x7d4;
        private int isSay = 0;
        private Point[] brithPoint = new Point[] { new Point(0x3d3, 630), new Point(0x3f5, 630), new Point(0x41c, 630), new Point(0x440, 630), new Point(0x476, 630) };
        private static string[] AllAttackChat = new string[] { LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg1", new object[0]), LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg2", new object[0]), LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg3", new object[0]) };
        private static string[] ShootChat = new string[] { LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg4", new object[0]), LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg5", new object[0]) };
        private static string[] KillPlayerChat = new string[] { LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg6", new object[0]), LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg7", new object[0]) };
        private static string[] CallChat = new string[] { LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg8", new object[0]), LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg9", new object[0]) };
        private static string[] JumpChat = new string[] { LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg10", new object[0]), LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg11", new object[0]), LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg12", new object[0]) };
        private static string[] KillAttackChat = new string[] { LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg13", new object[0]), LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg14", new object[0]) };
        private static string[] ShootedChat = new string[] { LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg15", new object[0]), LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg16", new object[0]) };
        private static string[] DiedChat = new string[] { LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg17", new object[0]) };

        private void Call()
        {
            ((SimpleBoss) base.Body).CreateChild(this.npcID, this.brithPoint, 9, 3, 9);
        }

        private void CreateChild()
        {
        }

        private void KillAttack(int fx, int tx)
        {
            int index = base.Game.Random.Next(0, KillAttackChat.Length);
            base.Body.Say(KillAttackChat[index], 1, 0x3e8);
            base.Body.CurrentDamagePlus = 10f;
            base.Body.PlayMovie("beatB", 0xbb8, 0);
            base.Body.RangeAttacking(fx, tx, "cry", 0x1388, null);
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            base.Body.CurrentDamagePlus = 1f;
            base.Body.CurrentShootMinus = 1f;
            this.isSay = 0;
        }

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnDiedSay()
        {
        }

        public override void OnKillPlayerSay()
        {
            base.OnKillPlayerSay();
            int index = base.Game.Random.Next(0, KillPlayerChat.Length);
            base.Body.Say(KillPlayerChat[index], 1, 0, 0x7d0);
        }

        public override void OnShootedSay()
        {
            int index = base.Game.Random.Next(0, ShootedChat.Length);
            if ((this.isSay == 0) && base.Body.IsLiving)
            {
                base.Body.Say(ShootedChat[index], 1, 900, 0);
                this.isSay = 1;
            }
            if (!base.Body.IsLiving)
            {
                index = base.Game.Random.Next(0, DiedChat.Length);
                base.Body.Say(DiedChat[index], 1, 100, 0x7d0);
            }
        }

        public override void OnStartAttacking()
        {
            base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
            bool flag = false;
            int num = 0;
            foreach (Player player in base.Game.GetAllFightPlayers())
            {
                if (player.IsLiving && (player.X > 0x491) && (player.X < (base.Game.Map.Info.ForegroundWidth + 1)))
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
                this.KillAttack(0x491, base.Game.Map.Info.ForegroundWidth + 1);
            }
            else if (this.m_attackTurn == 0)
            {
                if (((PVEGame) base.Game).GetLivedLivings().Count == 9)
                {
                    this.PersonalAttack();
                }
                else
                {
                    this.Summon();
                }
                this.m_attackTurn++;
            }
            else
            {
                this.PersonalAttack();
                this.m_attackTurn = 0;
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        private void PersonalAttack()
        {
            Player player = base.Game.FindRandomPlayer();
            if (player != null)
            {
                base.Body.CurrentDamagePlus = 0.8f;
                int index = base.Game.Random.Next(0, ShootChat.Length);
                base.Body.Say(ShootChat[index], 1, 0);
                int num2 = base.Game.Random.Next(670, 880);
                int num3 = base.Game.Random.Next(player.X - 10, player.X + 10);
                if (base.Body.ShootPoint(player.X, player.Y, 0x33, 0x3e8, 0x2710, 1, 3f, 0x9f6))
                {
                    base.Body.PlayMovie("beatA", 0x6a4, 0);
                }
            }
        }

        private void Summon()
        {
            int index = base.Game.Random.Next(0, CallChat.Length);
            base.Body.Say(CallChat[index], 1, 600);
            base.Body.PlayMovie("call", 0x6a4, 0x7d0, new LivingCallBack(this.Call));
            base.Body.CallFuction(new LivingCallBack(this.Call), 0x7d0);
        }
    }
}

