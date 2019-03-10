//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using AdvantShop.Configuration;
using Newtonsoft.Json;

namespace AdvantShop.Diagnostics
{
    public class LogTempData
    {
        public DateTime MailErrorLastSend { get; set; }
        public int MailErrorCurrentCount { get; set; }

        public LogTempData(int mailErrorCurrentCount, DateTime mailErrorLastSend)
        {
            MailErrorLastSend = mailErrorLastSend;
            MailErrorCurrentCount = mailErrorCurrentCount;
        }
    }

    public static class LogTempDataService
    {
        private static readonly string LogFile = SettingsGeneral.AbsolutePath + "App_Data/LogTempData.txt";

        public static LogTempData GetLogTempData()
        {
            if (!File.Exists(LogFile))
            {
                return new LogTempData(0, DateTime.Now);
            }

            using (var reader = new StreamReader(LogFile))
            {
                string input;
                if ((input = reader.ReadLine()) != null)
                {
                    return JsonConvert.DeserializeObject<LogTempData>(input) ?? new LogTempData(0, DateTime.Now);
                }
                return new LogTempData(0, DateTime.Now);
            }
        }

        public static void UpdateLogTempData(LogTempData data)
        {
            using (var writer = new StreamWriter(LogFile, false))
            {
                writer.WriteLine(JsonConvert.SerializeObject(data));
            }
        }
    }
}