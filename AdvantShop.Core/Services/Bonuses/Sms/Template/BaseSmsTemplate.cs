using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Services.Bonuses.Sms.Template
{
    public class BaseSmsTemplate
    {
        public const string ModelTag = "#";
        public virtual object Prepare()
        {
            return new { };
        }

        public static BaseSmsTemplate Factory(ESmsType type)
        {
            var typeInst = AvalibleType(type);
            return (BaseSmsTemplate)Activator.CreateInstance(typeInst);
        }


        public static Type AvalibleType(Enum enumValue)
        {
            var attrValue = AttributeHelper.GetAttributeValueField<LinkedClass, Type>(enumValue);
            return attrValue;
        }

        public static List<string> AvalibleVarible(Enum enumValue)
        {
            var attrValue = AttributeHelper.GetAttributeValueField<LinkedClass, Type>(enumValue);
            if (attrValue == null) return new List<string>();
            var t = attrValue.GetProperties().Select(x => ModelTag + x.Name + ModelTag).ToList();
            return t;
        }
    }

    public class OnClientRegistTempalte : BaseSmsTemplate
    {
        public long CardNumber { get; set; }
        public string CompanyName { get; set; }

        public override object Prepare()
        {
            return new
            {
                CardNumber = CardNumber.ToString(CultureInfo.InvariantCulture),
                CompanyName
            };
        }
    }

    public class OnPurchaseTempalte : BaseSmsTemplate
    {
        public string CompanyName { get; set; }
        public decimal PurchaseFull { get; set; }
        public decimal Purchase { get; set; }
        public decimal UsedBonus { get; set; }
        public decimal AddBonus { get; set; }
        public decimal Balance { get; set; }

        public override object Prepare()
        {
            return new
            {
                CompanyName,
                PurchaseFull = PurchaseFull.ToString("F2"),
                Purchase = Purchase.ToString("F2"),
                UsedBonus = UsedBonus.ToString("F2"),
                AddBonus = AddBonus.ToString("F2"),
                Balance = Balance.ToString("F2"),
            };
        }
    }

    public class OnAddBonusTempalte : BaseSmsTemplate
    {
        public string CompanyName { get; set; }
        public decimal Bonus { get; set; }
        public decimal Balance { get; set; }
        public string Basis { get; set; }

        public override object Prepare()
        {
            return new
            {
                CompanyName,
                Bonus = Bonus.ToString("F2"),
                Balance = Balance.ToString("F2"),
                Basis
            };
        }
    }

    public class OnSubtractBonusTempalte : BaseSmsTemplate
    {
        public string CompanyName { get; set; }
        public decimal Bonus { get; set; }
        public decimal Balance { get; set; }
        public string Basis { get; set; }

        public override object Prepare()
        {
            return new
            {
                CompanyName,
                Bonus = Bonus.ToString("F2"),
                Balance = Balance.ToString("F2"),
                Basis
            };
        }
    }

    public class OnCheckBalanceTempalte : BaseSmsTemplate
    {
        public string CompanyName { get; set; }
        public decimal Balance { get; set; }

        public override object Prepare()
        {
            return new
            {
                CompanyName,
                Balance = Balance.ToString("F2")
            };
        }
    }

    public class OnUpgradePercentTempalte : BaseSmsTemplate
    {
        public long CardNumber { get; set; }
        public string CompanyName { get; set; }
        public string GradeName { get; set; }
        public decimal GradePercent { get; set; }
        public decimal Balance { get; set; }

        public override object Prepare()
        {
            return new
            {
                CardNumber = CardNumber.ToString(CultureInfo.InvariantCulture),
                CompanyName,
                GradeName,
                GradePercent = GradePercent.ToString("F2"),
                Balance = Balance.ToString("F2"),
            };
        }
    }

    public class OnSmsCodeTempalte : BaseSmsTemplate
    {
        public int Code { get; set; }

        public override object Prepare()
        {
            return new { Code = Code.ToString(CultureInfo.InvariantCulture) };
        }
    }

    public class CancelPurchaseTempalte : BaseSmsTemplate
    {
        public string CompanyName { get; set; }
        public decimal Purchase { get; set; }
        public decimal UsedBonus { get; set; }
        public decimal AddBonus { get; set; }
        public decimal Balance { get; set; }

        public override object Prepare()
        {
            return new
            {
                CompanyName,
                Balance = Balance.ToString("F2"),
                Purchase = Purchase.ToString("F2"),
                UsedBonus = UsedBonus.ToString("F2"),
                AddBonus = AddBonus.ToString("F2"),
            };
        }
    }

    public class OnBirthdayRuleTempalte : BaseSmsTemplate
    {
        public string CompanyName { get; set; }
        public decimal AddBonus { get; set; }
        public DateTime ToDate { get; set; }
        public decimal Balance { get; set; }

        public override object Prepare()
        {
            return new
            {
                CompanyName,
                Balance = Balance.ToString("F2"),
                AddBonus = AddBonus.ToString("F2"),
                ToDate = ToDate.ToString("dd.MM.yyyy")
            };
        }
    }

    public class OnCancellationsBonusTempalte : BaseSmsTemplate
    {
        public string CompanyName { get; set; }
        public decimal Balance { get; set; }
        public int DayLeft { get; set; }

        public override object Prepare()
        {
            return new
            {
                CompanyName,
                Balance = Balance.ToString("F2"),
                DayLeft = DayLeft.ToString(CultureInfo.InvariantCulture)
            };
        }
    }

    public class ChangeGradePassivityCardTempalte : BaseSmsTemplate
    {
        public string CompanyName { get; set; }
        public int PassiveMouth { get; set; }
        public DateTime OnDate { get; set; }
        public string GradeName { get; set; }
        public decimal GradePercent { get; set; }
        public override object Prepare()
        {
            return new
            {
                CompanyName,
                PassiveMouth = PassiveMouth.ToString(CultureInfo.InvariantCulture),
                OnDate = OnDate.ToString("dd.MM.yyyy"),
                GradeName,
                GradePercent = GradePercent.ToString("F2")
            };
        }
    }

    public class OnAddAfilateTempalte : BaseSmsTemplate
    {
        public string CompanyName { get; set; }
        public decimal Purchase { get; set; }
        public decimal UsedBonus { get; set; }
        public decimal AddBonus { get; set; }
        public decimal Balance { get; set; }

        public override object Prepare()
        {
            return new
            {
                CompanyName,
                Purchase = Purchase.ToString("F2"),
                UsedBonus = UsedBonus.ToString("F2"),
                AddBonus = AddBonus.ToString("F2"),
                Balance = Balance.ToString("F2"),
            };
        }
    }

    public class OnSubtractAfilateTempalte : BaseSmsTemplate
    {
        public string CompanyName { get; set; }
        public decimal Purchase { get; set; }
        public decimal UsedBonus { get; set; }
        public decimal AddBonus { get; set; }
        public decimal Balance { get; set; }

        public override object Prepare()
        {
            return new
            {
                CompanyName,
                Purchase = Purchase.ToString("F2"),
                UsedBonus = UsedBonus.ToString("F2"),
                AddBonus = AddBonus.ToString("F2"),
                Balance = Balance.ToString("F2"),
            };
        }
    }

    public class OnCleanExpiredBonusTempalte : BaseSmsTemplate
    {
        public string CompanyName { get; set; }
        public decimal Balance { get; set; }
        public int DayLeft { get; set; }

        public override object Prepare()
        {
            return new
            {
                CompanyName,
                Balance = Balance.ToString("F2"),
                DayLeft = DayLeft.ToString(CultureInfo.InvariantCulture)
            };
        }
    }
}
