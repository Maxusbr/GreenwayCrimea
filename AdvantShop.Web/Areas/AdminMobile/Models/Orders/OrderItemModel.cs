using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Orders;

namespace AdvantShop.Areas.AdminMobile.Models.Orders
{
    public class OrderItemModel
    {
        public OrderItemModel()
        {
            Statuses = new List<SelectListItem>();
        }

        public Order Order { get; set; }
        public List<SelectListItem> Statuses { get; set; }

    }
}