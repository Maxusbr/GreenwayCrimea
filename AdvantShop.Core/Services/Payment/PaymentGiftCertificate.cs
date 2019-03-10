//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Payment
{
    public interface ICertificate
    {
    }

    [PaymentKey("GiftCertificate")]
    public class PaymentGiftCertificate : PaymentMethod, ICertificate, IPaymentCurrencyHide
    {
        public override ProcessType ProcessType
        {
            get { return ProcessType.None; }
        }

        public static PaymentGiftCertificate Factory()
        {
            var certificateMethod = new PaymentGiftCertificate
            {
                Enabled = true,
                Name = LocalizationService.GetResource("Core.Payment.GiftCertificate.PaymentTitle"),
                Description = LocalizationService.GetResource("Core.Payment.GiftCertificate.PaymentDescription"),
                SortOrder = 0,
            };
            return certificateMethod;
        }
    }
}