using System;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Core.Services.Bonuses.Sms;
using AdvantShop.Core.Services.Bonuses.Sms.Template;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using Quartz;

namespace AdvantShop.Core.Services.Bonuses.Model.Rules
{
    [DisallowConcurrentExecution]
    public class NewCardRule : BaseRule
    {
        public decimal GiftBonus { get; set; }
        public int? BonusAvailableDays { get; set; }

        public override void Process(IJobExecutionContext context)
        {
            if (!BonusSystem.IsActive)
                return;

            var bdrule = CustomRuleService.Get(ERule.NewCard);
            if (bdrule == null || !bdrule.Enabled) return;
            var rule = BaseRule.Get(bdrule) as NewCardRule;
            if (rule == null) return;
            var today = DateTime.Today;
            var days = 7;
            var result =
                SQLDataAccess2.ExecuteReadIEnumerable<NewCardRuleDto>(
                    "select card.CardId,customer.StandardPhone from Bonus.Card card " +
                    "inner join Customers.Customer customer on card.CardId=customer.CustomerID " +
                    "where DATEDIFF(day, card.CreateOn, @today) < @days " +
                    "and not exists(select top (1) * from Bonus.RuleLog where RuleLog.RuleType=@rule and RuleLog.CardId=card.CardId)",
                    new { today, days, rule = ERule.NewCard }).ToList();
            var companyname = SettingsMain.ShopName;
            foreach (var card in result)
            {
                processCard(card.CardId, card.StandardPhone, rule, bdrule.Name, companyname);
            }
        }

        public void Execute(Guid cardId)
        {
            var bdrule = CustomRuleService.Get(ERule.NewCard);
            if (bdrule == null || !bdrule.Enabled) return;
            var rule = BaseRule.Get(bdrule) as NewCardRule;
            if (rule == null) return;
            var companyname = SettingsMain.ShopName;
            var customer = CustomerService.GetCustomer(cardId);

            processCard(cardId, customer.StandardPhone, rule, bdrule.Name, companyname);

        }

        private void processCard(Guid cardId, long? cellPhone, NewCardRule rule, string title, string companyname)
        {
            var card = CardService.Get(cardId);
            if (card == null) return;
            var startD = rule.BonusAvailableDays.HasValue ? DateTime.Today : (DateTime?)null;
            var endD = rule.BonusAvailableDays.HasValue ? DateTime.Today.AddDays(rule.BonusAvailableDays.Value) : (DateTime?)null;

            var newbonus = new AdditionBonus
            {
                CardId = cardId,
                StartDate = startD,
                EndDate = endD,
                Amount = rule.GiftBonus,
                Name = title,
                Status = EAdditionBonusStatus.Create
            };
            var tmpSum = AdditionBonusService.ActualSum(cardId);
            var transLog = Transaction.Factory(cardId, rule.GiftBonus, title, EOperationType.AddAdditionBonus, tmpSum + rule.GiftBonus);
            TransactionService.Create(transLog);
            AdditionBonusService.Add(newbonus);
            var r = new RuleLog
            {
                CardId = cardId,
                RuleType = ERule.NewCard,
                Created = DateTime.Today
            };
            RuleLogService.Add(r);

            if (cellPhone.HasValue)
                SmsService.Process(cellPhone.Value, ESmsType.OnAddBonus, new OnAddBonusTempalte
                {
                    CompanyName = companyname,
                    Bonus = rule.GiftBonus,
                    Balance = (tmpSum + card.BonusAmount + rule.GiftBonus),
                    Basis = title
                });
        }

        private class NewCardRuleDto
        {
            public Guid CardId { get; set; }
            public long StandardPhone { get; set; }
        }

    }
}