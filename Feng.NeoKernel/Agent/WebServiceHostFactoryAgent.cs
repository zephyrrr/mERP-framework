using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Runtime.Serialization;
using System.Reflection;
using System.Reflection.Emit;
using Feng.Server.Service;
using Feng.Server.Wcf;

namespace Feng.NeoKernel.Agent
{
    public class WebServiceHostFactoryAgent : com.neokernel.nk.Agent
    {
        public override void initProps()
        {
            setDefault("uri", "http://localhost:8080");
        }

        private WebHttpBinding CreateWebHttpBinding()
        {
            WebHttpBinding binding = new WebHttpBinding();
            //binding.Security.Mode = WebHttpSecurityMode.TransportCredentialOnly;
            //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            return binding;
        }

        private BasicHttpBinding CreateBasicHttpBinding()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            //binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            return binding;
        }

        private void CreateCrossDomainService()
        {
            string realAddress = base.getString("uri");

            Type serviceContractType = typeof(ICrossDomainService);
            Type serviceServiceType = typeof(CrossDomainService);

            ServiceHost serviceHost = new ServiceHost(serviceServiceType);
            serviceHost.AddServiceEndpoint(serviceContractType, CreateWebHttpBinding(), realAddress)
                .Behaviors.Add(new WebHttpBehavior());

            AddServiceHost(serviceHost, "CrossDomainService", realAddress);
        }

        private void CreateApplicationService(Type serviceType, string address)
        {
            WebServiceHost serviceHost = new WebServiceHost(serviceType);

            string realAddress = base.getString("uri") + address;
            serviceHost.AddServiceEndpoint(serviceType, CreateBasicHttpBinding(), new Uri(realAddress))
                .Behaviors.Add(new WebHttpBehavior());

            AddServiceHost(serviceHost, serviceType.Name, realAddress);
        }

        private void CreateRestService(Type contractType, Type serviceType, string address, string tag)
        {
            string realAddress = base.getString("uri") + address;

            WebServiceHost serviceHost = new WebServiceHost(serviceType);
            serviceHost.AddServiceEndpoint(contractType, CreateWebHttpBinding(), realAddress);

            AddServiceHost(serviceHost, string.Format("Rest ServiceHost of {0}({1})", serviceType.Name, tag), realAddress);

            Feng.Server.Utils.WebServiceInstance.Instance.AddWebService(address, tag);
        }

        private void CreateSoapService(Type contractType, Type serviceType, string address, string tag)
        {
            string realAddress = base.getString("uri") + address;

            ServiceHost serviceHost = new ServiceHost(serviceType);
            serviceHost.AddServiceEndpoint(contractType, CreateBasicHttpBinding(), realAddress);

            AddServiceHost(serviceHost, string.Format("Soap ServiceHost of {0}({1})", serviceType.Name, tag), realAddress);

            Feng.Server.Utils.WebServiceInstance.Instance.AddWebService(address, tag);
        }

        private void CreateDataSearchViewSeriveHost(string tabName, string restAddress, string soapAddress)
        {
            WindowTabInfo tabInfo = ADInfoBll.Instance.GetWindowTabInfo(tabName);
            if (tabInfo == null)
                return;

            ISearchManager sm = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(tabInfo, null);
            Type entityType = Feng.Server.Utils.TypeHelper.GenerateTypeFromGridColumn(tabInfo.GridName);
            Type serviceServiceType = Feng.Utils.ReflectionHelper.CreateGenericType(typeof(DataSearchViewService<>), new Type[] { entityType });

            if (!string.IsNullOrEmpty(restAddress))
            {
                Type serviceContractType = Feng.Utils.ReflectionHelper.CreateGenericType(typeof(IDataSearchViewRestService<>), new Type[] { entityType });

                CreateRestService(serviceContractType, serviceServiceType, restAddress, tabName);
            }

            if (!string.IsNullOrEmpty(soapAddress))
            {
                Type serviceContractType = Feng.Utils.ReflectionHelper.CreateGenericType(typeof(IDataSearchViewSoapService<>), new Type[] { entityType });

                CreateSoapService(serviceContractType, serviceServiceType, soapAddress, tabName);
            }
        }

        private void CreateDataSearchViewSeriveHost(string restAddress, string soapAddress)
        {
            Type serviceServiceType = typeof(DataSearchViewService);

            if (!string.IsNullOrEmpty(restAddress))
            {
                Type serviceContractType = typeof(IDataSearchViewRestService);

                CreateRestService(serviceContractType, serviceServiceType, restAddress, string.Empty);
            }

            if (!string.IsNullOrEmpty(soapAddress))
            {
                Type serviceContractType = typeof(IDataSearchViewRestService);

                CreateRestService(serviceContractType, serviceServiceType, soapAddress, string.Empty);
            }
        }

