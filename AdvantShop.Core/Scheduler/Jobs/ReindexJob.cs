using System.Data;
using AdvantShop.Core.SQL;
using Quartz;

namespace AdvantShop.Core.Scheduler.Jobs
{
    [DisallowConcurrentExecution]
    public class ReindexJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            System.Threading.Thread.Sleep(new System.Random().Next(30 * 60 * 1000));
            SQLDataAccess.ExecuteNonQuery("[Settings].[sp_Reindex]", CommandType.StoredProcedure);
        }
    }
}