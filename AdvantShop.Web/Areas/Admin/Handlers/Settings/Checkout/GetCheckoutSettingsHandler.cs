using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Saas;
using AdvantShop.Shipping;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.Settings.Checkout
{
    public class GetCheckoutSettingsHandler : AbstractCommandHandler<CheckoutSettingsModel>
    {
        protected override CheckoutSettingsModel Handle()
        {
            List<SelectListItem> outOfStockActions = new List<SelectListItem>();

            if (SettingsCheckout.OutOfStockAction == eOutOfStockAction.Preorder)
            {
                outOfStockActions.Add(new SelectListItem() { Text = T("Admin.Settings.Checkout.CreatePreorder"), Value = eOutOfStockAction.Preorder.ToString() });
            }

            outOfStockActions.Add(new SelectListItem() { Text = T("Admin.Settings.Checkout.CreateOrder"), Value = eOutOfStockAction.Order.ToString() });
            outOfStockActions.Add(new SelectListItem() { Text = T("Admin.Settings.Checkout.CreateLead"), Value = eOutOfStockAction.Lead.ToString(), Disabled = SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCrm });
            outOfStockActions.Add(new SelectListItem() { Text = T("Admin.Settings.Checkout.AddToCart"), Value = eOutOfStockAction.Cart.ToString() });

            var shippingMethods = new List<SelectListItem> { new SelectListItem() { Text = "----", Value = "0" } };
            foreach (var shipping in ShippingMethodService.GetAllShippingMethods(true))
            {
                shippingMethods.Add(new SelectListItem() { Text = shipping.Name, Value = shipping.ShippingMethodId.ToString() });
            }


            var paymentMethods = new List<SelectListItem> { new SelectListItem() { Text = "----", Value = "0" } };
            foreach (var payment in PaymentService.GetAllPaymentMethods(true))
            {
                paymentMethods.Add(new SelectListItem() { Text = payment.Name, Value = payment.PaymentMethodId.ToString() });
            }


            var mapTypes = new List<SelectListItem>
            {
                //new SelectListItem() {Text = T("Admin.Settings.Checkout.GoogleMaps"), Value = "googlemap"},
                new SelectListItem() {Text = T("Admin.Settings.Checkout.YandexMaps"), Value = "yandexmap"}
            };

            var oneClickActions = new List<SelectListItem>
            {
                new SelectListItem() {Text = T("Admin.Settings.Checkout.CreateOrder"), Value = "order"},
                new SelectListItem()
                {
                    Text = T("Admin.Settings.Checkout.CreateLead"),
                    Value = "lead",
                    Disabled = SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCrm
                }
            };

            var model = new CheckoutSettingsModel()
            {
                BuyInOneClick = SettingsCheckout.BuyInOneClick,
                BuyInOneClickDisableInCheckout = SettingsCheckout.BuyInOneClickDisableInCheckout,
                BuyInOneClickFirstText = SettingsCheckout.BuyInOneClickFirstText,
                BuyInOneClickButtonText = SettingsCheckout.BuyInOneClickButtonText,
                BuyInOneClickAction = SettingsCheckout.BuyInOneClickCreateOrder ? "order" : "lead",
                BuyInOneClickActions = oneClickActions,
                BuyInOneClickLinkText = SettingsCheckout.BuyInOneClickLinkText,

                BuyInOneClickDefaultShippingMethod = SettingsCheckout.BuyInOneClickDefaultShippingMethod,
                ShippingMethods = shippingMethods,
                BuyInOneClickDefaultPaymentMethod = SettingsCheckout.BuyInOneClickDefaultPaymentMethod,
                PaymentMethods = paymentMethods,

                AmountLimitation = SettingsCheckout.AmountLimitation,
                OutOfStockAction = SettingsCheckout.OutOfStockAction,
                OutOfStockActions = outOfStockActions,

                DenyToByPreorderedProductsWithZerroAmount = SettingsCheckout.DenyToByPreorderedProductsWithZerroAmount,
                ProceedToPayment = SettingsCheckout.ProceedToPayment,
                MultiplyGiftsCount = SettingsCheckout.MultiplyGiftsCount,

                PrintOrder_ShowStatusInfo = SettingsCheckout.PrintOrder_ShowStatusInfo,
                PrintOrder_ShowMap = SettingsCheckout.PrintOrder_ShowMap,
                PrintOrder_MapType = SettingsCheckout.PrintOrder_MapType,
                MapTypes = mapTypes,

                EnableGiftCertificateService = SettingsCheckout.EnableGiftCertificateService,
                DisplayPromoTextbox = SettingsCheckout.DisplayPromoTextbox,
                MaximalPriceCertificate = SettingsCheckout.MaximalPriceCertificate,
                MinimalPriceCertificate = SettingsCheckout.MinimalPriceCertificate,

                MinimalOrderPriceForDefaultGroup = SettingsCheckout.MinimalOrderPriceForDefaultGroup,
                ManagerConfirmed = SettingsCheckout.ManagerConfirmed,

                OrderNumberFormat = SettingsCheckout.OrderNumberFormat,
                NextOrderNumber = OrderService.GetLastDbOrderId() + 1,
                SuccessOrderScript = SettingsCheckout.SuccessOrderScript,

                #region checkout fields

                CustomerFirstNameField = SettingsCheckout.CustomerFirstNameField,
                IsShowLastName = SettingsCheckout.IsShowLastName,
                IsRequiredLastName = SettingsCheckout.IsRequiredLastName,
                IsShowPatronymic = SettingsCheckout.IsShowPatronymic,
                IsRequiredPatronymic = SettingsCheckout.IsRequiredPatronymic,
                CustomerPhoneField = SettingsCheckout.CustomerPhoneField,
                IsShowPhone = SettingsCheckout.IsShowPhone,
                IsRequiredPhone = SettingsCheckout.IsRequiredPhone,

                // checkout
                IsShowCountry = SettingsCheckout.IsShowCountry,
                IsRequiredCountry = SettingsCheckout.IsRequiredCountry,
                IsShowState = SettingsCheckout.IsShowState,
                IsRequiredState = SettingsCheckout.IsRequiredState,
                IsShowCity = SettingsCheckout.IsShowCity,
                IsRequiredCity = SettingsCheckout.IsRequiredCity,
                IsShowZip = SettingsCheckout.IsShowZip,
                IsRequiredZip = SettingsCheckout.IsRequiredZip,
                IsShowAddress = SettingsCheckout.IsShowAddress,
                IsRequiredAddress = SettingsCheckout.IsRequiredAddress,
                IsShowUserComment = SettingsCheckout.IsShowUserComment,
                CustomShippingField1 = SettingsCheckout.CustomShippingField1,
                IsShowCustomShippingField1 = SettingsCheckout.IsShowCustomShippingField1,
                IsReqCustomShippingField1 = SettingsCheckout.IsReqCustomShippingField1,
                CustomShippingField2 = SettingsCheckout.CustomShippingField2,
                IsShowCustomShippingField2 = SettingsCheckout.IsShowCustomShippingField2,
                IsReqCustomShippingField2 = SettingsCheckout.IsReqCustomShippingField2,
                CustomShippingField3 = SettingsCheckout.CustomShippingField3,
                IsShowCustomShippingField3 = SettingsCheckout.IsShowCustomShippingField3,
                IsReqCustomShippingField3 = SettingsCheckout.IsReqCustomShippingField3,
                IsShowFullAddress = SettingsCheckout.IsShowFullAddress,

                // buy one click
                BuyInOneClickName = SettingsCheckout.BuyInOneClickName,
                IsShowBuyInOneClickName = SettingsCheckout.IsShowBuyInOneClickName,
                IsRequiredBuyInOneClickName = SettingsCheckout.IsRequiredBuyInOneClickName,
                BuyInOneClickEmail = SettingsCheckout.BuyInOneClickEmail,
                IsShowBuyInOneClickEmail = SettingsCheckout.IsShowBuyInOneClickEmail,
                IsRequiredBuyInOneClickEmail = SettingsCheckout.IsRequiredBuyInOneClickEmail,
                BuyInOneClickPhone = SettingsCheckout.BuyInOneClickPhone,
                IsShowBuyInOneClickPhone = SettingsCheckout.IsShowBuyInOneClickPhone,
                IsRequiredBuyInOneClickPhone = SettingsCheckout.IsRequiredBuyInOneClickPhone,
                BuyInOneClickComment = SettingsCheckout.BuyInOneClickComment,
                IsShowBuyInOneClickComment = SettingsCheckout.IsShowBuyInOneClickComment,
                IsRequiredBuyInOneClickComment = SettingsCheckout.IsRequiredBuyInOneClickComment,

                ZipDisplayPlace = SettingsCheckout.ZipDisplayPlace,
                #endregion

                ShowClientId = SettingsDesign.ShowClientId,
                EnableLogingInTariff = SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCustomerLog,



            };

            return model;
        }
    }
}
