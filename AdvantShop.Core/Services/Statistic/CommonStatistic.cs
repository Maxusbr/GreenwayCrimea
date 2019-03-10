using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.FilePath;
using AdvantShop.Helpers;

namespace AdvantShop.Statistic
{
    public class CommonStatistic
    {
        private static readonly object SyncObject = new object();
        private static readonly StatisticData Data = new StatisticData();

        public static readonly string VirtualFileLogPath = FoldersHelper.GetPath(FolderType.PriceTemp, "StatisticLog.txt", true);
        public static readonly string FileLog = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp, "StatisticLog.txt");


        static public void Init()
        {
            Data.ProcessedPercent = 0;
            Data.Processed = 0;
            Data.Total = 0;
            Data.IsRun = false;
            Data.Update = 0;
            Data.Add = 0;
            Data.Error = 0;
            Data.FileName = string.Empty;
            Data.FileSize = string.Empty;
            Data.CurrentProcess = string.Empty;
            if (!Directory.Exists(FoldersHelper.GetPathAbsolut(FolderType.PriceTemp)))
            {
                Directory.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.PriceTemp));
            }
            FileHelpers.DeleteFile(FileLog);
        }

        public static StatisticData CurrentData
        {
            get
            {
                Data.ProcessedPercent = Data.Total != 0 ? Convert.ToInt32((float)Data.Processed / ((float)Data.Total / 100)) : 0;
                return Data;
            }
        }

        //public static Thread StartNew(Action action, bool inBackGround = true)
        //{
        //    var temp = inBackGround ? new Thread(() => action()) { IsBackground = true } : Thread.CurrentThread;
        //    temp.SetCulture();
        //    IsRun = true;
        //    if (inBackGround)
        //        temp.Start();
        //    else
        //        action();
        //    return temp;
        //}

        public static Task StartNew(Action action)
        {
            return Task.Factory.StartNew(() =>
               {
                   Thread.CurrentThread.SetCulture();
                   action();
               }, TaskCreationOptions.LongRunning);
        }

        public static long TotalRow
        {
            get { lock (SyncObject) { return Data.Total; } }
            set { lock (SyncObject) { Data.Total = value; } }
        }

        public static long RowPosition
        {
            get { lock (SyncObject) { return Data.Processed; } }
            set { lock (SyncObject) { Data.Processed = value; } }
        }

        public static bool IsRun
        {
            get { lock (SyncObject) { return Data.IsRun; } }
            set { lock (SyncObject) { Data.IsRun = value; } }
        }

        public static long TotalUpdateRow
        {
            get { lock (SyncObject) { return Data.Update; } }
            set { lock (SyncObject) { Data.Update = value; } }
        }

        public static long TotalAddRow
        {
            get { lock (SyncObject) { return Data.Add; } }
            set { lock (SyncObject) { Data.Add = value; } }
        }

        public static long TotalErrorRow
        {
            get { lock (SyncObject) { return Data.Error; } }
            set { lock (SyncObject) { Data.Error = value; } }
        }

        public static string FileName
        {
            get { lock (SyncObject) { return Data.FileName; } }
            set { lock (SyncObject) { Data.FileName = value; } }
        }

        public static string FileSize
        {
            get { lock (SyncObject) { return Data.FileSize; } }
            set { lock (SyncObject) { Data.FileSize = value; } }
        }

        public static string CurrentProcess
        {
            get { lock (SyncObject) { return Data.CurrentProcess; } }
            set { lock (SyncObject) { Data.CurrentProcess = value; } }
        }

        public static string CurrentProcessName
        {
            get { lock (SyncObject) { return Data.CurrentProcessName; } }
            set { lock (SyncObject) { Data.CurrentProcessName = value; } }
        }

        public static void WriteLog(string message)
        {
            lock (SyncObject)
            {
                using (var fs = new FileStream(FileLog, FileMode.Append, FileAccess.Write))
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                    sw.WriteLine(message);
            }
        }

        public static string ReadLog()
        {
            var content = "";
            lock (SyncObject)
            {
                if (File.Exists(FileLog))
                {
                    using (var streamReader = new StreamReader(FileLog))
                        content = streamReader.ReadToEnd();
                }
            }
            return content;
        }
    }
}