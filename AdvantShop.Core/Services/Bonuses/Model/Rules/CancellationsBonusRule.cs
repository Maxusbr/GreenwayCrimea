using System;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Core.Services.Bonuses.Sms;
using AdvantShop.Core.Services.Bonuses.Sms.Template;
using AdvantShop.Core.SQL;
using Quartz;

namespace AdvantShop.Core.Services.Bonuses.Model.Rules
{
    [DisallowConcurrentExecution]
    public class CancellationsBonusRule : BaseRule
    {
        public int SmsDayBefore { get; set; }
        public int AgeCard { get; set; }
        public bool NotSendSms { get; set; }

        public override void Process(IJobExecutionContext context)
        {
            if (!BonusSystem.IsActive)
                return;

            var bdrule = CustomRuleService.Get(ERule.CancellationsBonus);
            if (bdrule == null || !bdrule.Enabled) return;
            ProcessNotification(bdrule);
            ProcessCard(bdrule);
        }

        private void ProcessNotification(CustomRule rule)
        {
            var cancellationsBonusRule = BaseRule.Get(rule) as CancellationsBonusRule;
            if (cancellationsBonusRule == null) return;

            var today = DateTime.Today;
            var smsbefore = cancellationsBonusRule.SmsDayBefore;
            var result = SQLDataAccess2.ExecuteReadIEnumerable<CancellationsBonusRuleDto>("select card.CardId,customer.StandardPhone from Bonus.Card card " +
                                                                                          "inner join Customers.Customer customer on card.CardId=customer.CustomerID " +
                                                                                          "where (card.DateLastWipeBonus is null " +
                                                                                          "and cast( DATEADD(year, DATEDIFF(year, card.CreateOn, @today), DATEADD(day, @smsbefore,card.CreateOn))  As Date) = @today ) or " +
                                                                                          "(card.DateLastWipeBonus is not null " +
                                                                                          "and cast( DATEADD(year, DATEDIFF(year, card.DateLastWipeBonus, @today), DATEADD(day, @smsbefore,card.DateLastWipeBonus)) as Date) = @today) " +
                                                                                          "and not exists(select top (1) * from Bonus.RuleLog where RuleLog.RuleType=@rule and RuleLog.CardId=card.CardId and RuleLog.Created =@today)",
                new { today, smsbefore, rule = ERule.CancellationsBonus }).ToList();
            var companyname = SettingsMain.ShopName;

            foreach (var item in result)
            {
                SendAndLog(item.CardId, item.StandardPhone, cancellationsBonusRule.SmsDayBefore, cancellationsBonusRule.NotSendSms, companyname);
            }
        }

        private void SendAndLog(Guid cardId, long cellPhone, int dayLeft, bool notSendSms, string companyname)
        {
            var card = CardService.Get(cardId);
            var balance = card.BonusAmount + AdditionBonusService.ActualSum(cardId);
            if (balance <= 0) return;
            var r = new RuleLog
            {
                CardId = card.CardId,
                RuleType = ERule.CancellationsBonus,
                Created = DateTime.Today
            };
            RuleLogService.Add(r);

            if (!notSendSms)
            {
                SmsService.Process(cellPhone, ESmsType.OnCancellationsBonus,
                    new OnCancellationsBonusTempalte
                    {
                        CompanyName = companyname,
                        Balance = balance,
                        DayLeft = dayLeft
                    });
            }

        }

        private void ProcessCard(CustomRule rule)
        {
            var cancellationsBonusRule = BaseRule.Get(rule) as CancellationsBonusRule;
            if (cancellationsBonusRule == null) return;
            var today = DateTime.Today;
            var ageCard = cancellationsBonusRule.AgeCard;
            var result = SQLDataAccess2.ExecuteReadIEnumerable<Guid>("select card.CardId from Bonus.Card card " +
                                                                     "where (( card.DateLastWipeBonus is null and DATEDIFF(month, card.CreateOn, @today) = @ageCard ) or" +
                                                                     "(card.DateLastWipeBonus is not null and DATEDIFF(month, card.DateLastWipeBonus, @today) = @ageCard )) " +
                                                                     "and not exists(select top (1) * from Bonus.RuleLog where RuleLog.RuleType=@rule and RuleLog.CardId=card.CardId and RuleLog.Created =@today)",
                new { today, ageCard, rule = ERule.CancellationsBonus }).ToList();

            foreach (var item in result)
            {
                ClearBonus(item, rule.Name);
            }
        }

        private void ClearBonus(Guid cardId, string basis)
        {
            var card = CardService.Get(cardId);
            var tmpSum = AdditionBonusService.ActualSum(cardId);
            var balance = card.BonusAmount + tmpSum;
            if (balance <= 0) return;

            var transLog = Transaction.Factory(card.CardId, card.BonusAmount, basis, EOperationType.SubtractMainBonus, 0);
            TransactionService.Create(transLog);
            card.BonusAmount = 0;
            card.DateLastWipeBonus = DateTime.Now;

            foreach (var item in AdditionBonusService.Actual(cardId))
            {
                tmpSum = tmpSum - item.Amount;
                transLog = Transaction.Factory(card.CardId, item.Amount, basis, EOperationType.SubtractMainBonus, tmpSum);
                TransactionService.Create(transLog);
                item.Status = EAdditionBonusStatus.Remove;
                AdditionBonusService.Update(item);
            }
            CardService.Update(card);
        }

        private class CancellationsBonusRuleDto
        {
            public Guid CardId { get; set; }
            public long StandardPhone { get; set; }
        }
    }
}