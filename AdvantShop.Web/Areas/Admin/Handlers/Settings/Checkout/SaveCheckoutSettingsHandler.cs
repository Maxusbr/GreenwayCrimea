using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Payment;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Checkout
{
    public class SaveCheckoutSettingsHandler
    {
        private CheckoutSettingsModel _model;

        public SaveCheckoutSettingsHandler(CheckoutSettingsModel model)
        {
            _model = model;
        }

        public void Execute()
        {
            SettingsCheckout.BuyInOneClick = _model.BuyInOneClick;
            SettingsCheckout.BuyInOneClickDisableInCheckout = _model.BuyInOneClickDisableInCheckout;
            SettingsCheckout.BuyInOneClickFirstText = _model.BuyInOneClickFirstText;
            SettingsCheckout.BuyInOneClickButtonText = _model.BuyInOneClickButtonText;
            SettingsCheckout.BuyInOneClickCreateOrder = _model.BuyInOneClickAction == "order";
            SettingsCheckout.BuyInOneClickLinkText = _model.BuyInOneClickLinkText;
            SettingsCheckout.BuyInOneClickDefaultShippingMethod = _model.BuyInOneClickDefaultShippingMethod;
            SettingsCheckout.BuyInOneClickDefaultPaymentMethod = _model.BuyInOneClickDefaultPaymentMethod;
            SettingsCheckout.AmountLimitation = _model.AmountLimitation;
            SettingsCheckout.OutOfStockAction = _model.OutOfStockAction;

            SettingsCheckout.DenyToByPreorderedProductsWithZerroAmount = _model.DenyToByPreorderedProductsWithZerroAmount;
            SettingsCheckout.ProceedToPayment = _model.ProceedToPayment;
            SettingsCheckout.MultiplyGiftsCount = _model.MultiplyGiftsCount;

            SettingsCheckout.PrintOrder_ShowStatusInfo = _model.PrintOrder_ShowStatusInfo;
            SettingsCheckout.PrintOrder_ShowMap = _model.PrintOrder_ShowMap;
            SettingsCheckout.PrintOrder_MapType = _model.PrintOrder_MapType;

            if (SettingsCheckout.EnableGiftCertificateService != _model.EnableGiftCertificateService)
            {
                var listMethod = PaymentService.GetAllPaymentMethods(true);
                var method = listMethod.FirstOrDefault(x => x is PaymentGiftCertificate);
                if (method == null && _model.EnableGiftCertificateService)
                {
                    PaymentService.AddPaymentMethod(new PaymentGiftCertificate
                    {
                        Enabled = true,
                        Name = LocalizationService.GetResource("Admin.Settings.Checkout.PaymentGiftCertificateName"),
                        Description = LocalizationService.GetResource("Admin.Settings.Checkout.PaymentGiftCertificateDescription"),
                        SortOrder = 0
                    });
                }
                else if (method != null && !_model.EnableGiftCertificateService)
                {
                    PaymentService.DeletePaymentMethod(method.PaymentMethodId);
                    SettingsDesign.GiftSertificateVisibility = false;
                }
            }

            SettingsCheckout.EnableGiftCertificateService = _model.EnableGiftCertificateService;
            SettingsCheckout.DisplayPromoTextbox = _model.DisplayPromoTextbox;
            SettingsCheckout.MaximalPriceCertificate = _model.MaximalPriceCertificate;
            SettingsCheckout.MinimalPriceCertificate = _model.MinimalPriceCertificate;

            SettingsCheckout.MinimalOrderPriceForDefaultGroup = _model.MinimalOrderPriceForDefaultGroup;
            SettingsCheckout.ManagerConfirmed = _model.ManagerConfirmed;

            SettingsCheckout.SuccessOrderScript = _model.SuccessOrderScript;

            SettingsCheckout.OrderNumberFormat = _model.OrderNumberFormat.Trim().Replace("\t", "").Replace("\r", "").Replace("\n", "");

            #region checkout fields

            if (!Saas.SaasDataService.IsSaasEnabled || Saas.SaasDataService.CurrentSaasData.OrderAdditionFields)
            {
                SettingsCheckout.CustomerFirstNameField = _model.CustomerFirstNameField;
                SettingsCheckout.IsShowLastName = _model.IsShowLastName;
                SettingsCheckout.IsRequiredLastName = _model.IsRequiredLastName;
                SettingsCheckout.IsShowPatronymic = _model.IsShowPatronymic;
                SettingsCheckout.IsRequiredPatronymic = _model.IsRequiredPatronymic;
                SettingsCheckout.CustomerPhoneField = _model.CustomerPhoneField;
                SettingsCheckout.IsShowPhone = _model.IsShowPhone;
                SettingsCheckout.IsRequiredPhone = _model.IsRequiredPhone;

                // checkout
                SettingsCheckout.IsShowCountry = _model.IsShowCountry;
                SettingsCheckout.IsRequiredCountry = _model.IsRequiredCountry;
                SettingsCheckout.IsShowState = _model.IsShowState;
                SettingsCheckout.IsRequiredState = _model.IsRequiredState;
                SettingsCheckout.IsShowCity = _model.IsShowCity;
                SettingsCheckout.IsRequiredCity = _model.IsRequiredCity;
                SettingsCheckout.IsShowZip = _model.IsShowZip;
                SettingsCheckout.IsRequiredZip = _model.IsRequiredZip;
                SettingsCheckout.IsShowAddress = _model.IsShowAddress;
                SettingsCheckout.IsRequiredAddress = _model.IsRequiredAddress;
                SettingsCheckout.IsShowUserComment = _model.IsShowUserComment;
                SettingsCheckout.CustomShippingField1 = _model.CustomShippingField1;
                SettingsCheckout.IsShowCustomShippingField1 = _model.IsShowCustomShippingField1;
                SettingsCheckout.IsReqCustomShippingField1 = _model.IsReqCustomShippingField1;
                SettingsCheckout.CustomShippingField2 = _model.CustomShippingField2;
                SettingsCheckout.IsShowCustomShippingField2 = _model.IsShowCustomShippingField2;
                SettingsCheckout.IsReqCustomShippingField2 = _model.IsReqCustomShippingField2;
                SettingsCheckout.CustomShippingField3 = _model.CustomShippingField3;
                SettingsCheckout.IsShowCustomShippingField3 = _model.IsShowCustomShippingField3;
                SettingsCheckout.IsReqCustomShippingField3 = _model.IsReqCustomShippingField3;
                SettingsCheckout.IsShowFullAddress = _model.IsShowFullAddress;

                // buy one click
                SettingsCheckout.BuyInOneClickName = _model.BuyInOneClickName;
                SettingsCheckout.IsShowBuyInOneClickName = _model.IsShowBuyInOneClickName;
                SettingsCheckout.IsRequiredBuyInOneClickName = _model.IsRequiredBuyInOneClickName;
                SettingsCheckout.BuyInOneClickEmail = _model.BuyInOneClickEmail;
                SettingsCheckout.IsShowBuyInOneClickEmail = _model.IsShowBuyInOneClickEmail;
                SettingsCheckout.IsRequiredBuyInOneClickEmail = _model.IsRequiredBuyInOneClickEmail;
                SettingsCheckout.BuyInOneClickPhone = _model.BuyInOneClickPhone;
                SettingsCheckout.IsShowBuyInOneClickPhone = _model.IsShowBuyInOneClickPhone;
                SettingsCheckout.IsRequiredBuyInOneClickPhone = _model.IsRequiredBuyInOneClickPhone;
                SettingsCheckout.BuyInOneClickComment = _model.BuyInOneClickComment;
                SettingsCheckout.IsShowBuyInOneClickComment = _model.IsShowBuyInOneClickComment;
                SettingsCheckout.IsRequiredBuyInOneClickComment = _model.IsRequiredBuyInOneClickComment;
                SettingsCheckout.ZipDisplayPlace = _model.ZipDisplayPlace;
            }
            #endregion

            SettingsDesign.ShowClientId = _model.ShowClientId;
        }
    }
}
