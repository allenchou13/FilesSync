using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FilesSync
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = ProcessOptions(args);
            if (options.ContainsKey("daemon"))
            {
                var fileMgr = new FileMgr(GetDictItem(options, "target-dir"));
                var listener = new FileMgrListener(GetDictItem(options, "service-url"), fileMgr);
                Console.WriteLine("server listen uri: " + listener.BaseUri + ".");
                listener.Run();
            }
            else if (options.ContainsKey("sync"))
            {
                var client = new FileSync();
                client.Sync(GetDictItem(options, "service-url"),
                    GetDictItem(options, "name"),
                    GetDictItem(options, "targit-dir"));
                Console.WriteLine("done");
            }
            else
            {
                Console.Write(GetHelpText());
            }
        }

        static string GetHelpText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Usage: \n");
            sb.Append("    FileSync --daemon [options]      run file sync server.\n");
            sb.Append("    FileSync --sync [options]        sync files from server.\n");
            sb.Append("    sync files from server target dir's subdirectory(name) to client target dir.");
            sb.Append("Options:\n");
            sb.Append("    --name=<name> version name.\n");
            sb.Append("    --service-url=<url> service url.\n");
            sb.Append("    --target-dir=<path> target dir.\n");
            sb.Append("Examples:\n");
            sb.Append("    FileSync --daemon --service-url=http://127.0.0.1:12000/ --target-dir=./files\n");
            sb.Append("    FileSync --sync --service-url=http://127.0.0.1:12000/ --name=latest --target-dir=./files\n");
            return sb.ToString();
        }

        static Dictionary<string, string> ProcessOptions(string[] args)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();
            Match matchRlt;
            string[] optionNames = new string[] {
                    "service-url",
                    "target-dir",
                    "name"
                };
            for (var argi = 1; argi < args.Length; argi++)
            {
                var arg = args[argi];
                foreach(var optionName in optionNames)
                {
                    if (Match(arg, $"^--{optionName}=(.*)$", out matchRlt))
                    {
                        var value = matchRlt.Groups[1].Value;
                        options.Add(optionName, value);
                    }
                }
            }
            return options;
        }

        static bool Match(string input, string pattern, out Match rlt)
        {
            var matchRlt = Regex.Match(input, pattern);
            rlt = matchRlt;
            return matchRlt.Success;
        }

        static string GetDictItem(Dictionary<string,string> dict, string name)
        {
            string value;
            if (dict.TryGetValue(name, out value))
            {
                if(value == "")
                {
                    return null;
                }
                else
                {
                    return value;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
