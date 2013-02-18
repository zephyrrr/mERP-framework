using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Feng.NH
{
    /// <summary>
    /// 创建Repository的Factory
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        //private static Dictionary<string, IRepository> s_reps = new Dictionary<string, IRepository>();

        //private const int releaseTime = 600000;
        //private static void OnTimer(object stateInfo)
        //{
        //    foreach(IRepository rep in s_reps.Values)
        //    {
        //        if (rep.Session != null && rep.Session.IsConnected)
        //        {
        //            stateTimer.Change(releaseTime, Timeout.Infinite);
        //        }
        //        else
        //        {
        //            rep.Destruct();
        //        }
        //    }
        //}

        //static RepositoryFactory()
        //{
            //timerDelegate = new TimerCallback(OnTimer);
            //stateTimer = new Timer(timerDelegate, null, Timeout.Infinite, Timeout.Infinite);
        //}
        //private static TimerCallback timerDelegate;
        //private static Timer stateTimer;

        public IRepository GenerateRepository<T>()
        {
            return Feng.RepositoryFactoryExtention.GenerateRepository<T>(this);
        }

        /// <summary>
        /// 根据配置名生成Repository
        /// </summary>
        /// <param name="repCfgName"></param>
        /// <returns></returns>
        public IRepository GenerateRepository(string repCfgName = null)
        {
            //stateTimer.Change(releaseTime, Timeout.Infinite);

            //if (!s_reps.ContainsKey(cfgName))
            //{
            //    s_reps[cfgName] = new NH.Repository(cfgName);
            //    return s_reps[cfgName];
            //}
            //IRepository rep = s_reps[cfgName];
            // 如果某个Session是Open的，但需要不同Session调用（在某个Transaction内Load其他，但这个Load不保证一定在Transaction内），此时无法解决。
            //if (rep.Session == null || !rep.Session.IsOpen)
            //{
            //    s_reps[cfgName] = rep = new NH.Repository(cfgName);
            //    return rep;
            //}

            //if (!rep.Session.IsConnected)
            //{
            //    rep.Session.Reconnect();
            //}
            //return rep;
            return new NH.Repository(repCfgName);
        }

        public IRepository GetCurrentRepository()
        {
            return null;
        }
    }
}
