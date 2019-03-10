using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Services.Bonuses.Sms.Template;

namespace AdvantShop.Core.Services.Bonuses.Sms
{
    public enum ESmsType : byte
    {
        //[Display(ResourceType = typeof(Resources.Translations), Name = "Action")]
        //[Display(Name = "Не указано")]
        [LinkedClass(typeof(BaseSmsTemplate))]
        None = 0,
        
        //[Localize("Core.Bonuses.ESmsType.OnClientRegist")]
        //[LinkedClass(typeof(OnClientRegistTempalte))]
        //OnClientRegist = 1,
        
        [Localize("Core.Bonuses.ESmsType.OnPurchase")]
        [LinkedClass(typeof(OnPurchaseTempalte))]
        OnPurchase = 2,
        
        [Localize("Core.Bonuses.ESmsType.OnAddBonus")]
        [LinkedClass(typeof(OnAddBonusTempalte))]
        OnAddBonus = 3,
        
        [Localize("Core.Bonuses.ESmsType.OnSubtractBonus")]
        [LinkedClass(typeof(OnSubtractBonusTempalte))]
        OnSubtractBonus = 4,
        
        //[Localize("Core.Bonuses.ESmsType.OnCheckBalance")]
        //[LinkedClass(typeof(OnCheckBalanceTempalte))]
        //OnCheckBalance = 5,
        
        [Localize("Core.Bonuses.ESmsType.OnUpgradePercent")]
        [LinkedClass(typeof(OnUpgradePercentTempalte))]
        ChangeGrade = 6,
        
        //[Localize("Core.Bonuses.ESmsType.OnSmsCode")]
        //[LinkedClass(typeof(OnSmsCodeTempalte))]
        //OnSmsCode = 7,

        [Localize("Core.Bonuses.ESmsType.CancelPurchase")]
        [LinkedClass(typeof(CancelPurchaseTempalte))]
        CancelPurchase = 8,
        
        //[Localize("Core.Bonuses.ESmsType.OnBirthdayRule")]
        //[LinkedClass(typeof(OnBirthdayRuleTempalte))]
        //OnBirthdayRule = 9,
        
        [Localize("Core.Bonuses.ESmsType.OnCancellationsBonus")]
        [LinkedClass(typeof(OnCancellationsBonusTempalte))]
        OnCancellationsBonus = 10,

        [Localize("Core.Bonuses.ESmsType.OnCleanExpiredBonus")]
        [LinkedClass(typeof(OnCleanExpiredBonusTempalte))]
        OnCleanExpiredBonus = 11
        
    }
}
