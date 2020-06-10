using System.Text;

namespace GameServerScript.AI.Game
{
    public class ExplorationGame5 : ExplorationGame
    {
        private string GetMissionIds()
        {
            int[] numArray = new int[] { 0x3ed, 0x3ee };
            int index = base.Game.Random.Next(0, numArray.Length);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                builder.Append(numArray[index] + ",");
            }
            return builder.Remove(builder.Length - 1, 1).ToString();
        }

        public override void OnCreated()
        {
            base.Game.SetupMissions(this.GetMissionIds());
            base.Game.TotalMissionCount = 5;
            base.OnCreated();
        }

        public override void OnPrepated()
        {
            base.OnPrepated();
        }
    }
}

