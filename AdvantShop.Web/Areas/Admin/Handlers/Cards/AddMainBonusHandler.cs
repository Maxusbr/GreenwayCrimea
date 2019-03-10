using System.Data.SqlClient;
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
    public class AddMainBonusHandler : AbstractCommandHandler<bool>
    {
        private readonly AddMainBonusModel _model;
        private Card _card;

        public AddMainBonusHandler(AddMainBonusModel model)
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
        }

        protected override bool Handle()
        {
            _card.BonusAmount += _model.Amount;
            var transLog = Transaction.Factory(_card.CardId, _model.Amount, _model.Reason, EOperationType.AddMainBonus, _card.BonusAmount);
            TransactionService.Create(transLog);
            CardService.Update(_card);
            var addBonus = AdditionBonusService.ActualSum(_card.CardId);
            var customer = CustomerService.GetCustomer(_card.CardId);
            if (!customer.StandardPhone.HasValue) return true;
            SmsService.Process(customer.StandardPhone.Value, ESmsType.OnAddBonus, new OnAddBonusTempalte()
            {
                Bonus = _model.Amount,
                CompanyName = SettingsMain.ShopName,
                Basis = _model.Reason,
                Balance = (_card.BonusAmount + addBonus)
            });
            return true;
        }
    }
}
