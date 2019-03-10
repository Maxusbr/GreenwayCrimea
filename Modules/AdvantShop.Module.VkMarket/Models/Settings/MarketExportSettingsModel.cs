using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Services.Vk;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Module.VkMarket.Models.Settings
{
    public class MarketExportSettingsModel
    {
        public MarketExportSettingsModel()
        {
            Currencies = CurrencyService.GetAllCurrencies(true).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Iso3
            }).ToList();
        }
        
        public bool ExportUnavailableProducts { get; set; }
        public bool AddSizeAndColorInDescription { get; set; }
        public bool AddSizeAndColorInName { get; set; }
        public int ShowDescription { get; set; }
        public bool ShowProperties { get; set; }
        public int AddLinkToSite { get; set; }
        public string TextBeforeLinkToSite { get; set; }
        public VkGroup Group { get; set; }

        public List<SelectListItem> Currencies { get; set; }
        public string CurrencyIso3 { get; set; }
        public bool ExportOnShedule { get; set; }
        public bool IsExportRun { get; set; }
        public List<string> Reports { get; set; }
    }
}
