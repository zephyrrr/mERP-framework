using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using com.neokernel.nk;

namespace Feng.NeoKernel.Agent
{
    public class SoaDataServiceAgent : com.neokernel.nk.Agent
    {
        public override void initProps()
        {
            ProgramHelper.InitProgram();

            setDefault("uri", "http://localhost:8080/Services/SoaDataService.svc");
        }

        public override void start()
        {
            println("Starting SoaDataService at " + getString("uri"));

            //ServiceHost serviceHost = new ServiceHost(typeof(SoaDataService), new Uri(getString("uri")));
            //serviceHost.Open();

            string addressName = base.getString("uri");

            Type serviceContractType = Feng.Utils.ReflectionHelper.GetTypeFromName("ComponentArt.SOA.UI.ISoaDataGridService, ComponentArt.SOA.UI");
            //typeof(ComponentArt.SOA.UI.ISoaDataGridService);

            ServiceHost serviceHost = new ServiceHost(Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Server.ComponentArtSOA.SoaDataService, Feng.Server.ComponentArtSOA"));
            //typeof(SoaDataService));
            
            serviceHost.AddServiceEndpoint(serviceContractType, new BasicHttpBinding(), addressName);

            ServiceMetadataBehavior serviceBehavior = new ServiceMetadataBehavior();
            serviceBehavior.HttpGetEnabled = true;
            serviceBehavior.HttpGetUrl = new Uri(addressName);
            serviceHost.Description.Behaviors.Add(serviceBehavior);

            serviceHost.Open();

            println("Service Open At:");
            foreach (ServiceEndpoint e in serviceHost.Description.Endpoints)
            {
                println(e.Address.ToString());
            }

            //println("press Q to quit.");
            //while (Console.ReadKey(true).Key != ConsoleKey.Q) ;

            //serviceHost.Close();
        }
    }
}
