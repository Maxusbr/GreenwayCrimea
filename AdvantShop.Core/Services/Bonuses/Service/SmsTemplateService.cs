using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Bonuses.Sms;
using AdvantShop.Core.SQL;

namespace AdvantShop.Core.Services.Bonuses.Service
{
    public class SmsTemplateService
    {
        public static SmsTemplate Get(ESmsType type)
        {
            return SQLDataAccess2.Query<SmsTemplate>("Select * from Bonus.SmsTemplate where SmsTypeId=@SmsTypeId", new { SmsTypeId = type });
        }

        public static ESmsType Add(SmsTemplate model)
        {
            var temp = SQLDataAccess2.ExecuteScalar<ESmsType>("insert into Bonus.SmsTemplate (SmsTypeId,SmsBody) values (@SmsTypeId,@SmsBody); select @SmsTypeId", model);
            return temp;
        }

        public static void Update(SmsTemplate model)
        {
            SQLDataAccess2.ExecuteNonQuery("Update Bonus.SmsTemplate set SmsBody=@SmsBody where SmsTypeId=@SmsTypeId", model);
        }

        public static void AddSmsLog(SmsLog model)
        {
            SQLDataAccess2.ExecuteNonQuery("Insert into Bonus.SmsLog (Body,State,Phone,Created) values (@Body,@State,@Phone,@Created)", model);
        }

        public static List<SmsTemplate> GetAll()
        {
            return SQLDataAccess2.ExecuteReadIEnumerable<SmsTemplate>("Select * from Bonus.SmsTemplate").ToList();
        }

        public static void Delete(ESmsType type)
        {
            SQLDataAccess2.ExecuteNonQuery("Delete from Bonus.SmsTemplate where SmsTypeId=@SmsTypeId", new { SmsTypeId = type });
        }
    }
}
