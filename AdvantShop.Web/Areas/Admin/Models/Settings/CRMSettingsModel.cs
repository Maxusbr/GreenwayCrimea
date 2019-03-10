using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Services.Crm.DealStatuses;
using AdvantShop.Orders;

namespace AdvantShop.Web.Admin.Models.Settings
{
    public class CrmSettingsModel
    {
        public CrmSettingsModel()
        {
            DealStatuses =
                DealStatusService.GetList()
                    .Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() })
                    .ToList();

            DealStatuses.Insert(0, new SelectListItem() { Text = "Не выбран", Value = "0" });


            OrderStatuses =
                OrderStatusService.GetOrderStatuses()
                    .Select(x => new SelectListItem() { Text = x.StatusName, Value = x.StatusID.ToString() })
                    .ToList();

            OrderStatuses.Insert(0, new SelectListItem() { Text = "Не выбран", Value = "0" });
        }

        public int FinalDealStatusId { get; set; }
        public List<SelectListItem> DealStatuses { get; set; }


        public int OrderStatusIdFromLead { get; set; }
        public List<SelectListItem> OrderStatuses { get; set; }
    }
}
