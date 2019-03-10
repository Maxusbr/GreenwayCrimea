//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Threading;

namespace AdvantShop.Tools
{
    public sealed class ProgressStatistic
    {
        private static readonly object SyncObject = new object();

        static int _currentStep;
        static long _currentNamber;
        static long _totalCount;
        static bool _isRun;

        static public void Init()
        {
            _currentNamber = 0;
            _totalCount = 0;
            _currentStep = 0;
            _isRun = false;
        }

        public static int Step
        {
            get
            {
                lock (SyncObject)
                {
                    return _currentStep;
                }
            }
            set
            {
                lock (SyncObject)
                {
                    _currentStep = value;
                }
            }
        }

        public static long Count
        {
            get
            {
                lock (SyncObject)
                {
                    return _totalCount;
                }
            }
            set
            {
                lock (SyncObject)
                {
                    _totalCount = value;
                }
            }
        }

        public static long Index
        {
            get
            {
                lock (SyncObject)
                {
                    return _currentNamber;
                }
            }
            set
            {
                lock (SyncObject)
                {
                    _currentNamber = value;
                }
            }
        }

        public static bool IsRun
        {
            get
            {
                return _isRun;
            }
            set
            {
                _isRun = value;
            }
        }

        public static Thread ThreadImport { get; set; }
    }
}