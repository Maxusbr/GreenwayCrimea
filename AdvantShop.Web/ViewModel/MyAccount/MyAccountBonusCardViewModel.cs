using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Bonuses.Model;

namespace AdvantShop.ViewModel.MyAccount
{
    public class MyAccountBonusCardViewModel
    {
        //public string BonusLastName { get; set; }
        //public string BonusFirstName { get; set; }
        //public string BonusSecondName { get; set; }
        //public string BonusDate { get; set; }
        //public string BonusPhone { get; set; }
        //public bool BonusGender { get; set; }
        public Card BonusCard { get; set; }

        public float BonusPlus { get; set; }
    }
}