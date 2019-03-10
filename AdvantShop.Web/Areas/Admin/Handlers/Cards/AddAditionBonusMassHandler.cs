using System;
using System.Transactions;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Core.Services.Bonuses.Sms;
using AdvantShop.Core.Services.Bonuses.Sms.Template;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Statistic;
using AdvantShop.Web.Admin.Models.Cards;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Handlers;
using Transaction = AdvantShop.Core.Services.Bonuses.Model.Transaction;

namespace AdvantShop.Web.Admin.Handlers.Cards
{
    public class AddAditionBonusMassHandler : AbstractCommandHandler<bool>
    {
        private readonly CardFilterModelAddAdditionBonus _model;

        public AddAditionBonusMassHandler(CardFilterModelAddAdditionBonus model)
        {
            _model = model;
        }

        protected override bool Handle()
        {
            if (CommonStatistic.IsRun)
            {
                return true;
            }
            CommonStatistic.Init();
            CommonStatistic.CurrentProcess = "cards/AddBonusMass";
            CommonStatistic.CurrentProcessName = "начисление бонусов";
            CommonStatistic.IsRun = true;
            CommonStatistic.StartNew(() =>
            {
                //add bonus
                try
                {
                    using (var scope = new TransactionScope())
                    {
                        process((id, c) =>
                        {
                            addbonus(id, c);
                            CommonStatistic.RowPosition++;
                            CommonStatistic.TotalUpdateRow++;
                            return true;
                        });
                        scope.Complete();
                    }                
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    CommonStatistic.WriteLog(ex.Message);
                }
                finally
                {
                    CommonStatistic.IsRun = false;
                }
            });
            return true;
        }

        private void process(Func<Guid, CardFilterModelAddAdditionBonus, bool> func)
        {
            if (_model.SelectMode == SelectModeCommand.None && _model.Ids != null)
            {
                CommonStatistic.TotalRow = _model.Ids.Count * 2;
                foreach (var id in _model.Ids)
                {
                    func(id, _model);
                }
            }
            else
            {
                var handler = new GetCardHandler(_model);
                var ids = handler.GetItemsIds("CardId");
                CommonStatistic.TotalRow = ids.Count;
                foreach (var id in ids)
                {
                    if (_model.Ids == null || !_model.Ids.Contains(id))
                        func(id, _model);
                }
            }
        }

        private void addbonus(Guid id, CardFilterModelAddAdditionBonus model)
        {
            var card = CardService.Get(id);
            if (card == null) return;
            if (card.Blocked) return;

            var tempBonus = new AdditionBonus
            {
                CardId = card.CardId,
                Amount = _model.Amount,
                Description = _model.Reason,
                StartDate = _model.StartDate,
                EndDate = _model.EndDate,
                Name = _model.Name,
                Status = EAdditionBonusStatus.Create
            };
            var addBonus = AdditionBonusService.ActualSum(card.CardId);
            AdditionBonusService.Add(tempBonus);
            var transLog = Transaction.Factory(card.CardId, _model.Amount, _model.Reason, EOperationType.AddAdditionBonus, addBonus + _model.Amount);
            TransactionService.Create(transLog);
            CardService.Update(card);
        }

        private void sendsms(Guid id, CardFilterModelAddAdditionBonus model)
        {
            var card = CardService.Get(id);
            if (card == null) return;
            if (card.Blocked) return;
           
            var customer = CustomerService.GetCustomer(card.CardId);
            if (!customer.StandardPhone.HasValue) return;

            var addBonus = AdditionBonusService.ActualSum(card.CardId);
            SmsService.Process(customer.StandardPhone.Value, ESmsType.OnAddBonus, new OnAddBonusTempalte
            {
                Bonus = _model.Amount,
                CompanyName = SettingsMain.ShopName,
                Basis = _model.Reason,
                Balance = (card.BonusAmount + addBonus + _model.Amount)
            });
        }
    }
}