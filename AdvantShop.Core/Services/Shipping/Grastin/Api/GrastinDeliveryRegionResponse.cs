using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "Regions")]
    public class GrastinDeliveryRegionResponse
    {
        [XmlElement("Region")]
        public List<DeliveryRegion> DeliveryRegions { get; set; }
    }

    [Serializable]
    public class DeliveryRegion
    {
        [XmlElement("Id")]
        public string Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("IsCourier")]
        public bool IsCourier { get; set; }
    }
}
