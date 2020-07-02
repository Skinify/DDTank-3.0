using Game.Server.GameObjects;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Reflection;

namespace Game.Server.Achievement
{
    public class BaseCondition
    {
        private BaseAchievement baseAchievement_0;
        private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private int int_0;
        protected AchievementCondictionInfo m_info;

        public BaseCondition(BaseAchievement ach, AchievementCondictionInfo info, int value)
        {
            this.baseAchievement_0 = ach;
            this.m_info = info;
            this.int_0 = value;
        }

        public virtual void AddTrigger(GamePlayer player)
        {
        }

        public static BaseCondition CreateCondition(BaseAchievement ach, AchievementCondictionInfo info, int value)
        {
            BaseCondition condition;
            switch (info.CondictionType)
            {
                case 1:
                    condition = new PropertisCharacterCondition(ach, info, value, "attack");
                    break;

                case 2:
                    condition = new PropertisCharacterCondition(ach, info, value, "defence");
                    break;

                case 3:
                    condition = new PropertisCharacterCondition(ach, info, value, "agility");
                    break;

                case 4:
                    condition = new PropertisCharacterCondition(ach, info, value, "luck");
                    break;

                case 5:
                {
                    int[] arrayId = new int[] { 0xfa9, 0x100d, 0x1071, 0x10d5 };
                    condition = new GameKillingBossCondition(ach, info, value, arrayId);
                    break;
                }
                case 6:
                    condition = new GameOverPassCondition(ach, info, 6, value);
                    break;

                case 7:
                    condition = new GameKillingBossCondition(ach, info, value, new int[] { 0xcf5 });
                    break;

                case 8:
                    condition = new GameKillingBossCondition(ach, info, value, new int[] { 0x10d5 });
                    break;

                case 9:
                    condition = new PropertisCharacterCondition(ach, info, value, "fightpower");
                    break;

                case 10:
                    condition = new LevelUpgradeCondition(ach, info, value);
                    break;

                case 11:
                    condition = new FightCompleteCondition(ach, info, value);
                    break;

                case 13:
                    condition = new OnlineTimeCondition(ach, info, value);
                    break;

                case 14:
                    condition = new FightMatchWinCondition(ach, info, value);
                    break;

                case 15:
                    condition = new GuildFightWinCondition(ach, info, value);
                    break;

                case 0x13:
                    condition = new FightKillPlayerCondition(ach, info, value);
                    break;

                case 0x15:
                    condition = new QuestGreenFinishCondition(ach, info, value);
                    break;

                case 0x16:
                    condition = new QuestDailyFinishCondition(ach, info, value);
                    break;

                case 0x18:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x19:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x1a:
                    condition = new GameKillingBossCondition(ach, info, value, new int[] { 0x7d3, 0x837 });
                    break;

                case 0x1b:
                {
                    int[] arrayId = new int[] { 0x3ee, 0x452, 0x4b6, 0x51a };
                    condition = new GameKillingBossCondition(ach, info, value, arrayId);
                    break;
                }
                case 0x1c:
                    condition = new GameOverPassCondition(ach, info, 5, value);
                    break;

                case 0x1d:
                    condition = new GameOverPassCondition(ach, info, 4, value);
                    break;

                case 30:
                    condition = new GameOverPassCondition(ach, info, 3, value);
                    break;

                case 0x1f:
                {
                    int[] arrayId = new int[] { 0xbc9, 0xc2d, 0xc91, 0xcf5 };
                    condition = new GameKillingBossCondition(ach, info, value, arrayId);
                    break;
                }
                case 0x20:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x21:
                    condition = new HotSpingEnterCondition(ach, info, value);
                    break;

                case 0x22:
                    condition = new UsingItemCondition(ach, info, 0x2724, value);
                    break;

                case 0x23:
                    condition = new UsingItemCondition(ach, info, 0x2726, value);
                    break;

                case 0x24:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x25:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x26:
                    condition = new GoldCollectionCondition(ach, info, value);
                    break;

                case 0x27:
                    condition = new GiftTokenCollectionCondition(ach, info, value);
                    break;

                case 40:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x29:
                    condition = new FightOneBloodIsWinCondition(ach, info, value);
                    break;

                case 0x2a:
                    condition = new ItemEquipCondition(ach, info, value, 0x426a);
                    break;

                case 0x2d:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x2f:
                    condition = new UseBigBugleCondition(ach, info, value);
                    break;

                case 0x30:
                    condition = new UseSmaillBugleCondition(ach, info, value);
                    break;

                case 50:
                    condition = new FightAddOfferCondition(ach, info, value);
                    break;

                case 0x33:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x34:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x35:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x36:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x37:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x38:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x3b:
                    condition = new CompleteQuestGoodManCondtion(ach, info, value);
                    break;

                case 60:
                {
                    int[] arrayId = new int[] { 0x51c, 0x4b8, 0x454, 0x3f0 };
                    condition = new GameKillingBossCondition(ach, info, value, arrayId);
                    break;
                }
                case 0x3d:
                    condition = new GameKillingBossCondition(ach, info, value, new int[] { 0x517 });
                    break;

                case 0x3e:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x41:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x42:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x43:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x44:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x48:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x49:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x4a:
                    condition = new GClass11(ach, info, value);
                    break;

                case 0x4b:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x4c:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x4d:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x4e:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x4f:
                {
                    int[] arrayId = new int[] { 0x1b77, 0x1bdb, 0x1c3f };
                    condition = new GameKillingBossCondition(ach, info, value, arrayId);
                    break;
                }
                case 80:
                    condition = new GameOverPassCondition(ach, info, 2, value);
                    break;

                case 0x51:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x52:
                    condition = new GameOverPassCondition(ach, info, 1, value);
                    break;

                case 0x53:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x54:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x55:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x56:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x57:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x58:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x59:
                {
                    int[] arrayId = new int[] { 0x140b, 0x146f, 0x14d3 };
                    condition = new GameKillingBossCondition(ach, info, value, arrayId);
                    break;
                }
                case 90:
                    condition = new GameOverPassCondition(ach, info, 3, value);
                    break;

                case 0x5b:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x5c:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x5d:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x5e:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                case 0x5f:
                    condition = new DefaultCondition(ach, info, value);
                    break;

                default:
                    if (ilog_0.IsErrorEnabled)
                    {
                        ilog_0.Error($"Can't find achievement condition : {info.CondictionType}");
                    }
                    condition = null;
                    break;
            }
            return condition;
        }

        public virtual bool Finish(GamePlayer player) => 
            true;

        public virtual bool IsCompleted(GamePlayer player) => 
            false;

        public virtual void RemoveTrigger(GamePlayer player)
        {
        }

        public AchievementCondictionInfo Info =>
            this.m_info;

        public int Value
        {
            get => 
                this.int_0;
            set
            {
                if (this.int_0 != value)
                {
                    this.int_0 = value;
                    this.baseAchievement_0.Update();
                }
            }
        }
    }
}

