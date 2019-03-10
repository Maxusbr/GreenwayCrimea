using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Repository;
using AdvantShop.Shipping;
using AdvantShop.Shipping.Sdek;
using AdvantShop.Taxes;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.ShippingMethods;
using AdvantShop.Web.Admin.Models.ShippingMethods;
using AdvantShop.Web.Infrastructure.Admin.ModelBinders;
using AdvantShop.Web.Infrastructure.Admin.ShippingMethods;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class ShippingMethodsController : BaseAdminController
    {
        #region List

        public JsonResult GetShippingMethods()
        {
            var list = AdvantshopConfigService.GetDropdownShippings();
            var result = new List<AdminShippingMethodItemModel>();

            foreach (var shipping in ShippingMethodService.GetAllShippingMethods())
            {
                var type = list.FirstOrDefault(p => p.Value.ToLower() == shipping.ShippingType.ToLower());

                result.Add(new AdminShippingMethodItemModel()
                {
                    ShippingMethodId = shipping.ShippingMethodId,
                    Name = shipping.Name,
                    ShippingType = type != null ? type.Text : null,
                    SortOrder = shipping.SortOrder,
                    Enabled = shipping.Enabled,
                    Icon =
                        ShippingIcons.GetShippingIcon(shipping.ShippingType,
                            shipping.IconFileName != null ? shipping.IconFileName.PhotoName : null, shipping.Name),
                });
            }

            return Json(result);
        }


        public JsonResult GetTypesList()
        {
            var list = AdvantshopConfigService.GetDropdownShippings().Select(x => new {label = x.Text, value = x.Value});
            return Json(list);
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeSorting(int id, int? prevId, int? nextId)
        {
            var handler = new ChangeShippingSorting(id, prevId, nextId);
            var result = handler.Execute();

            return Json(new {result = result});
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SetEnabled(int id, bool enabled)
        {
            var shipping = ShippingMethodService.GetShippingMethod(id);
            if (shipping == null)
                return Json(new {result = false});

            shipping.Enabled = enabled;

            ShippingMethodService.UpdateShippingMethod(shipping, false);

            return Json(new {result = true});
        }

        #endregion

        #region Add / Edit / Delete

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddShippingMethod(string name, string type, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Json(new { result = false });

            try
            {
                var method = new ShippingMethod
                {
                    ShippingType = type,
                    Name = name.Trim(),
                    Description = description ?? "",
                    Enabled = type == "FreeShipping",
                    DisplayCustomFields = true,
                    ZeroPriceMessage = T("Admin.ShippingMethods.ZeroPriceMessage"),
                    TaxType = TaxType.Without,
                    ShowInDetails = true
                };

                TrialService.TrackEvent(TrialEvents.AddShippingMethod, method.ShippingType);

                var methodId = ShippingMethodService.InsertShippingMethod(method);

                return Json(new { result = true, id = methodId });
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return Json(new { result = false });
        }

        public ActionResult Edit(int id)
        {
            var model = ShippingMethodService.GetShippingMethodAdminModel(id);
            if (model == null)
                return Error404();
            
            SetMetaInformation(T("Admin.ShippingMethods.Edit.Title") + " - " + model.Name);
            SetNgController(NgControllers.NgControllersTypes.ShippingMethodCtrl);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit([ModelBinder(typeof(ModelTypeBinder))] ShippingMethodAdminModel model)
        {
            var method = ShippingMethodService.GetShippingMethod(model.ShippingMethodId);
            if (method == null)
            {
                ShowMessage(NotifyType.Error, "Метод не найден");
                return RedirectToAction("Edit", new { id = model.ShippingMethodId });
            }

            if (!ModelState.IsValid)
            {
                ShowErrorMessages();
                return RedirectToAction("Edit", new { id = model.ShippingMethodId });
            }

            try
            {
                method.Name = model.Name.DefaultOrEmpty();
                method.Description = model.Description.DefaultOrEmpty();
                method.Enabled = model.Enabled;
                method.SortOrder = model.SortOrder;
                method.ShowInDetails = model.ShowInDetails;
                method.DisplayCustomFields = model.DisplayCustomFields;
                method.DisplayIndex = model.DisplayIndex;
                method.ZeroPriceMessage = model.ZeroPriceMessage;
                method.TaxType = model.TaxType;

                method.Params = model.Params;

                ShippingMethodService.UpdateShippingMethod(method);

                var allPayments = ShippingMethodService.GetPayments(method.ShippingMethodId).Select(x => x.PaymentMethodId);
                var selectedPayments = model.Payments.Trim(new[] { '[', ']' }).Split(',').Select(x => x.TryParseInt());

                var payments = allPayments.Where(x => !selectedPayments.Contains(x)).ToList();

                ShippingMethodService.UpdateShippingPayments(method.ShippingMethodId, payments);

                ShowMessage(NotifyType.Success, "Изменения успешно сохранены");
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return RedirectToAction("Edit", new {id = model.ShippingMethodId});
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteMethod(int methodId)
        {
            var shipping = ShippingMethodService.GetShippingMethod(methodId);
            if (shipping != null)
                ShippingMethodService.DeleteShippingMethod(methodId);
            
            return Json(new {result = true});
        }


        #region Icon

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadIcon(int methodId)
        {
            var handler = new UploadShippingMethodIcon(methodId);
            var result = handler.Execute();

            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteIcon(int methodId)
        {
            var method = ShippingMethodService.GetShippingMethod(methodId);
            if (method == null)
                return Json(new { result = false });

            PhotoService.DeletePhotos(method.ShippingMethodId, PhotoType.Shipping);

            return Json(new { result = true });
        }

        #endregion

        #region Countries and Cities

        [HttpGet]
        public JsonResult GetAvailableLocations(int methodId)
        {
            var countries = ShippingPaymentGeoMaping.GetCountryByShippingId(methodId);
            var cities = ShippingPaymentGeoMaping.GetCityByShippingId(methodId);

            return Json(new
            {
                countries = countries.Select(x => new {x.CountryId, x.Name}),
                cities = cities.Select(x => new {x.CityId, x.Name}),
            });
        }

        [HttpGet]
        public JsonResult GetExcludedLocations(int methodId)
        {
            var cities = ShippingPaymentGeoMaping.GetCityByShippingIdExcluded(methodId);
            var country = ShippingPaymentGeoMaping.GetCountryByShippingIdExcluded(methodId);

            return Json(new
            {
                cities = cities.Select(x => new {x.CityId, x.Name, CountryId = CityService.GetCountryIdByCity(x.CityId)}),
                country = country.Select(x => new { x.CountryId, x.Name })
            });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddAvailableCountry(int methodId, string countryName)
        {
            var country = CountryService.GetCountryByName(countryName);
            if (country == null)
                return Json(new { result = false });

            if (!ShippingPaymentGeoMaping.IsExistShippingCountry(methodId, country.CountryId))
                ShippingPaymentGeoMaping.AddShippingCountry(methodId, country.CountryId);

            return Json(new {result = true});
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddAvailableCity(int methodId, string cityName)
        {
            var city = CityService.GetCityByName(cityName);
            if (city == null)
                return Json(new { result = false });

            if (!ShippingPaymentGeoMaping.IsExistShippingCity(methodId, city.CityId))
                ShippingPaymentGeoMaping.AddShippingCity(methodId, city.CityId);

            return Json(new {result = true});
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddExcludedCity(int methodId, string cityName)
        {
            var city = CityService.GetCityByName(cityName);
            if (city == null)
                return Json(new { result = false });

            if (!ShippingPaymentGeoMaping.IsExistShippingCityExcluded(methodId, city.CityId))
                ShippingPaymentGeoMaping.AddShippingCityExcluded(methodId, city.CityId);

            return Json(new {result = true});
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddExcludedCountry(int methodId, string countryName)
        {
            var country = CountryService.GetCountryByName(countryName);
            if (country == null)
                return Json(new { result = false });

            if (!ShippingPaymentGeoMaping.IsExistShippingCountryExcluded(methodId, country.CountryId))
                ShippingPaymentGeoMaping.AddShippingCountryExcluded(methodId, country.CountryId);

            return Json(new { result = true });
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteAvailableCountry(int methodId, int countryId)
        {
            ShippingPaymentGeoMaping.DeleteShippingCountry(methodId, countryId);
            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteAvailableCity(int methodId, int cityId)
        {
            ShippingPaymentGeoMaping.DeleteShippingCity(methodId, cityId);
            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteExcludedCity(int methodId, int cityId)
        {
            ShippingPaymentGeoMaping.DeleteShippingCityExcluded(methodId, cityId);
            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteExcludedCountry(int methodId, int CountryId)
        {
            ShippingPaymentGeoMaping.DeleteShippingCountryExcluded(methodId, CountryId);
            return Json(new { result = true });
        }

        #endregion

        [HttpGet]
        public JsonResult GetPayments(int methodId)
        {
            var items = ShippingMethodService.GetPayments(methodId);
            return Json(items);
        }



        #endregion

        #region CallSdekCourier

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult CallSdekCourier(SdekCallCourierModel model)
        {
            if (ModelState.IsValid)
            {
                var dict = new Dictionary<string, string>
                {
                    {SdekTemplate.DefaultCourierCity, model.DefaultCourierCity},
                    {SdekTemplate.DefaultCourierPhone, model.DefaultCourierPhone},
                    {SdekTemplate.DefaultCourierStreet, model.DefaultCourierStreet},
                    {SdekTemplate.DefaultCourierHouse, model.DefaultCourierHouse},
                    {SdekTemplate.DefaultCourierFlat, model.DefaultCourierFlat},
                    {SdekTemplate.DefaultCourierNameContact, model.DefaultCourierNameContact},
                };

                ShippingMethodService.UpdateShippingParams(model.MethodId, dict);

                var timeBegin = model.TimeFrom.Trim().Split(':');
                var timeEnd = model.TimeTo.Trim().Split(':');

                var date = Convert.ToDateTime(model.Date);
                var dateBegin = new DateTime(date.Year, date.Month, date.Day, Convert.ToInt32(timeBegin[0]), Convert.ToInt32(timeBegin[1]), 0);
                var dateEnd = new DateTime(date.Year, date.Month, date.Day, Convert.ToInt32(timeEnd[0]), Convert.ToInt32(timeEnd[1]), 0);

                var method = ShippingMethodService.GetShippingMethod(model.MethodId);

                var result = (new Sdek(method, null)).CallCourier(
                    date,
                    dateBegin,
                    dateEnd,
                    model.DefaultCourierCity,
                    model.DefaultCourierStreet,
                    model.DefaultCourierHouse,
                    model.DefaultCourierFlat,
                    model.DefaultCourierPhone,
                    model.DefaultCourierNameContact,
                    model.Weight.ToString());
                
                return Json(new
                {
                    result = result.Status,
                    msg = result.Status ? "Заявка отпавлена" : "Ошибка: " + result.Message
                });
            }

            var errors = new List<string>();
            foreach (var modelState in ViewData.ModelState.Values)
                foreach (var error in modelState.Errors)
                    errors.Add(error.ErrorMessage);

            return Json(new { result = false, msg = errors });
        }

        #endregion
    }
}
