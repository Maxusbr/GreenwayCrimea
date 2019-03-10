using System.Collections.Generic;
using System.Web.Mvc;

namespace AdvantShop.Areas.AdminMobile.Models.Tasks
{
    public class TasksViewModel
    {
        public TasksViewModel()
        {
            Statuses = new List<SelectListItem>();
        }

        public List<SelectListItem> Statuses { get; set; }
    }
}