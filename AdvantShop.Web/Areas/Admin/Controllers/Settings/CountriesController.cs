using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Repository;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings;
using AdvantShop.Web.Admin.Handlers.Settings.System;
using AdvantShop.Web.Admin.Models;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class CountriesController : BaseAdminController
    {
        public JsonResult GetCountries()
        {
            var result = CountryService.GetAllCountries().Select(x => new SelectItemModel(x.Name, x.CountryId.ToString())).ToList();
            return Json(result);
        }

        public JsonResult GetCountriesAutocomplete(string q)
        {
            var result = CountryService.GetCountriesByName(q);
            return Json(result);
        }

        #region Add/Edit/Get/Delete

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddCountry(Country model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(new { result = false });
            }

            try
            {
                var country = new Country()
                {
                    Name = model.Name,
                    Iso2 = model.Iso2.Remove(2, model.Iso2.Length - 2) ?? string.Empty,
                    Iso3 = model.Iso3.Remove(3, model.Iso3.Length - 3) ?? string.Empty,
                    DisplayInPopup = model.DisplayInPopup,
                    DialCode = model.DialCode,
                    SortOrder = model.SortOrder
                };

                CountryService.Add(country);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }
        
        public JsonResult EditCountry(Country model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(new { result = false });
            }

            try
            {
                var country = new Country()
                {
                    CountryId = model.CountryId,
                    Name = model.Name,
                    Iso2 = model.Iso2.Remove(2, model.Iso2.Length - 2) ?? string.Empty,
                    Iso3 = model.Iso3.Remove(3, model.Iso3.Length - 3) ?? string.Empty,
                    DisplayInPopup = model.DisplayInPopup,
                    DialCode = model.DialCode,
                    SortOrder = model.SortOrder
                };

                CountryService.Update(country);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }
        
        public JsonResult GetCountryItem(int CountryId)
        {
            var country = CountryService.GetCountry(CountryId);
            return JsonOk(country);
        }

        
        public JsonResult GetCountry(AdminCountryFilterModel model)
        {
            var hendler = new GetCountry(model);
            var result = hendler.Execute();

            return Json(result);
        }
        
        public JsonResult DeleteCountry(AdminCountryFilterModel model)
        {
            Command(model, (id, c) =>
            {
                CountryService.Delete(id);
                return true;
            });

            return Json(true);
        }

        #endregion    

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ActivateCountry(AdminCountryFilterModel model)
        {
            Command(model, (id, c) =>
            {
                var country = CountryService.GetCountry(id);
                country.DisplayInPopup = true;
                CountryService.Update(country);
                return true;
            });
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DisableCountry(AdminCountryFilterModel model)
        {
            Command(model, (id, c) =>
            {
                var country = CountryService.GetCountry(id);
                country.DisplayInPopup = false;
                CountryService.Update(country);
                return true;
            });
            return Json(true);
        }
        
        #region Inplace

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InplaceCountry(Country model)
        {
            var country = CountryService.GetCountry(model.CountryId);

            country.Name = model.Name;
            country.Iso2 = model.Iso2 ?? string.Empty;
            country.Iso3 = model.Iso3 ?? string.Empty;
            country.DisplayInPopup = model.DisplayInPopup;
            country.DialCode = model.DialCode;
            country.SortOrder = model.SortOrder;

            CountryService.Update(country);

            return Json(new { result = true });
        }

        #endregion

        #region Command

        private void Command(AdminCountryFilterModel model, Func<int, AdminCountryFilterModel, bool> func)
        {
            if (model.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in model.Ids)
                {
                    func(id, model);
                }
            }
            else
            {
                var handler = new GetCountry(model);
                var CountryiD = handler.GetItemsIds();

                foreach (int id in CountryiD)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }
        #endregion
    }
}
