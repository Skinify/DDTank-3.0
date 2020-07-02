﻿namespace Game.Server.Achievement
{
    using Game.Server.GameObjects;
    using SqlDataProvider.Data;
    using System;

    public class QuestGreenFinishCondition : BaseCondition
    {
        public QuestGreenFinishCondition(BaseAchievement quest, AchievementCondictionInfo info, int value) : base(quest, info, value)
        {
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.OnQuestFinishEvent += new GamePlayer.PlayerQuestFinish(this.method_0);
        }

        public override bool IsCompleted(GamePlayer player) => 
            (base.Value >= base.m_info.Condiction_Para2);

        private void method_0(QuestDataInfo questDataInfo_0, QuestInfo questInfo_0)
        {
            if (questDataInfo_0.RandDobule > 1)
            {
                int num = base.Value;
                base.Value = num + 1;
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.OnQuestFinishEvent -= new GamePlayer.PlayerQuestFinish(this.method_0);
        }
    }
}
