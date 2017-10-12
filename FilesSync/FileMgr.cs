using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace FilesSync
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FileMgr : FileMgrInterface
    {
        private string targetDir;

        public FileMgr(string targetDir)
        {
            this.targetDir = Path.GetFullPath(targetDir);
        }

        public FileDigest[] ListAllFiles(string dirName)
        {
            var baseDir = Path.Combine(this.targetDir, dirName);
            List<FileDigest> rlt = new List<FileDigest>();
            EnumFiles(baseDir, rlt);
            return rlt.ToArray();
        }

        private void EnumFiles(string dir, ICollection<FileDigest> col)
        {
            foreach(var path in Directory.EnumerateFiles(dir))
            {
                var fi = new FileInfo(path);
                var fd = new FileDigest();
                fd.FileName = Path.GetFileName(path);
                fd.LastModifyTime = fi.LastWriteTimeUtc;
                col.Add(fd);
            }
            foreach(var path in Directory.EnumerateDirectories(dir))
            {
                EnumFiles(path, col);
            }
        }

        public byte[] GetFileContent(string dirName, string fileName)
        {
            var filePath = Path.Combine(this.targetDir, dirName, fileName);
            if (!File.Exists(filePath))
            {
                return null;
            }
            else
            {
                return File.ReadAllBytes(filePath);
            }
        }
    }
}
