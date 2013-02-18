using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using IronPython.Hosting;

namespace Feng.Scripts
{
    /// <summary>
    /// 
    /// </summary>
    public static class PythonHelper
    {
        static PythonHelper()
        {
            Reset();
            //s_pythonRuntimes.AddObjectToPool(new PythonRuntime());
            //s_pythonRuntimes.AddObjectToPool(new PythonRuntime());
        }

        public static void Reset()
        {
            if (s_pythonRuntimes != null)
            {
                s_pythonRuntimes.Dispose();
            }
            s_pythonRuntimes = new ObjectPool<PythonRuntime>();
            s_pythonRuntimes.MaxObjectsInPool = 5;
        }
        private static ObjectPool<PythonRuntime> s_pythonRuntimes;
        private static PythonRuntime GetRuntime()
        {
            return s_pythonRuntimes.GetOne();
        }

        ///// <summary>
        ///// ClearRuntime
        ///// </summary>
        //public static void ClearRuntime()
        //{
        //    if (m_pythonRuntime != null)
        //    {
        //        m_pythonRuntime.Dispose();
        //        m_pythonRuntime = null;
        //    }
        //}

        //public static object ExecutePythonResource(string assemblyFile, string pythonFileName, Dictionary<string, object> processParams)
        //{
        //    PythonRuntime runtime = GetRuntime();
        //    try
        //    {
        //        return runtime.ExecutePythonAssembly(assemblyFile, pythonFileName, processParams);
        //    }
        //    finally
        //    {
        //        s_pythonRuntimes.AddObjectToPool(runtime);
        //    }
        //}

        /// <summary>
        /// 执行通过pyc.py编译后的Assembly
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <param name="pythonFileName"></param>
        /// <param name="processParams"></param>
        public static object ExecutePythonAssembly(string assemblyFile, string pythonFileName, Dictionary<string, object> processParams)
        {
            PythonRuntime runtime = GetRuntime();
            try
            {
                return runtime.ExecutePythonAssembly(assemblyFile, pythonFileName, processParams);
            }
            finally
            {
                s_pythonRuntimes.AddObjectToPool(runtime);
            }
        }

        /// <summary>
        /// 执行Python脚本文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public static object ExecutePythonFile(string fileName, Dictionary<string, object> processParams)
        {
            PythonRuntime runtime = GetRuntime();
            try
            {
                return runtime.ExecutePythonFile(fileName, processParams);
            }
            finally
            {
                s_pythonRuntimes.AddObjectToPool(runtime);
            }
        }

        /// <summary>
        /// 执行Python表达式。带返回值
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public static object ExecutePythonExpression(string expression, Dictionary<string, object> processParams)
        {
            PythonRuntime runtime = GetRuntime();
            try
            {
                return runtime.ExecutePythonExpression(expression, processParams);
            }
            finally
            {
                s_pythonRuntimes.AddObjectToPool(runtime);
            }
        }

        /// <summary>
        /// 执行Python Statement。不带返回值。如果要返回，必须在Statement中指定result变量
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public static object ExecutePythonStatement(string statement, Dictionary<string, object> processParams)
        {
            PythonRuntime runtime = GetRuntime();
            try
            {
                return runtime.ExecutePythonStatement(statement, processParams);
            }
            finally
            {
                s_pythonRuntimes.AddObjectToPool(runtime);
            }
        }
    }
}
