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
    public class AddAditionBonusHandler: AbstractCommandHandler<bool>
    {
        private readonly AddAdditionalBonusModel _model;

        public AddAditionBonusHandler(AddAdditionalBonusModel model)
        {
            _model = model;
        }

        protected override void Validate()
        {
            var card = CardService.Get(_model.CardId);
            if (card == null) throw new BlException(T("Admin.Cards.AddMainBonusHandler.Error.CardNotExist"));
            if (card.Blocked) throw new BlException(T("Admin.Cards.AddMainBonusHandler.Error.CardIsBlock"));
        }

        protected override bool Handle()
        {
            var tempBonus = new AdditionBonus
            {
                CardId = _model.CardId,
                Amount = _model.Amount,
                Description = _model.Reason,
                StartDate = _model.StartDate,
                EndDate = _model.EndDate,
                Name = _model.Name,
                Status = EAdditionBonusStatus.Create
            };
            var tmpSum = AdditionBonusService.ActualSum(_model.CardId);
            var transLog = Transaction.Factory(_model.CardId, tempBonus.Amount, tempBonus.Description, EOperationType.AddAdditionBonus, tmpSum + tempBonus.Amount);
            TransactionService.Create(transLog);
            AdditionBonusService.Add(tempBonus);
            var card = CardService.Get(_model.CardId);
            var customer = CustomerService.GetCustomer(_model.CardId);
            if (!customer.StandardPhone.HasValue) return true;
            SmsService.Process(customer.StandardPhone.Value, ESmsType.OnAddBonus, new OnAddBonusTempalte()
            {
                Bonus = _model.Amount,
                CompanyName = SettingsMain.ShopName,
                Basis = _model.Reason,
                Balance = (card.BonusAmount + tmpSum + _model.Amount)
            });
            return true;
        }
    }
}
