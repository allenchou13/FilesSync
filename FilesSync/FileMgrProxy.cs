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
            :base(FileMgrListener.CreateServiceEndpoint(serviceUrl))
        {
            if(serviceUrl == null)
            {
                serviceUrl = FileMgrListener.GetDefaultListenUri();
            }

            this.serviceUrl = serviceUrl;
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
