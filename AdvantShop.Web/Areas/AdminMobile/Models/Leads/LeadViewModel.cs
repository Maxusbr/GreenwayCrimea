using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Customers;
using AdvantShop.Orders;

namespace AdvantShop.Areas.AdminMobile.Models.Leads
{
    public class LeadViewModel
    {
        public LeadViewModel()
        {
            Statuses = new List<SelectListItem>();
            Managers = new List<SelectListItem>();
        }

        public Lead Lead { get; set; }


        public OrderCurrency Currency { get; set; }

        public Customer Customer { get; set; }
        public Customer CurrentCustomer { get; set; }
        public Customer Manager { get; set; }


        public List<SelectListItem> Statuses { get; set; }
        public List<SelectListItem> Managers { get; set; }
    }
}