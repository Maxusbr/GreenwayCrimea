using System;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.Services.Bonuses.Service;

namespace AdvantShop.Core.Services.Bonuses.Model
{
    public class Transaction
    {
        public int Id { get; set; }
        public Guid CardId { get; set; }
        public decimal Amount { get; set; }
        public string Basis { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime CreateOnCut { get; set; }
        public EOperationType OperationType { get; set; }
        public decimal Balance { get; set; }
        public int? PurchaseId { get; set; }
        public int? AdditionalBonusId { get; set; }

        private Card _card;
        public Card Card
        {
            get { return _card ?? (_card = CardService.Get(CardId)); }
            set { _card = value; }
        }

        private AdditionBonus _additionalBonus;
        public AdditionBonus AdditionalBonus
        {
            get { return !AdditionalBonusId.HasValue ? null : _additionalBonus ?? (_additionalBonus = AdditionBonusService.Get(AdditionalBonusId.Value)); }
            set { _additionalBonus = value; }
        }

        public static Transaction Factory(Guid cardId, decimal amount, string basis, EOperationType type, decimal balance, int? purchaseId, int? additionalBonusId)
        {
            return new Transaction
            {
                CreateOn = DateTime.Now,
                CreateOnCut = DateTime.Now,
                CardId = cardId,
                Amount = amount,
                Basis = basis,
                OperationType = type,
                Balance = balance,
                PurchaseId = purchaseId,
                AdditionalBonusId = additionalBonusId,
            };
        }

        public static Transaction Factory(Guid cardId, decimal amount, string basis, EOperationType type, decimal balance)
        {
            return Factory(cardId, amount, basis, type, balance, null, null);
        }

        public static Transaction Factory(Guid cardId, decimal amount, string basis, EOperationType type, decimal balance, int? purchaseId)
        {
            return Factory(cardId, amount, basis, type, balance, purchaseId, null);
        }
    }
}
