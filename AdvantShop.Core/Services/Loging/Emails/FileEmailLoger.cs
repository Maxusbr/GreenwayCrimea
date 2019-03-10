using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using AdvantShop.Core.Common.Extensions;
using CsvHelper;

namespace AdvantShop.Core.Services.Loging.Emails
{
    public class FileEmailLoger : IEmailLoger
    {
        private const string LogDirectoryPath = "~/app_data/EmailLogs/";
        private const string LogFileName = "LogEmail_{0}.txt";
        private const int MaxFileLength = 1*1024*1024;
        private const int MaxFilesCont = 10;

        public void LogEmail(Email email)
        {
            new Task(() => LogEmailInternal(email)).Start();
        }


        public List<Email> GetEmails(Guid customerid, string email)
        {
            var list = new List<Email>();

            string directoryPath = HostingEnvironment.MapPath(LogDirectoryPath);
            if (directoryPath == null)
            {
                throw new Exception("log folder not set");
            }

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            foreach (string file in Directory.GetFiles(directoryPath))
            {
                using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (TextReader tr = new StreamReader(stream, true))
                using (var reader = new CsvReader(tr))
                {
                    reader.Configuration.AutoMap<Email>();
                    reader.Configuration.HasHeaderRecord = true;
                    list.AddRange(reader.GetRecords<Email>());
                }
            }

            return
                list.Where(
                    mail =>
                        (customerid != Guid.Empty && mail.CustomerId == customerid) ||
                        email.IsNotEmpty() && mail.EmailAddress == email).ToList();
        }

        private void LogEmailInternal(Email email)
        {
            string directoryPath = HostingEnvironment.MapPath(LogDirectoryPath);
            if (directoryPath == null)
            {
                throw new Exception("log folder not set");
            }

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string[] files = Directory.GetFiles(directoryPath);
            string file = files.LastOrDefault();

            for (int i = 0; i < files.Count() - MaxFilesCont; i++)
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
                    writer.Configuration.AutoMap<Email>();
                    writer.Configuration.HasHeaderRecord = true;
                    writer.WriteHeader<Email>();
                }
            }


            using (var stream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (TextWriter tw = new StreamWriter(stream))
            using (var writer = new CsvWriter(tw))
            {
                writer.Configuration.AutoMap<Email>();
                writer.Configuration.HasHeaderRecord = true;

                writer.WriteRecord(email);
            }
        }
    }
}