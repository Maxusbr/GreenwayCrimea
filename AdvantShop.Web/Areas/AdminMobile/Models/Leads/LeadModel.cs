using System;
using AdvantShop.Helpers;

namespace AdvantShop.Areas.AdminMobile.Models.Leads
{
    public class LeadModel
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedDateFormated
        {
            get { return CreatedDate.ToString("dd.MM.yy HH:mm"); }
        }

        public Guid? CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerPatronymic { get; set; }
        public string FullName
        {
            get { return StringHelper.AggregateStrings(" ", CustomerLastName, CustomerFirstName, CustomerPatronymic); }
        }

        public string Status { get; set; }
    }
}