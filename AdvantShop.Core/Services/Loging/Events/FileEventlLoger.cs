using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using AdvantShop.Customers;
using CsvHelper;
using System.Web;

namespace AdvantShop.Core.Services.Loging.Events
{
    public class FileEventLoger : IEventLoger
    {
        private const string LogDirectoryPath = "~/app_data/EventLogs/";
        private const string LogFileName = "LogEvent_{0}.txt";
        private const int MaxFileLength = 1*1024*1024;
        private const int MaxFilesCont = 10;

        public void LogEvent(Event @event)
        {
            @event.CreateOn = DateTime.Now;
            @event.CustomerId = CustomerContext.CustomerId;
            @event.IP = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : null;

            new Task(() => LogEventInternal(@event)).Start();
        }


        public List<Event> GetEvents(Guid customerid)
        {
            var list = new List<Event>();

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
                    reader.Configuration.AutoMap<Event>();
                    reader.Configuration.HasHeaderRecord = true;
                    list.AddRange(reader.GetRecords<Event>());
                }
            }

            return list.Where(mail => (customerid != Guid.Empty && mail.CustomerId == customerid)).ToList();
        }

        private void LogEventInternal(Event email)
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
                    writer.Configuration.AutoMap<Event>();
                    writer.Configuration.HasHeaderRecord = true;
                    writer.WriteHeader<Event>();
                }
            }


            using (var stream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (TextWriter tw = new StreamWriter(stream))
            using (var writer = new CsvWriter(tw))
            {
                writer.Configuration.AutoMap<Event>();
                writer.Configuration.HasHeaderRecord = true;

                writer.WriteRecord(email);
            }
        }
    }
}