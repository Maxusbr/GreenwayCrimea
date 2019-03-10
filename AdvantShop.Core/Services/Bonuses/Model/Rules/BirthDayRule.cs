using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Core.Services.Bonuses.Sms;
using AdvantShop.Core.Services.Bonuses.Sms.Template;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using Quartz;

namespace AdvantShop.Core.Services.Bonuses.Model.Rules
{
    [DisallowConcurrentExecution]
    public class BirthDayRule : BaseRule
    {
        public decimal GiftBonus { get; set; }
        public int DaysBefore { get; set; }
        public int BonusAvailableDays { get; set; }

        public override void Process(IJobExecutionContext context)
        {
            if (!BonusSystem.IsActive)
                return;

            var bdrule = CustomRuleService.Get(ERule.BirthDay);
            if (bdrule == null || !bdrule.Enabled) return;
            var birthDayRule = BaseRule.Get(bdrule) as BirthDayRule;
            if (birthDayRule == null) return;
            var daysBefore = birthDayRule.DaysBefore * -1;
            var cards = _getCards(daysBefore);
            foreach (var card in cards)
            {
                card.BonusAvailableDays = birthDayRule.BonusAvailableDays;
                card.CompanyName = SettingsMain.ShopName;
                card.GiftBonus = birthDayRule.GiftBonus;
                card.BonusTitle = bdrule.Name;

                _processRule(card);
            }
        }

        private List<BirthDayRuleDto> _getCards(int daysBefore)
        {
            var today = DateTime.Today;
            var result = SQLDataAccess2.ExecuteReadIEnumerable<BirthDayRuleDto>("select card.CardId, customer.StandardPhone, card.BonusAmount from Bonus.Card card " +
                                                                                "inner join Customers.Customer customer on card.CardId=customer.CustomerID " +
                                                                                "where customer.BirthDay is not null " +
                                                                                "and DATEADD(year, DATEDIFF(year,customer.BirthDay, @today), DATEADD(day,@daysBefore,customer.BirthDay)) = @today " +
                                                                                "and not exists(select top (1) * from Bonus.RuleLog where RuleLog.RuleType=@rule and RuleLog.CardId=card.CardId and RuleLog.Created =@today)",
                new { today, daysBefore, rule = ERule.BirthDay }).ToList();
            return result;
        }

        private void _processRule(BirthDayRuleDto card)
        {
            var newbonus = new AdditionBonus
            {
                CardId = card.CardId,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(card.BonusAvailableDays),
                Amount = card.GiftBonus,
                //Name = rule.,
                Status = EAdditionBonusStatus.Create
            };
            AdditionBonusService.Add(newbonus);
            var tmpSum = AdditionBonusService.ActualSum(card.CardId);
            var transLog = Transaction.Factory(card.CardId, card.GiftBonus, card.BonusTitle, EOperationType.AddAdditionBonus, tmpSum + card.GiftBonus);
            TransactionService.Create(transLog);
            var r = new RuleLog
            {
                CardId = card.CardId,
                RuleType = ERule.BirthDay,
                Created = DateTime.Today
            };
            RuleLogService.Add(r);

            //SmsService.Process(card.StandardPhone, ESmsType.OnBirthdayRule, new OnBirthdayRuleTempalte
            //{
            //    CompanyName = SettingsMain.ShopName,
            //    AddBonus = card.GiftBonus,
            //    ToDate = DateTime.Today.AddDays(card.BonusAvailableDays),
            //    Balance = (tmpSum + card.BonusAmount + card.GiftBonus)
            //});
        }

        private class BirthDayRuleDto
        {
            public Guid CardId { get; set; }
            public long StandardPhone { get; set; }
            public decimal BonusAmount { get; set; }

            public string CompanyName { get; set; }
            public int BonusAvailableDays { get; set; }
            public decimal GiftBonus { get; set; }
            public string BonusTitle { get; set; }
        }
    }
}