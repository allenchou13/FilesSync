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
        private FileMgr fileMgr;

        public FileMgrListener(string listenUri, FileMgr fileMgr)
        {
            if(listenUri == null)
            {
                this.listenUri = GetDefaultListenUri();
            }
            else
            {
                this.listenUri = listenUri;
            }

            this.fileMgr = fileMgr;
        }

        public void Run()
        {
            var listener = new ServiceHost(fileMgr, new Uri(listenUri));
            listener.AddServiceEndpoint(CreateServiceEndpoint(listenUri));
            listener.Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetEnabled = true });

            listener.Open();

            Console.ReadKey();
        }

        public static ServiceEndpoint CreateServiceEndpoint(string listenUri)
        {
            if (listenUri == null)
            {
                listenUri = FileMgrListener.GetDefaultListenUri();
            }

            var endpoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(FileMgrInterface)));
            endpoint.Address = new EndpointAddress(listenUri);
            endpoint.Binding = new WebHttpBinding();
            endpoint.EndpointBehaviors.Add(new WebHttpBehavior { HelpEnabled = true });

            return endpoint;
        }

        public string BaseUri => this.listenUri;



        public static string GetDefaultListenUri()
        {
            return "http://localhost:13990/";
        }
    }
}
