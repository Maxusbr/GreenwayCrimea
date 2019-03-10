using System.Collections.Generic;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Areas.AdminMobile.Models.Home
{
    public class HomeOrdersModel
    {
        public Currency CurrentCurrency { get; set; }

        public List<OrderStatus> OrderStatuses { get; set; }

        public string DailyOrdersSum { get; set; }

        public int DailyOrdersCount { get; set; }
        
        public string Sales { get; set; }

        public float PlanPercent { get; set; }

        public string DailyVisitors { get; set; }

        public int  AllOrdersCount { get; set; }
    }
}