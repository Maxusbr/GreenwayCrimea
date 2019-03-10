using System;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Bonuses.Model.Enums;
using AdvantShop.Core.Services.Bonuses.Model.Rules;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Customers;
using AdvantShop.ViewModel.MyAccount;

namespace AdvantShop.Handlers.MyAccount
{
    public class MyAccountBonusSystemHandler
    {
        private readonly Customer _customer;

        public MyAccountBonusSystemHandler(Customer customer)
        {
            _customer = customer;
        }

        public MyAccountBonusCardViewModel Get()
        {
            Card bonusCard;
            MyAccountBonusCardViewModel model;

            if (_customer.BonusCardNumber != null && (bonusCard = BonusSystemService.GetCard(_customer.Id)) != null)
            {
                model = new MyAccountBonusCardViewModel()
                {
                    BonusCard = bonusCard,
                    //BonusFirstName = bonusCard.FirstName,
                    //BonusLastName = bonusCard.LastName,
                    //BonusSecondName = bonusCard.SecondName,
                    //BonusDate = bonusCard.DateOfBirth != null ? ((DateTime)bonusCard.DateOfBirth).ToString("dd.MM.yyyy") : string.Empty,
                    //BonusPhone = bonusCard.CellPhone,
                    //BonusGender = bonusCard.Gender
                };
            }
            else
            {
                model = new MyAccountBonusCardViewModel()
                {
                    //BonusFirstName = _customer.FirstName,
                    //BonusLastName = _customer.LastName,
                    //BonusSecondName = _customer.Patronymic,
                    BonusPlus = BonusSystem.BonusesForNewCard
                };
            }

            return model;
        }
    }
}