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
            try
            {
                return base.Channel.ListAllFiles(dirName);
            }
            catch(Exception ex)
            {
                Logger.Instance.Write(LogLevel.Error, ex.Message);
                throw new Exception("failed", ex);
            }
        }

        public byte[] GetFileContent(string dirName, string fileName)
        {
            try
            {
                return base.Channel.GetFileContent(dirName, fileName);
            }
            catch (Exception ex)
            {
                Logger.Instance.Write(LogLevel.Error, ex.Message);
                throw new Exception("failed", ex);
            }
        }
    }
}
