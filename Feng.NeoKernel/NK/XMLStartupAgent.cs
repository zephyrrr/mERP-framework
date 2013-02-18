using com.neokernel.xml;
using com.neokernel.nk;
using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Feng.NeoKernel.nk
{
    internal class LicenseFreeXMLStartupAgent : StartupAgent
    {
        public override void initProps()
        {
            this.setDefault("startup_dir", Application.StartupPath);
            this.setDefault("registration_url", "http://www.neokernel.com/content.agent?page_name=Neokernel+Server");
            this.setDefault("license_file", Application.StartupPath + Path.DirectorySeparatorChar + "license");
            this.setDefault("startup_file", "neokernel_props.xml");
            this.setDefault("nothanks_url", "http://www.neokernel.com/nothanks.htm");
        }

        public override void start()
        {
            try
            {
                ArrayList inputList = null;
                string path = this.getString("startup_file");
                if (this.hasProperty("startup_dir"))
                {
                    string fullPath = Path.GetFullPath(this.getString("startup_dir"));
                    string str3 = Path.DirectorySeparatorChar.ToString();
                    if (!fullPath.EndsWith(str3))
                    {
                        fullPath = fullPath + str3;
                    }
                    path = fullPath + path;
                }
                if (File.Exists(path))
                {
                    inputList = XMLPropsList.loadFromFile(path);
                    if (inputList == null)
                    {
                        inputList = new ArrayList();
                    }
                    inputList = base.addDefaultAgentPropsToList(inputList);
                }
                else
                {
                    inputList = this.AgentList;
                }
                //if (base.s2(this.getString("license_file")))
                {
                    this.startAgents(inputList);
                }
                //else
                //{
                    //NK.DefaultReporter.error(null, "No valid license file was found at " + this.getString("license_file"));
                //}
            }
            catch (IOException exception)
            {
                this.error("There is a problem starting the Neokernel. The assemblies in the directory " + Path.GetFullPath(this.getString("startup_dir")) + " may need to be recompiled.", exception);
                Thread.Sleep(-1);
            }
            catch (Exception exception2)
            {
                this.error("There is a problem starting the Neokernel in " + Path.GetFullPath(this.getString("startup_dir")) + ".", exception2);
                Thread.Sleep(0x1388);
                throw;
            }
        }
    }
}

