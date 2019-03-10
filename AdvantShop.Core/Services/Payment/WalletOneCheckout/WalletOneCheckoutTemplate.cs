//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Payment
{
    public struct WalletOneCheckoutTemplate
    {
        public const string MerchantId = "WMI_MERCHANT_ID";
        public const string PaymentAmount = "WMI_PAYMENT_AMOUNT";
        public const string CurrencyCode = "WMI_CURRENCY_ID";
        public const string PaymentNumber = "WMI_PAYMENT_NO";
        public const string Description = "WMI_DESCRIPTION";
        public const string SuccessUrl = "WMI_SUCCESS_URL";
        public const string FailUrl = "WMI_FAIL_URL";
        public const string ExpiredDate = "WMI_EXPIRED_DATE";
        public const string Ptenabled = "WMI_PTENABLED";
        public const string Ptdisabled = "WMI_PTDISABLED";
        public const string RecepientLogin = "WMI_RECIPIENT_LOGIN";
        public const string Culture = "WMI_CULTURE_ID";
        public const string Signature = "WMI_SIGNATURE";
        public const string SecretKey = "Sekretkey";
        public const string PayWaysEnabled = "PayWaysEnabled";
        public const string PayWaysDisabled = "PayWaysDisabled";
        public const string SendReceiptData = "SendReceiptData";
    }

    public class WalletOneCheckoutProductItem
    {
        public string Title { get; set; }
        public float Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float SubTotal { get; set; }
        public string TaxType { get; set; }
        public float Tax { get; set; }
    }
}