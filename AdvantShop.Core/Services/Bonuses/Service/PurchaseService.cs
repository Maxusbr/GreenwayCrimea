using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.SQL;

namespace AdvantShop.Core.Services.Bonuses.Service
{
    public class PurchaseService
    {
        public static Purchase Get(int id)
        {
            return SQLDataAccess2.Query<Purchase>("Select * from Bonus.Purchase where Id=@id", new { id = id });
        }

        public static Purchase HasImport(Guid cardid, string t)
        {
            return SQLDataAccess2.Query<Purchase>("Select * from Bonus.Purchase where CardId=@cardid and  comment=@t", new { cardid = cardid, t = t });
        }

        public static Purchase GetByOrderId(int id)
        {
            return SQLDataAccess2.Query<Purchase>("Select * from Bonus.Purchase where OrderId=@id and Status<>@status", new { id = id, status = EPuchaseState.Deleted });
        }


        public static int Add(Purchase model)
        {
            var temp = SQLDataAccess2.ExecuteScalar<int>(@"insert into Bonus.Purchase (CardId,
                                                                                       CreateOn,
                                                                                       CreateOnCut,
                                                                                       PurchaseAmount,
                                                                                       CashAmount,
                                                                                       MainBonusAmount,
                                                                                       AdditionBonusAmount,
                                                                                       NewBonusAmount,
                                                                                       Comment,
                                                                                       MainBonusBalance,
                                                                                       AdditionBonusBalance,
                                                                                       Status,
                                                                                       PurchaseFullAmount,
                                                                                       OrderId) 
                                                                                       values
                                                                                       (@CardId,
                                                                                       @CreateOn,
                                                                                       @CreateOnCut,
                                                                                       @PurchaseAmount,
                                                                                       @CashAmount,
                                                                                       @MainBonusAmount,
                                                                                       @AdditionBonusAmount,
                                                                                       @NewBonusAmount,
                                                                                       @Comment,
                                                                                       @MainBonusBalance,
                                                                                       @AdditionBonusBalance,
                                                                                       @Status,
                                                                                       @PurchaseFullAmount,
                                                                                       @OrderId);
                                                                                       select cast(scope_identity() as int)", model);
            return temp;
        }

        public static void Update(Purchase model)
        {
            SQLDataAccess2.ExecuteNonQuery(@"Update Bonus.Purchase set CardId=@CardId,
                                                                         CreateOn=@CreateOn,
                                                                         CreateOnCut=@CreateOnCut,
                                                                         PurchaseAmount=@PurchaseAmount,
                                                                         CashAmount=@CashAmount,
                                                                         MainBonusAmount=@MainBonusAmount,
                                                                         AdditionBonusAmount=@AdditionBonusAmount,
                                                                         NewBonusAmount=@NewBonusAmount,
                                                                         Comment=@Comment,
                                                                         MainBonusBalance=@MainBonusBalance,
                                                                         AdditionBonusBalance=@AdditionBonusBalance,
                                                                         Status=@Status,
                                                                         PurchaseFullAmount=@PurchaseFullAmount,
                                                                         OrderId=@OrderId
                                                                         where Id=@Id", model);
        }

        public static void RollBack(Purchase purchase)
        {
            using (var scope = new TransactionScope())
            {
                if (purchase.Status != EPuchaseState.Hold) return;
                purchase.Status = EPuchaseState.Deleted;
                PurchaseService.Update(purchase);
                var transLogs = purchase.Transaction.ToList();
                foreach (var item in transLogs)
                {
                    TransactionService.RollBack(item);
                }
                scope.Complete();
            }
        }

        public static List<Purchase> GetLast(Guid cardId, int top = 10)
        {
            return SQLDataAccess2.ExecuteReadIEnumerable<Purchase>("Select top(@top) * from Bonus.Purchase where CardId=@id Order By CreateOn desc", new { id = cardId, top = top }).ToList();
        }

        public static void DeleteByCard(Guid cardId)
        {
            SQLDataAccess2.ExecuteNonQuery("Delete from Bonus.Purchase where CardId=@cardId", new { cardId = cardId });
        }
    }
}
