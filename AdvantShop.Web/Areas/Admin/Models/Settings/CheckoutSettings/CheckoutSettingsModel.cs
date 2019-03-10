using AdvantShop.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AdvantShop.Web.Admin.Models.Settings
{
    public class CheckoutSettingsModel : IValidatableObject
    {
        public bool AmountLimitation { get; set; }

        public eOutOfStockAction OutOfStockAction { get; set; }
        public List<SelectListItem> OutOfStockActions { get; set; }

        public float MinimalOrderPriceForDefaultGroup { get; set; }

        public bool ProceedToPayment { get; set; }

        public bool ManagerConfirmed { get; set; }

        [Obsolete("Not actual for 6.0 and grater")]
        public bool DenyToByPreorderedProductsWithZerroAmount { get; set; }

        public bool BuyInOneClick { get; set; }
        public bool BuyInOneClickDisableInCheckout { get; set; }
        public string BuyInOneClickLinkText { get; set; }
        public string BuyInOneClickFirstText { get; set; }
        public string BuyInOneClickButtonText { get; set; }

        public string BuyInOneClickAction { get; set; }
        public List<SelectListItem> BuyInOneClickActions { get; set; }

        public int BuyInOneClickDefaultShippingMethod { get; set; }
        public List<SelectListItem> ShippingMethods { get; set; }

        public int BuyInOneClickDefaultPaymentMethod { get; set; }
        public List<SelectListItem> PaymentMethods { get; set; }

        public bool EnableGiftCertificateService { get; set; }
        public bool DisplayPromoTextbox { get; set; }
        public float MaximalPriceCertificate { get; set; }
        public float MinimalPriceCertificate { get; set; }

        public bool MultiplyGiftsCount { get; set; }

        public bool PrintOrder_ShowStatusInfo { get; set; }
        public bool PrintOrder_ShowMap { get; set; }
        public string PrintOrder_MapType { get; set; }
        public List<SelectListItem> MapTypes { get; set; }
        
        public string OrderNumberFormat { get; set; }

        public int NextOrderNumber { get; set; }

        public string SuccessOrderScript { get; set; }

        public bool ShowClientId { get; set; }

        public bool EnableLogingInTariff { get; set; }

        public bool ZipDisplayPlace { get; set; }
        

        #region checkout fields
        // customer
        public string CustomerFirstNameField { get; set; }

        public bool IsShowLastName { get; set; }
        public bool IsRequiredLastName { get; set; }

        public bool IsShowPatronymic { get; set; }
        public bool IsRequiredPatronymic { get; set; }

        public string CustomerPhoneField { get; set; }
        public bool IsShowPhone { get; set; }
        public bool IsRequiredPhone { get; set; }

        // checkout
        public bool IsShowCountry { get; set; }
        public bool IsRequiredCountry { get; set; }

        public bool IsShowState { get; set; }
        public bool IsRequiredState { get; set; }

        public bool IsShowCity { get; set; }
        public bool IsRequiredCity { get; set; }

        public bool IsShowZip { get; set; }
        public bool IsRequiredZip { get; set; }

        public bool IsShowAddress { get; set; }
        public bool IsRequiredAddress { get; set; }

        public bool IsShowFullAddress { get; set; }

        public bool IsShowUserComment { get; set; }

        public string CustomShippingField1 { get; set; }
        public bool IsShowCustomShippingField1 { get; set; }
        public bool IsReqCustomShippingField1 { get; set; }

        public string CustomShippingField2 { get; set; }
        public bool IsShowCustomShippingField2 { get; set; }
        public bool IsReqCustomShippingField2 { get; set; }

        public string CustomShippingField3 { get; set; }
        public bool IsShowCustomShippingField3 { get; set; }
        public bool IsReqCustomShippingField3 { get; set; }

        // buy one click
        public string BuyInOneClickName { get; set; }
        public bool IsShowBuyInOneClickName { get; set; }
        public bool IsRequiredBuyInOneClickName { get; set; }

        public string BuyInOneClickEmail { get; set; }
        public bool IsShowBuyInOneClickEmail { get; set; }
        public bool IsRequiredBuyInOneClickEmail { get; set; }


        public string BuyInOneClickPhone { get; set; }
        public bool IsShowBuyInOneClickPhone { get; set; }
        public bool IsRequiredBuyInOneClickPhone { get; set; }

        public string BuyInOneClickComment { get; set; }
        public bool IsShowBuyInOneClickComment { get; set; }
        public bool IsRequiredBuyInOneClickComment { get; set; }

        #endregion

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MinimalOrderPriceForDefaultGroup < 0)
            {
                yield return new ValidationResult("Введите корректную минимальную сумму заказа");
            }

            if (MinimalPriceCertificate <= 0)
            {
                yield return new ValidationResult("Введите корректную минимальную сумму сертификата");
            }
            
            if (string.IsNullOrWhiteSpace(OrderNumberFormat) || (!OrderNumberFormat.Contains("#NUMBER#") && !OrderNumberFormat.Contains("#RRR#")))
            {
                yield return new ValidationResult("Неправильный формат номера заказа");
            }
        }
    }
}
