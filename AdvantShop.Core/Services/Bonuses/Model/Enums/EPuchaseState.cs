using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Services.Bonuses.Model.Enums
{
    public enum EPuchaseState: short
    {
        [Localize("Core.Bonuses.EPuchaseState.None")]
        None = 0,
        [Localize("Core.Bonuses.EPuchaseState.Complete")]
        Complete = 1,
        [Localize("Core.Bonuses.EPuchaseState.Hold")]
        Hold = 2,
        [Localize("Core.Bonuses.EPuchaseState.Deleted")]
        Deleted = 3
    }
}
