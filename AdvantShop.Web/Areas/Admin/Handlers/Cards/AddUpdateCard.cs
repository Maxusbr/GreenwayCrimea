using System;
using AdvantShop.Core;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Bonuses.Model.Rules;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Diagnostics;
using AdvantShop.Web.Admin.Models.Cards;
using AdvantShop.Web.Infrastructure.Handlers;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.Services.Bonuses.Sms;
using AdvantShop.Core.Services.Bonuses.Sms.Template;
using AdvantShop.Configuration;
using AdvantShop.Customers;

namespace AdvantShop.Web.Admin.Handlers.Cards
{
    public class AddUpdateCard : AbstractCommandHandler<Guid>
    {
        private readonly CardModel _model;

        public AddUpdateCard(CardModel model)
        {
            _model = model;
        }

        protected override void Validate()
        {
            if (_model.IsEditMode) return;

            var existuerCard = CardService.Get(_model.CardId);
            if (existuerCard != null)
                throw new BlException(T("Admin.Cards.AddUpdateCard.Error.UserExist"), "CardNumber");

            if (_model.CardNumber == 0)
            {
                _model.CardNumber = BonusSystemService.GenerateCardNumber(_model.CardNumber);
            }
            else
            {
                if (_model.CardNumber < BonusSystem.CardFrom || _model.CardNumber > BonusSystem.CardTo)
                {
                    throw new BlException(T("Admin.Cards.AddUpdateCard.Error.RangeCard"), "CardNumber");
                }

                var card = CardService.Get(_model.CardNumber);
                if (card != null)
                {
                    throw new BlException(T("Admin.Cards.AddUpdateCard.Error.CardNumberExist"), "CardNumber");
                }
            }
        }

        protected override Guid Handle()
        {
            var card = CardService.Get(_model.CardId) ?? new Card
            {
                CardNumber = _model.CardNumber,
                CardId = _model.CardId,
                CreateOn = DateTime.Now,
            };

            card.Blocked = _model.Blocked;
            card.ManualGrade = _model.ManualGrade;

            var  needinsertGradeHistory = card.GradeId != _model.GradeId;
            card.GradeId = _model.GradeId;
            var newgrade = GradeService.Get(_model.GradeId);
            try
            {
                if (_model.IsEditMode)
                {
                    CardService.Update(card);
                    if (needinsertGradeHistory)
                    {
                        var bonusHistory = new PersentHistory
                        {
                            GradeName = newgrade.Name,
                            BonusPersent = newgrade.BonusPercent,
                            CardId = card.CardId,
                            CreateOn = DateTime.Now,
                            ByAction = EHistoryAction.HandChangeUI
                        };
                        PersentHistoryService.Add(bonusHistory);
                    }

                    var customer = CustomerService.GetCustomer(card.CardId);
                    if (customer != null && customer.StandardPhone != null && newgrade != null)
                    {
                        var balance = card.BonusAmount + AdditionBonusService.ActualSum(card.CardId);
                        SmsService.Process(customer.StandardPhone.Value, ESmsType.ChangeGrade, new OnUpgradePercentTempalte
                        {
                            CardNumber = card.CardNumber,
                            CompanyName = SettingsMain.ShopName,
                            GradeName = newgrade.Name,
                            GradePercent = newgrade.BonusPercent,
                            Balance = balance
                        });
                    }
                }
                else
                {
                    CardService.Add(card);                    
                }               

                return _model.CardId;
            }
            catch (Exception ex)
            {
                Debug.Log.Error("AddUpdate card", ex);
            }
            return Guid.Empty;
        }
    }
}
