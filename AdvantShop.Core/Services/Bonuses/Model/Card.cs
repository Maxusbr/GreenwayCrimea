using System;
using System.Collections.Generic;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Customers;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.Bonuses.Model
{
    public class Card
    {
        /// <summary>
        /// Id карты (равен CustomerId)
        /// </summary>
        public Guid CardId { get; set; }

        /// <summary>
        /// Номер бонусной карты (то что видит пользователь)
        /// </summary>
        public long CardNumber { get; set; }

        /// <summary>
        /// Сумма основных бонусов
        /// </summary>
        public decimal BonusAmount { get; set; }

        public bool Blocked { get; set; }

        public int GradeId { get; set; }

        public bool ManualGrade { get; set; }

        public DateTime CreateOn { get; set; }
        public DateTime? DateLastWipeBonus { get; set; }

        private Grade _grade;
        [JsonIgnore]
        public Grade Grade
        {
            get { return _grade ?? (_grade = GradeService.Get(GradeId)); }
            set { _grade = value; }
        }

        private Customer _customer;
        [JsonIgnore]
        public Customer Customer
        {
            get { return _customer ?? (_customer = CustomerService.GetCustomer(CardId)); }
            set { _customer = value; }
        }

        private List<AdditionBonus> _additionBonuses;
        [JsonIgnore]
        public List<AdditionBonus> AdditionBonuses
        {
            get { return _additionBonuses ?? (_additionBonuses = AdditionBonusService.Actual(CardId)); }
        }

        [JsonIgnore]
        public decimal AdditionBonusesActualSum
        {
            get { return AdditionBonusService.ActualSum(CardId); }
        }

        /// <summary>
        /// Сумма всех бонусов
        /// </summary>
        public decimal BonusesTotalAmount
        {
            get { return !Blocked ? BonusAmount + AdditionBonusesActualSum : 0; }
        }

        private List<Transaction> _lasttransactions;
        [JsonIgnore]
        public List<Transaction> LastTransactions
        {
            get { return _lasttransactions ?? (_lasttransactions = TransactionService.GetLast(CardId)); }
        }

        private List<Purchase> _lastPurchases;
        [JsonIgnore]
        public List<Purchase> LastPurchases
        {
            get { return _lastPurchases ?? (_lastPurchases = PurchaseService.GetLast(CardId)); }
        }
    }
}
