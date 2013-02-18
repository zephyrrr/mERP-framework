using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using Microsoft.Scripting.Hosting.Shell;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.IO;

//[assembly: System.Security.SecurityTransparent]
//[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]

namespace Feng.Scripts
{
    [System.CLSCompliant(false)]
    [System.Runtime.CompilerServices.CompilationRelaxations(8)]
    public sealed class PythonConsoleHost : ConsoleHost
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern int AllocConsole();
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern int FreeConsole();

        private static PythonConsoleHost m_pythonConsoleHost;
        public static PythonConsoleHost OpenPythonConsole(System.Windows.Forms.Form mainForm)
        {
            // ipy -D -X:TabCompletion -X:ColorfulConsole
            if (m_pythonConsoleHost == null)
            {
                m_pythonConsoleHost = new PythonConsoleHost();
                //System.Threading.ManualResetEvent eve = new System.Threading.ManualResetEvent(false);

                dispatcher = mainForm;

                System.Threading.AutoResetEvent are = new System.Threading.AutoResetEvent(false);

                System.Threading.Thread _debugThread = new System.Threading.Thread(() =>
                {
                    int r = AllocConsole();
                    if (r == 0)
                    {
                        throw new InvalidOperationException("Can't AllocConsole!");
                    }
                    StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
                    standardOutput.AutoFlush = true;
                    Console.SetOut(standardOutput);

                    //host.Options.RunAction = Microsoft.Scripting.Hosting.Shell.ConsoleHostOptions.Action.RunConsole;
                    m_pythonConsoleHost.Run(new string[] { "-X:ColorfulConsole", "-X:Debug", "-X:TabCompletion", "-X:ExceptionDetail", "-X:ShowClrExceptions"  }); // 

                    //are.Set();

                    m_pythonConsoleHost.Runtime.IO.RedirectToConsole();
                    IronPython.Runtime.ClrModule.SetCommandDispatcher(IronPython.Runtime.DefaultContext.Default, null);

                    r = FreeConsole();
                    if (r == 0)
                    {
                        throw new InvalidOperationException("Can't FreeConsole!");
                    }

                    m_pythonConsoleHost = null;
                });
                _debugThread.IsBackground = true;
                _debugThread.SetApartmentState(System.Threading.ApartmentState.STA);
                _debugThread.Start();
                
                // Don't establish the alternative input execution behavior until the other thread is ready.  Note, 'are' starts out unsignalled.
                //are.WaitOne();
                
                while (m_pythonConsoleHost.ScriptScope == null)
                {
                    System.Threading.Thread.Sleep(1000);
                }

                IronPython.Runtime.ClrModule.SetCommandDispatcher(IronPython.Runtime.DefaultContext.Default, DispatchConsoleCommand);
            }

            return m_pythonConsoleHost;
        }

        private static System.Windows.Forms.Form dispatcher;
        private static void DispatchConsoleCommand(Delegate consoleCommand)
        {
            if (consoleCommand != null)
            {
                // consoleCommand is a delegate for a dynamic method that embodies the
                // input expression from the user.  Run it on the other thread.
                dispatcher.Invoke(consoleCommand);
            }
            else
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ScriptScope ScriptScope
        {
            get 
            {
                if (this.CommandLine == null)
                    return null;
                return this.CommandLine.ScriptScope; 
            }
        }

        protected override CommandLine CreateCommandLine()
        {
            return new PythonCommandLine();
        }


        protected override OptionsParser CreateOptionsParser()
        {
            return new PythonOptionsParser();
        }

        protected override ScriptRuntimeSetup CreateRuntimeSetup()
        {
            ScriptRuntimeSetup setup = ScriptRuntimeSetup.ReadConfiguration();
            foreach (LanguageSetup setup2 in setup.LanguageSetups)
            {
                if (setup2.FileExtensions.Contains(".py"))
                {
                    setup2.Options["SearchPaths"] = new string[0];
                }
            }
            return setup;
        }

        protected override void ExecuteInternal()
        {
            //this.Runtime.IO.SetOutput(System.Console.OpenStandardOutput(), System.Console.OutputEncoding);
            //this.Runtime.IO.SetInput(System.Console.OpenStandardInput(), System.Console.InputEncoding);
            //this.Runtime.IO.SetErrorOutput(System.Console.OpenStandardError(), System.Console.OutputEncoding);

            (HostingHelpers.GetLanguageContext(base.Engine) as PythonContext).SetModuleState(typeof(ScriptEngine), base.Engine);
            base.ExecuteInternal();
        }

        //[STAThread]
        //public static int Main(string[] args)
        //{
        //    if (Environment.GetEnvironmentVariable("TERM") == null)
        //    {
        //        Environment.SetEnvironmentVariable("TERM", "dumb");
        //    }
        //    return new PythonConsoleHost().Run(args);
        //}

        ///// <summary>
        ///// RunIt
        ///// </summary>
        ///// <param name="args"></param>
        ///// <returns></returns>
        //public static PythonConsoleHost RunIt(string[] args)
        //{
        //    if (Environment.GetEnvironmentVariable("TERM") == null)
        //    {
        //        Environment.SetEnvironmentVariable("TERM", "dumb");
        //    }
        //    PythonConsoleHost host = new PythonConsoleHost();
        //    host.Run(args);
        //    return host;
        //}

        protected override void ParseHostOptions(string[] args)
        {
            foreach (string str in args)
            {
                base.Options.IgnoredArgs.Add(str);
            }
        }

        protected override Type Provider
        {
            get
            {
                return typeof(PythonContext);
            }
        }
    }
}

