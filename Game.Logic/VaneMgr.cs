using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Logic
{

    public class VaneMgr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Dictionary<byte[], VaneInfo> m_infos;

        public static bool ByteAray() =>
            ReLoad();

        private static Dictionary<byte[], VaneInfo> LoadFromDatabase()
        {
            Console.WriteLine("Game.Logic.VaneMgr.LoadFromDatabase.teste1");
            Dictionary<byte[], VaneInfo> dictionary = new Dictionary<byte[], VaneInfo>();
            using (ProduceBussiness bussiness = new ProduceBussiness())
            {
                foreach (VaneInfo info in bussiness.GetAllVane())
                {
                    Console.WriteLine("Game.Logic.VaneMgr.LoadFromDatabase.teste2");
                    if (!dictionary.ContainsKey(info.bmp))
                    {
                        dictionary.Add(info.bmp, info);
                    }
                }
            }
            return dictionary;
        }

        public static bool ReLoad()
        {
            try
            {
                Dictionary<byte[], VaneInfo> dictionary = LoadFromDatabase();
                Console.WriteLine("Game.Logic.VaneMgr.ReLoad.teste1");
                if (dictionary.Values.Count > 0)
                {
                    Interlocked.Exchange<Dictionary<byte[], VaneInfo>>(ref m_infos, dictionary);
                    return true;
                }
            }
            catch (Exception exception)
            {
                log.Error("Vane Mgr byte[] error:", exception);
            }
            return false;
        }
    }
}

