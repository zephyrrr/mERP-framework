using com.neokernel.nk;

namespace Feng.NeoKernel.nk
{
    using com.neokernel.props;
    using com.neokernel.util;
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;
    using System.Security;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    internal class StartupAgent : com.neokernel.nk.Agent
    {
        //private static string EVAL_STR = "evaluation license";
        private const string message = "There is a problem with the license.";
        private const string message2 = "The information in the license file is invalid.";
        private const string message3 = "The license is invalid.";
        private const string message4 = "*** The server will stop in 5 minutes because no valid license was found. Please obtain a current license to continue using this software.";
        private const string message5 = "License problem.";
        private const string msg1 = "The license expired on ";
        private const string msg2 = ". Please obtain a current license to continue using this software.";
        private static int WAIT_TIME = 0x1b58;
        //private static int xx = 7;
        //private static int yy = 6;

        protected ArrayList addDefaultAgentPropsToList(ArrayList inputList)
        {
            ArrayList agentprops = new ArrayList(inputList);
            DirectoryInfo dir = null;
            if (this.hasProperty("startup_dir"))
            {
                dir = new DirectoryInfo(Path.GetFullPath(this.getString("startup_dir")));
            }
            else
            {
                dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            }
            IEnumerator enumerator = this.getPropsForAgentsInDirectory(dir).GetEnumerator();
            while (enumerator.MoveNext())
            {
                Props current = (Props)enumerator.Current;
                bool flag = false;
                IEnumerator enumerator2 = inputList.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    Props props2 = (Props)enumerator2.Current;
                    if (current.getString("classname").Equals(props2.getString("classname")))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    agentprops.Add(current);
                }
            }
            if (!this.getBoolean("dont_start_agents") && this.needServiceManager(agentprops))
            {
                Props props3 = new Props();
                props3.setProperty("classname", "com.neokernel.nk.ServiceManager");
                props3.setProperty("startup", true);
                props3.setProperty("hide_debug", true);
                agentprops.Insert(0, props3);
            }
            if ((!this.getBoolean("dont_start_webserver") && !this.getBoolean("dont_start_agents")) && this.needWebServer(agentprops))
            {
                Props props4 = new Props();
                IEnumerator enumerator3 = agentprops.GetEnumerator();
                bool flag2 = false;
                while (enumerator3.MoveNext())
                {
                    Props props5 = (Props) enumerator3.Current;
                    if (props5.hasProperty("service_name"))
                    {
                        props4.setProperty("index_agent", props5.getString("service_name"));
                        flag2 = true;
                        break;
                    }
                }
                props4.setProperty("classname", "com.neokernel.httpd.WebServer");
                props4.setProperty("log_requests", true);
                props4.setProperty("log_file_dir", Application.StartupPath + Path.DirectorySeparatorChar + "logs");
                props4.setProperty("startup", true);
                if (!flag2)
                {
                    Props props6 = new Props();
                    props6.setProperty("classname", "com.neokernel.httpd.HTTPFileServerAgent");
                    props6.setProperty("startup", true);
                    agentprops.Add(props6);
                }
                agentprops.Add(props4);
            }
            return agentprops;
        }

        private void complain(string message)
        {
            lock (this)
            {
                NK.DefaultReporter.error(NK.neokernel, message + "\n\nPlease go to http://www.neokernel.com to obtain a valid software license.");
                Monitor.Wait(this, TimeSpan.FromMilliseconds((double) WAIT_TIME));
            }
        }

