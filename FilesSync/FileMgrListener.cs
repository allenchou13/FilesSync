using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace FilesSync
{
    public class FileMgrListener
    {
        private string listenUri;

        public FileMgrListener(string listenUri = null)
        {
            if(listenUri == null)
            {
                this.listenUri = GetDefaultListenUri();
            }
            else
            {
                this.listenUri = listenUri;
            }
        }

        public void Run()
        {
            var listener = new ServiceHost(typeof(FileMgr), new Uri(listenUri));
            var endpoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(FileMgrInterface)));
            endpoint.Address = new EndpointAddress(listenUri);
            endpoint.Binding = new WebHttpBinding();
            endpoint.EndpointBehaviors.Add(new WebHttpBehavior { HelpEnabled = true });

            listener.AddServiceEndpoint(endpoint);
            listener.Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetEnabled = true });

            listener.Open();

            Console.ReadKey();
        }

        public string BaseUri => this.listenUri;



        public static string GetDefaultListenUri()
        {
            return "http://localhost:13990/";
        }
    }
}
