//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Payment
{
    [Serializable]
    public class PaymentDetails
    {
        [Compare("Core.Payment.PaymentDetails.CompanyName")]
        public string CompanyName { get; set; }

        [Compare("Core.Payment.PaymentDetails.INN")]
        public string INN { get; set; }

        [Compare("Core.Payment.PaymentDetails.Phone")]
        public string Phone { get; set; }
    }
}