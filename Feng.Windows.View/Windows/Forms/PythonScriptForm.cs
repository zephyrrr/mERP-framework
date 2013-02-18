using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Windows.Forms
{
    public class PythonScriptForm : ScriptForm
    {
        /// <summary>
        /// 
        /// </summary>
        public PythonScriptForm()
            : base(Feng.Scripts.PythonHost.Instance)
        {
        }

        private const string m_initializeCommand = "import clr;import sys; sys.path.append('.\\LocalResource'); sys.path.append('.\\ServerResource');";
        protected override string InitializeCommand
        {
            get { return m_initializeCommand; }
        }
        private const string m_fileFilter = "Python and text files (*.py *.txt)|*.py;*.txt";
        protected override string FileFilter
        {
            get { return m_fileFilter; }
        }
        protected override bool IsCommandComplete
        {
            get { return !CurrentCommand.EndsWith(":"); }
        }

        protected override IList<string> GetGlobalsOfLanguage(string name)
        {
            List<string> res = new List<string>();
            IronPython.Runtime.PythonContext context = IronPython.Runtime.DefaultContext.DefaultPythonContext;
            foreach (object builtinName in context.BuiltinModuleInstance.Get__dict__().Keys)
            {
                string strName = builtinName as string;
                if (strName != null && strName.StartsWith(name))
                {
                    res.Add(strName);
                }
            }
            return res;
        }
    }
}
