using System.Threading.Tasks;
using AdvantShop.Models.Catalog;
using AdvantShop.Configuration;
using Resources;
using AdvantShop.Core.Services.Localization;
using System.Collections.Generic;

namespace AdvantShop.Handlers.Catalog
{
    public class FilterAvailabilityHandler
    {
        private readonly bool _isSelectedAvailable;

        public FilterAvailabilityHandler(bool available)
        {
            _isSelectedAvailable = available;
        }


        public List<FilterItemModel> Get()
        {
            var model = new List<FilterItemModel>();
            if (SettingsCatalog.AvaliableFilterEnabled)
            {
                var filter = new FilterItemModel()
                {
                    Title = LocalizationService.GetResource("Catalog.Availability"),
                    Control = "checkbox",
                    Type = "available",
                    Expanded = false,
                };

                filter.Values.Add(new FilterListItemModel()
                {
                    Id = "true",
                    Text = LocalizationService.GetResource("Catalog.InStock"),
                    Available = true,
                    Selected = _isSelectedAvailable
                });


                model.Add(filter);
            }
            return model;
        }


        public Task<List<FilterItemModel>> GetAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                Localization.Culture.InitializeCulture();
                return Get();
            });
        }

    }
}