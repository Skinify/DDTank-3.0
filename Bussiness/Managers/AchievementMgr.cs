using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
    public class AchievementMgr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Dictionary<int, AchievementInfo> m_achievement = new Dictionary<int, AchievementInfo>();
        private static Dictionary<int, List<AchievementConditionInfo>> m_achievementCondition = new Dictionary<int, List<AchievementConditionInfo>>();
        private static Dictionary<int, List<AchievementRewardInfo>> m_achievementReward = new Dictionary<int, List<AchievementRewardInfo>>();
        private static Hashtable m_distinctCondition = new Hashtable();
        private static Dictionary<int, List<ItemRecordTypeInfo>> m_itemRecordType = new Dictionary<int, List<ItemRecordTypeInfo>>();
        private static Hashtable m_ItemRecordTypeInfo = new Hashtable();
        private static Dictionary<int, List<int>> m_recordLimit = new Dictionary<int, List<int>>();



        public static List<AchievementConditionInfo> GetAchievementCondition(AchievementInfo info) =>
            (!m_achievementCondition.ContainsKey(info.ID) ? null : m_achievementCondition[info.ID]);

        public static List<AchievementRewardInfo> GetAchievementReward(AchievementInfo info) =>
            (!m_achievementReward.ContainsKey(info.ID) ? null : m_achievementReward[info.ID]);

        public static int GetNextLimit(int recordType, int recordValue)
        {
            int num2;
            bool flag = !m_recordLimit.ContainsKey(recordType);
            if (flag)
            {
                num2 = 0x7fff_ffff;
            }
            else
            {
                using (List<int>.Enumerator enumerator = m_recordLimit[recordType].GetEnumerator())
                {
                    while (true)
                    {
                        flag = enumerator.MoveNext();
                        if (!flag)
                        {
                            break;
                        }
                        int current = enumerator.Current;
                        if (current > recordValue)
                        {
                            return current;
                        }
                    }
                }
                num2 = 0x7fff_ffff;
            }
            return num2;
        }

        public static AchievementInfo GetSingleAchievement(int id) =>
            (!m_achievement.ContainsKey(id) ? null : m_achievement[id]);

        public static bool Init() =>
            Reload();

        public static Dictionary<int, List<AchievementConditionInfo>> LoadAchievementConditionInfoDB(Dictionary<int, AchievementInfo> achievementInfos)
        {
            Dictionary<int, List<AchievementConditionInfo>> dictionary = new Dictionary<int, List<AchievementConditionInfo>>();
            ProduceBussiness objA = new ProduceBussiness();
            try
            {
                bool flag;
                AchievementConditionInfo[] aLlAchievementCondition = objA.GetALlAchievementCondition();
                using (Dictionary<int, AchievementInfo>.ValueCollection.Enumerator enumerator = achievementInfos.Values.GetEnumerator())
                {
                    Func<AchievementConditionInfo, bool> func3 = null;
                    Func<AchievementConditionInfo, bool> func = null;
                    Func<AchievementConditionInfo, bool> func2 = null;
                    while (true)
                    {
                        flag = enumerator.MoveNext();
                        if (!flag)
                        {
                            break;
                        }
                        AchievementInfo achievementInfo = enumerator.Current;
                        if (func2 == null)
                        {
                            if (func == null)
                            {
                                if (func3 == null)
                                {
                                    //<> c__DisplayClass2 class2;
                                    //func3 = new Func<AchievementConditionInfo, bool> (class, this.< LoadAchievementConditionInfoDB > b__0);
                                }
                                func = func3;
                            }
                            func2 = func;
                        }
                        IEnumerable<AchievementConditionInfo> source = Enumerable.Where<AchievementConditionInfo>(aLlAchievementCondition, func2);
                        dictionary.Add(achievementInfo.ID, source.ToList<AchievementConditionInfo>());
                        flag = ReferenceEquals(source, null);
                        if (!flag)
                        {
                            IEnumerator<AchievementConditionInfo> enumerator2 = source.GetEnumerator();
                            try
                            {
                                while (true)
                                {
                                    flag = enumerator2.MoveNext();
                                    if (!flag)
                                    {
                                        break;
                                    }
                                    AchievementConditionInfo current = enumerator2.Current;
                                    if (!m_distinctCondition.Contains(current.CondictionType))
                                    {
                                        m_distinctCondition.Add(current.CondictionType, current.CondictionType);
                                    }
                                }
                            }
                            finally
                            {
                                if (!ReferenceEquals(enumerator2, null))
                                {
                                    enumerator2.Dispose();
                                }
                            }
                        }
                    }
                }
                AchievementConditionInfo[] infoArray2 = aLlAchievementCondition;
                int index = 0;
                while (true)
                {
                    flag = index < infoArray2.Length;
                    if (!flag)
                    {
                        using (Dictionary<int, List<int>>.KeyCollection.Enumerator enumerator3 = m_recordLimit.Keys.GetEnumerator())
                        {
                            while (true)
                            {
                                flag = enumerator3.MoveNext();
                                if (!flag)
                                {
                                    break;
                                }
                                int current = enumerator3.Current;
                                m_recordLimit[current].Sort();
                            }
                        }
                        break;
                    }
                    AchievementConditionInfo info2 = infoArray2[index];
                    int condictionType = info2.CondictionType;
                    int item = info2.Condiction_Para2;
                    if (!m_recordLimit.ContainsKey(condictionType))
                    {
                        m_recordLimit.Add(condictionType, new List<int>());
                    }
                    if (!m_recordLimit[condictionType].Contains(item))
                    {
                        m_recordLimit[condictionType].Add(item);
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
            return dictionary;
        }

        public static Dictionary<int, AchievementInfo> LoadAchievementInfoDB()
        {
            Dictionary<int, AchievementInfo> dictionary = new Dictionary<int, AchievementInfo>();
            ProduceBussiness objA = new ProduceBussiness();
            try
            {
                AchievementInfo[] aLlAchievement = objA.GetALlAchievement();
                int index = 0;
                while (true)
                {
                    if (index >= aLlAchievement.Length)
                    {
                        break;
                    }
                    AchievementInfo info = aLlAchievement[index];
                    if (!dictionary.ContainsKey(info.ID))
                    {
                        dictionary.Add(info.ID, info);
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
            return dictionary;
        }

        public static Dictionary<int, List<AchievementRewardInfo>> LoadAchievementRewardInfoDB(Dictionary<int, AchievementInfo> achievementInfos)
        {
            Dictionary<int, List<AchievementRewardInfo>> dictionary = new Dictionary<int, List<AchievementRewardInfo>>();
            ProduceBussiness objA = new ProduceBussiness();
            try
            {
                AchievementRewardInfo[] aLlAchievementReward = objA.GetALlAchievementReward();
                using (Dictionary<int, AchievementInfo>.ValueCollection.Enumerator enumerator = achievementInfos.Values.GetEnumerator())
                {
                    Func<AchievementRewardInfo, bool> func3 = null;
                    Func<AchievementRewardInfo, bool> func = null;
                    Func<AchievementRewardInfo, bool> func2 = null;
                    while (true)
                    {
                        if (!enumerator.MoveNext())
                        {
                            break;
                        }
                        AchievementInfo achievementInfo = enumerator.Current;
                        if (func2 == null)
                        {
                            if (func == null)
                            {
                                if (func3 == null)
                                {
                                    //<> c__DisplayClass6 class2;
                                    //func3 = new Func<AchievementRewardInfo, bool>(class2, this.< LoadAchievementRewardInfoDB > b__4);
                                }
                                func = func3;
                            }
                            func2 = func;
                        }
                        IEnumerable<AchievementRewardInfo> source = Enumerable.Where<AchievementRewardInfo>(aLlAchievementReward, func2);
                        dictionary.Add(achievementInfo.ID, source.ToList<AchievementRewardInfo>());
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
            return dictionary;
        }

        public static void LoadItemRecordTypeInfoDB()
        {
            ProduceBussiness objA = new ProduceBussiness();
            try
            {
                ItemRecordTypeInfo[] allItemRecordType = objA.GetAllItemRecordType();
                int index = 0;
                while (true)
                {
                    if (index >= allItemRecordType.Length)
                    {
                        break;
                    }
                    ItemRecordTypeInfo info = allItemRecordType[index];
                    if (!m_ItemRecordTypeInfo.Contains(info.RecordID))
                    {
                        m_ItemRecordTypeInfo.Add(info.RecordID, info.Name);
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

        public static bool Reload()
        {
            try
            {
                LoadItemRecordTypeInfoDB();
                Dictionary<int, AchievementInfo> achievementInfos = LoadAchievementInfoDB();
                Dictionary<int, List<AchievementConditionInfo>> dictionary2 = LoadAchievementConditionInfoDB(achievementInfos);
                Dictionary<int, List<AchievementRewardInfo>> dictionary3 = LoadAchievementRewardInfoDB(achievementInfos);
                if (achievementInfos.Count > 0)
                {
                    Interlocked.Exchange<Dictionary<int, AchievementInfo>>(ref m_achievement, achievementInfos);
                    Interlocked.Exchange<Dictionary<int, List<AchievementConditionInfo>>>(ref m_achievementCondition, dictionary2);
                    Interlocked.Exchange<Dictionary<int, List<AchievementRewardInfo>>>(ref m_achievementReward, dictionary3);
                }
                return true;
            }
            catch (Exception exception)
            {
                log.Error("AchievementMgr", exception);
            }
            return false;
        }

        public static Dictionary<int, AchievementInfo> Achievement
        {
            get { return m_achievement; }
        }

        public static Hashtable ItemRecordType
        {
            get { return m_ItemRecordTypeInfo; }
        }

    }
        
}
