using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Service
{
    public static class LimitRequestService
    {
        private static int LimitRequest { get; set; }
        private static Stopwatch Timer { get; set; }

        public static void Init()
        {
            LimitRequest = 0;
        }

        public static void CheckLimit()
        {
            if (Timer == null || !Timer.IsRunning)
            {
                Timer = Stopwatch.StartNew();
            }
            LimitRequest++;
            if (LimitRequest > 250 && Timer.Elapsed.TotalMilliseconds < 10000)
            {
                double r = 10000 - Timer.Elapsed.TotalMilliseconds;
                Thread.Sleep(Convert.ToInt32(r));
                Timer.Restart();
                LimitRequest = 0;
            }
            if (LimitRequest > 250 || Timer.Elapsed.TotalMilliseconds > 10000)
            {
                //LogService.HistoryLog("Запросов: " + LimitRequest + " за: " + Timer.Elapsed);
                LimitRequest = 0;
                Timer.Restart();
            }
        }

        public static void Stop()
        {
            Timer.Stop();
        }
    }
}
