using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using CsvHelper;

namespace AdvantShop.Core.Services.Loging.Smses
{
    public class FileSmsLoger : ISmsLoger
    {
        private const string LogDirectoryPath = "~/app_data/SmsLogs/";
        private const string LogFileName = "LogSms_{0}.txt";
        private const int MaxFileLength = 1 * 1024 * 1024;
        private const int MaxFilesCont = 10;

        public void LogSms(TextMessage message)
        {
            new Task(() => LogEmailInternal(message)).Start();
        }

        private void LogEmailInternal(TextMessage message)
        {
            string directoryPath = HostingEnvironment.MapPath(LogDirectoryPath);
            if (directoryPath == null)
            {
                throw new Exception("log folder not set");
            }

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var files = Directory.GetFiles(directoryPath);
            var file = files.LastOrDefault();

            for (int i=0; i < files.Count() - MaxFilesCont; i++)
            {
                File.Delete(files[i]);
            }

            if (file == null || (new FileInfo(file)).Length > MaxFileLength)
            {
                file =
                    HostingEnvironment.MapPath(LogDirectoryPath +
                                               string.Format(LogFileName, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
            }


            if (!File.Exists(file))
            {
                using (var stream = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                using (TextWriter tw = new StreamWriter(stream))
                using (var writer = new CsvWriter(tw))
                {
                    writer.Configuration.AutoMap<TextMessage>();
                    writer.Configuration.HasHeaderRecord = true;
                    writer.WriteHeader<TextMessage>();
                }
            }

            
            using (var stream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (TextWriter tw = new StreamWriter(stream))
            using (var writer = new CsvWriter(tw))
            {
                writer.Configuration.AutoMap<TextMessage>();
                writer.Configuration.HasHeaderRecord = true;
                
                writer.WriteRecord(message);
            }
        }


        public List<TextMessage> GetSms(Guid customerid, long phone)
        {
            List<TextMessage> list = new List<TextMessage>();

            string directoryPath = HostingEnvironment.MapPath(LogDirectoryPath);
            if (directoryPath == null)
            {
                throw new Exception("log folder not set");
            }

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            foreach (var file in Directory.GetFiles(directoryPath))
            {
                using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (TextReader tr = new StreamReader(stream, true))
                using (CsvReader reader = new CsvReader(tr))
                {
                    reader.Configuration.AutoMap<TextMessage>();
                    reader.Configuration.HasHeaderRecord = true;
                    list.AddRange(reader.GetRecords<TextMessage>());
                }
            }

            return list.Where(mail => (customerid != Guid.Empty && mail.CustomerId == customerid) || mail.Phone == phone).ToList();
        }
    }
}
