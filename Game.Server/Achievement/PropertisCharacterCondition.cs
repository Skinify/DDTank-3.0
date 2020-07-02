namespace Game.Server.Achievement
{
    using Game.Server.GameObjects;
    using SqlDataProvider.Data;
    using System;

    public class PropertisCharacterCondition : BaseCondition
    {
        private string string_0;

        public PropertisCharacterCondition(BaseAchievement quest, AchievementCondictionInfo info, int value, string type) : base(quest, info, value)
        {
            this.string_0 = type;
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.OnPropertisChangeEvent += new GamePlayer.PlayerPropertisChange(this.method_0);
        }

        public override bool IsCompleted(GamePlayer player) => 
            (base.Value >= base.m_info.Condiction_Para2);

        private void method_0(PlayerInfo playerInfo_0)
        {
            string objA = this.string_0;
            if (!ReferenceEquals(objA, null))
            {
                if (objA == "attack")
                {
                    base.Value = playerInfo_0.Attack;
                }
                else if (objA == "agility")
                {
                    base.Value = playerInfo_0.Agility;
                }
                else if (objA == "luck")
                {
                    base.Value = playerInfo_0.Luck;
                }
                else if (objA == "defence")
                {
                    base.Value = playerInfo_0.Defence;
                }
                else if (objA == "fightpower")
                {
                    base.Value = playerInfo_0.FightPower;
                }
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.OnPropertisChangeEvent -= new GamePlayer.PlayerPropertisChange(this.method_0);
        }
    }
}

