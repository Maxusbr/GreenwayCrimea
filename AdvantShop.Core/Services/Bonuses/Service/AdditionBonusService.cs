using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.SQL;

namespace AdvantShop.Core.Services.Bonuses.Service
{
    public class AdditionBonusService
    {
        public static AdditionBonus Get(int id)
        {
            return SQLDataAccess2.Query<AdditionBonus>("Select * from Bonus.AdditionBonus where Id=@id", new { id = id });
        }

        public static int Add(AdditionBonus model)
        {
            var temp = SQLDataAccess2.ExecuteScalar<int>("insert into Bonus.AdditionBonus (CardId, Name, Amount, StartDate, EndDate, Description, Status)" +
                                                         " values (@CardId, @Name, @Amount, @StartDate, @EndDate, @Description, @Status);" +
                                                         " select cast(scope_identity() as int)", model);
            return temp;
        }

        public static void Update(AdditionBonus model)
        {
            SQLDataAccess2.ExecuteNonQuery("Update Bonus.AdditionBonus set CardId=@CardId, " +
                                           "Name=@Name, " +
                                           "Amount=@Amount, " +
                                           "StartDate=@StartDate, " +
                                           "EndDate=@EndDate, " +
                                           "Description=@Description, " +
                                           "Status=@Status where Id=@Id", model);
        }

        public static decimal ActualSum(Guid cardId)
        {
            var temp = SQLDataAccess2.ExecuteScalar<decimal>("Select Sum(Amount) from Bonus.AdditionBonus where CardId=@cardId" +
                                                             " and (EndDate is null or EndDate>=@end)" +
                                                             " and (StartDate is null or StartDate<=@start)" +
                                                             " and Amount > 0" +
                                                             " and Status <> @status", new
                                                             {
                                                                 cardId = cardId,
                                                                 start = DateTime.Today,
                                                                 end = DateTime.Today,
                                                                 status = (int)EAdditionBonusStatus.Remove
                                                             });
            return temp;
        }

        public static List<AdditionBonus> Actual(Guid cardId)
        {
            var temp = SQLDataAccess2.ExecuteReadIEnumerable<AdditionBonus>("Select * from Bonus.AdditionBonus where CardId=@cardId" +
                                                             " and (EndDate is null or EndDate>=@end)" +
                                                             " and (StartDate is null or StartDate<=@start)" +
                                                             " and Amount > 0" +
                                                             " and Status <> @status", new
                                                             {
                                                                 cardId = cardId,
                                                                 start = DateTime.Today,
                                                                 end = DateTime.Today,
                                                                 status = (int)EAdditionBonusStatus.Remove
                                                             }).ToList();
            return temp;
        }

        public static AdditionBonus RollBack(AdditionBonus item, decimal amount)
        {
            if (item.Status == EAdditionBonusStatus.RecoveryAdd)
            {
                item.Status = EAdditionBonusStatus.Substract;
                item.Amount -= amount;
            }
            if (item.Status == EAdditionBonusStatus.Create)
            {
                item.Status = EAdditionBonusStatus.Remove;
            }
            if (item.Status == EAdditionBonusStatus.Substract)
            {
                item.Status = EAdditionBonusStatus.RecoveryAdd;
                item.Amount += amount;
            }
            if (item.Status == EAdditionBonusStatus.Remove)
            {
                item.Status = EAdditionBonusStatus.Create;
            }
            Update(item);
            return item;
        }

        public static void DeleteByCard(Guid cardId)
        {
             SQLDataAccess2.ExecuteNonQuery("Delete from Bonus.AdditionBonus where CardId=@cardId", new { cardId = cardId });
        }
    }
}
