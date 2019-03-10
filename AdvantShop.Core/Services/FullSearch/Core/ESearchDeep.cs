using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Services.FullSearch.Core
{
    public enum ESearchDeep
    {
        [Localize("AdvantShop.Core.Services.FullSearch.Core.ESearchDeep.StrongPhase")]
        StrongPhase,
        [Localize("AdvantShop.Core.Services.FullSearch.Core.ESearchDeep.SepareteWords")]
        SepareteWords,
        [Localize("AdvantShop.Core.Services.FullSearch.Core.ESearchDeep.WordsStartFrom")]
        WordsStartFrom,
        [Localize("AdvantShop.Core.Services.FullSearch.Core.ESearchDeep.WordsBetween")]
        WordsBetween
    }
}
