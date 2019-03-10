using System;
using System.Xml.Serialization;

namespace AdvantShop.Shipping.Sdek
{
    [Serializable]
    [XmlRoot("response", IsNullable = false)]
    public class SdekXmlResponse
    {
        [XmlElement("CallCourier")]
        public SdekXmlResponseObject CallCourier;
    }

    [Serializable]
    public class SdekXmlResponseObject
    {
        [XmlAttribute("Date")]
        public DateTime Date { get; set; }
        [XmlAttribute("ErrorCode")]
        public string ErrorCode { get; set; }
        [XmlAttribute("Msg")]
        public string Msg { get; set; }
    }
}
