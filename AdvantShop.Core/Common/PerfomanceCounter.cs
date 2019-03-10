//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Core
{
    /// <summary>
    /// Class for calculaite process time 
    /// </summary>
    public class PerfomanceCounter
    {
        [System.Runtime.InteropServices.DllImport("KERNEL32")]
        private static extern bool QueryPerformanceCounter(ref long lpPerformanceCount);

        [System.Runtime.InteropServices.DllImport("KERNEL32")]
        private static extern bool QueryPerformanceFrequency(ref long lpFrequency);

        private long _totalCount = 0;
        private long _startCount = 0;
        private long _stopCount = 0;
        private long _freq = 0;

        public void Start()
        {
            _startCount = 0;
            QueryPerformanceCounter(ref _startCount);
        }

        public void Stop()
        {
            _stopCount = 0;
            QueryPerformanceCounter(ref _stopCount);
            _totalCount += _stopCount - _startCount;
        }

        public void Reset()
        {
            _totalCount = 0;
        }

        public float TotalSeconds
        {
            get
            {
                _freq = 0;
                QueryPerformanceFrequency(ref _freq);
                return ((float) _totalCount/(float) _freq);
            }
        }

        public double MFlops(double totalFlops)
        {
            return (totalFlops/(1e6*TotalSeconds));
        }

        public override string ToString()
        {
            return String.Format("{0:F3} seconds", TotalSeconds);
        }
    }
}