using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using AdvantShop.Core.Common.Extensions;
using CsvHelper;

namespace AdvantShop.Core.Services.Loging.Calls
{
    public class FileCallLoger : ICallLoger
    {
        private const string LogDirectoryPath = "~/app_data/CallLogs/";
        private const string LogFileName = "LogCall_{0}.txt";
        private const int MaxFileLength = 1 * 1024 * 1024;
        private const int MaxFilesCount = 10;

        public void LogCall(Call call)
        {
            new Task(() => LogCallInternal(call)).Start();
        }

        public List<Call> GetCalls(Guid customerid, string phone)
        {
            var list = new List<Call>();

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
                    reader.Configuration.AutoMap<Call>();
                    reader.Configuration.HasHeaderRecord = true;
                    list.AddRange(reader.GetRecords<Call>());
                }
            }

            return
                list.Where(
                    call =>
                        (customerid != Guid.Empty && call.CustomerId == customerid) ||
                        phone.IsNotEmpty() && call.Phone == phone).ToList();
        }

        private void LogCallInternal(Call call)
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

            for (int i = 0; i < files.Count() - MaxFilesCount; i++)
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
                    writer.Configuration.AutoMap<Call>();
                    writer.Configuration.HasHeaderRecord = true;
                    writer.WriteHeader<Call>();
                }
            }


            using (var stream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (TextWriter tw = new StreamWriter(stream))
            using (var writer = new CsvWriter(tw))
            {
                writer.Configuration.AutoMap<Call>();
                writer.Configuration.HasHeaderRecord = true;

                writer.WriteRecord(call);
            }
        }
    }
}