        private ArrayList getPropsForAgentsInDirectory(DirectoryInfo dir)
        {
            //DirectoryInfo info = dir;
            ArrayList list = new ArrayList();
            //Hashtable hashtable = new Hashtable();
            //FileInfo[] files = info.GetFiles("*.dll");
            //FileInfo[] infoArray2 = info.GetFiles("*.exe");
            //FileInfo[] array = new FileInfo[files.Length + infoArray2.Length];
            //files.CopyTo(array, 0);
            //infoArray2.CopyTo(array, files.Length);
            //IEnumerator enumerator = array.GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    FileInfo current = (FileInfo) enumerator.Current;
            //    Assembly assembly = null;
            //    try
            //    {
            //        assembly = Assembly.Load(current.Name.Substring(0, current.Name.LastIndexOf(".")));
            //    }
            //    catch (BadImageFormatException)
            //    {
            //    }
            //    catch (IOException)
            //    {
            //    }
            //    catch (ArgumentException)
            //    {
            //    }
            //    catch (SecurityException)
            //    {
            //    }
            //    if (assembly == null)
            //    {
            //        try
            //        {
            //            assembly = Assembly.LoadFrom(current.FullName);
            //        }
            //        catch (BadImageFormatException)
            //        {
            //        }
            //        catch (IOException)
            //        {
            //        }
            //        catch (ArgumentException)
            //        {
            //        }
            //        catch (SecurityException)
            //        {
            //        }
            //    }
            //    if (assembly != null)
            //    {
            //        bool flag = false;
            //        IEnumerator enumerator2 = assembly.GetExportedTypes().GetEnumerator();
            //        while (enumerator2.MoveNext())
            //        {
            //            System.Type type = (System.Type) enumerator2.Current;
            //            if (type.FullName.IndexOf("com.neokernel") != -1)
            //            {
            //                flag = true;
            //                break;
            //            }
            //        }
            //        if (!flag)
            //        {
            //            try
            //            {
            //                for (int i = 0; i < assembly.GetTypes().Length; i++)
            //                {
            //                    System.Type type2 = assembly.GetTypes()[i];
            //                    if (type2.GetInterface("com.neokernel.nk.AgentInterface") != null)
            //                    {
            //                        string fullName = type2.FullName;
            //                        if (type2.Namespace == null)
            //                        {
            //                            fullName = current.Name.Substring(0, current.Name.IndexOf(".dll")) + "." + type2.Name;
            //                        }
            //                        Props props = new Props();
            //                        props.setProperty("classname", fullName);
            //                        props.setProperty("startup", true);
            //                        props.setProperty("assembly_loc", current.FullName);
            //                        list.Add(props);
            //                        hashtable.Add(type2.FullName, type2);
            //                    }
            //                }
            //                continue;
            //            }
            //            catch (ArgumentException exception)
            //            {
            //                Console.Out.WriteLine(string.Concat(new object[] { "Exception while analyzing ", current.FullName, ". ", exception }));
            //                continue;
            //            }
            //            catch (ReflectionTypeLoadException exception2)
            //            {
            //                Console.Out.WriteLine(string.Concat(new object[] { "Exception while analyzing ", current.FullName, ". ", exception2 }));
            //                continue;
            //            }
            //        }
            //    }
            //}
            return list;
        }

        public override void initProps()
        {
            this.setDefault("startup_dir", Application.StartupPath);
            this.setDefault("license_file", Application.StartupPath + Path.DirectorySeparatorChar.ToString() + "license");
            this.setDefault("startup_file", "neokernel_props.xml");
        }

