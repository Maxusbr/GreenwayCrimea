using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.CMS;

namespace AdvantShop.Core.Services.CMS
{
    public class MenuItemModel
    {
        public MenuItemModel()
        {
            DisplaySubItems = true;

            SubItems = new List<MenuItemModel>();
        }

        public int ItemId { get; set; }

        public int ItemParentId { get; set; }

        public string Name { get; set; }

        public string IconPath { get; set; }

        public string UrlPath { get; set; }

        public bool HasChild { get; set; }

        public bool Blank { get; set; }

        public bool NoFollow { get; set; }

        public bool DisplayBrandsInMenu { get; set; }

        public bool DisplaySubItems { get; set; }

        public int ProductsCount { get; set; }

        public EMenuType MenuType { get; set; }

        public List<MenuItemModel> SubItems { get; set; }

        public List<Brand> Brands { get; set; }
    }
}