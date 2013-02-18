using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class ResourceInfoHelper
    {
        private static Dictionary<string, string> s_resourceMd5 = null;
        
        private static bool IsResourceMd5Right(string resourceName, string md5)
        {
            if (s_resourceMd5 == null)
            {
                s_resourceMd5 = Cache.TryGetCache<Dictionary<string, string>>("Resource_Md5", new Func<Dictionary<string, string>>(delegate()
                {
                    System.Data.DataTable dt = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT ResourceName, Md5 FROM AD_Resource");
                    Dictionary<string, string> resourceMd5 = new Dictionary<string, string>();
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        resourceMd5[row["ResourceName"].ToString()] = row["Md5"].ToString();
                    }
                    return resourceMd5;
                }));
            }
            if (s_resourceMd5 != null && s_resourceMd5.ContainsKey(resourceName) && s_resourceMd5[resourceName] == md5)
            {
                return true;
            }
            return false;
        }
        private static void ResolveResourceDependency(string resourceName, ResourceType resourceType)
        {
            IList<ResourceDependencyInfo> infos = ADInfoBll.Instance.GetInfos<ResourceDependencyInfo>(
                string.Format("from ResourceDependencyInfo where ResourceName = '{0}' and IsActive = true order by SeqNo", resourceName));

            foreach (ResourceDependencyInfo i in infos)
            {
                ResolveResource(i.DependentResourceName, resourceType, true);
            }

            //switch (resourceType)
            //{
            //    // Text
            //    case ResourceType.PythonSource:
            //    case ResourceType.MsReport:
            //    case ResourceType.Dataset:
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            foreach (ResourceDependencyInfo i in infos)
            //            {
            //                sb.Append(ResolveResource(i.DependentResourceName, resourceType));
            //                sb.Append(System.Environment.NewLine);
            //            }

            //            return sb.ToString();
            //        }
            //    // Binary
            //    case ResourceType.Report:
            //    case ResourceType.File:
            //        {
            //            foreach (ResourceDependencyInfo i in infos)
            //            {
            //                ResolveResource(i.DependentResourceName, resourceType);
            //            }
            //            return null;

            //            //StringBuilder sb = new StringBuilder();
            //            //foreach (ResourceDependencyInfo i in infos)
            //            //{
            //            //    sb.Append(Convert.ToBase64String(ResolveResource(i.DependentResourceName, resourceType) as byte[]));
            //            //}

            //            //return Convert.FromBase64String(sb.ToString());
            //        }
            //}

            //return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public static ResourceContent ResolveResource(string resourceName, ResourceType resourceType)
        {
            return ResolveResource(resourceName, resourceType, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="resourceType"></param>
        /// <param name="forcePersist"></param>
        /// <returns></returns>
        public static ResourceContent ResolveResource(string resourceName, ResourceType resourceType, bool forcePersist)
        {
            ResolveResourceDependency(resourceName, resourceType);

            switch (resourceType)
            {
                // Text
                case ResourceType.PythonSource:
                case ResourceType.MsReport:
                case ResourceType.Dataset:
                case ResourceType.Config:
                    {
                        string fileName = ServiceProvider.GetService<IApplicationDirectory>().GetLocalResourcePath(resourceName);
                        if (System.IO.File.Exists(fileName))
                        {
                            //return new ResourceContent { Type = ResourceContentType.String, Content = System.IO.File.ReadAllText(fileName) };
                            return new ResourceContent { Type = ResourceContentType.File, Content = fileName };
                        }

                        fileName = ServiceProvider.GetService<IApplicationDirectory>().GetServerResourcePath(resourceName);
                        if (System.IO.File.Exists(fileName))
                        {
                            string s = System.IO.File.ReadAllText(fileName);
                            string md5 = Cryptographer.Md5(s);
                            if (IsResourceMd5Right(resourceName, md5))
                            {
                                //return new ResourceContent { Type = ResourceContentType.String, Content = s };
                                return new ResourceContent { Type = ResourceContentType.File, Content = fileName };
                            }
                        }
                        bool persistLocal;
                        ResourceContent res = GetResource(resourceName, resourceType, out persistLocal);
                        if (res == null)
                            return null;

                        if (res.Type != ResourceContentType.String)
                        {
                            throw new ArgumentException("Resource should be String Type!");
                        }
                        if (forcePersist || persistLocal)
                        {
                            Feng.Utils.IOHelper.TryCreateDirectory(fileName);
                            System.IO.File.WriteAllText(fileName, (string)res.Content);
                            return new ResourceContent { Type = ResourceContentType.File, Content = fileName };
                        }
                        else
                        {
                            return res;
                        }
                        
                    }
                // Binary
                case ResourceType.Report:
                case ResourceType.File:
                    {
                        string fileName = ServiceProvider.GetService<IApplicationDirectory>().GetLocalResourcePath(resourceName);
                        if (System.IO.File.Exists(fileName))
                        {
                            //return System.IO.File.ReadAllBytes(fileName);
                            return new ResourceContent { Type = ResourceContentType.File, Content = fileName };
                        }
                        fileName = ServiceProvider.GetService<IApplicationDirectory>().GetServerResourcePath(resourceName);
                        if (System.IO.File.Exists(fileName))
                        {
                            byte[] s = System.IO.File.ReadAllBytes(fileName);
                            string md5 = Cryptographer.Md5(Convert.ToBase64String(s));
                            if (IsResourceMd5Right(resourceName, md5))
                            {
                                //return s;
                                return new ResourceContent { Type = ResourceContentType.File, Content = fileName };
                            }
                        }
                        bool persistLocal;
                        ResourceContent res = GetResource(resourceName, resourceType, out persistLocal);
                        if (res == null)
                            return null;

                        if (res.Type != ResourceContentType.Binary)
                        {
                            throw new ArgumentException("Resource should be Binary[] Type!");
                        }
                        if (forcePersist || persistLocal)
                        {
                            Feng.Utils.IOHelper.TryCreateDirectory(fileName);
                            System.IO.File.WriteAllBytes(fileName, (byte[])res.Content);
                            return new ResourceContent { Type = ResourceContentType.File, Content = fileName };
                        }
                        else
                        {
                            return res;
                        }
                    }
            }

            return null;
        }

        private static ResourceContent GetResource(string resourceName, ResourceType resourceType, out bool persistLocal)
        {
            ResourceInfo info = ADInfoBll.Instance.GetResourceInfo(resourceName, resourceType);
            if (info == null)
            {
                persistLocal = false;
                return null;
            }
            persistLocal = info.PersistLocal;
            switch (resourceType)
            {
                case ResourceType.PythonSource:
                case ResourceType.MsReport:
                case ResourceType.Dataset:
                case ResourceType.Config:
                    return new ResourceContent { Type = ResourceContentType.String, Content = info.Content };
                // Binary
                case ResourceType.Report:
                case ResourceType.File:
                    return new ResourceContent { Type = ResourceContentType.Binary, Content = Convert.FromBase64String(info.Content) };
                default:
                    throw new NotSupportedException("Invalid ResourceType!");
            }
            //if (ReflectionHelper.GetTypeFromName(info.ContentType) == typeof(byte[]))
            //{
            //    return Convert.FromBase64String(info.Content);
            //}
            //else
            //{
            //    return Convert.ChangeType(info.Content, ReflectionHelper.GetTypeFromName(info.ContentType));
            //}
        }
    }
}
