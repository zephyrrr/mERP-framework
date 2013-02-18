using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace Feng.Scripts
{
    /// <summary>
    /// Python Runtime
    /// 一个程序中可能只能有一个，如果开PythonConsole，需要先关闭这个
    /// </summary>
    public class PythonRuntime : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PythonRuntime()
        {
            InitIronPython();
        }

        /// <summary>
        /// Dispose 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (m_engine != null)
                {
                    m_runtime.Shutdown();
                    m_engine = null;
                    m_scope = null;
                }
            }
        }

        private void InitIronPython()
        {
            if (m_engine == null)
            {
                //s_engine = Python.CreateEngine();
                //s_runtime = s_engine.Runtime;
                // s_scope = s_engine.CreateScope();

                m_runtime = Python.CreateRuntime();
                m_runtime.LoadAssembly(typeof(String).Assembly);
                //runtime.LoadAssembly(typeof(Uri).Assembly);

                // no module name System
                //m_runtime.ImportModule("System");
               
                m_scope = m_runtime.CreateScope();
                
                m_engine = Python.GetEngine(m_runtime);

                ICollection<string> paths = m_engine.GetSearchPaths();
                foreach (string s in s_pythonLibPath)
                {
                    paths.Add(s);
                }
                m_engine.SetSearchPaths(paths);
            }
        }

        private static string[] s_pythonLibPath = new string[] { ".\\LocalResource", ".\\ServerResource" };

        // 光Scope多线程不够，Engine也要
        //private ScriptScope GetScriptScope()
        //{
        //    lock (m_scopes)
        //    {
        //        if (m_scopes.Count == 0)
        //        {
        //            m_scopes.Add(m_runtime.CreateScope());
        //        }
        //        return m_scopes[0];
        //    }
        //}
        //private void AddScope(ScriptScope scope)
        //{
        //    lock (m_scopes)
        //    {
        //        m_scopes.Add(scope);
        //    }
        //}

        //private ScriptEngine Engine
        //{
        //    get 
        //    {
        //        lock (this)
        //        {
        //            return m_engine;
        //        }
        //    }
        //}
        //private List<ScriptScope> m_scopes = new List<ScriptScope>();

        private ScriptEngine m_engine;
        private ScriptScope m_scope;
        private ScriptRuntime m_runtime;

        /// <summary>
        /// 
        /// </summary>
        public ScriptRuntime Runtime
        {
            get { return m_runtime; }
        }
        
        private const string s_pythonHeadMasterForm = @"import clr;
clr.AddReferenceToFileAndPath(r""{0}"")
import {1}
{1}.execute(masterForm)";
        private const string s_pythonHeadNoParam = @"import clr;
clr.AddReferenceToFileAndPath(r""{0}"")
import {1}
{1}.execute()";

        /// <summary>
        /// 执行通过pyc.py编译后的Assembly
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <param name="pythonFileName"></param>
        /// <param name="processParams"></param>
        public object ExecutePythonAssembly(string assemblyFile, string pythonFileName, Dictionary<string, object> processParams)
        {
            ScriptScope scope = m_scope;

            if (processParams != null)
            {
                foreach (KeyValuePair<string, object> kvp in processParams)
                {
                    scope.SetVariable(kvp.Key, kvp.Value);
                }
            }

            scope.RemoveVariable(s_result);
            try
            {
                //StringBuilder sb = new StringBuilder(100);
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    using (StringWriter writer = new StringWriter(sb))
                //    {
                //Assembly assembly = Assembly.LoadFile(assemblyPath);
                //s_runtime.LoadAssembly(assembly);
                //s_runtime.IO.SetOutput(ms, writer);
                // in Python, __name__ == pythonFileName

                string code = string.Format(processParams != null && processParams.ContainsKey("masterForm") ? s_pythonHeadMasterForm : s_pythonHeadNoParam,
                        assemblyFile,
                        pythonFileName.Replace(".py", "")
                        );
                ScriptSource source = m_engine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
                source.Execute(scope);
                //}
                //}
                //content = sb.ToString();
            }
            catch (Exception ex)
            {
                ExceptionOperations eo;
                eo = m_engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(ex);
                throw new ArgumentException(error, ex);
            }
            finally
            {
            }
            object ret;
            scope.TryGetVariable(s_result, out ret);

            return ret;
        }

        /// <summary>
        /// 执行Python脚本文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public object ExecutePythonFile(string fileName, Dictionary<string, object> processParams)
        {
            ScriptScope scope = m_scope;

            if (processParams != null)
            {
                foreach (KeyValuePair<string, object> kvp in processParams)
                {
                    scope.SetVariable(kvp.Key, kvp.Value);
                }
            }
            
            scope.RemoveVariable(s_result);
            try
            {
                ScriptSource source;
                string dir = System.IO.Path.GetDirectoryName(fileName);
                if (string.IsNullOrEmpty(dir))
                {
                    source = m_engine.CreateScriptSourceFromFile(
                        ServiceProvider.GetService<IApplicationDirectory>().GetLocalResourcePath(fileName), Encoding.UTF8);
                }
                else
                {
                    //var s = m_engine.CreateScriptSourceFromString(string.Format("import sys; sys.path.append('{0}');", dir));
                    //s.Execute(scope);
                    source = m_engine.CreateScriptSourceFromFile(fileName, Encoding.UTF8);
                }
                source.Execute(scope);
            }
            catch (Exception ex)
            {
                ExceptionOperations eo;
                eo = m_engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(ex);
                throw new ArgumentException(error, ex);
            }
            object ret;
            scope.TryGetVariable(s_result, out ret);

            return ret;
        }

        private void ExecuteDefaultStatement(ScriptScope scope)
        {
            ScriptSource source = m_engine.CreateScriptSourceFromString("import clr; import System", SourceCodeKind.Statements);
            source.Execute(scope);
        }

        /// <summary>
        /// 执行Python表达式。带返回值
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public object ExecutePythonExpression(string expression, Dictionary<string, object> processParams)
        {
            ScriptScope scope = m_scope;
            ExecuteDefaultStatement(scope);

            object ret = null;

            if (processParams != null)
            {
                foreach (KeyValuePair<string, object> kvp in processParams)
                {
                    scope.SetVariable(kvp.Key, kvp.Value);
                }
            }
            try
            {
                ScriptSource source = m_engine.CreateScriptSourceFromString(expression, SourceCodeKind.Expression);
                ret = source.Execute(scope);
            }
            catch (Exception ex)
            {
                ExceptionOperations eo;
                eo = m_engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(ex);
                throw new Microsoft.Scripting.SyntaxErrorException(error + System.Environment.NewLine + expression, ex);
            }

            return ret;
        }

        private const string s_result = "result";
        /// <summary>
        /// 执行Python Statement。不带返回值。如果要返回，必须在Statement中指定result变量
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public object ExecutePythonStatement(string statement, Dictionary<string, object> processParams)
        {
            ScriptScope scope = m_scope;

            if (processParams != null)
            {
                foreach (KeyValuePair<string, object> kvp in processParams)
                {
                    scope.SetVariable(kvp.Key, kvp.Value);
                }
            }
            scope.RemoveVariable(s_result);
            try
            {
                ScriptSource source = m_engine.CreateScriptSourceFromString(statement, SourceCodeKind.Statements);
                source.Execute(scope);
            }
            catch (Exception ex)
            {
                ExceptionOperations eo;
                eo = m_engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(ex);
                throw new ArgumentException(error + System.Environment.NewLine + statement, ex);
            }
            object ret;
            scope.TryGetVariable(s_result, out ret);

            return ret;
        }
    }
}
