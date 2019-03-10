using System.Collections.Generic;
using System.Web.Mvc;

namespace AdvantShop.Areas.AdminMobile.Models.Leads
{
    public class LeadsViewModel
    {
        public LeadsViewModel()
        {
            Statuses = new List<SelectListItem>();
        }

        public List<SelectListItem> Statuses { get; set; }
    }
}