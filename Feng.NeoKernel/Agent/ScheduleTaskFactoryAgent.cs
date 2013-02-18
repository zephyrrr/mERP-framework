using System;
using System.Collections.Generic;
using System.Text;
using com.neokernel.nk;
using com.neokernel.props;
using Feng;

namespace Feng.NeoKernel.Agent
{
    public class ScheduleTaskFactoryAgent : com.neokernel.nk.Agent
    {
        public override void start()
        {
            println("Begin ScheduleTaskFactoryAgent");

            try
            {
                ProgramHelper.InitProgram();

                IList<ServerTaskScheduleInfo> list = ADInfoBll.Instance.GetInfos<ServerTaskScheduleInfo>();
                foreach (ServerTaskScheduleInfo i in list)
                {
                    Props agentInfo = new Props();
                    agentInfo.setProperty("classname", "Feng.NeoKernel.Agent.ScheduleTaskAgent");
                    agentInfo.setProperty("startup", true);
                    agentInfo.setProperty("hide_println", false);
                    agentInfo.setProperty("hide_debug", false);
                    agentInfo.setProperty("hide_warning", false);
                    agentInfo.setProperty("startup", true);
                    agentInfo.setProperty("schedule", i.Schedule);
                    agentInfo.setProperty("process", i.Process.Name);
                    agentInfo.setProperty("startAtStart", i.StartAtStart);

                    AgentControllerInterface interface2 = NK.neokernel.createAgent(agentInfo);
                    if (interface2 != null)
                    {
                        interface2.start();

                        println(i.Name + " Schedule Agent is created!");
                    }
                    else
                    {
                        warning(i.Name + " Schedule Agent is not created!");
                    }
                }
            }
            catch (Exception ex)
            {
                println("Error :" + ex.Message);
            }

            println("End ScheduleTaskFactoryAgent");
        }
    }
}
