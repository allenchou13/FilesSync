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

        public void Sync(string[] args)
        {
            string serviceUrl = null;
            string targetDir = null;
            string name = null;
            for (var argi = 1; argi < args.Length; argi++)
            {
                var arg = args[argi];
                Match matchRlt;
                if (Match(arg, "^--service-url=(.+)$", out matchRlt))
                {
                    serviceUrl = matchRlt.Groups[1].Value;
                }
                else if (Match(arg, "^--target-dir=(.+)$", out matchRlt))
                {
                    targetDir = matchRlt.Groups[1].Value;
                }
                else if (Match(arg, "^--target-dir=(.+)$", out matchRlt))
                {
                    name = matchRlt.Groups[1].Value;
                }
            }

            Sync(serviceUrl, name, targetDir);
        }

        private bool Match(string input, string pattern, out Match rlt)
        {
            var matchRlt = Regex.Match(input, pattern);
            rlt = matchRlt;
            return matchRlt.Success;
        }
    }
}
