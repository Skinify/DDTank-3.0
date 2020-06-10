using Game.Logic.AI;

namespace GameServerScript.AI.Game
{

    public class EvilTribeHardGame : APVEGameControl
    {
        public override int CalculateScoreGrade(int score)
        {
            if (score > 800)
            {
                return 3;
            }
            if (score > 0x2d5)
            {
                return 2;
            }
            if (score > 650)
            {
                return 1;
            }
            return 0;
        }

        public override void OnCreated()
        {
            base.Game.SetupMissions("1272,1273,1276,1277");
            base.Game.TotalMissionCount = 4;
        }

        public override void OnGameOverAllSession()
        {
        }

        public override void OnPrepated()
        {
            base.Game.SessionId = 0;
        }
    }
}

