using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.SQL;

namespace AdvantShop.Core.Services.Bonuses.Service
{
    public class RuleLogService
    {
        public static void Add(RuleLog model)
        {
            SQLDataAccess2.ExecuteNonQuery("insert into Bonus.RuleLog (CardId, RuleType, Created)" +
                                           " values (@CardId, @RuleType, @Created);"
                , model);
        }
    }
}