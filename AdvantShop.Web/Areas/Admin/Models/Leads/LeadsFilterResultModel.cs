using System;
using AdvantShop.Helpers;
using AdvantShop.Localization;

namespace AdvantShop.Web.Admin.Models.Leads
{
    public class LeadsFilterResultModel
    {
        public int Id { get; set; }

        public Guid? CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerPatronymic { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string FullName
        {
            get
            {
                return CustomerId != null
                    ? StringHelper.AggregateStrings(" ", CustomerLastName, CustomerFirstName, CustomerPatronymic)
                    : StringHelper.AggregateStrings(" ", LastName, FirstName, Patronymic);
            }
        }

        public string ManagerName { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public string CreatedDateFormatted { get { return Culture.ConvertDateWithoutSeconds(CreatedDate); } }

        public float Sum { get; set; }
        public float ProductsSum { get; set; }
        public float ProductsCount { get; set; }
        public string DealStatusName { get; set; }
    }
}
