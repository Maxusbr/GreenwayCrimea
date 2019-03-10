using System;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Module.ReturnCustomer.Models
{
    public class ReturnCustomerRecordFilterModel : BaseFilterModel<Guid>
    {
        public Guid CustomerID { get; set; }

        public string CustomerName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime LastActionDate { get; set; }

        public string LastSendingDate { get; set; }

        public string LastActionDateFrom { get; set; }

        public string LastActionDateTo { get; set; }

        public string LastSendingDateFrom { get; set; }

        public string LastSendingDateTo { get; set; }
    }
}