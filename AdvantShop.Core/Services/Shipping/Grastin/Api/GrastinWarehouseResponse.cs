using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "WarehouseList")]
    public class GrastinWarehouseResponse
    {
        [XmlElement("Warehouse")]
        public List<Warehouse> Warehouses { get; set; }
    }

    [Serializable]
    public class Warehouse
    {
        [XmlElement("Name")]
        public string Name { get; set; }
    }
}
