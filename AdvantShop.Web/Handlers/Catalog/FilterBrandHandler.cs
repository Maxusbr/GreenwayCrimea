using System.Collections.Generic;
using System.Threading.Tasks;
using AdvantShop.Catalog;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Models.Catalog;

namespace AdvantShop.Handlers.Catalog
{
    public class FilterBrandHandler
    {
        #region Fields

        private readonly int _categoryId;
        private readonly bool _indepth;
        private readonly EProductOnMain _flag;
        private readonly int? _listId;

        private readonly List<int> _selectedBrandIds;
        private readonly List<int> _availableBrandIds;

        #endregion

        #region Constructor

        public FilterBrandHandler(int categoryId, bool indepth, List<int> selectedBrandIds, List<int> availableBrandIds,
                                    EProductOnMain flag = EProductOnMain.None, int? listId = null)
        {
            _categoryId = categoryId;
            _indepth = indepth;
            _flag = flag;
            _selectedBrandIds = selectedBrandIds ?? new List<int>();
            _availableBrandIds = availableBrandIds;
            _listId = listId;
        }

        #endregion

        public FilterItemModel Get()
        {
            var brands = _flag == EProductOnMain.None
                ? CacheManager.Get(CacheNames.BrandsInCategoryCacheName(_categoryId, _indepth, (int)_flag), () => BrandService.GetBrandsByCategoryId(_categoryId, _indepth))
                : CacheManager.Get(CacheNames.BrandsInCategoryCacheName(0, false, (int)_flag, _listId ?? 0), () => BrandService.GetBrandsByProductOnMain(_flag, _listId));

            if (brands == null || brands.Count == 0)
                return null;

            var filterBrand = new FilterItemModel()
            {
                Expanded = true,
                Type = "brand",
                Title = LocalizationService.GetResource("Catalog.FilterBrand.Brands"),
                Subtitle = "",
                Control = "checkbox"
            };

            foreach (var brand in brands)
            {
                filterBrand.Values.Add(new FilterListItemModel()
                {
                    Id = brand.BrandId.ToString(),
                    Text = brand.Name,
                    Selected = _selectedBrandIds.Contains(brand.BrandId),
                    Available = _availableBrandIds == null || _availableBrandIds.Contains(brand.BrandId)
                });
            }

            return filterBrand;
        }

        public Task<List<FilterItemModel>> GetAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                Localization.Culture.InitializeCulture();
                return new List<FilterItemModel>
                {
                    Get()
                };
            });
        }
    }
}