using System;

namespace AdvantShop.Module.VkMarket.Domain
{
    public static class ImportProgress
    {
        private static int _total;
        private static int _current;

        public static void Start(int total)
        {
            _total = total;
            _current = 0;
        }

        public static void Inc()
        {
            _current += 1;
        }

        public static Tuple<int, int> State()
        {
            return new Tuple<int, int>(_total, _current);
        }
    }

    public static class ExportProgress
    {
        private static int _total;
        private static int _current;

        public static void Start(int total)
        {
            _total = total;
            _current = 0;
        }

        public static void Inc()
        {
            _current += 1;
        }

        public static Tuple<int, int> State()
        {
            return new Tuple<int, int>(_total, _current);
        }
    }
}
