using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
    public class ESM1006 : ExplorationMission
    {
        public override void OnPrepareNewSession()
        {
            base.Game.AddLoadingFile(1, "bombs/12.swf", "tank.resource.bombs.Bomb12");
            base.Game.AddLoadingFile(1, "bombs/41.swf", "tank.resource.bombs.Bomb41");
            base.remoteIds = new int[] { 0x65, 0x66 };
            base.livingIds = new int[] { 0x3e9, 0x3ea };
            base.Game.LoadResources(base.livingIds);
            base.Game.LoadResources(base.remoteIds);
            base.Game.LoadNpcGameOverResources(base.livingIds);
            base.Game.LoadNpcGameOverResources(base.remoteIds);
            int[] mapIds = new int[] { 0x44f, 0x44d, 0x452 };
            base.Game.SetMap(base.GetMapId(mapIds, 0x44f));
            base.npcCreateParamSimple = new NpcCreateParam(2, 0, 2, 2, 15, 0);
            base.npcCreateParamNormal = new NpcCreateParam(3, 0, 3, 6, 15, 0);
            base.npcCreateParamHard = new NpcCreateParam(4, 0, 4, 6, 15, 0);
            base.npcCreateParamTerror = new NpcCreateParam(4, 0, 4, 6, 15, 0);
            base.ballIds = new Dictionary<int, int>();
            base.ballIds.Add(0x65, 12);
            base.ballIds.Add(0x66, 0x29);
            base.OnPrepareNewSession();
        }
    }
}

