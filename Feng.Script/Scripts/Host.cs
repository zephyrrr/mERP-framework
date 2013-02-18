using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting;
using System.IO;
using System.Reflection;

namespace Feng.Scripts
{
    public class Host
    {
        ScriptRuntime _runtime;
        ScriptEngine _engine;
        ScriptScope _theScope;

        MemoryStream _output;
        MemoryStream _error;

        public ScriptScope Scope
        {
            get { return _theScope; }
        }
        public ScriptRuntime Runtime
        {
            get { return _runtime; }
        }
        public ScriptEngine Engine
        {
            get { return _engine; }
        }

        public string ErrorFromLastExecution { get; set; }

        /* <configSections>
    <section name="microsoft.scripting" type="Microsoft.Scripting.Hosting.Configuration.Section, Microsoft.Scripting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" requirePermission="false" />
  </configSections>

  <microsoft.scripting>
    <languages>
      <language names="IronPython;Python;py" extensions=".py" displayName="IronPython 2.7.1" type="IronPython.Runtime.PythonContext, IronPython, Version=2.7.1.0, Culture=neutral, PublicKeyToken=null" />
      <language names="IronRuby;Ruby;rb" extensions=".rb" displayName="IronRuby" type="IronRuby.Runtime.RubyContext, IronRuby, Version=0.9.5.0, Culture=neutral, PublicKeyToken=null" />
    </languages>

    <options>
      <set language="Ruby" option="LibraryPaths" value="..\..\Languages\Ruby\libs\;..\..\..\External.LCA_RESTRICTED\Languages\Ruby\redist-libs\ruby\site_ruby\1.8\;..\..\..\External.LCA_RESTRICTED\Languages\Ruby\redist-libs\ruby\1.8\" />
    </options>
  </microsoft.scripting>*/

        internal Host(LanguageSetup language, string languageName)
            : this(language, languageName, true)
        {

        }
        internal Host(LanguageSetup language, string languageName, bool enableDebug)
        {
            _output = new MemoryStream();
            _error = new MemoryStream();

            //var configFile = Path.GetFullPath(Uri.UnescapeDataString(new Uri(typeof(Host).Assembly.CodeBase).AbsolutePath)) + ".config";
            //_runtime = new ScriptRuntime(ScriptRuntimeSetup.ReadConfiguration(configFile));

            ScriptRuntimeSetup setup = new ScriptRuntimeSetup();
            if (enableDebug)
            {
                language.Options["Debug"] = Microsoft.Scripting.Runtime.ScriptingRuntimeHelpers.True;
                setup.DebugMode = true;
            }
            setup.LanguageSetups.Add(language);

            _runtime = new ScriptRuntime(setup);
            _engine = _runtime.GetEngine(languageName);

            _runtime.IO.SetOutput(_output, new StreamWriter(_output));
            _runtime.IO.SetErrorOutput(_error, new StreamWriter(_error));

            _theScope = _engine.CreateScope();
            _theScope.SetVariable("_host", this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            _theScope = _engine.CreateScope();
            _theScope.SetVariable("_host", this);
        }

        protected virtual string GetVersionString()
        {
            return string.Empty;
        }

        public string GetLogoDisplay()
        {
            return (GetVersionString() + "\nType \"help\", \"copyright\", \"credits\" or \"license\" for more information.\n");
        }

        public string ExecuteInCurrentScope(string snippet)
        {
            return ExecuteInCurrentScope(snippet, SourceCodeKind.InteractiveCode);
        }

        public string ExecuteInCurrentScope(string snippet, SourceCodeKind kind)
        {
            ScriptSource src = _engine.CreateScriptSourceFromString(snippet, kind);

            //CompiledCode compiledCode = src.Compile(_engine.GetCompilerOptions(_theScope));
            //return compiledCode.Execute(compiledCode, _theScope);

            try
            {
                object o = src.Execute(_theScope);
            }
            catch (Exception ex)
            {
                ErrorFromLastExecution = ex.Message;
                return null;
            }
            return ReadOutput();
        }

        public string ReadOutput()
        {
            return ReadFromStream(_output);
        }

        private string ReadError()
        {
            return ReadFromStream(_error);
        }

        private string ReadFromStream(MemoryStream ms)
        {
            int length = (int)ms.Length;
            Byte[] bytes = new Byte[length];

            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(bytes, 0, (int)ms.Length);

            ms.SetLength(0);

            return Encoding.GetEncoding("utf-8").GetString(bytes, 0, (int)bytes.Length);
        }
    }
}
