using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Services.Bonuses.Model.Enums
{
    public enum ERule: short
    {
        //None = 0,
        
        [Localize("Core.Bonuses.ERule.BirthDay")]
        BirthDay = 1,
        
        [Localize("Core.Bonuses.ERule.CancellationsBonus")]
        CancellationsBonus = 2,

        [Localize("Core.Bonuses.ERule.NewCard")]
        NewCard = 3,
        
        [Localize("Core.Bonuses.ERule.ChangeGrade")]
        ChangeGrade = 4,

        [Localize("Core.Bonuses.ERule.CleanExpiredBonus")]
        CleanExpiredBonus = 5

    }
}