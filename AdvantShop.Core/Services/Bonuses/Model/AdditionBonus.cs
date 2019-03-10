using System;
using AdvantShop.Core.Services.Bonuses.Model.Enums;

namespace AdvantShop.Core.Services.Bonuses.Model
{
    public class AdditionBonus
    {
        public int Id { get; set; }
        public Guid CardId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public EAdditionBonusStatus Status { get; set; }
    }
}
