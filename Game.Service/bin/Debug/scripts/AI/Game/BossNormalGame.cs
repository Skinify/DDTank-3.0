﻿using Game.Logic.AI;

namespace GameServerScript.AI.Game
{

    public class BossNormalGame : APVEGameControl
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
            base.Game.SetupMissions("1076");
            base.Game.TotalMissionCount = 1;
        }

        public override void OnGameOverAllSession()
        {
        }

        public override void OnPrepated()
        {
        }
    }
}

