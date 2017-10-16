using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FilesSync
{
    public class Logger
    {
        public static Logger Instance = new Logger();

        private Logger() { }

        public void Write(LogLevel level, string msg, [CallerMemberName]string callerMemberName = "")
        {
            var record = new LogRecord() { Level = level, CallerName = callerMemberName, Message = msg };
            SaveRecord(record);
        }

        private void SaveRecord(LogRecord record)
        {
            Console.WriteLine(record.ToString());
            File.AppendText("log.txt").With(x=>x.WriteLine(record.ToString())).Close();
        }
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error
    }

    public class LogRecord
    {
        private Dictionary<string, object> dynamicFileds = new Dictionary<string, object>();

        public DateTime Time { get; set; }
        public LogLevel Level { get; set; }
        public string CallerName { get; set; }
        public string Message { get; set; }

        public LogRecord()
        {
            Time = DateTime.Now;
        }

        public string[] GetDynamicFieldNames()
        {
            return dynamicFileds.Keys.ToArray();
        }

        public object GetDynamicField(string name)
        {
            if (dynamicFileds.ContainsKey(name))
            {
                return dynamicFileds[name];
            }
            else
            {
                return null;
            }
        }

        public T GetDynamicField<T>(string name)
        {
            object value = GetDynamicField(name);
            try
            {
                return (T)value;
            }
            catch(Exception ex)
            {
                return default(T);
            }
        }


        public override string ToString()
        {
            return $"{DateTime.Now}  {this.Level,6}  {this.CallerName}: {this.Message}";
        }
    }
}
