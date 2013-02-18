using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Windows.Forms
{
    public class RubyScriptForm : ScriptForm
    {
        /// <summary>
        /// 
        /// </summary>
        public RubyScriptForm()
            : base(Feng.Scripts.RubyHost.Instance)
        {
        }

        //private const string m_promptString = ">";
        //protected override string PromptString
        //{
        //    get { return m_promptString; }
        //}
        //private const string m_promptContinuationString = "*";
        //protected override string PromptContinuationString
        //{
        //    get { return m_promptContinuationString; }
        //}
        private const string m_fileFilter = "Ruby and text files (*.rb *.txt)|*.rb;*.txt";
        protected override string FileFilter
        {
            get { return m_fileFilter; }
        }
        protected override bool IsCommandComplete
        {
            get 
            {
                string code = base.CurrentCommand;
                Microsoft.Scripting.ScriptCodeParseResult commandProperties =
                 base.Host.Engine.CreateScriptSourceFromString(code, Microsoft.Scripting.SourceCodeKind.InteractiveCode)
                    .GetCodeProperties(base.Host.Engine.GetCompilerOptions(base.Host.Scope));

                 return Microsoft.Scripting.SourceCodePropertiesUtils.IsCompleteOrInvalid(commandProperties, false);
            }
        }

        protected override IList<string> GetGlobalsOfLanguage(string name)
        {
            //List<string> res = new List<string>();
            //IronRuby.Runtime.RubyContext context = IronRuby.Runtime.RubyContext...DefaultPythonContext;
            //foreach (object builtinName in context.BuiltinModuleInstance.Get__dict__().Keys)
            //{
            //    string strName = builtinName as string;
            //    if (strName != null && strName.StartsWith(name))
            //    {
            //        res.Add(strName);
            //    }
            //}
            //return res;
            return null;
        }
    }
}
