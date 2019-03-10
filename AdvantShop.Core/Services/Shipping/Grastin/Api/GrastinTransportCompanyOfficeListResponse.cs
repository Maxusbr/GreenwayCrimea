using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "officelist")]
    public class GrastinTransportCompanyOfficeListResponse
    {
        [XmlElement("office")]
        public List<Office> Offices { get; set; }
    }

    [Serializable]
    public class Office
    {
        [XmlElement("Id")]
        public string Id { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("company")]
        public string Company { get; set; }

        [XmlElement("adres")]
        public string Address { get; set; }
    }
}
