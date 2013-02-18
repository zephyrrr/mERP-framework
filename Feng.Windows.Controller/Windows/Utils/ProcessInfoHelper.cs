using System;
using System.Collections.Generic;
using System.Text;
using Feng.Scripts;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// ProcessInfoHelper
    /// </summary>
    public static class ProcessInfoHelper
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public static object ExecuteProcess(string processName, Dictionary<string, object> processParams)
        {
            ProcessInfo info = ADInfoBll.Instance.GetProcessInfo(processName);
            return ExecuteProcess(info, processParams);
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="processInfo"></param>
        /// <returns></returns>
        public static object ExecuteProcess(ProcessInfo processInfo)
        {
            return ExecuteProcess(processInfo, null);
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="processInfo"></param>
        /// <param name="processParams"></param>
        public static object ExecuteProcess(ProcessInfo processInfo, Dictionary<string, object> processParams)
        {
            if (processInfo == null)
                return null;

            if (!processInfo.IsActive)
            {
                return null;
            }

            switch (processInfo.Type)
            {
                case ProcessType.CSharp:
                    string[] ss = processInfo.ExecuteParam.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (ss.Length != 2)
                    {
                        throw new ArgumentException("processInfo.ExecuteParam of " + processInfo.Name + " count should be 2", "ExecuteParam");
                    }

                    if (processParams == null)
                    {
                        return Feng.Utils.ReflectionHelper.RunStaticMethod(ss[0], ss[1], null);
                    }
                    else
                    {
                        object[] p = new object[processParams.Count];
                        int i = 0;
                        foreach (object j in processParams.Values)
                        {
                            p[i] = j;
                            i++;
                        }
                        return Feng.Utils.ReflectionHelper.RunStaticMethod(ss[0], ss[1], p);
                    }
                case ProcessType.PythonFile:
                    {
                        return PythonHelper.ExecutePythonFile(processInfo.ExecuteParam, processParams);
                    }
                case ProcessType.PythonStatement:
                    {
                        return PythonHelper.ExecutePythonStatement(processInfo.ExecuteParam, processParams);
                    }
                case ProcessType.Python:
                    {
                        return TryExecutePython(processInfo.ExecuteParam, processParams);
                    }
                default:
                    throw new NotSupportedException("processInfo.Type of " + processInfo.Name + "is Invalid!");
            }
        }

        private const string s_pythonCompiledAssembly = "PythonScript.dll";
        /// <summary>
        /// 执行Python。通过source和是否存在文件自动判别执行类型
        /// 如果是文件（.py结尾）
        /// 首先在Script目录下寻找.py源文件名，再在数据库ResourceInfo中寻找
        /// 再在Script目录下寻找单个.py编译成的同名Assembly。
        /// 如果还找不到，则在工作目录下找"PythonScript.dll，执行对应方法。
        /// 在.py文件中，执行的是execute方法（如果带当前窗口参数，参数是masterForm)
        /// 如果不是，则按照Statement执行。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="processParams"></param>
        /// <returns></returns>
        public static object TryExecutePython(string source, Dictionary<string, object> processParams)
        {
            try
            {
                if (source.EndsWith(".py", StringComparison.InvariantCulture))
                {
                    string filePath = ServiceProvider.GetService<IApplicationDirectory>().GetLocalResourcePath(source);
                    if (System.IO.File.Exists(filePath))
                    {
                        return PythonHelper.ExecutePythonFile(filePath, processParams);
                    }
                    else
                    {
                        ResourceContent pythonResource = ResourceInfoHelper.ResolveResource(source, ResourceType.PythonSource);

                        if (pythonResource != null)
                        {
                            switch (pythonResource.Type)
                            {
                                case ResourceContentType.File:
                                    return PythonHelper.ExecutePythonFile(pythonResource.Content.ToString(), processParams);
                                case ResourceContentType.String:
                                    return PythonHelper.ExecutePythonStatement(pythonResource.Content.ToString(), processParams);
                                default:
                                    throw new NotSupportedException("Invalid Resource Content Type!");
                            }
                        }
                        else
                        {
                            string singleAssemblyPath = System.IO.Path.ChangeExtension(filePath, ".dll");
                            if (System.IO.File.Exists(singleAssemblyPath))
                            {
                                return PythonHelper.ExecutePythonAssembly(singleAssemblyPath, System.IO.Path.GetFileName(filePath), processParams);
                            }
                            else
                            {
                                string combileAssemblyPath = System.IO.Path.Combine(ServiceProvider.GetService<IApplicationDirectory>().GetMainDirectory(), s_pythonCompiledAssembly);
                                if (System.IO.File.Exists(combileAssemblyPath))
                                {
                                    return PythonHelper.ExecutePythonAssembly(combileAssemblyPath, System.IO.Path.GetFileName(filePath), processParams);
                                }
                                else
                                {
                                    return null;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (source.Contains("result = "))
                    {
                        return PythonHelper.ExecutePythonStatement(source, processParams);
                    }
                    else
                    {
                        return PythonHelper.ExecutePythonExpression(source, processParams);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Python source {0} is invalid!", source), ex);
            }
        }
    }
}
