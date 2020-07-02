using Bussiness.Managers;
using Game.Server.GameObjects;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Achievement
{

    public class BaseAchievement
    {
        private AchievementData achievementData_0;
        private AchievementInfo achievementInfo_0;
        private GamePlayer gamePlayer_0;
        private List<BaseCondition> list_0;

        public BaseAchievement(AchievementInfo info, AchievementData data)
        {
            this.CreateBaseAchievement(info, data, null);
        }

        public BaseAchievement(AchievementInfo info, AchievementData data, Dictionary<int, AchievementProcessInfo> processInfo)
        {
            this.CreateBaseAchievement(info, data, processInfo);
        }

        public void AddToPlayer(GamePlayer player)
        {
            this.gamePlayer_0 = player;
            this.achievementData_0.UserID = player.PlayerCharacter.ID;
            if (!this.achievementData_0.IsComplete)
            {
                this.method_0(player);
            }
        }

        public bool CanCompleted(GamePlayer player)
        {
            bool flag;
            bool flag2 = !this.achievementData_0.IsComplete;
            if (flag2)
            {
                using (List<BaseCondition>.Enumerator enumerator = this.list_0.GetEnumerator())
                {
                    while (true)
                    {
                        flag2 = enumerator.MoveNext();
                        if (!flag2)
                        {
                            break;
                        }
                        if (!enumerator.Current.IsCompleted(player))
                        {
                            return false;
                        }
                    }
                }
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public void CreateBaseAchievement(AchievementInfo info, AchievementData data, Dictionary<int, AchievementProcessInfo> processInfo)
        {
            Console.WriteLine("Game.Server.Achievement.BaseAchievement.CreateBaseAchievement.teste1");
            this.achievementInfo_0 = info;
            this.achievementData_0 = data;
            this.achievementData_0.AchievementID = achievementInfo_0.ID;
            this.list_0 = new List<BaseCondition>();
            Console.WriteLine("Game.Server.Achievement.CreateBaseAchievement.teste2");
            try
            {
                using (List<AchievementCondictionInfo>.Enumerator enumerator = QuestMgr.GetAchievementCondiction(info).GetEnumerator())
                {
                    Console.WriteLine("Game.Server.Achievement.CreateBaseAchievement.teste3");
                    while (true)
                    {
                        Console.WriteLine("Game.Server.Achievement.CreateBaseAchievement.teste4");
                        if (!enumerator.MoveNext())
                        {
                            break;
                        }
                        AchievementCondictionInfo current = enumerator.Current;
                        int num = 0;
                        if ((processInfo != null) && processInfo.ContainsKey(current.CondictionType))
                        {
                            num = processInfo[current.CondictionType].Value;
                        }
                        BaseCondition objA = BaseCondition.CreateCondition(this, current, num);
                        if (!ReferenceEquals(objA, null))
                        {
                            this.list_0.Add(objA);
                        }
                    }
                }
            }catch(Exception erro)
            {
                Console.WriteLine(erro);
            }
        }

        public bool Finish(GamePlayer player)
        {
            bool flag;
            bool flag2 = this.CanCompleted(player);
            if (flag2)
            {
                using (List<BaseCondition>.Enumerator enumerator = this.list_0.GetEnumerator())
                {
                    while (true)
                    {
                        flag2 = enumerator.MoveNext();
                        if (!flag2)
                        {
                            break;
                        }
                        if (!enumerator.Current.Finish(player))
                        {
                            return false;
                        }
                    }
                }
                this.achievementData_0.IsComplete = true;
                this.method_1(player);
                this.achievementData_0.DateComplete = DateTime.Now;
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public BaseCondition GetConditionById(int id)
        {
            using (List<BaseCondition>.Enumerator enumerator = this.list_0.GetEnumerator())
            {
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                    BaseCondition current = enumerator.Current;
                    if (current.Info.CondictionID == id)
                    {
                        return current;
                    }
                }
            }
            return null;
        }

        private void method_0(GamePlayer gamePlayer_1)
        {
            using (List<BaseCondition>.Enumerator enumerator = this.list_0.GetEnumerator())
            {
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                    enumerator.Current.AddTrigger(gamePlayer_1);
                }
            }
        }

        private void method_1(GamePlayer gamePlayer_1)
        {
            using (List<BaseCondition>.Enumerator enumerator = this.list_0.GetEnumerator())
            {
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                    enumerator.Current.RemoveTrigger(gamePlayer_1);
                }
            }
        }

        public void RemoveFromPlayer(GamePlayer player)
        {
            if (this.achievementData_0.IsComplete)
            {
                this.method_1(player);
            }
            this.gamePlayer_0 = null;
        }

        public void SaveData()
        {
            bool flag = ReferenceEquals(this.gamePlayer_0, null);
            if (!flag)
            {
                using (List<BaseCondition>.Enumerator enumerator = this.list_0.GetEnumerator())
                {
                    while (true)
                    {
                        flag = enumerator.MoveNext();
                        if (!flag)
                        {
                            break;
                        }
                        BaseCondition current = enumerator.Current;
                        this.gamePlayer_0.AchievementInventory.UpdateProcess(current);
                    }
                }
            }
        }

        public void Update()
        {
            this.SaveData();
            if (!ReferenceEquals(this.gamePlayer_0, null))
            {
                this.gamePlayer_0.AchievementInventory.Update(this);
                if (this.CanCompleted(this.gamePlayer_0))
                {
                    this.gamePlayer_0.AchievementInventory.Finish(this);
                }
            }
        }

        public AchievementData Data =>
            this.achievementData_0;

        public AchievementInfo Info =>
            this.achievementInfo_0;
    }
}

