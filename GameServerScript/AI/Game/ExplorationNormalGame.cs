namespace GameServerScript.AI.Game
{
    public class ExplorationNormalGame : ExplorationGame
    {
        public override void OnCreated()
        {
            base.totalMissionCount = 5;
            base.missionIds = "1001,1002,1003,1004,1005";
            base.OnCreated();
        }
    }
}

