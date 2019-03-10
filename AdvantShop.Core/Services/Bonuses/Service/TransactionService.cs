using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.SQL;

namespace AdvantShop.Core.Services.Bonuses.Service
{
    public class TransactionService
    {
        public static int Create(Transaction tr)
        {
            return SQLDataAccess2.ExecuteScalar<int>("insert into [Bonus].[Transaction] (CardId,Amount,Basis,CreateOn, CreateOnCut,OperationType,Balance, PurchaseId,AdditionalBonusId )" +
                                                     " values (@CardId, @Amount, @Basis, @CreateOn, @CreateOnCut, @OperationType, @Balance, @PurchaseId, @AdditionalBonusId);" +
                                                     " SELECT CAST(SCOPE_IDENTITY() AS INT)", tr);
        }

        public static void RollBack(Transaction item)
        {
            if (item.OperationType == EOperationType.AddAdditionBonus)
            {
                AdditionBonusService.RollBack(item.AdditionalBonus, item.Amount);
                var transLog = Transaction.Factory(item.CardId, item.Amount, item.Basis, EOperationType.SubtractAdditionBonus, item.Balance - item.Amount, item.PurchaseId, item.AdditionalBonusId);
                Create(transLog);
            }
            if (item.OperationType == EOperationType.SubtractAdditionBonus)
            {
                AdditionBonusService.RollBack(item.AdditionalBonus, item.Amount);
                var transLog = Transaction.Factory(item.CardId, item.Amount, item.Basis, EOperationType.AddAdditionBonus, item.Balance + item.Amount, item.PurchaseId, item.AdditionalBonusId);
                Create(transLog);
            }

            if (item.OperationType == EOperationType.AddMainBonus)
            {
                var card = item.Card;
                card.BonusAmount -= item.Amount;
                CardService.Update(card);
                var transLog = Transaction.Factory(item.CardId, item.Amount, item.Basis, EOperationType.SubtractMainBonus, item.Balance - item.Amount, item.PurchaseId, item.AdditionalBonusId);
                Create(transLog);
            }

            if (item.OperationType == EOperationType.SubtractMainBonus)
            {
                var card = item.Card;
                card.BonusAmount += item.Amount;
                CardService.Update(card);
                var transLog = Transaction.Factory(item.CardId, item.Amount, item.Basis, EOperationType.AddMainBonus, item.Balance + item.Amount, item.PurchaseId, item.AdditionalBonusId);
                Create(transLog);
            }


            if (item.OperationType == EOperationType.AddAfiliate)
            {
                var card = item.Card;
                card.BonusAmount -= item.Amount;
                CardService.Update(card);
                var transLog = Transaction.Factory(item.CardId, item.Amount, item.Basis, EOperationType.SubtractAfiliate, item.Balance - item.Amount, item.PurchaseId, item.AdditionalBonusId);
                Create(transLog);
            }

            if (item.OperationType == EOperationType.SubtractAfiliate)
            {
                var card = item.Card;
                card.BonusAmount += item.Amount;
                CardService.Update(card);
                var transLog = Transaction.Factory(item.CardId, item.Amount, item.Basis, EOperationType.AddAfiliate, item.Balance + item.Amount, item.PurchaseId, item.AdditionalBonusId);
                Create(transLog);
            }

        }

        public static List<Transaction> GetByPurchase(int id)
        {
            return SQLDataAccess2.ExecuteReadIEnumerable<Transaction>("Select * from [Bonus].[Transaction] where PurchaseId=@p", new { p = id }).ToList();
        }

        public static List<Transaction> GetLast(Guid cardId, int top = 10)
        {
            return SQLDataAccess2.ExecuteReadIEnumerable<Transaction>("Select top(@top) * from [Bonus].[Transaction] where CardId=@p order by Id DESC", new { p = cardId, top = top }).ToList();
        }

        public static void DeleteByCard(Guid cardId)
        {
            SQLDataAccess2.ExecuteNonQuery("Delete from [Bonus].[Transaction] where CardId=@cardId", new { cardId = cardId });
        }
    }
}
