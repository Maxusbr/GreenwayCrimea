using System;
using AdvantShop.Core.Services.Bonuses.Model.Enums;

namespace AdvantShop.Core.Services.Bonuses.Model
{
   public class RuleLog
    {
        public Guid CardId { get; set; }
        public ERule RuleType { get; set; }
        public DateTime Created { get; set; }
    }
}
