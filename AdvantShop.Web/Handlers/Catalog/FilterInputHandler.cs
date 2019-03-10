using System.Collections.Generic;
using System.Threading.Tasks;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Models.Catalog;

namespace AdvantShop.Handlers.Catalog
{
    public class FilterInputHandler
    {
        #region Fields

        private readonly int _categoryId;
        private readonly bool _indepth;
        private readonly string _searchQuery;

        #endregion

        #region Constructor

        public FilterInputHandler(int categoryId, bool indepth, string searchQuery)
        {
            _categoryId = categoryId;
            _indepth = indepth;
            _searchQuery = searchQuery;
        }

        #endregion

        public FilterItemModel Get()
        {
            var model = new FilterItemModel()
            {
                Expanded = true,
                Type = "searchQuery",
                Title = LocalizationService.GetResource("Catalog.FilterInput.SearchTitle"), 
                Subtitle = "",
                Control = "input"
            };

            model.Values.Add(new FilterListItemModel()
            {
                Id = "searchInputFilter",
                Text = _searchQuery
            });

            return model;
        }

        public Task<List<FilterItemModel>> GetAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                Localization.Culture.InitializeCulture();
                return new List<FilterItemModel> {Get()};
            });
        }
    }
}