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

namespace AdvantShop.Module.SupplierOfHappiness.Domain
{
    public class SupplierOfHappinessLog
    {
        public int TotalRow;
        public int TotalAddRow;
        public int TotalUpdateRow;
        public int TotalErrorRow;

        private string _logFile;

        public SupplierOfHappinessLog()
        {
            TotalRow = 0;
            TotalAddRow = 0;
            TotalUpdateRow = 0;
            TotalErrorRow = 0;
            var logDirectory = SettingsGeneral.AbsolutePath + "Modules\\" + SupplierOfHappiness.ModuleID + "\\log\\";
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);
            var files = Directory.GetFiles(logDirectory, "importLog_*.txt").Select(Path.GetFileName).OrderByDescending(fName => fName).ToList();
            foreach (var file in files.Where(file => files.IndexOf(file) > 6)) // оставляем последние 7 файлов
            {
                if (File.Exists(logDirectory + file))
                    File.Delete(logDirectory + file);
            }
            _logFile = logDirectory + "importLog_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
        }

        public void Write(string value)
        {
            using (var streamWriter = new StreamWriter(_logFile, true, Encoding.UTF8))
            {
                streamWriter.WriteLine(value);
            }
        }

        public void WriteTotal()
        {
            Write("Всего: " + TotalRow);
            Write("Добавлено: " + TotalAddRow);
            Write("Обновлено: " + TotalUpdateRow);
            Write(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Закончил обновление");
            Write(string.Empty);
        }

        public static List<string> GetLogFiles()
        {
            var logDirectory = SettingsGeneral.AbsolutePath + "Modules\\" + SupplierOfHappiness.ModuleID + "\\log\\";
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);
            return Directory.GetFiles(logDirectory, "importLog_*.txt").Select(Path.GetFileName).ToList();
        }
    }
}