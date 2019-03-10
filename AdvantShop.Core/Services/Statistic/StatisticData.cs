namespace AdvantShop.Statistic
{
    public class StatisticData
    {
        public long Processed;
        public long Total;
        public long Update;
        public long Add;
        public long Error;
        public bool IsRun;

        public int ProcessedPercent;

        public string CurrentProcess;
        public string CurrentProcessName;

        public string FileName;
        public string FileSize;
    }
}