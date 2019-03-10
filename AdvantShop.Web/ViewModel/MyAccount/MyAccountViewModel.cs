using System.Collections.Generic;
using AdvantShop.Core.Services.MyAccount;

namespace AdvantShop.ViewModel.MyAccount
{
    public class MyAccountViewModel
    {
        public bool DisplayBonuses { get; set; }
        public decimal BonusesAmount { get; set; }

        public bool DisplayChangeEmail { get; set; }

        public List<MyAccountTab> Tabs { get; set; }
    }
}