using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting;
using System.Reflection;

namespace Feng.Scripts
{
    public class PythonHost : Host
    {
        private static PythonHost m_instance;
        public static PythonHost Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new PythonHost();
                }
                return m_instance;
            }
        }
        public PythonHost()
            : base(new LanguageSetup("IronPython.Runtime.PythonContext, IronPython",
                "IronPython", new string[] { "IronPython", "Python", "py" }, new string[] { ".py" }), "py")
        {
        }

        protected override string GetVersionString()
        {
            return string.Format("{0}{3} ({1}) on .NET {2}", new object[] { "IronPython", new AssemblyName(typeof(IronPython.Runtime.DefaultContext).Assembly.FullName).Version.ToString(), Environment.Version, "" });
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //[CLSCompliant(false)]
        //public IronPython.Runtime.Exceptions.TraceBackFrame TraceBackFrame
        //{
        //    get;
        //    internal set;
        //}
    }
}
