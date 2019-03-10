using System;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace AdvantShop.Module.VkMarket.Services
{
    public static class VkMarketExportState
    {
        private static readonly object SyncObject = new object();
        private static string _fileLog = "";

        private static bool _isRun = false;
        public static bool IsRun
        {
            get { return _isRun; }
        }

        public const string FileLogPath = "~/modules/vkmarket/content/reports/";

        
        public static void Start()
        {
            lock (SyncObject)
            {
                _isRun = true;
                _fileLog = HostingEnvironment.MapPath(FileLogPath + "report_" + DateTime.Now.ToString("yy-MM-dd_hh-mm") + ".txt");
                if (File.Exists(_fileLog))
                    File.Delete(_fileLog);
            }
        }

        public static void Stop()
        {
            lock (SyncObject)
            {
                _isRun = false;
                _fileLog = "";
            }
        }

        public static void WriteLog(string message, params object[] p)
        {
            WriteLog(string.Format(message, p));
        }

        public static void WriteLog(string message)
        {
            lock (SyncObject)
            {
                if (!string.IsNullOrEmpty(_fileLog))
                    using (var fs = new FileStream(_fileLog, FileMode.Append, FileAccess.Write))
                    using (var sw = new StreamWriter(fs, Encoding.UTF8))
                        sw.WriteLine(message);
            }
        }
    }
}
