using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Repository;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings.System;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class CitiesController : BaseAdminController
    {
        [ExcludeFilter(typeof(AuthAttribute))]
        public JsonResult GetCitiesAutocomplete(string q)
        {
            var result = CityService.GetCitiesByName(q);
            return Json(result);
        }

        #region Add/Edit/Get/Delete

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddCity(City model)
        {
            if (string.IsNullOrEmpty(model.Name))
                return Json(new { result = false });

            try
            {
                var city = new City()
                {
                    Name = model.Name,
                    RegionId = model.RegionId,
                    CitySort = model.CitySort,
                    DisplayInPopup = model.DisplayInPopup,
                    PhoneNumber = model.PhoneNumber,
                    MobilePhoneNumber = model.MobilePhoneNumber
                };

                CityService.Add(city);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        public JsonResult EditCity(City model)
        {
            if (string.IsNullOrEmpty(model.Name))
                return Json(new { result = false });

            try
            {
                var city = new City()
                {
                    CityId = model.CityId,
                    Name = model.Name,
                    RegionId = model.RegionId,
                    CitySort = model.CitySort,
                    DisplayInPopup = model.DisplayInPopup,
                    PhoneNumber = model.PhoneNumber,
                    MobilePhoneNumber = model.MobilePhoneNumber
                };

                CityService.Update(city);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        public JsonResult GetCityItem(int CityId)
        {
            var city = CityService.GetCity(CityId);

            return Json(city);
        }


        public JsonResult GetCitys(AdminCityFilterModel model)
        {
            if (model.id != null)
            {
                model.RegionId = (int)model.id;
            }

            var hendler = new GetCity(model);
            var result = hendler.Execute();

            return Json(result);
        }

        public JsonResult GetRegions(int objId, string objType)
        {
            List<Region> result = new List<Region>();
            int countryId = 0;
            int regionId = 0;
            int cityId = 0;

            if (objType == "country")
            {
                countryId = objId;
            }
            else if (objType == "region")
            {
                regionId = objId;
            }
            else if (objType == "city")
            {
                cityId = objId;
            }

            if (countryId == 0)
            {
                if (regionId != 0)
                {
                    var region = RegionService.GetRegion(regionId);
                    countryId = region.CountryId;
                }

                if (countryId == 0)
                {
                    var city = CityService.GetCity(cityId);
                    var region = RegionService.GetRegion(regionId);
                    countryId = region.CountryId;
                }
            }

            result = RegionService.GetRegions(countryId).OrderBy(x => x.SortOrder).ToList();

            return Json(result);
        }

        public JsonResult DeleteCity(AdminCityFilterModel model)
        {
            Command(model, (id, c) =>
            {
                CityService.Delete(id);
                return true;
            });

            return Json(true);
        }

        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ActivateCity(AdminCityFilterModel model)
        {
            Command(model, (id, c) =>
            {
                var city = CityService.GetCity(id);
                city.DisplayInPopup = true;
                CityService.Update(city);
                return true;
            });
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DisableCity(AdminCityFilterModel model)
        {
            Command(model, (id, c) =>
            {
                var city = CityService.GetCity(id);
                city.DisplayInPopup = false;
                CityService.Update(city);
                return true;
            });
            return Json(true);
        }

        #region Inplace

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InplaceCity(AdminCityFilterModel model)
        {
            var city = CityService.GetCity(model.CityId);

            var sorting = 0;
            Int32.TryParse(model.CitySort, out sorting);
            city.Name = model.Name;
            city.RegionId = model.RegionId;
            city.CitySort = sorting;
            city.DisplayInPopup = (bool)model.DisplayInPopup;
            city.PhoneNumber = model.PhoneNumber;
            city.MobilePhoneNumber = model.MobilePhoneNumber;

            CityService.Update(city);

            return Json(new { result = true });
        }

        #endregion

        #region Command

        private void Command(AdminCityFilterModel model, Func<int, AdminCityFilterModel, bool> func)
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
                model.RegionId = model.id != null ? (int)model.id : 0;
                var handler = new GetCity(model);
                var CityiDs = handler.GetItemsIds();

                foreach (int id in CityiDs)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

        #endregion

    }
}
