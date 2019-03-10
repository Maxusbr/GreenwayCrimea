using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Service
{
    public class LogService
    {
        public static void ErrLog(string message)
        {
            try
            {
                var path = System.Web.Hosting.HostingEnvironment.MapPath("~/modules/SimaLand/") + "logs/";
                var errFile = "err.txt";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path += errFile;

                FileInfo file = new FileInfo(path);

                if (file.Exists)
                {
                    var now = DateTime.Now.AddDays(-7);
                    var created = file.CreationTime;
                    if (now > created)
                    {
                        file.CreationTime = DateTime.Now;
                        file.Delete();
                    }
                }

                message = DateTime.Now.ToString() + " -----------" + Environment.NewLine + message + Environment.NewLine + "---" + Environment.NewLine;

                File.AppendAllText(path, message);
            }
            catch (Exception ex)
            {
                Diagnostics.Debug.Log.Error(new Exception("Error in module sima-land ErrLog writer." + Environment.NewLine + " Message: " + ex.Message));
            }
        }

        public static void HistoryLog(string message)
        {
            try
            {
                var path = System.Web.Hosting.HostingEnvironment.MapPath("~/modules/SimaLand/") + "logs/";
                var hisFile = "log.txt";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path += hisFile;

                FileInfo file = new FileInfo(path);

                if (file.Exists)
                {
                    var now = DateTime.Now.AddDays(-7);
                    var created = file.CreationTime;
                    if (now > created)
                    {
                        file.CreationTime = DateTime.Now;
                        file.Delete();
                    }
                }

                message = DateTime.Now.ToString() + " -----------" + Environment.NewLine + message + Environment.NewLine + "---" + Environment.NewLine;

                File.AppendAllText(path, message);
            }
            catch (Exception ex)
            {
                Diagnostics.Debug.Log.Error(new Exception("Error in module sima-land HistoryLog writer." + Environment.NewLine + " Message: " + ex.Message));
            }
        }

    }
}
