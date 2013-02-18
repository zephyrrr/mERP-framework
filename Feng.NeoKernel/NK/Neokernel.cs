using com.neokernel.nk;

namespace Feng.NeoKernel.nk
{
    using com.neokernel.props;
    using com.neokernel.util;
    using System;

    internal class Neokernel : com.neokernel.nk.Agent, NeokernelInterface
    {
        protected internal virtual AgentControllerInterface createAgent(Props agentProps)
        {
            AgentController controller = null;
            string classname = agentProps.getString("classname");
            if (classname.Length > 0)
            {
                if (classname.Equals("com.neokernel.xml.XMLStartupAgent"))
                {
                    classname = "Feng.NeoKernel.nk.LicenseFreeXMLStartupAgent";
                }

                if (!classname.Equals("com.neokernel.xml.XMLStartupAgent") && !classname.Equals("com.neokernel.nk.Trmntr"))
                {
                    this.println("Creating agent " + classname);
                }
                try
                {
                    AgentInterface agent = null;
                    IClassFactoryInterface defaultClassFactory = (IClassFactoryInterface) agentProps.getProperty("class_factory");
                    if (defaultClassFactory == null)
                    {
                        defaultClassFactory = NK.DefaultClassFactory;
                    }
                    agent = (AgentInterface) defaultClassFactory.createInstance(classname);
                    Type type = agent.GetType();
                    if (!agentProps.hasProperty("name"))
                    {
                        string fullName = type.FullName;
                        int num = fullName.LastIndexOf('.');
                        if (num > 0)
                        {
                            fullName = fullName.Substring(num + 1);
                        }
                        agentProps.setProperty("name", fullName);
                    }
                    agentProps.setDefault("agent_id", this.NextAgentID);
                    agent.Props = agentProps;
                    controller = new AgentController(agent);
                }
                catch (ArgumentException exception)
                {
                    this.error("Could not create Agent: " + classname, exception);
                }
                catch (ClassFactoryException exception2)
                {
                    this.error("Could not create Agent: " + classname, exception2);
                }
            }
            return controller;
        }

        public virtual AgentControllerInterface createAgent(object agentInfo)
        {
            if (agentInfo is Props)
            {
                return this.createAgent((Props) agentInfo);
            }
            Props agentProps = new Props();
            agentProps.setProperty("classname", agentInfo.ToString());
            return this.createAgent(agentProps);
        }

        public override void initProps()
        {
            this.setDefault("next_agent_id", "1001");
        }

        public override void stop()
        {
            this.println("Shutting down...");
            Environment.Exit(0);
        }

        protected internal virtual string NextAgentID
        {
            get
            {
                int num = this.getInteger("next_agent_id");
                this.setInteger("next_agent_id", num + 1);
                return this.getString("next_agent_id");
            }
        }
    }
}

