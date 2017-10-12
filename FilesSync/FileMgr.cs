using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesSync
{
    public class FileMgr : FileMgrInterface
    {
        public FileDigest[] ListAllFiles(string dirName)
        {
            var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dirName);
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
            var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dirName);
            var filePath = Path.Combine(baseDir, fileName);
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
