using Bussiness;
using Bussiness.Managers;
using Game.Server.GameObjects;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Game.Server.Achievement
{

    public class AchievementInventory
    {
        private Dictionary<int, AchievementData> dictionary_0;
        private Dictionary<int, AchievementProcessInfo> dictionary_1;
        private GamePlayer gamePlayer_0;
        private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private int int_0;
        protected List<AchievementProcessInfo> m_changedAchs = new List<AchievementProcessInfo>();
        protected List<BaseAchievement> m_list;
        private object object_0;

        public AchievementInventory(GamePlayer player)
        {
            gamePlayer_0 = player;
            object_0 = new object();
            m_list = new List<BaseAchievement>();
            dictionary_1 = new Dictionary<int, AchievementProcessInfo>();
            dictionary_0 = new Dictionary<int, AchievementData>();
        }

        public bool AddAchievement(AchievementInfo info)
        {
            bool flag;
            try
            {
                if ((info != null) && (gamePlayer_0.PlayerCharacter.Grade >= info.NeedMinLevel) && (gamePlayer_0.PlayerCharacter.Grade <= info.NeedMaxLevel))
                {
                    bool flag2 = info.PreAchievementID == "0,";
                    if (!flag2)
                    {
                        char[] separator = new char[] { ',' };
                        string[] strArray = info.PreAchievementID.Split(separator);
                        int index = 0;
                        while (true)
                        {
                            flag2 = index < (strArray.Length - 1);
                            if (!flag2)
                            {
                                break;
                            }
                            if (method_2(Convert.ToInt32(strArray[index])))
                            {
                                index++;
                                continue;
                            }
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                ilog_0.Info(exception.InnerException);
            }
            if (!ReferenceEquals(FindAchievement(info.ID), null))
            {
                flag = false;
            }
            else
            {
                method_4();
                BaseAchievement achievement = new BaseAchievement(info, new AchievementData(), GetRealProcessAchievement());
                method_0(achievement);
                method_5();
                flag = true;
            }
            return flag;
        }

        public void AddAchievementPre()
        {
            using (List<AchievementInfo>.Enumerator enumerator = QuestMgr.GetAllAchievements().GetEnumerator())
            {
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                    AchievementInfo current = enumerator.Current;
                    if (!((FindAchievement(current.ID) != null) || method_2(current.ID)))
                    {
                        Console.WriteLine("Game.Server.Achievement.AddAchievementPre.teste6");
                        AddAchievement(current);
                    }
                }
            }
        }

        public void AddProcess(AchievementProcessInfo info)
        {
            lock (object_0)
            {
                method_4();
                if (!dictionary_1.ContainsKey(info.CondictionType))
                {
                    dictionary_1.Add(info.CondictionType, info);
                }
                method_5();
                gamePlayer_0.PlayerCharacter.AchievementProcess = method_7();
                OnAchievementsChanged(info);
            }
        }

        public BaseAchievement FindAchievement(int id)
        {
            using (List<BaseAchievement>.Enumerator enumerator = m_list.GetEnumerator())
            {
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                    BaseAchievement current = enumerator.Current;
                    if (current.Info.ID == id)
                    {
                        return current;
                    }
                }
            }
            return null;
        }

        public bool Finish(BaseAchievement baseAch)
        {
            bool flag;
            AchievementInfo info = baseAch.Info;
            AchievementData d = baseAch.Data;
            gamePlayer_0.BeginAllChanges();
            try
            {
                bool flag3 = baseAch.Finish(gamePlayer_0);
                if (flag3)
                {
                    Console.WriteLine("Game.Server.Achievement.AchievementInventory.Finish.GetAchievementGoods retornando nulo");
                    List<AchievementGoodsInfo> achievementGoods = QuestMgr.GetAchievementGoods(info);
                    using (List<AchievementGoodsInfo>.Enumerator enumerator = achievementGoods.GetEnumerator())
                    {
                        while (true)
                        {
                            flag3 = enumerator.MoveNext();
                            if (!flag3)
                            {
                                break;
                            }
                            AchievementGoodsInfo current = enumerator.Current;
                            if (current.RewardType == 1)
                            {
                                //gamePlayer_0.Rank.AddRank(current.RewardPara);
                            }
                        }
                    }
                    if (info.AchievementPoint != 0)
                    {
                        gamePlayer_0.AddAchievementPoint(info.AchievementPoint);
                    }
                    gamePlayer_0.Out.SendAchievementSuccess(d);
                    if (achievementGoods.Count > 0)
                    {
                        //gamePlayer_0.Out.SendUserRanks(gamePlayer_0.Rank.GetRank());
                    }
                    method_3(d);
                    gamePlayer_0.OnAchievementFinish(d);
                    method_1(d.AchievementID);
                    RemoveAchievement(baseAch);
                    flag = true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception exception)
            {
                if (ilog_0.IsErrorEnabled)
                {
                    ilog_0.Error("Achivement Finish：" + exception);
                }
                flag = false;
            }
            finally
            {
                gamePlayer_0.CommitAllChanges();
            }
            return flag;
        }

        public List<AchievementProcessInfo> GetProcessAchievement()
        {
            lock (dictionary_1)
            {
                return dictionary_1.Values.ToList<AchievementProcessInfo>();
            }
        }

        public Dictionary<int, AchievementProcessInfo> GetRealProcessAchievement()
        {
            lock (dictionary_1)
            {
                return dictionary_1;
            }
        }

        public List<AchievementData> GetSuccessAchievement()
        {
            lock (dictionary_0)
            {
                return dictionary_0.Values.ToList<AchievementData>();
            }
        }

        public AchievementData GetSuccessAchievement(int achid)
        {
            lock (dictionary_0)
            {
                return (!dictionary_0.ContainsKey(achid) ? null : dictionary_0[achid]);
            }
        }

        public void LoadFromDatabase(int playerId)
        {
            lock (object_0)
            {
                PlayerBussiness objA = new PlayerBussiness();
                try
                {
                    method_6(gamePlayer_0.PlayerCharacter.AchievementProcess);
                    AchievementData[] userAchievement = objA.GetUserAchievement(playerId);
                    int index = 0;
                    while (true)
                    {
                        if (index >= userAchievement.Length)
                        {
                            AddAchievementPre();
                            break;
                        }

                        AchievementData data = userAchievement[index];
                        if (data.IsComplete)
                        {   
                            dictionary_0.Add(data.AchievementID, data);
                        }
                        else
                        {
                            AchievementInfo singleAchievement = QuestMgr.GetSingleAchievement(data.AchievementID);

                            if (singleAchievement != null)
                            {
                                method_0(new BaseAchievement(singleAchievement, data, GetRealProcessAchievement()));
                            }
                        }
                        index++;
                    }
                }
                finally
                {
                    if (!ReferenceEquals(objA, null))
                    {
                        objA.Dispose();
                    }
                }
            }
        }

        private bool method_0(BaseAchievement baseAchievement_0)
        {
            lock (m_list)
            {
                m_list.Add(baseAchievement_0);
            }
            baseAchievement_0.AddToPlayer(gamePlayer_0);
            if (baseAchievement_0.CanCompleted(gamePlayer_0))
            {
                Finish(baseAchievement_0);
            }
            return true;
        }

        private void method_1(int int_1)
        {
            using (List<AchievementInfo>.Enumerator enumerator = QuestMgr.GetAllAchievements().GetEnumerator())
            {
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                    AchievementInfo current = enumerator.Current;
                    if (current.PreAchievementID != "0,")
                    {
                        char[] separator = new char[] { ',' };
                        string[] strArray = current.PreAchievementID.Split(separator);
                        int index = 0;
                        while (true)
                        {
                            if (index >= strArray.Length)
                            {
                                break;
                            }
                            string s = strArray[index];
                            if ((s != null) && (s != ""))
                            {
                                AchievementInfo singleAchievement = QuestMgr.GetSingleAchievement(int.Parse(s));
                                if ((singleAchievement != null) && (singleAchievement.ID == int_1))
                                {
                                    AddAchievement(current);
                                    break;
                                }
                            }
                            index++;
                        }
                    }
                }
            }
        }

        private bool method_2(int int_1)
        {
            lock (dictionary_0)
            {
                return dictionary_0.ContainsKey(int_1);
            }
        }

        private void method_3(AchievementData achievementData_0)
        {
            lock (dictionary_0)
            {
                if (!dictionary_0.ContainsKey(achievementData_0.AchievementID))
                {
                    dictionary_0.Add(achievementData_0.AchievementID, achievementData_0);
                }
            }
        }

        private void method_4()
        {
            Interlocked.Increment(ref int_0);
        }

        private void method_5()
        {
            int num = Interlocked.Decrement(ref int_0);
            if (num < 0)
            {
                if (ilog_0.IsErrorEnabled)
                {
                    ilog_0.Error("Inventory changes counter is bellow zero (forgot to use BeginChanges?)!\n\n" + Environment.StackTrace);
                }
                Thread.VolatileWrite(ref int_0, 0);
            }
            if ((num <= 0) && (m_changedAchs.Count > 0))
            {
                UpdateChangedAchievements();
            }
        }

        private void method_6(string string_0)
        {
            bool flag = (string_0 == null) || (string_0 == "");
            if (!flag)
            {
                lock (dictionary_1)
                {
                    char[] separator = new char[] { '|' };
                    string[] strArray2 = string_0.Split(separator);
                    int index = 0;
                    while (true)
                    {
                        flag = index < strArray2.Length;
                        if (!flag)
                        {
                            break;
                        }
                        string str = strArray2[index];
                        if ((str != null) && (str != ""))
                        {
                            string[] strArray = str.Split(new char[] { ',' });
                            if (strArray.Length >= 2)
                            {
                                int key = int.Parse(strArray[0]);
                                int num2 = int.Parse(strArray[1]);
                                if (!dictionary_1.ContainsKey(key))
                                {
                                    dictionary_1.Add(key, new AchievementProcessInfo(key, num2));
                                }
                            }
                        }
                        index++;
                    }
                }
            }
        }

        private string method_7()
        {
            string str;
            List<string> list = new List<string>();
            lock (dictionary_1)
            {
                bool flag = dictionary_1.Count <= 0;
                if (flag)
                {
                    str = "";
                }
                else
                {
                    using (Dictionary<int, AchievementProcessInfo>.ValueCollection.Enumerator enumerator = dictionary_1.Values.GetEnumerator())
                    {
                        while (true)
                        {
                            flag = enumerator.MoveNext();
                            if (!flag)
                            {
                                break;
                            }
                            AchievementProcessInfo current = enumerator.Current;
                            list.Add(current.CondictionType + "," + current.Value);
                        }
                    }
                    str = string.Join("|", list.ToArray());
                }
            }
            return str;
        }

        protected void OnAchievementsChanged(AchievementProcessInfo ach)
        {
            if (!m_changedAchs.Contains(ach))
            {
                m_changedAchs.Add(ach);
            }
            if ((int_0 <= 0) && (m_changedAchs.Count > 0))
            {
                UpdateChangedAchievements();
            }
        }

        public bool RemoveAchievement(BaseAchievement ach)
        {
            ach.RemoveFromPlayer(gamePlayer_0);
            return true;
        }

        public void SaveToDatabase()
        {
            lock (object_0)
            {
                PlayerBussiness objA = new PlayerBussiness();
                try
                {
                    using (Dictionary<int, AchievementData>.ValueCollection.Enumerator enumerator = dictionary_0.Values.GetEnumerator())
                    {
                        while (true)
                        {
                            if (!enumerator.MoveNext())
                            {
                                break;
                            }
                            AchievementData current = enumerator.Current;
                            if (current.IsDirty)
                            {
                                objA.UpdateDbAchievementDataInfo(current);
                            }
                        }
                    }
                }
                finally
                {
                    if (!ReferenceEquals(objA, null))
                    {
                        objA.Dispose();
                    }
                }
            }
        }

        public void Update(BaseAchievement ach)
        {
        }

        public void UpdateChangedAchievements()
        {
            gamePlayer_0.Out.SendUpdateAchievementInfo(m_changedAchs.ToList());
            m_changedAchs.Clear();
        }

        public void UpdateProcess(BaseCondition info)
        {
            if (!ReferenceEquals(info, null))
            {
                AchievementProcessInfo info2 = new AchievementProcessInfo(info.Info.CondictionType, info.Value);
                UpdateProcess(info2);
            }
        }

        public void UpdateProcess(AchievementProcessInfo info)
        {
            lock (object_0)
            {
                bool flag = true;
                method_4();
                if (!dictionary_1.ContainsKey(info.CondictionType))
                {
                    dictionary_1.Add(info.CondictionType, info);
                }
                else if (dictionary_1[info.CondictionType].Value < info.Value)
                {
                    dictionary_1[info.CondictionType] = info;
                }
                else
                {
                    flag = false;
                }
                method_5();
                if (flag)
                {
                    gamePlayer_0.PlayerCharacter.AchievementProcess = method_7();
                    OnAchievementsChanged(info);
                }
            }
        }
    }
}

