using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System.ServiceModel.Syndication;
using System.Net;
using System.Xml;
using System.IO;
using System.Globalization;
using System.ComponentModel;
using System.Reflection;
using System.ServiceModel.Description;
using System.Linq;

namespace Feng.Server.Utils
{
    public static class ServiceHelper
    {
        public static string GetWindowTabNameFromAddress(string address)
        {
            return WebServiceInstance.Instance.GetWebServiceTag(address);
        }

        public static ISearchManager GetSearchManagerFromWindowTab(string tabName)
        {
            if (s_searchManagers.ContainsKey(tabName))
                return s_searchManagers[tabName];

            WindowTabInfo tabInfo = ADInfoBll.Instance.GetWindowTabInfo(tabName);
            ISearchManager sm = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(tabInfo, null);
            Feng.Windows.Utils.ManagerFactory.CreateSearchManagerEagerFetchs(sm, tabInfo.GridName);

            s_searchManagers[tabName] = sm;
            return s_searchManagers[tabName];
        }

        private static Dictionary<string, ISearchManager> s_searchManagers = new Dictionary<string, ISearchManager>();
        //public static ISearchManager GetSearchManagerFromAddress(string address)
        //{
        //    string tabName = GetWindowTabNameFromAddress(address);
        //    if (string.IsNullOrEmpty(tabName))
        //        return null;

        //    return GetSearchManagerFromWindowTab(tabName);
        //}

        internal static void CopyAttributeBehaviors(ServiceDescription description, Type[] behaviorTypes, string derivedMethodName, string[] operationNames)
        {
            Type serviceType = description.ServiceType;
            MethodInfo getEntriesMethod = serviceType.GetMethod(derivedMethodName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (Type behaviorType in behaviorTypes)
            {
                object[] attrs = getEntriesMethod.GetCustomAttributes(behaviorType, true);
                if (attrs != null && attrs.Length > 0)
                {
                    IOperationBehavior attrAsBehavior = (IOperationBehavior)attrs[0];
                    foreach (ServiceEndpoint endpoint in description.Endpoints)
                    {
                        foreach (OperationDescription od in endpoint.Contract.Operations)
                        {
                            if (operationNames.Contains(od.Name))
                            {
                                od.Behaviors.Remove(behaviorType);
                                od.Behaviors.Add(attrAsBehavior);
                            }
                        }
                    }
                }
            }
        }
    }
}
