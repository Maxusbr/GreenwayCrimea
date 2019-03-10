using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "SelfpickupHermesList")]
    public class GrastinHermesSelfPickupResponse
    {
        [XmlElement("SelfpickupHermes")]
        public List<SelfpickupHermes> SelfpickupHermes { get; set; }
    }

    [Serializable]
    public class SelfpickupHermes
    {
        [XmlElement("Id")]
        public string Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }
    }
}
