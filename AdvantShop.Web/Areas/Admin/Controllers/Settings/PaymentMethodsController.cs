using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Payment;
using AdvantShop.Repository;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.PaymentMethods;
using AdvantShop.Web.Admin.Models.PaymentMethods;
using AdvantShop.Web.Infrastructure.Admin.ModelBinders;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class PaymentMethodsController : BaseAdminController
    {
        #region List

        public JsonResult GetPaymentMethods()
        {
            var list = AdvantshopConfigService.GetDropdownPayments();
            var result = new List<AdminPaymentMethodItemModel>();
            
            foreach (var payment in PaymentService.GetAllPaymentMethods(false))
            {
                var type = list.FirstOrDefault(p => p.Value.ToLower() == payment.PaymentKey.ToLower());

                result.Add(new AdminPaymentMethodItemModel()
                {
                    PaymentMethodId = payment.PaymentMethodId,
                    Name = payment.Name,
                    PaymentType = type != null ? type.Text : null,
                    SortOrder = payment.SortOrder,
                    Enabled = payment.Enabled,
                    Icon = PaymentIcons.GetPaymentIcon(payment.PaymentKey, payment.IconFileName != null ? payment.IconFileName.PhotoName : null, payment.Name),
                });
            }

            return Json(result);
        }


        public JsonResult GetTypesList()
        {
            var list = AdvantshopConfigService.GetDropdownPayments().Select(x => new {label = x.Text, value = x.Value});
            return Json(list);
        }
        

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeSorting(int id, int? prevId, int? nextId)
        {
            var handler = new ChangePaymentSorting(id, prevId, nextId);
            var result = handler.Execute();

            return Json(new { result = result });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SetEnabled(int id, bool enabled)
        {
            var payment = PaymentService.GetPaymentMethod(id);
            if (payment == null)
                return Json(new { result = false });

            payment.Enabled = enabled;

            PaymentService.UpdatePaymentMethod(payment, true);

            return Json(new { result = true });
        }


        #endregion

        #region Add / Edit / Delete

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddPaymentMethod(string name, string type, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Json(new { result = false });

            var method = PaymentMethod.Create(type);
            if (method == null)
                return Json(new { result = false });

            try
            {
                var methods = PaymentService.GetAllPaymentMethods(false);

                method.Name = name.Trim();
                method.Description = description ?? "";
                method.SortOrder = methods != null && methods.Count > 0 ? methods.Max(x => x.SortOrder) + 10 : 0;

                method.Enabled = method is Cash;
                
                method.Parameters = method.GetDefaultParameters() ?? method.Parameters;

                //Some dirty magic
                if (method.Parameters.ContainsKey(AssistTemplate.CurrencyValue))
                {
                    var parameters = method.Parameters;
                    parameters[AssistTemplate.CurrencyValue] = "1";
                    method.Parameters = parameters;
                }
                //End of dirty magic

                TrialService.TrackEvent(TrialEvents.AddPaymentMethod, method.PaymentKey);

                var methodId = PaymentService.AddPaymentMethod(method);

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
            var model = PaymentService.GetPaymentMethodAdminModel(id);
            if (model == null)
                return Error404();
            
            SetMetaInformation(T("Admin.PaymentMethods.Edit.Title"));
            SetNgController(NgControllers.NgControllersTypes.PaymentMethodCtrl);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit([ModelBinder(typeof(ModelTypeBinder))] PaymentMethodAdminModel model)
        {
            var method = PaymentService.GetPaymentMethod(model.PaymentMethodId);
            if (method == null)
            {
                ShowMessage(NotifyType.Error, "Метод не найден");
                return RedirectToAction("Edit", new { id = model.PaymentMethodId });
            }

            if (!ModelState.IsValid)
            {
                ShowErrorMessages();
                return RedirectToAction("Edit", new { id = model.PaymentMethodId });
            }

            try
            {
                method.Name = model.Name.DefaultOrEmpty();
                method.Description = model.Description.DefaultOrEmpty();
                method.Enabled = model.Enabled;
                method.SortOrder = model.SortOrder;
                method.ExtrachargeType = model.ExtrachargeType;
                method.Extracharge = model.Extracharge;
                method.CurrencyId = model.CurrencyId;

                method.Parameters = model.Parameters;

                PaymentService.UpdatePaymentMethod(method);

                ShowMessage(NotifyType.Success, "Изменения успешно сохранены");
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return RedirectToAction("Edit", new { id = model.PaymentMethodId });
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteMethod(int methodId)
        {
            var shipping = PaymentService.GetPaymentMethod(methodId);
            if (shipping != null)
                PaymentService.DeletePaymentMethod(methodId);

            return Json(new { result = true });
        }


        #region Icon

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadIcon(int methodId)
        {
            var handler = new UploadPaymentMethodIcon(methodId);
            var result = handler.Execute();

            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteIcon(int methodId)
        {
            var method = PaymentService.GetPaymentMethod(methodId);
            if (method == null)
                return Json(new { result = false });

            PhotoService.DeletePhotos(method.PaymentMethodId, PhotoType.Payment);

            return Json(new { result = true });
        }

        #endregion

        #region Countries and Cities

        [HttpGet]
        public JsonResult GetAvailableLocations(int methodId)
        {
            var countries = ShippingPaymentGeoMaping.GetCountryByPaymentId(methodId);
            var cities = ShippingPaymentGeoMaping.GetCityByPaymentId(methodId);

            return Json(new
            {
                countries = countries.Select(x => new { x.CountryId, x.Name }),
                cities = cities.Select(x => new { x.CityId, x.Name }),
            });
        }
        

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddAvailableCountry(int methodId, string countryName)
        {
            var country = CountryService.GetCountryByName(countryName);
            if (country == null)
                return Json(new { result = false });

            if (!ShippingPaymentGeoMaping.IsExistPaymentCountry(methodId, country.CountryId))
                ShippingPaymentGeoMaping.AddPaymentCountry(methodId, country.CountryId);

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddAvailableCity(int methodId, string cityName)
        {
            var city = CityService.GetCityByName(cityName);
            if (city == null)
                return Json(new { result = false });

            if (!ShippingPaymentGeoMaping.IsExistPaymentCity(methodId, city.CityId))
                ShippingPaymentGeoMaping.AddPaymentCity(methodId, city.CityId);

            return Json(new { result = true });
        }
        

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteAvailableCountry(int methodId, int countryId)
        {
            ShippingPaymentGeoMaping.DeletePaymentCountry(methodId, countryId);
            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteAvailableCity(int methodId, int cityId)
        {
            ShippingPaymentGeoMaping.DeletePaymentCity(methodId, cityId);
            return Json(new { result = true });
        }

        #endregion

        #endregion

        #region Bill Stamp

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadBillStamp(int methodId)
        {
            var method = PaymentService.GetPaymentMethod(methodId);
            if (method == null)
                return Json(new { result = false });

            try
            {
                if (HttpContext.Request.Files.Count != 1)
                    return Json(new { result = false });

                var file = HttpContext.Request.Files[0];

                if (string.IsNullOrEmpty(file.FileName))
                    return Json(new {result = false});
                
                if (!file.FileName.Contains(".") || !FileHelpers.CheckFileExtension(file.FileName, EAdvantShopFileTypes.Image))
                {
                    return Json(new { result = false, error = "Неправильное разрешение файла" });
                }

                if (method.Parameters == null)
                    method.Parameters = new Dictionary<string, string>();

                var parameters = method.Parameters;

                if (!parameters.ContainsKey(BillTemplate.StampImageName))
                    parameters.Add(BillTemplate.StampImageName, "");

                if (!string.IsNullOrEmpty(parameters[BillTemplate.StampImageName]))
                    FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, parameters[BillTemplate.StampImageName]));

                var imgName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                file.SaveAs(FoldersHelper.GetPathAbsolut(FolderType.Pictures, imgName));

                parameters[BillTemplate.StampImageName] = imgName;
                method.Parameters = parameters;

                PaymentService.UpdatePaymentParams(method.PaymentMethodId, method.Parameters);

                return Json(new
                {
                    result = true,
                    src = FoldersHelper.GetPath(FolderType.Pictures, imgName, false),
                    imgName = imgName
                });
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return Json(new {result = false});
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteBillStamp(int methodId)
        {
            var method = PaymentService.GetPaymentMethod(methodId);

            if (method == null)
                return Json(new { result = false });

            try
            {
                if (method.Parameters == null)
                    method.Parameters = new Dictionary<string, string>();

                if (!method.Parameters.ContainsKey(BillTemplate.StampImageName))
                    method.Parameters.Add(BillTemplate.StampImageName, "");

                if (!string.IsNullOrEmpty(method.Parameters[BillTemplate.StampImageName]))
                    FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, method.Parameters[BillTemplate.StampImageName]));

                var parameters = method.Parameters;
                parameters[BillTemplate.StampImageName] = "";

                method.Parameters = parameters;

                PaymentService.UpdatePaymentParams(method.PaymentMethodId, method.Parameters);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return Json(new {result = true});
        }

        #endregion
    }
}
