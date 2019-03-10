using System;

namespace AdvantShop.Web.Admin.Models.Cards
{
    public class AddMainBonusModel
    {
        public Guid CardId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }
}
