using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using AdvantShop.Repository;
using AdvantShop.Configuration;

namespace AdvantShop.Web.Admin.Handlers.Settings.Common
{
    public class GetRegions
    {
        private int _countryId;

        public GetRegions(int countryId)
        {
            _countryId = countryId;
        }

        public List<SelectListItem> Execute()
        {
            var regionId = SettingsMain.SellerRegionId;

            var regions = RegionService.GetRegions(_countryId).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.RegionId.ToString(),
                Selected = x.RegionId == regionId
            }).ToList();

            if(regions.Any() && regions.Any(item=> item.Selected))
            {
                regions[0].Selected = true;
            }

            return regions;
        }
    }
}