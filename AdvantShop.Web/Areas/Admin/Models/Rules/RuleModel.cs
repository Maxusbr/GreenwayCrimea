using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.Services.Bonuses.Model.Rules;

namespace AdvantShop.Web.Admin.Models.Rules
{
    public abstract class RuleModel : IValidatableObject
    {
        public ERule RuleType { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        protected BaseRule CurentRule { get; set; }

        public virtual string ModelType
        {
            get { return this.GetType().AssemblyQualifiedName; }
        }

        public virtual string ShippingViewPath
        {
            get { return "_" + RuleType; }
        }

        public static RuleModel Get(CustomRule rule)
        {
            switch (rule.RuleType)
            {
                case ERule.BirthDay:
                    return new BirthDayRuleModel(rule);
                case ERule.NewCard:
                    return new NewCardRuleModel(rule);
                case ERule.CancellationsBonus:
                    return new CancellationsBonusRuleModel(rule);
                case ERule.ChangeGrade:
                    return new ChangeGradeRuleModel(rule);
                case ERule.CleanExpiredBonus:
                    return new CleanExpiredBonusRuleModel(rule);
                default:
                    throw new BlException("rule type unknown");
            }
        }

        protected RuleModel() { }

        protected RuleModel(CustomRule rule)
        {
            RuleType = rule.RuleType;
            Name = string.IsNullOrWhiteSpace( rule.Name) ? rule.RuleType.Localize() : rule.Name;
            Enabled = rule.Enabled;
            CurentRule = BaseRule.Get(rule);
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        public virtual BaseRule ToBaseRule()
        {
            throw new NotImplementedException();
        }
    }

    public class BirthDayRuleModel : RuleModel
    {
        public decimal GiftBonus { get; set; }
        public int DaysBefore { get; set; }
        public int BonusAvailableDays { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (GiftBonus < 0)
            {
                yield return new ValidationResult("GiftBonus must be positive");
            }
        }

        public override BaseRule ToBaseRule()
        {
            return new BirthDayRule
            {
                GiftBonus = this.GiftBonus,
                DaysBefore =this.DaysBefore,
                BonusAvailableDays = this.BonusAvailableDays
            };
        }

        public BirthDayRuleModel() { }

        public BirthDayRuleModel(CustomRule rule) : base(rule)
        {
            var curent = CurentRule as BirthDayRule;
            if (curent == null) return;
            GiftBonus = curent.GiftBonus;
            DaysBefore = curent.DaysBefore;
            BonusAvailableDays = curent.BonusAvailableDays;
        }
    }

    public class NewCardRuleModel : RuleModel
    {
        public decimal GiftBonus { get; set; }
        public int? BonusAvailableDays { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (GiftBonus < 0)
            {
                yield return new ValidationResult("GiftBonus must be positive");
            }
        }

        public NewCardRuleModel() { }

        public NewCardRuleModel(CustomRule rule) : base(rule)
        {
            var curent = CurentRule as NewCardRule;
            if (curent == null) return;
            GiftBonus = curent.GiftBonus;
            BonusAvailableDays = curent.BonusAvailableDays;
        }

        public override BaseRule ToBaseRule()
        {
            return new NewCardRule
            {
                GiftBonus = this.GiftBonus,
                BonusAvailableDays = this.BonusAvailableDays
            };
        }
    }

    public class CancellationsBonusRuleModel : RuleModel
    {
        public int SmsDayBefore { get; set; }
        public int AgeCard { get; set; }
        public bool NotSendSms { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }

        public CancellationsBonusRuleModel() { }

        public CancellationsBonusRuleModel(CustomRule rule) : base(rule)
        {
            var curent = CurentRule as CancellationsBonusRule;
            if (curent == null) return;
            SmsDayBefore = curent.SmsDayBefore;
            AgeCard = curent.AgeCard;
            NotSendSms = curent.NotSendSms;
        }

        public override BaseRule ToBaseRule()
        {
            return new CancellationsBonusRule
            {
                SmsDayBefore = this.SmsDayBefore,
                AgeCard = this.AgeCard,
                NotSendSms=this.NotSendSms
            };
        }
    }

    public class ChangeGradeRuleModel : RuleModel
    {
        public bool AllPeriod { get; set; }
        public int Period { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }

        public ChangeGradeRuleModel() { }

        public ChangeGradeRuleModel(CustomRule rule) : base(rule)
        {
            var curent = CurentRule as ChangeGradeRule;
            if (curent == null) return;
            AllPeriod = curent.AllPeriod;
            Period = curent.Period;
        }

        public override BaseRule ToBaseRule()
        {
            return new ChangeGradeRule
            {
                AllPeriod = this.AllPeriod,
                Period = this.Period
            };
        }
    }

    public class CleanExpiredBonusRuleModel : RuleModel
    {
        public bool NeedSms { get; set; }
        public int DayBefore { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }

        public CleanExpiredBonusRuleModel() { }

        public CleanExpiredBonusRuleModel(CustomRule rule) : base(rule)
        {
            var curent = CurentRule as CleanExpiredBonusRule;
            if (curent == null) return;
            DayBefore = curent.DayBefore;
            NeedSms = curent.NeedSms;
        }

        public override BaseRule ToBaseRule()
        {
            return new CleanExpiredBonusRule
            {
                DayBefore = this.DayBefore,
                NeedSms = this.NeedSms
            };
        }
    }
}
