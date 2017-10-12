using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.IO;
using System.Text.RegularExpressions;

namespace FilesSync
{
    public class FileSync
    {
        private void SyncFiles(string serviceUrl, string name, string targetDir)
        {
            FileMgrInterface mgr = new FileMgrProxy();
            var files = mgr.ListAllFiles(name);
            for (var i = 0; i < files.Length; i++)
            {
                var f = files[i];
                var fc = mgr.GetFileContent(name, f.FileName);
                var fpath = Path.Combine(targetDir, f.FileName);
                File.WriteAllBytes(fpath, fc);
                File.SetLastWriteTimeUtc(fpath, f.LastModifyTime);
            }
        }

        public void Sync(string serviceUrl, string name, string targetDir)
        {
            try
            {
                if (serviceUrl == null)
                {
                    serviceUrl = FileMgrListener.GetDefaultListenUri();
                }
                if (name == null)
                {
                    name = "latest";
                }
                if (targetDir == null)
                {
                    targetDir = AppDomain.CurrentDomain.BaseDirectory;
                }

                SyncFiles(serviceUrl, name, targetDir);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
