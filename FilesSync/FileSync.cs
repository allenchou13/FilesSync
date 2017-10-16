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
            targetDir = Path.GetFullPath(targetDir);
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }
            FileMgrInterface mgr = new FileMgrProxy(serviceUrl);
            var files = mgr.ListAllFiles(name);
            for (var i = 0; i < files.Length; i++)
            {
                var f = files[i];
                var fpath = Path.Combine(targetDir, f.FileName);

                if (File.Exists(fpath))         // if file is newer than server's, not need to update
                {
                    var time = File.GetLastWriteTimeUtc(fpath);
                    if(time >= f.LastModifyTime)
                    {
                        Logger.Instance.Write(LogLevel.Info, $"skip file {f.FileName}.");
                        continue;
                    }
                }

                var fc = mgr.GetFileContent(name, f.FileName);
                File.WriteAllBytes(fpath, fc);
                File.SetLastWriteTimeUtc(fpath, f.LastModifyTime);
                Logger.Instance.Write(LogLevel.Info, $"updated file {f.FileName}.");
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
                Logger.Instance.Write(LogLevel.Warn, ex.Message);
            }
        }
    }
}
