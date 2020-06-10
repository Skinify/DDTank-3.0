using Game.Logic.AI;

namespace GameServerScript.AI.NPC
{
    public class TrainingEliteNpc : ABrain
    {
        private int dis = 0;

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
            int[] numArray = new int[] { 1, -1 };
            this.dis = base.Game.Random.Next(50, 100);
            base.Body.MoveTo(base.Body.X + (this.dis * numArray[base.Game.Random.Next(0, 2)]), base.Body.Y, "walk", 0xbb8);
        }
    }
}

