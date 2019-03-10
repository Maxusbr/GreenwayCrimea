using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "Orders")]
    public class GrastinOrderCourierResponse
    {
        [XmlElement("Order")]
        public List<OrderResponse> OrdersResponse { get; set; }
    }

    [Serializable]
    public class OrderResponse
    {
        [XmlElement("number")]
        public string Number { get; set; }

        [XmlElement("Status")]
        public string Status { get; set; }

        [XmlElement("Error")]
        public string Error { get; set; }
    }
}
