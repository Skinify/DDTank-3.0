﻿using Game.Logic;
using Game.Server.GameObjects;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Quests
{
    public class AchievementCondition : BaseCondition
    {
        public AchievementCondition(BaseQuest quest, QuestConditionInfo info, int value) : base(quest, info, value)
        {
        }

        public override void AddTrigger(GamePlayer player)
        {
            base.Value = base.m_info.Para2;
        }

        public override bool IsCompleted(GamePlayer player) =>
            base.Value >= base.m_info.Para2;

        private void player_MissionOver(AbstractGame game, int missionId, int turnCount)
        {
        }

        public override void RemoveTrigger(GamePlayer player)
        {
        }
    }
}