        private void CreateDataSearchSeriveHost(string tabName, string restAddress, string soapAddress)
        {
            WindowTabInfo tabInfo = ADInfoBll.Instance.GetWindowTabInfo(tabName);
            if (tabInfo == null)
                return;

            ISearchManager sm = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(tabInfo, null);

            Type entityType = Utils.ReflectionHelper.GetGenericUnderlyingType(sm.GetType());
            if (entityType == null)
            {
                throw new ArgumentException("SearchManager should be Generic type!", "entityType");
            }
            entityType = Feng.Server.Utils.TypeHelper.AddDataContractAttributeToType(entityType);

            Type serviceServiceType = Feng.Utils.ReflectionHelper.CreateGenericType(typeof(DataSearchService<,>), new Type[] { entityType, entityType });

            if (!string.IsNullOrEmpty(restAddress))
            {
                Type serviceContractType = Feng.Utils.ReflectionHelper.CreateGenericType(typeof(IDataSearchRestService<>), new Type[] { entityType });

                CreateRestService(serviceContractType, serviceServiceType, restAddress, tabName);
            }

            if (!string.IsNullOrEmpty(soapAddress))
            {
                Type serviceContractType = Feng.Utils.ReflectionHelper.CreateGenericType(typeof(IDataSearchSoapService<>), new Type[] { entityType });

                CreateSoapService(serviceContractType, serviceServiceType, soapAddress, tabName);
            }
        }

        private void CreateNameValueMappingSeriveHost(string restAddress, string soapAddress)
        {
            Type serviceServiceType = typeof(NameValueMappingService);
            Type serviceContractType = typeof(INameValueMappingService);

            if (!string.IsNullOrEmpty(restAddress))
            {
                CreateRestService(serviceContractType, serviceServiceType, restAddress, string.Empty);
            }

            if (!string.IsNullOrEmpty(soapAddress))
            {
                CreateSoapService(serviceContractType, serviceServiceType, soapAddress, string.Empty);
            }
        }


        IList<ServiceHost> m_hosts = new List<ServiceHost>();
        public override void start()
        {
            ProgramHelper.InitProgram();

            CreateCrossDomainService();

            IList<WebServiceInfo> webServiceInfos = ADInfoBll.Instance.GetInfos<WebServiceInfo>();
            foreach (WebServiceInfo info in webServiceInfos)
            {
                try
                {
                    switch (info.Type)
                    {
                        case WebServiceType.DataSearchView:
                            {
                                CreateDataSearchViewSeriveHost(info.ExecuteParam, info.RestAddress, info.SoapAddress);
                            }
                            break;
                        case WebServiceType.DataSearchViewAll:
                            {
                                CreateDataSearchViewSeriveHost(info.RestAddress, info.SoapAddress);
                            }
                            break;
                        case WebServiceType.DataSearch:
                            {
                                CreateDataSearchSeriveHost(info.ExecuteParam, info.RestAddress, info.SoapAddress);
                            }
                            break;
                        case WebServiceType.ADInfoSearch:
                            {
                                string[] ss = new string[] {"AD_Menu", "AD_Action", "AD_Window", "AD_Window_Tab",
                                    "AD_Grid", "AD_Grid_Column", "AD_Grid_Row", "AD_Grid_Cell"};
                                foreach (string s in ss)
                                {
                                    CreateDataSearchSeriveHost(s, string.IsNullOrEmpty(info.RestAddress) ? null : info.RestAddress + "/" + s,
                                        string.IsNullOrEmpty(info.SoapAddress) ? null : info.SoapAddress + "/" + s);
                                }
                            }
                            break;
                        case WebServiceType.NameValueMapping:
                            {
                                CreateNameValueMappingSeriveHost(info.RestAddress, info.SoapAddress);
                            }
                            break;
                        case WebServiceType.AuthenticationService:
                            {
                                CreateApplicationService(typeof(System.Web.ApplicationServices.AuthenticationService), info.SoapAddress);
                            }
                            break;
                        case WebServiceType.RoleService:
                            {
                                CreateApplicationService(typeof(System.Web.ApplicationServices.RoleService), info.SoapAddress);
                            }
                            break;
                        case WebServiceType.ProfileService:
                            {
                                CreateApplicationService(typeof(System.Web.ApplicationServices.ProfileService), info.SoapAddress);
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithResume(ex);
                    println("Create Service " + info.Name + " Failed!" + System.Environment.NewLine + ex.Message);
                }
            }

            //while (Console.ReadKey(true).Key != ConsoleKey.Q) ;

            //foreach (ServiceHost host in m_hosts)
            //{
            //    host.Close();
            //}
        }

        private void AddServiceHost(ServiceHost serviceHost, string hostDescription, string address)
        {
            ServiceMetadataBehavior smb = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (smb == null)
            {
                smb = new ServiceMetadataBehavior();
                serviceHost.Description.Behaviors.Add(smb);
            }
            smb.HttpGetEnabled = true;
            smb.HttpGetUrl = new Uri(address);

            //ServiceAuthenticationBehavior sab = serviceHost.Description.Behaviors.Find<ServiceAuthenticationBehavior>();
            //sab.ServiceAuthenticationManager = new ServiceAuthenticationManager();

#if DEBUG
            ServiceDebugBehavior sdb = serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();
            if (sdb == null)
            {
                sdb = new ServiceDebugBehavior();
                serviceHost.Description.Behaviors.Add(sdb);
            }
            sdb.HttpHelpPageEnabled = true;
            sdb.IncludeExceptionDetailInFaults = true;

            foreach (var endpoint in serviceHost.Description.Endpoints)
            {
                MonitorBehavior mb = endpoint.Behaviors.Find<MonitorBehavior>();
                if (mb == null)
                {
                    mb = new MonitorBehavior();
                    endpoint.Behaviors.Add(mb);
                }
            }
#endif

            m_hosts.Add(serviceHost);
            serviceHost.Open();
            println(string.Format("{0} is opened in {1}!", hostDescription, address));
        }

        
    }
}
