using System.Transactions;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Core.Services.Bonuses.Sms;
using AdvantShop.Core.Services.Bonuses.Sms.Template;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Models.Cards;
using AdvantShop.Web.Infrastructure.Handlers;
using Transaction = AdvantShop.Core.Services.Bonuses.Model.Transaction;

namespace AdvantShop.Web.Admin.Handlers.Cards
{
    public class SubtractAditionBonusHandler : AbstractCommandHandler<bool>
    {
        private readonly SubctractAdditionalBonusModel _model;
        private Card _card;
        private AdditionBonus _bonus;
        public SubtractAditionBonusHandler(SubctractAdditionalBonusModel model)
        {
            _model = model;
        }

        protected override void Load()
        {
            _card = CardService.Get(_model.CardId);
            _bonus = AdditionBonusService.Get(_model.AdditionId);
        }

        protected override void Validate()
        {
            if (_card == null) throw new BlException(T("Admin.Cards.AddMainBonusHandler.Error.CardNotExist"));
            if (_card.Blocked) throw new BlException(T("Admin.Cards.AddMainBonusHandler.Error.CardIsBlock"));

            if (_bonus == null) throw new BlException(T("Admin.Cards.AddMainBonusHandler.Error.BonusNotExist"));
            if (_bonus.Amount < _model.Amount) throw new BlException(T("Admin.Cards.SubstractMainBonusHandler.Error.MoreSubstractThatHave"));
        }

        protected override bool Handle()
        {

            if (_bonus.Amount == _model.Amount)
            {
                _bonus.Status = EAdditionBonusStatus.Remove;
            }
            else
            {
                _bonus.Amount -= _model.Amount;
                _bonus.Status = EAdditionBonusStatus.Substract;
            }
            var tmpSum = AdditionBonusService.ActualSum(_model.CardId);
            using (var tr = new TransactionScope())
            {
                var transLog = Transaction.Factory(_model.CardId, _model.Amount, _model.Reason, EOperationType.SubtractAdditionBonus, tmpSum - _model.Amount);
                TransactionService.Create(transLog);
                AdditionBonusService.Update(_bonus);
                tr.Complete();
            }
            var customer = CustomerService.GetCustomer(_card.CardId);
            if (customer != null && customer.StandardPhone.HasValue)
            {
                SmsService.Process(customer.StandardPhone.Value, ESmsType.OnSubtractBonus, new OnSubtractBonusTempalte
                {
                    Bonus = _model.Amount,
                    CompanyName = SettingsMain.ShopName,
                    Balance = (_card.BonusAmount + tmpSum - _model.Amount),
                    Basis = _model.Reason
                });
            }
            return true;
        }
    }
}
