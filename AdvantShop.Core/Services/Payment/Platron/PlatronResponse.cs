//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Xml.Serialization;

namespace AdvantShop.Payment
{
    [Serializable]
    [XmlRoot("response", IsNullable = false)]
    public class PlatronResponse
    {
        [XmlElement("pg_status")]
        public string Status { get; set; }

        [XmlElement("pg_salt")]
        public string Salt { get; set; }

        [XmlElement("pg_error_code")]
        public string ErrorCode { get; set; }

        [XmlElement("pg_error_description")]
        public string ErrorDescription { get; set; }

        [XmlElement("pg_sig")]
        public string Sig { get; set; }
    }

    [XmlRoot("response", IsNullable = false)]
    public class PlatronPaymentResponse : PlatronResponse
    {
        [XmlElement("pg_payment_id")]
        public string PaymentId { get; set; }

        [XmlElement("pg_redirect_url")]
        public string RedirectUrl { get; set; }

        [XmlElement("pg_redirect_url_type")]
        public string RedirectUrlType { get; set; }
    }
}