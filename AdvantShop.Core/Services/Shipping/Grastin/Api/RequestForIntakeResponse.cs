using System;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "Status")]
    public class RequestForIntakeResponse
    {
        [XmlText]
        public string Status { get; set; }
    }
}
