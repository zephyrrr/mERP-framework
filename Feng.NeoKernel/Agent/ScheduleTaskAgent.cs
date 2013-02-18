using System;
using System.Collections.Generic;
using System.Text;
using com.neokernel.nk;
using Feng.Windows.Utils;

namespace Feng.NeoKernel.Agent
{
    /// <summary>
    /// 执行ServerTaskSchedule的Agent
    /// </summary>
	/* date = mm/dd/yyyy
time = hh:mm:ss

[starting startDateTime] [every n[y|m|w|d]] | [on [[SU][MO][TU][WE][TH][FI][SA]] | [startDay-endDay]]]
[between startTime-endTime every n[h|m|s]] |
[at time1, time2, time3, ...]
[until endDateTime]

[starting startDateTime]
[[every n[y|m|w|d]] | [on [[SU][MO][TU][WE][TH][FR][SA] | [startDay-endDay]]]]
[[between startTime-endTime every n[h|m|s]] | [at time1,time2,time3]]
[until endDateTime] */
    public class ScheduleTaskAgent : ScheduledAgent
    {
        public override void start()
        {
            if (this.getBoolean("startAtStart"))
            {
                string processId = this.getString("process");
                println("process " + processId + " is executing at start!");

                ExecuteProcess(processId);
            }

            base.start();
        }

        private Dictionary<string, bool> m_processExecuting = new Dictionary<string, bool>();
        private void ExecuteProcess(string processId)
        {
            if (!string.IsNullOrEmpty(processId))
            {
                ProcessInfo info;
                lock (m_processExecuting)
                {
                    if (m_processExecuting.ContainsKey(processId) && m_processExecuting[processId])
                    {
                        println("process " + processId + " is still executing!");
                        return;
                    }

                    m_processExecuting[processId] = true;
                    info = ADInfoBll.Instance.GetProcessInfo(processId);
                }

                println("process " + processId + " start to execute!");

                Feng.Async.AsyncHelper asyncHelper = new Feng.Async.AsyncHelper(
                        new Feng.Async.AsyncHelper.DoWork(delegate()
                        {
                            try
                            {
                                ProcessInfoHelper.ExecuteProcess(info);
                                return true;
                            }
                            catch (Exception ex)
                            {
                                println(ex.Message);
                            }
                            return false;
                        }),
                        new Feng.Async.AsyncHelper.WorkDone(delegate(object result)
                        {
                            println("process " + processId + " has executed!");
                            lock (m_processExecuting)
                            {
                                m_processExecuting[processId] = false;
                            }
                        }));
            }
            else
            {
                debug("there is no process property!");
            }
        }

        /// <summary>
        /// 需要执行的任务
        /// </summary>
        public override void wakeup()
        {
            string processId = this.getString("process");
            println("process " + processId + " is wakeup!");

            ExecuteProcess(processId);
        }
    }
}
