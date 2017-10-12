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
            if(args.Length < 1)
            {
                Console.Write(GetHelpText());
            } else if(args[0] == "--daemon")
            {
                var srv = new FileMgrListener();
                Console.WriteLine("server listen uri: " + srv.BaseUri + ".");
                srv.Run();
            }
            else if (args[0] == "sync")
            {
                var client = new FileSync();
                client.Sync(args);
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
            sb.Append("    FileSync --daemon             run file sync server.\n");
            sb.Append("    FileSync --server=<url>       sync files from server.\n");
            return sb.ToString();
        }
    }
}
