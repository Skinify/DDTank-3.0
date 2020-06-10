using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
    public class ESM1003 : ExplorationMission
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
            int[] mapIds = new int[] { 0x448, 0x44a, 0x457, 0x450, 0x44c, 0x446 };
            base.Game.SetMap(base.GetMapId(mapIds, 0x448));
            base.npcCreateParamSimple = new NpcCreateParam(2, 8, 1, 4, 5, 10);
            base.npcCreateParamNormal = new NpcCreateParam(3, 12, 1, 4, 5, 10);
            base.npcCreateParamHard = new NpcCreateParam(4, 0x10, 1, 4, 5, 10);
            base.npcCreateParamTerror = new NpcCreateParam(5, 20, 1, 4, 5, 10);
            base.ballIds = new Dictionary<int, int>();
            base.ballIds.Add(0x65, 12);
            base.ballIds.Add(0x66, 0x29);
            base.OnPrepareNewSession();
        }
    }
}

