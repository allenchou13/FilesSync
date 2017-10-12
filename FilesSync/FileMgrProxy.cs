using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace FilesSync
{
    public class FileMgrProxy : ClientBase<FileMgrInterface>, FileMgrInterface
    {
        private string serviceUrl;

        public FileMgrProxy(string serviceUrl = null)
            :base(CreateServiceEndpoint(serviceUrl))
        {
            if(serviceUrl == null)
            {
                serviceUrl = FileMgrListener.GetDefaultListenUri();
            }

            this.serviceUrl = serviceUrl;
        }

        static ServiceEndpoint CreateServiceEndpoint(string serviceUrl)
        {
            if(serviceUrl == null)
            {
                serviceUrl = FileMgrListener.GetDefaultListenUri();
            }


            var endpoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(FileMgrInterface)));
            endpoint.Address = new EndpointAddress(serviceUrl);
            endpoint.Binding = new WebHttpBinding();
            endpoint.EndpointBehaviors.Add(new WebHttpBehavior { HelpEnabled = true });

            return endpoint;
        }

        public FileDigest[] ListAllFiles(string dirName)
        {
            return base.Channel.ListAllFiles(dirName);
        }

        public byte[] GetFileContent(string dirName, string fileName)
        {
            return base.Channel.GetFileContent(dirName, fileName);
        }
    }
}
