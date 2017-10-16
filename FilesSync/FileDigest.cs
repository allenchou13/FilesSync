using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FilesSync
{
    [DataContract]
    public class FileDigest
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public DateTime LastModifyTime { get; set; }
    }
}
