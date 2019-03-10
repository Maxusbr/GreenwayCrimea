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
    public class ChangeGradeRule : BaseRule
    {
        public bool AllPeriod { get; set; }
        public int Period { get; set; }

        public ChangeGradeRule()
        {
            AllPeriod = true;
        }

        public override void Process(IJobExecutionContext context)
        {
            if (!BonusSystem.IsActive) return;

            var bdrule = CustomRuleService.Get(ERule.ChangeGrade);
            if (bdrule == null || !bdrule.Enabled) return;
            var rule = BaseRule.Get(bdrule) as ChangeGradeRule;
            if (rule == null) return;
            var today = DateTime.Today;
            var days = 7;
            var minDate = DateTime.Today.AddMonths(-rule.Period);
            var result = rule.AllPeriod
                ? SQLDataAccess2.ExecuteReadIEnumerable<ChangeGradeRuleDto>(
                    "select * from( select *, (select top 1 Id from Bonus.Grade g where g.PurchaseBarrier < = temp.PurchaseSum order by PurchaseBarrier DESC ) as NewGradeId " +
                    "from (select card.CardId, card.CardNumber, customer.StandardPhone, card.GradeId, sum(pucharse.PurchaseAmount) PurchaseSum from Bonus.Card card " +
                    "inner join Customers.Customer customer on card.CardId=customer.CustomerID " +
                    "left join Bonus.Purchase pucharse on card.CardId = pucharse.CardId and pucharse.Status = @status " +
                    "where card.ManualGrade=0 and (select count(*) from Bonus.RuleLog where RuleLog.RuleType=@rule and RuleLog.CardId=card.CardId and RuleLog.Created =@today) < 5 " +
                    "group by card.CardId, card.CardNumber, customer.StandardPhone, card.GradeId) temp) t where newGradeId is not null and GradeId <> NewGradeId",
                    new { today, days, status = EPuchaseState.Complete, rule = ERule.ChangeGrade }).ToList()

                : SQLDataAccess2.ExecuteReadIEnumerable<ChangeGradeRuleDto>(
                    "select * from( select *, (select top 1 Id from Bonus.Grade g where g.PurchaseBarrier < = temp.PurchaseSum order by PurchaseBarrier DESC ) as NewGradeId " +
                    "from (select card.CardId, card.CardNumber, customer.StandardPhone, card.GradeId, sum(pucharse.PurchaseAmount) PurchaseSum from Bonus.Card card " +
                    "inner join Customers.Customer customer on card.CardId=customer.CustomerID " +
                    "left join Bonus.Purchase pucharse on card.CardId = pucharse.CardId and pucharse.Status = @status and pucharse.CreateOnCut>=@minDate " +
                    "where card.ManualGrade=0 and (select count(*) from Bonus.RuleLog where RuleLog.RuleType=@rule and RuleLog.CardId=card.CardId and RuleLog.Created =@today) < 5" +
                    "group by card.CardId, card.CardNumber, customer.StandardPhone, card.GradeId) temp) t where newGradeId is not null and GradeId <> NewGradeId",
                    new { today, days, status = EPuchaseState.Complete, rule = ERule.ChangeGrade, minDate }).ToList();

            if (!result.Any()) return;

            var grades = GradeService.GetAll();

            foreach (var card in result)
            {
                var grade = grades.FirstOrDefault(x => x.Id == card.NewGradeId);
                //if (curent == null) continue;
                //var gradeAva = grades.Where(x => x.PurchaseBarrier <= card.PurchaseSum).OrderByDescending(x => x.PurchaseBarrier).ToList();
                //var grade = gradeAva.FirstOrDefault();
                if (grade != null)// && grade.Id != curent.Id)
                {
                    UpdateCard(card, grade, SettingsMain.ShopName);
                }
            }
        }

        private static void UpdateCard(ChangeGradeRuleDto card, Grade grade, string companyname)
        {
            var updateCard = CardService.Get(card.CardId);
            updateCard.GradeId = grade.Id;
            CardService.Update(updateCard);

            var bonusHistory = new PersentHistory
            {
                GradeName = grade.Name,
                BonusPersent = grade.BonusPercent,
                CardId = card.CardId,
                CreateOn = DateTime.Now,
                ByAction = EHistoryAction.ChangeGradeRule
            };

            PersentHistoryService.Add(bonusHistory);
            var r = new RuleLog
            {
                CardId = card.CardId,
                RuleType = ERule.ChangeGrade,
                Created = DateTime.Today
            };
            RuleLogService.Add(r);            

            var balance = updateCard.BonusAmount + AdditionBonusService.ActualSum(card.CardId);
            SmsService.Process(card.StandardPhone, ESmsType.ChangeGrade, new OnUpgradePercentTempalte
            {
                CardNumber = card.CardNumber,
                CompanyName = companyname,
                GradeName = grade.Name,
                GradePercent = grade.BonusPercent,
                Balance = balance
            });
        }

        public void Execute(Guid cardId)
        {
            if (!BonusSystem.IsActive) return;

            var bdrule = CustomRuleService.Get(ERule.ChangeGrade);
            if (bdrule == null || !bdrule.Enabled) return;
            var rule = BaseRule.Get(bdrule) as ChangeGradeRule;
            if (rule == null) return;

            var today = DateTime.Today;            
            var minDate = DateTime.Today.AddMonths(-rule.Period);
            var result = rule.AllPeriod
                ? SQLDataAccess2.Query<ChangeGradeRuleDto>(
                    "select * from( select *, (select top 1 Id from Bonus.Grade g where g.PurchaseBarrier < = temp.PurchaseSum order by PurchaseBarrier DESC ) as NewGradeId " +
                    "from (select card.CardId, card.CardNumber, customer.StandardPhone, card.GradeId, sum(pucharse.PurchaseAmount) PurchaseSum from Bonus.Card card " +
                    "inner join Customers.Customer customer on card.CardId=customer.CustomerID " +
                    "inner join Bonus.Purchase pucharse on card.CardId = pucharse.CardId " +
                    "where pucharse.Status = @status and card.CardId=@cardId and card.ManualGrade=0 " +
                    "and (select count(*) from Bonus.RuleLog where RuleLog.RuleType=@rule and RuleLog.CardId=card.CardId and RuleLog.Created =@today) < 5" +
                    "group by card.CardId, card.CardNumber, customer.StandardPhone, card.GradeId) temp) t where newGradeId is not null and GradeId <> NewGradeId",
                    new { today, status = EPuchaseState.Complete, rule = ERule.ChangeGrade, cardId = cardId })

                : SQLDataAccess2.Query<ChangeGradeRuleDto>(
                    "select * from( select *, (select top 1 Id from Bonus.Grade g where g.PurchaseBarrier < = temp.PurchaseSum order by PurchaseBarrier DESC ) as NewGradeId " +
                    "from (select card.CardId, card.CardNumber, customer.StandardPhone, card.GradeId, sum(pucharse.PurchaseAmount) PurchaseSum from Bonus.Card card " +
                    "inner join Customers.Customer customer on card.CardId=customer.CustomerID " +
                    "inner join Bonus.Purchase pucharse on card.CardId = pucharse.CardId " +
                    "where pucharse.Status = @status and pucharse.CreateOnCut>=@minDate and card.CardId=@cardId and card.ManualGrade=0 " +
                    "and (select count(*) from Bonus.RuleLog where RuleLog.RuleType=@rule and RuleLog.CardId=card.CardId and RuleLog.Created =@today) < 5" +
                    "group by card.CardId, card.CardNumber, customer.StandardPhone, card.GradeId) temp) t where newGradeId is not null and GradeId <> NewGradeId",
                    new { today, status = EPuchaseState.Complete, rule = ERule.ChangeGrade, minDate, cardId = cardId });

            if (result == null) return;
            var grade = GradeService.Get(result.NewGradeId);
            //var curent = grades.FirstOrDefault(x => x.Id == result.GradeId);
            //if (curent == null) return;

            //var gradeAva = grades.Where(x => x.PurchaseBarrier <= result.PurchaseSum).OrderByDescending(x => x.PurchaseBarrier).ToList();
            //var grade = gradeAva.FirstOrDefault();
            if (grade != null)// && curent.Id != grade.Id)
            {
                UpdateCard(result, grade, SettingsMain.ShopName);
            }

        }

        private class ChangeGradeRuleDto
        {
            public Guid CardId { get; set; }
            public long StandardPhone { get; set; }
            public int GradeId { get; set; }
            public long CardNumber { get; set; }
            public decimal PurchaseSum { get; set; }
            public int NewGradeId { get; set; }
        }
    }
}