        private bool needServiceManager(ArrayList agentprops)
        {
            bool flag = false;
            IEnumerator enumerator = agentprops.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Props current = (Props) enumerator.Current;
                if (current.getString("classname").Equals("com.neokernel.nk.ServiceManager"))
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                return false;
            }
            return true;
        }

        private bool needWebServer(ArrayList agentprops)
        {
            bool flag = false;
            IEnumerator enumerator = agentprops.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Props current = (Props) enumerator.Current;
                string str = current.getString("classname");
                if (str.Equals("com.neokernel.httpd.WebServer") || str.Equals("com.neokernel.httpd.SecureWebServer"))
                {
                    flag = true;
                    break;
                }
                if (str.Equals("com.neokernel.httpd.HTTPProxyAgent") && current.hasProperty("listeners"))
                {
                    foreach (string str2 in current.getString("listeners").Split(new char[] { ':' }))
                    {
                        if (str2.StartsWith("80,") || str2.Equals("80"))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                }
            }
            if (flag)
            {
                return false;
            }
            return true;
        }

        internal bool s2(string lf)
        {
            //string str = "0";
            //if (!File.Exists(lf))
            //{
            //    this.complain("License missing: " + lf + ". ");
            //    str = "evaluation license";
            //}
            //Props srcProps = Props.loadProps(lf);
            //this.copyFrom(srcProps);
            //str = this.getString("registration_number");
            //if (str.ToLower().Equals("evaluation license"))
            //{
            //    this.sT();
            //    return true;
            //}
            //if (!str.StartsWith("EVAL") && !str.StartsWith("NKS"))
            //{
            //    this.complain("The license is invalid.");
            //    return false;
            //}
            //try
            //{
            //    string str2 = s1.u1(str.Substring(4), xx, yy, this.a1);
            //    int hashCode = (str2.Substring(2, 8) + "-" + str2.Substring(14, 4) + "-" + str2.Substring(20, 4) + "-" + str2.Substring(0x1a, 4)).GetHashCode();
            //    if ((hashCode != 0x836c942) && (hashCode != -1329864495))
            //    {
            //        this.complain(". Please obtain a current license to continue using this software.");
            //        return false;
            //    }
            //    int num2 = 1;
            //    foreach (char ch in str2.ToCharArray())
            //    {
            //        if (num2 > 0x1d)
            //        {
            //            break;
            //        }
            //        int num3 = ch;
            //        if ((num3 < 0x30) || (num3 > 0x7a))
            //        {
            //            this.complain(". Please obtain a current license to continue using this software.");
            //            return false;
            //        }
            //        num2++;
            //    }
            //    DateTime time = new DateTime(Convert.ToInt16(str2.Substring(10, 4)), Convert.ToInt16(str2.Substring(0x12, 2)), Convert.ToInt16(str2.Substring(0x18, 2)));
            //    if (((time.Ticks < DateTime.Now.Ticks) || (DateTime.Compare(time, DateTime.Now.AddDays(34.0)) == 1)) && (time.Year > 0x6f0))
            //    {
            //        if (time.Ticks < DateTime.Now.Ticks)
            //        {
            //            this.complain("The license expired on " + time.ToLongDateString() + ". Please obtain a current license to continue using this software.");
            //        }
            //        else
            //        {
            //            this.complain(". Please obtain a current license to continue using this software.");
            //        }
            //        return false;
            //    }
            //}
            //catch (Exception)
            //{
            //    this.complain("There is a problem with the license.");
            //    return false;
            //}
            return true;
        }

        internal bool sT()
        {
            Props agentInfo = new Props();
            agentInfo.setProperty("classname", "com.neokernel.nk.Trmntr");
            agentInfo.setProperty("startup", true);
            agentInfo.setProperty("seconds_to_live", 240);
            agentInfo.setProperty("hide_println", true);
            agentInfo.setProperty("hide_debug", true);
            agentInfo.setProperty("hide_warning", true);
            AgentControllerInterface interface2 = NK.neokernel.createAgent(agentInfo);
            if (interface2 != null)
            {
                this.complain("*** The server will stop in 5 minutes because no valid license was found. Please obtain a current license to continue using this software.");
                interface2.start();
                return true;
            }
            this.complain("License problem.");
            return false;
        }

        public override void start()
        {
            try
            {
                if (this.s2(this.getString("license_file")))
                {
                    ArrayList agentList = this.AgentList;
                    this.startAgents(agentList);
                }
            }
            catch (ArgumentException exception)
            {
                this.error("Could not continue", exception);
            }
        }

        protected internal virtual void startAgents(ArrayList agentList)
        {
            int count = agentList.Count;
            for (int i = 0; i < count; i++)
            {
                Props agentInfo = (Props) agentList[i];
                string str = agentInfo.getString("agent_id") + "_" + agentInfo.getString("name");
                if (agentInfo.getBoolean("startup"))
                {
                    AgentControllerInterface objectRef = NK.neokernel.createAgent(agentInfo);
                    if (objectRef != null)
                    {
                        try
                        {
                            objectRef.start();
                        }
                        catch (Exception)
                        {
                            NK.DefaultReporter.error(objectRef, "Couldn't start agent " + agentInfo.getString("classname"));
                            Thread.Sleep(0x1388);
                        }
                    }
                }
                else
                {
                    this.debug("Skipping: " + str);
                }
            }
        }

        private char[] a1
        {
            get
            {
                byte[] bytes = new byte[] { 
                    0x35, 0x36, 0x45, 0x52, 0x31, 50, 0x54, 0x59, 0x39, 0x30, 0x55, 0x49, 0x33, 0x37, 0x38, 0x4f, 
                    80, 0x51, 0x57, 0x34, 0x41, 0x53, 0x44, 70, 0x47, 0x48, 0x4a, 0x4b, 0x4c, 90, 0x58, 0x43, 
                    0x56, 0x42, 0x4e, 0x4d
                 };
                char[] chars = new char[Encoding.GetEncoding("ASCII").GetCharCount(bytes, 0, bytes.Length)];
                Encoding.GetEncoding("ASCII").GetChars(bytes, 0, bytes.Length, chars, 0);
                return chars;
            }
        }

        protected internal virtual ArrayList AgentList
        {
            get
            {
                return this.addDefaultAgentPropsToList(new ArrayList());
            }
        }
    }
}

