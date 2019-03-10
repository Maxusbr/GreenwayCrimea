//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.IO;
using System.Linq;
using System.Text;
using AdvantShop.Configuration;

namespace AdvantShop.Module.ReturnCustomer.Service
{
    public class RCLog
    {
        private string _logFile;

        public RCLog()
        {
            var logDirectory = SettingsGeneral.AbsolutePath + "userfiles\\Modules\\" + ReturnCustomer.ModuleStringId + "\\log\\";

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            _logFile = logDirectory + "SendingLog.txt";
        }

        public void Write(string value)
        {
            using (var streamWriter = new StreamWriter(_logFile, true, Encoding.UTF8))
            {
                streamWriter.WriteLine(value);
            }
        }

        public static string GetLogFile()
        {
            var logDirectory = SettingsGeneral.AbsolutePath + "userfiles\\Modules\\" + ReturnCustomer.ModuleStringId + "\\log\\";

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            return Directory.GetFiles(logDirectory).Select(Path.GetFileName).FirstOrDefault();
        }
    }
}