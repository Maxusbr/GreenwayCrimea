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

namespace AdvantShop.Web.Admin.Handlers.Cards
{
    public class SubstractMainBonusHandler : AbstractCommandHandler<bool>
    {
        private readonly AddMainBonusModel _model;
        private Card _card;

        public SubstractMainBonusHandler(AddMainBonusModel model)
        {
            _model = model;
        }

        protected override void Load()
        {
            _card = CardService.Get(_model.CardId);
        }
        protected override void Validate()
        {
            if (_card == null) throw new BlException(T("Admin.Cards.AddMainBonusHandler.Error.CardNotExist"));
            if (_card.Blocked) throw new BlException(T("Admin.Cards.AddMainBonusHandler.Error.CardIsBlock"));
            if (_card.BonusAmount < _model.Amount) throw new BlException(T("Admin.Cards.SubstractMainBonusHandler.Error.MoreSubstractThatHave"));
        }


        protected override bool Handle()
        {
            _card.BonusAmount -= _model.Amount;
            var transLog = Transaction.Factory(_card.CardId, _model.Amount, _model.Reason, EOperationType.SubtractMainBonus, _card.BonusAmount);
            TransactionService.Create(transLog);
            CardService.Update(_card);
            var customer = CustomerService.GetCustomer(_card.CardId);
            if (customer != null && customer.StandardPhone.HasValue)
            {
                var addBonus = AdditionBonusService.ActualSum(_card.CardId);
                SmsService.Process(customer.StandardPhone.Value, ESmsType.OnSubtractBonus, new OnSubtractBonusTempalte
                {
                    Bonus = _model.Amount,
                    CompanyName = SettingsMain.ShopName,
                    Balance = (_card.BonusAmount + addBonus),
                    Basis= _model.Reason
                });
            }
            return true;
        }
    }
}
