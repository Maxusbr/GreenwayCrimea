using System;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Cards
{
    public class TransactionFilterModel : BaseFilterModel
    {
        public Guid CardId { get; set; }
    }
}