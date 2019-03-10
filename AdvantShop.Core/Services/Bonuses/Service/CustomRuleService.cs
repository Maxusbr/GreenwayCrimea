using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.Services.Bonuses.Model.Rules;
using AdvantShop.Core.SQL;

namespace AdvantShop.Core.Services.Bonuses.Service
{
    public class CustomRuleService
    {
        public static CustomRule Get(ERule id)
        {
            return SQLDataAccess2.Query<CustomRule>("Select * from Bonus.CustomRule where RuleType=@id", new { id = id });
        }

        public static int Add(CustomRule model)
        {
            var temp = SQLDataAccess2.ExecuteScalar<int>("insert into Bonus.CustomRule (RuleType,Enabled,Name,Params) values (@RuleType,@Enabled,@Name,@Params); select @RuleType", model);
            return temp;
        }

        public static void Update(CustomRule model)
        {
            SQLDataAccess2.ExecuteNonQuery("Update Bonus.CustomRule set Enabled=@Enabled,Name=@Name,Params=@Params where RuleType=@RuleType", model);
        }

        public void AddRuleLog(RuleLog model)
        {
            SQLDataAccess2.ExecuteNonQuery("Insert into Bonus.RuleLog (CardId,RuleType,Created) values (@CardId,@RuleType,@Created)", model);
        }

        public static List<CustomRule> GetAll()
        {
            return SQLDataAccess2.ExecuteReadIEnumerable<CustomRule>("Select * from Bonus.CustomRule").ToList();
        }

        public static void Delete(ERule id)
        {
            SQLDataAccess2.ExecuteNonQuery("Delete from Bonus.CustomRule where RuleType=@id", new {id=id});
        }
    }
}
