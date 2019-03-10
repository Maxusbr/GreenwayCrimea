//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Modules.RetailCRM
{
    public class RetailCRMLog
    {
        public int TotalRow;
        public int TotalAddRow;
        public int TotalUpdateRow;
        public int TotalErrorRow;

        private string _logFile;

        public RetailCRMLog()
        {
            TotalRow = 0;
            TotalAddRow = 0;
            TotalUpdateRow = 0;
            TotalErrorRow = 0;
            var logDirectory = SettingsGeneral.AbsolutePath + "Modules\\" + RetailCRMModule.ModuleStringId + "\\log\\";
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);
            var files = Directory.GetFiles(logDirectory, "importLog_*.txt").Select(Path.GetFileName).OrderByDescending(fName => fName).ToList();
            foreach (var file in files.Where(file => files.IndexOf(file) > 9)) // оставляем последние 10 файлов
            {
                if (File.Exists(logDirectory + file))
                    File.Delete(logDirectory + file);
            }

            var key = ModuleSettingsProvider.GetSettingValue<string>("ApiKey", RetailCRMModule.ModuleStringId);

            _logFile = logDirectory + "importLog_" + DateTime.Now.ToString("yyyy-MM-dd")
                    + "_" + (key.IsNullOrEmpty() ? "" : key.Md5()) + ".txt";
        }

        public void Write(string value)
        {
            using (var streamWriter = new StreamWriter(_logFile, true, Encoding.UTF8))
            {
                streamWriter.WriteLine("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss"), value);
            }
        }

        public static List<string> GetLogFiles()
        {
            var logDirectory = SettingsGeneral.AbsolutePath + "Modules\\" + RetailCRMModule.ModuleStringId + "\\log\\";
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);
            return Directory.GetFiles(logDirectory, "importLog_*.txt").Select(Path.GetFileName).ToList();
        }
    }
}