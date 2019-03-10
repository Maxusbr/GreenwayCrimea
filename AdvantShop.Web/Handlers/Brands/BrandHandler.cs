using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Models.Brand;
using AdvantShop.Repository;
using AdvantShop.ViewModel.Brand;

namespace AdvantShop.Handlers.Brands
{
    public class BrandHandler
    {
        private readonly UrlHelper _urlHelper;

        public BrandHandler()
        {
            _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        public BrandViewModel GetBrandList(string letterStr, string countryId, string q, int? page)
        {
            var brands = BrandService.GetBrands(false);
            var enLetters = BrandService.GetEngBrandChars();
            var ruLetters = BrandService.GetRusBrandChars();

            char letter;
            if (letterStr != "0-9")
            {
                char.TryParse(letterStr, out letter);
            }
            else
            {
                letter = '0';
            }

            var model = new BrandViewModel()
            {
                EnLetters = GetEnLetters(brands, enLetters, letter),
                RuLetters = GetRuLetters(brands, ruLetters, letter),
            };

            var countries =
                CountryService.GetAllCountries()
                    .Where(c => brands.FindLast(b => b.CountryId == c.CountryId) != null)
                    .ToList();

            var selectedCountry = countryId != null ? countries.Find(x => x.CountryId.ToString() == countryId) : null;
            model.CurentCountyId = selectedCountry != null ? selectedCountry.CountryId : 0;

            model.Countries.Add(new SelectListItem() { Text = LocalizationService.GetResource("Brand.Brands.AllCoutries"), Value = "0", Selected = selectedCountry == null });
            model.Countries.AddRange(
                countries.Select(
                    x =>
                        new SelectListItem()
                        {
                            Text = x.Name,
                            Value = x.CountryId.ToString(),
                            Selected = (selectedCountry != null && x.CountryId == selectedCountry.CountryId)
                        }));

            if (!String.IsNullOrWhiteSpace(q))
            {
                var query = HttpUtility.HtmlEncode(q).ToLower();
                model.SearchBrand = query;
                model.Brands = brands.Where(b => b.Name.ToLower().Contains(query)).ToList();
            }
            else if (!char.IsWhiteSpace(letter) && char.IsLetterOrDigit(letter))
            {
                model.Brands = letter == '0'
                                ? brands.Where(b => char.IsDigit(b.Name[0])).ToList()
                                : brands.Where(b => b.Name.ToLower()[0] == letter).ToList();
            }
            else if (selectedCountry != null)
            {
                model.Brands = brands.Where(b => b.CountryId == selectedCountry.CountryId).ToList();
            }
            else
            {
                model.Brands = brands;
            }

            model.BreadCrumbs = new List<BreadCrumbs>()
            {
                new BreadCrumbs(LocalizationService.GetResource("MainPage"), _urlHelper.RouteUrl("Home")),
                new BreadCrumbs(LocalizationService.GetResource("Brand.Index.BrandsHeader"), _urlHelper.RouteUrl("BrandRoot")),
            };

            return model;
        }

        public BrandItemViewModel GetBrandItem(Brand brand, BrandModel brandModel)
        {
            var brands = BrandService.GetBrands(false);
            var enLetters = BrandService.GetEngBrandChars();
            var ruLetters = BrandService.GetRusBrandChars();
            var letter = brand.Name.ToLower()[0];

            if (!string.IsNullOrWhiteSpace(brand.BrandSiteUrl))
            {
                if (!brand.BrandSiteUrl.Contains("http://") && !brand.BrandSiteUrl.Contains("https://"))
                    brand.BrandSiteUrl = "http://" + brand.BrandSiteUrl;
            }

            var model = new BrandItemViewModel()
            {
                Brand = brand,
                EnLetters = GetEnLetters(brands, enLetters, letter),
                RuLetters = GetRuLetters(brands, ruLetters, letter),
            };

            if (SettingsCatalog.ShowProductsInBrand)
            {
                brandModel.BrandId = brand.BrandId;
                model.ProductsList = new BrandProductPagingHandler(brandModel).GetForBrandItem();
            }

            if (SettingsCatalog.ShowCategoryTreeInBrand)
            {
                model.Categories = GetCategories(brand.BrandId);
            }

            var countries =
                CountryService.GetAllCountries()
                    .Where(c => brands.FindLast(b => b.CountryId == c.CountryId) != null)
                    .ToList();

            var selectedCountryId = brand.BrandCountry != null ? brand.BrandCountry.CountryId : 0;
            model.CurentCountyId = selectedCountryId;

            model.Countries.Add(new SelectListItem() { Text = LocalizationService.GetResource("Brand.Brands.AllCoutries"), Value = "0", Selected = selectedCountryId == 0 });
            model.Countries.AddRange(
                countries.Select(
                    x =>
                        new SelectListItem()
                        {
                            Text = x.Name,
                            Value = x.CountryId.ToString(),
                            Selected = x.CountryId == selectedCountryId
                        }));

            model.BreadCrumbs = new List<BreadCrumbs>()
            {
                new BreadCrumbs(LocalizationService.GetResource("MainPage"), _urlHelper.RouteUrl("Home")),
                new BreadCrumbs(LocalizationService.GetResource("Brand.Index.BrandsHeader"), _urlHelper.RouteUrl("BrandRoot")),
                new BreadCrumbs(brand.Name, _urlHelper.RouteUrl("Brand", new {url = brand.UrlPath})),
            };

            return model;
        }


        #region Help methods

        private List<BrandLetter> GetEnLetters(List<Brand> brands, List<char> enLetters, char letter)
        {
            var list = new List<BrandLetter>();

            list.Add(new BrandLetter()
            {
                Name = "0-9",
                Url = _urlHelper.RouteUrl("BrandRoot", new { letter = "0" }),
                Active = brands.Any(b => char.IsDigit(b.Name[0])),
                Selected = char.IsDigit(letter)
            });

            foreach (var enLetter in enLetters)
            {
                list.Add(new BrandLetter()
                {
                    Name = enLetter.ToString(),
                    Url = _urlHelper.RouteUrl("BrandRoot", new { letter = enLetter }),
                    Active = brands.Find(b => b.Name.ToLower().StartsWith(enLetter.ToString())) != null,
                    Selected = enLetter == letter
                });
            }

            return list;
        }

        private List<BrandLetter> GetRuLetters(List<Brand> brands, List<char> ruLetters, char letter)
        {
            var list = new List<BrandLetter>();

            if (SettingsMain.Language == "ru-RU")
            {
                foreach (var ruLetter in ruLetters)
                {
                    list.Add(new BrandLetter()
                    {
                        Name = ruLetter.ToString(),
                        Url = _urlHelper.RouteUrl("BrandRoot", new { letter = HttpUtility.UrlEncode(ruLetter.ToString()) }),
                        Active = brands.Find(b => b.Name.ToLower().StartsWith(ruLetter.ToString())) != null,
                        Selected = ruLetter == letter
                    });
                }
            }

            return list;
        }

        private List<BrandCategoryViewModel> GetCategories(int brandId)
        {
            var brandCategories = new List<BrandCategoryViewModel>();
            var avalibleCategories = BrandService.GetCategoriesByBrand(brandId);
            var categories = BrandService.GetParentCategoriesbyChildsId(avalibleCategories.Keys.ToList());
            if (!categories.Any())
            {
                return new List<BrandCategoryViewModel>();
            }
            foreach (var item in avalibleCategories)
            {
                var temp = categories.SingleOrDefault(x => x.CategoryId == item.Key);
                if (temp == null) continue;
                temp.InCategoryCount = item.Value.InCategoryCount;
                temp.InChildsCategoryCount = item.Value.InChildsCategoryCount;
            }
            var maxlvl = categories.Max(x => x.Level);
            var minlvl = categories.Min(x => x.Level);
            while (maxlvl > 0)
            {
                var cats = categories.Where(x => x.Level == maxlvl).ToList();
                foreach (var i in cats)
                {
                    var v = categories.SingleOrDefault(x => x.CategoryId == i.ParentId);
                    if (v == null) continue;
                    v.InCategoryCount += i.InCategoryCount;
                    v.InChildsCategoryCount += i.InChildsCategoryCount;
                }
                maxlvl--;
            }
            brandCategories = categories
                                .Where(x => x.Level == minlvl)
                                .Select(x => new BrandCategoryViewModel
                                {
                                    Name = x.Name,
                                    Url = _urlHelper.RouteUrl("Category", new { url = x.Url, brand = brandId, indepth = true }),
                                    Count = x.InChildsCategoryCount,
                                    SubCategories = categories.Where(y => y.Level == minlvl + 1).Where(z => z.ParentId == x.CategoryId)
                                                                .Select(y => new BrandCategoryViewModel
                                                                {
                                                                    Name = y.Name,
                                                                    Url = _urlHelper.RouteUrl("Category", new { url = y.Url, brand = brandId, indepth = true }),
                                                                    Count = y.InChildsCategoryCount
                                                                }).ToList()
                                }).ToList();
            return brandCategories;
        }

        #endregion
    }
}