using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.FilePath;
using AdvantShop.Models.Catalog;

namespace AdvantShop.Handlers.Catalog
{
    public class FilterColorHandler
    {
        #region Fields

        private readonly int _categoryId;
        private readonly bool _indepth;
        private readonly bool _onlyAvailable;

        private readonly List<int> _selectedColorIds;
        private readonly List<int> _availableColorIds;
       
        #endregion

        #region Constructor

        public FilterColorHandler(int categoryId, bool indepth, List<int> selectedColorIds, List<int> availableColorIds, bool onlyAvailable)
        {
            _categoryId = categoryId;
            _indepth = indepth;
            _selectedColorIds = selectedColorIds ?? new List<int>();
            _availableColorIds = availableColorIds;
            _onlyAvailable = onlyAvailable;
        }

        #endregion

        public FilterItemModel Get()
        {
            var colors = ColorService.GetColorsByCategoryId(_categoryId, _indepth, _onlyAvailable)
                            .Where(c => _availableColorIds == null || _availableColorIds.Any(x => x == c.ColorId))
                            .Select(x => new FilterColor()
                            {
                                ColorId = x.ColorId,
                                ColorName = x.ColorName,
                                ColorCode = x.ColorCode,
                                IconFileName = x.IconFileName,
                                SortOrder = x.SortOrder,
                                Checked = _selectedColorIds.Contains(x.ColorId)
                            })
                            .ToList();

            if (colors.Count == 0)
                return null;
            
            var model = new FilterItemModel()
            {
                Expanded = true,
                Type = "color",
                Title = SettingsCatalog.ColorsHeader,
                Subtitle = "",
                Control = "color"
            };

            foreach (var color in colors)
            {
                model.Values.Add(new {
                    color.ColorName,
                    color.ColorCode,
                    Selected = color.Checked,
                    Id = color.ColorId,
                    Available = true,
                    ImageHeight = color.IconFileName.ImageHeightCatalog,
                    ImageWidth = color.IconFileName.ImageWidthCatalog,
                    color.IconFileName.PhotoName,
                    ImageSrc = color.IconFileName.ImageSrc(ColorImageType.Catalog),
                });
            }

            return model;
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