using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC
{
    public class TrainingNpc : ABrain
    {
        private static int direction = 1;
        private int dis = 0;
        private int mtX = 0;

        public void NextMove()
        {
            direction = -direction;
            if (direction == 1)
            {
                this.mtX = base.Body.X + this.dis;
                base.Body.MoveTo(this.mtX, base.Body.Y, "walk", 100);
            }
            else
            {
                this.mtX = base.Body.X - this.dis;
                base.Body.MoveTo(this.mtX, base.Body.Y, "walk", 100);
            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            this.dis = base.Game.Random.Next(((SimpleNpc) base.Body).NpcInfo.MoveMin, ((SimpleNpc) base.Body).NpcInfo.MoveMax);
            if (direction == 1)
            {
                this.mtX = base.Body.X + this.dis;
                if (this.mtX > 800)
                {
                    base.Body.MoveTo(800, base.Body.Y, "walk", 100);
                    direction = -direction;
                }
                else
                {
                    base.Body.MoveTo(this.mtX, base.Body.Y, "walk", 100);
                }
            }
            else
            {
                this.mtX = base.Body.X - this.dis;
                if (this.mtX < 100)
                {
                    base.Body.MoveTo(100, base.Body.Y, "walk", 100);
                    direction = -direction;
                }
                else
                {
                    base.Body.MoveTo(this.mtX, base.Body.Y, "walk", 100);
                }
            }
        }
    }
}

