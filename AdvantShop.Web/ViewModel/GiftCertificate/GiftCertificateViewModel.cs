using System.Collections.Generic;
using AdvantShop.Payment;

namespace AdvantShop.ViewModel.GiftCertificate
{
    public class GiftCertificateViewModel
    {
        public List<PaymentMethod> PaymentMethods { get; set; }

        public int PaymentMethod { get; set; }

        public string PaymentKey { get; set; }

        public string NameTo { get; set; }

        public string NameFrom { get; set; }

        public float Sum { get; set; }

        public string Message { get; set; }

        public string EmailTo { get; set; }

        public string EmailFrom { get; set; }

        public string Phone { get; set; }
        
        public string CaptchaBase64 { get; set; }

        public string CaptchaSource { get; set; }

        public string CaptchaCode { get; set; }

        public string MinimumOrderPrice { get; set; }

        public string MinimalPriceCertificate { get; set; }

        public string MaximalPriceCertificate { get; set; }

        public bool Agreement { get; set; }
    }
}