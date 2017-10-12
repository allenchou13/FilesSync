using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace FilesSync
{
    [ServiceContract]
    public interface FileMgrInterface
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        FileDigest[] ListAllFiles(string dirName);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        byte[] GetFileContent(string dirName, string fileName);
    }
}
