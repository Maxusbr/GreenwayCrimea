using System;
using AdvantShop.Localization;

namespace AdvantShop.Web.Admin.Models.Cards
{
    public class CardsModel
    {
        public Guid CardId { get; set; }
        public long CardNumber { get; set; }
        public string Email { get; set; }
        public long MobilePhone { get; set; }
        //public string LastName { get; set; }
        //public string FirstName { get; set; }

        public string FIO { get; set; }
        //public string FIO
        //{
        //    get { return LastName + " " + FirstName; }
        //}

        public string GradeName { get; set; }
        public decimal GradePersent { get; set; }
        public DateTime Created { get; set; }

        public string CreatedFormatted
        {
            get { return Culture.ConvertDateWithoutSeconds(Created); }
        }
    }
}
