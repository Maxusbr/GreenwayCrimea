using System.Collections.Generic;
using System.Web.Mvc;

namespace AdvantShop.Web.Admin.Models.Settings
{
    public class APISettingsModel
    {
        public APISettingsModel()
        {
        }
        public string Key { get; set; }
        public bool IsRus { get; set; }
        public bool _1CEnabled { get; set; }
        public bool _1CDisableProductsDecremention { get; set; }
        public bool _1CUpdateStatuses { get; set; }
        public bool ExportOrdersType { get; set; }
        public List<SelectListItem> ExportOrders { get; set; }
        public bool _1CUpdateProducts { get; set; }
        public List<SelectListItem> UpdateProducts { get; set; }
        public bool _1CSendProducts { get; set; }
        public List<SelectListItem> SendProducts { get; set; }
        public string ImportPhotosUrl { get; set; }
        public string ImportProductsUrl { get; set; }
        public string ExportProductsUrl { get; set; }
        public string DeletedProducts { get; set; }
        public string ExportOrdersUrl { get; set; }
        public string ChangeOrderStatusUrl { get; set; }
        public string DeletedOrdersUrl { get; set; }

        public string LeadAddUrl { get; set; }
        public string VkUrl { get; set; }

        public bool ShowOneC { get; set; }
    }
}