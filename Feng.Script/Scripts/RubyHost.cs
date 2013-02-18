using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting;
using System.Reflection;

namespace Feng.Scripts
{
    public class RubyHost : Host
    {
        private static RubyHost m_instance;
        public static RubyHost Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new RubyHost();
                }
                return m_instance;
            }
        }
        public RubyHost()
            : base(new LanguageSetup("IronRuby.Runtime.RubyContext, IronRuby",
                "IronRuby", new string[] { "IronRuby", "Ruby", "rb" }, new string[] { ".rb" }), "rb")
        {
        }

        protected override string GetVersionString()
        {
            return string.Format("{0}{3} ({1}) on .NET {2}",
                new object[] { "IronRuby", new AssemblyName(Assembly.Load("IronRuby").FullName).Version.ToString(), Environment.Version, "" });
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
