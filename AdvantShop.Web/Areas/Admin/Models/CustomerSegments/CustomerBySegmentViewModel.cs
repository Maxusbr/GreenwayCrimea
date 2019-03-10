using System;
using AdvantShop.Localization;

namespace AdvantShop.Web.Admin.Models.CustomerSegments
{
    public class CustomerBySegmentViewModel
    {
        public int Id { get; set; }

        public Guid CustomerId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string Name
        {
            get { return Firstname + " " + Lastname; }
        }

        public int OrdersCount { get; set; }
        public float OrdersSum { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public string RegistrationDateTimeFormatted { get { return Culture.ConvertDateWithoutSeconds(RegistrationDateTime); } }


        public int LastOrderId { get; set; }
        public string LastOrderNumber { get; set; }
        public string Location { get; set; }
        public string ManagerName { get; set; }
    }
}
