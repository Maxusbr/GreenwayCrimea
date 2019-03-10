using System;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Cards;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.Cards
{

    public class GetTransactionHandler : AbstractHandler<TransactionFilterModel, int, Transaction>
    {
        public GetTransactionHandler(TransactionFilterModel filterModel) : base(filterModel)
        {
        }

        protected override SqlPaging Select(SqlPaging paging)
        {
            paging.Select("Id", "Balance", "CreateOn", "Amount", "Basis",
                "OperationType", "PurchaseId");
            paging.From("[Bonus].[Transaction]");
            return paging;
        }

        protected override SqlPaging Filter(SqlPaging paging)
        {
            if (FilterModel.CardId != Guid.Empty)
            {
                paging.Where("CardId = {0}", FilterModel.CardId);
            }
            return paging;
        }

        protected override SqlPaging Sorting(SqlPaging paging)
        {
            paging.OrderByDesc("CreateOn");
            return paging;
        }
    }
}