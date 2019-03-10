using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Areas.AdminMobile.Models.Orders;
using AdvantShop.Customers;

namespace AdvantShop.Areas.AdminMobile.Models.Tasks
{
    public class TaskViewModel
    {
        public TaskViewModel()
        {
            Statuses = new List<SelectListItem>();
        }

        public ManagerTask Task { get; set; }

        public OrderModel Order { get; set; }

        public List<SelectListItem> Statuses { get; set; }

    }
}