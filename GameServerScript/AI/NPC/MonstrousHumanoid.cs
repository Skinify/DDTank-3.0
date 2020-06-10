using System;
using System.Collections.Generic;
using Game.Logic.Phy.Object;

using Game.Logic.AI;

namespace GameServerScript.AI.NPC
{
    public class MonstrousHumanoid : ABrain
    {
        private Player m_target;

        private void NpcAttack()
        {
            int num = 1;
            if (this.m_target.X > base.Body.X)
            {
                num = 1;
                base.Body.ChangeDirection(1, 0);
            }
            else
            {
                num = -1;
                base.Body.ChangeDirection(-1, 0);
            }
            int num2 = Math.Abs((int) (this.m_target.X - base.Body.X));
            if (num2 < 300)
            {
                this.ShootAttack();
            }
            else
            {
                int num3 = base.Game.Random.Next(((SimpleBoss) base.Body).NpcInfo.MoveMin, ((SimpleBoss) base.Body).NpcInfo.MoveMax) * 3;
                if (num3 > num2)
                {
                    num3 = num2 - 300;
                }
                num3 *= num;
                if (!base.Body.MoveTo(base.Body.X + num3, this.m_target.Y - 20, "walk", 0, new LivingCallBack(this.ShootAttack)))
                {
                    this.ShootAttack();
                }
            }
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
            base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
            if (!this.ShootLowestBooldPlayer())
            {
                this.RandomShootPlayer();
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        private void RandomShootPlayer()
        {
            List<Player> allLivingPlayers = base.Game.GetAllLivingPlayers();
            int num = base.Game.Random.Next(0, allLivingPlayers.Count);
            this.m_target = allLivingPlayers[num];
            this.NpcAttack();
        }

        private void ShootAttack()
        {
            int num = Math.Abs((int) (this.m_target.X - base.Body.X));
            int num2 = 30;
            if (num < 200)
            {
                num2 = 10;
            }
            else if (num < 500)
            {
                num2 = 30;
            }
            else
            {
                num2 = 50;
            }
            int x = base.Game.Random.Next(this.m_target.X - num2, this.m_target.X + num2);
            if (base.Body.ShootPoint(x, this.m_target.Y, ((SimpleBoss) base.Body).NpcInfo.CurrentBallId, 0x3e8, 0x2710, 1, 2f, 0x6a4))
            {
                base.Body.PlayMovie("beat", 0x6a4, 0);
            }
        }

        private bool ShootLowestBooldPlayer()
        {
            List<Player> list = new List<Player>();
            foreach (Player player in base.Game.GetAllLivingPlayers())
            {
                if (player.Blood < (player.MaxBlood * 0.2))
                {
                    list.Add(player);
                }
            }
            if (list.Count > 0)
            {
                int num = base.Game.Random.Next(0, list.Count);
                this.m_target = list[num];
                this.NpcAttack();
                return true;
            }
            return false;
        }
    }
}

