using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    ///// <summary>
    ///// 服务器定时任务种类
    ///// </summary>
    //public enum TaskScheduleType
    //{
    //    /// <summary>
    //    /// 每天
    //    /// </summary>
    //    Daily = 1
    //    //Weekly = 2,
    //    //Monthly = 3
    //}

    /*starting 10/31/2003 every 2h until 10/20/2004
starting 10/31/2003 between 10:00:00 and 11:00:00 every 20s until 12/01/2003
between 0:0:0 and 23:59:59 every 5m

SceduledAgents all have the following prop:

schedule
    Specifies the schedule that specifies when this Agent will wake up.

     Schedule Syntax:

     date = mm/dd/yyyy
     time = hh:mm:ss
     [starting startDateTime]
     [[every n[y|m|w|d]] | [on [[SU][MO][TU][WE][TH][FR][SA] | [startDay-endDay]]]]
     [[between startTime-endTime every n[h|m|s]] | [at time1,time2,time3]]
     [until endDateTime]
     */

    /// <summary>
    /// 服务器定时任务配置信息
    /// 利用NeoKennel的ScheduleAgent
    /// </summary>
    [Class(0, Name = "Feng.ServerTaskScheduleInfo", Table = "AD_Server_TaskSchedule", OptimisticLock = OptimisticLockMode.Version)]
    [Serializable]
    public class ServerTaskScheduleInfo : BaseADEntity
    {
        /// <summary>
        /// 定时设置字符串，格式同ScheduleAgent.schedule, 例如between 0:0:0-23:59:59 every 5s.
        /// 具体格式如下：
        /// date = mm/dd/yyyy
        /// time = hh:mm:ss
        ///[starting startDateTime]
        ///[[every n[y|m|w|d]] | [on [[SU][MO][TU][WE][TH][FR][SA] | [startDay-endDay]]]]
        ///[[between startTime-endTime every n[h|m|s]] | [at time1,time2,time3]]
        ///[until endDateTime]
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string Schedule
        {
            get;
            set;
        }

        /// <summary>
        ///要执行的<see cref="ProcessInfo"/>
        /// </summary>
        [ManyToOne(ForeignKey = "FK_ServerTaskScheduleInfo_Process")]
        public virtual ProcessInfo Process
        {
            get;
            set;
        }

        /// <summary>
        /// 服务器启动时启动
        /// </summary>
        [Property(NotNull = true)]
        public virtual bool StartAtStart
        {
            get;
            set;
        }
    }
}
