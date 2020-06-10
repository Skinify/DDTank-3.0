using Game.Logic.AI;

namespace GameServerScript.AI.Game
{

    public class SecondCopyNormalGame : APVEGameControl
    {
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 900)
            {
                return 3;
            }
            if (score > 0x339)
            {
                return 2;
            }
            if (score > 0x2d5)
            {
                return 1;
            }
            return 0;
        }

        public override void OnCreated()
        {
            base.OnCreated();
            base.Game.SetupMissions("2101, 2102");
            base.Game.TotalMissionCount = 2;
        }

        public override void OnGameOverAllSession()
        {
            base.OnGameOverAllSession();
        }

        public override void OnPrepated()
        {
            base.OnPrepated();
            base.Game.SessionId = 0;
        }
    }
}

