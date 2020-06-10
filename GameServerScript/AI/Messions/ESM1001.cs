using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
    public class ESM1001 : ExplorationMission
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
            int[] mapIds = new int[] { 0x445, 0x444, 0x454, 0x441, 0x44b };
            base.Game.SetMap(base.GetMapId(mapIds, 0x445));
            base.npcCreateParamSimple = new NpcCreateParam(0, 8, 0, 4, 0, 15);
            base.npcCreateParamNormal = new NpcCreateParam(0, 12, 0, 4, 0, 15);
            base.npcCreateParamHard = new NpcCreateParam(0, 0x10, 0, 4, 0, 15);
            base.npcCreateParamTerror = new NpcCreateParam(0, 20, 0, 4, 0, 15);
            base.ballIds = new Dictionary<int, int>();
            base.ballIds.Add(0x65, 12);
            base.ballIds.Add(0x66, 0x29);
            base.OnPrepareNewSession();
        }
    }
}

