using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Taxes;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Certificates;
using AdvantShop.Web.Admin.Models.Certificates;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Marketing
{
    [Auth(RoleAction.Marketing)]
    public partial class CertificatesController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Certificates.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.CertificatesCtrl);

            return View();
        }

        public JsonResult GetCertificates(CertificatesFilterModel model)
        {
            return Json(new GetCertificates(model).Execute());
        }

        #region Commands

        private void Command(CertificatesFilterModel command, Func<int, CertificatesFilterModel, bool> func)
        {
            if (command.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in command.Ids)
                {
                    func(id, command);
                }
            }
            else
            {
                var handler = new GetCertificates(command);
                var ids = handler.GetItemsIds();

                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteCertificates(CertificatesFilterModel model)
        {
            Command(model, (id, c) =>
            {
                GiftCertificateService.DeleteCertificateById(id);
                return true;
            });

            return JsonOk();
        }

        #endregion

        #region Get/Add/Edit

        public JsonResult GetCertificatesItem(int? id)
        {
            var allowedPaymentsIds = GiftCertificateService.GetCertificatePaymentMethodsID();

            var payment = PaymentService.GetAllPaymentMethods(true).Where(x => !(x is PaymentGiftCertificate) && allowedPaymentsIds.Contains(x.PaymentMethodId)).ToList();

            var listsPayment = new List<SelectListItem>();

            if (id == null)
            {
                listsPayment = payment.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.Name,
                            Value = x.PaymentMethodId.ToString(),
                            Selected = x.PaymentMethodId == payment.FirstOrDefault().PaymentMethodId
                        })
                    .ToList();

                return Json(new { listsPayment });
            }

            var result = GiftCertificateService.GetCertificateById((int)id);

            listsPayment = payment.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.PaymentMethodId.ToString(),
                        Selected = result.CertificateOrder != null && result.CertificateOrder.PaymentMethodId != 0
                            ? x.PaymentMethodId == result.CertificateOrder.PaymentMethodId
                            : false
                    })
                .ToList();

            if (result == null)
                return JsonError();

            var model = new AdminCertificatesModel(result);

            return Json(new { model, listsPayment });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddCertificates(CertificatesFilterModel model, int paymentid)
        {
            try
            {
                var certificate = new GiftCertificateOrderModel
                {
                    GiftCertificate = new GiftCertificate()
                    {
                        CertificateCode = model.CertificateCode,
                        FromName = model.FromName ?? string.Empty,
                        ToName = model.ToName ?? string.Empty,
                        Sum = model.Sum != null ? model.Sum.TryParseFloat() : 0.0f,
                        ToEmail = model.ToEmail ?? string.Empty,
                        Enable = model.Enable != null ? (bool) model.Enable : false,
                        CertificateMessage = model.CertificateMessage ?? string.Empty
                    },
                    EmailFrom = model.FromEmail,
                    PaymentId = paymentid,
                    Phone = string.Empty
                };

                GiftCertificateService.CreateCertificateOrder(certificate);

            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return JsonError();
            }

            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult EditCertificates(CertificatesFilterModel model)
        {
            try
            {
                var giftCertificate = GiftCertificateService.GetCertificateById(model.CertificateId);

                if (giftCertificate == null)
                    return JsonError();

                giftCertificate.CertificateId = model.CertificateId;
                giftCertificate.FromName = model.FromName ?? string.Empty;
                giftCertificate.ToName = model.ToName ?? string.Empty;
                giftCertificate.Sum = model.Sum != null ? model.Sum.TryParseFloat() : 0.0f;
                giftCertificate.ToEmail = model.ToEmail ?? string.Empty;
                giftCertificate.Enable = model.Enable != null ? (bool) model.Enable : false;
                giftCertificate.Used = model.Used != null ? (bool) model.Used : false;
                giftCertificate.CertificateMessage = model.CertificateMessage ?? string.Empty;

                GiftCertificateService.UpdateCertificateById(giftCertificate);

                if (model.Payed != null && (bool) model.Payed != giftCertificate.Paid)
                {
                    OrderService.PayOrder(giftCertificate.OrderId, (bool) model.Payed);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return JsonError();
            }

            return JsonOk();
        }

        #endregion
        

        #region Settings

        public JsonResult GetSettings()
        {
            var paymentMethodIds = GiftCertificateService.GetCertificatePaymentMethodsID();
            var taxIds = TaxService.GetCertificateTaxes().Select(tax => tax.TaxId).ToList();

            return Json(new CertificatesSettings
            {
                PaymentMethods = PaymentService.GetAllPaymentMethods(true).Where(x => !(x is PaymentGiftCertificate))
                    .Select(x => new SelectListItem()
                    {
                        Value = x.PaymentMethodId.ToString(),
                        Text = x.Name,
                        Selected = paymentMethodIds.Any(id => id == x.PaymentMethodId)
                    }).ToList(),

                Taxes = TaxService.GetTaxes()
                    .Select(x => new SelectListItem()
                    {
                        Value = x.TaxId.ToString(),
                        Text = x.Name,
                        Selected = taxIds.Any(id => id == x.TaxId)
                    }).ToList()
            });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveSettings(CertificatesSettings model)
        {
            var paymentsIds =
                model != null && model.PaymentMethods != null
                    ? model.PaymentMethods.Where(x => x.Selected).Select(x => x.Value.TryParseInt()).ToList()
                    : new List<int>();

            GiftCertificateService.SaveCertificatePaymentMethods(paymentsIds);

            var taxIds =
                model != null && model.Taxes != null
                    ? model.Taxes.Where(x => x.Selected).Select(x => x.Value.TryParseInt()).ToList()
                    : new List<int>();

            TaxService.SaveCertificateTaxes(taxIds);

            return JsonOk();
        }
        
        #endregion

    }
}
