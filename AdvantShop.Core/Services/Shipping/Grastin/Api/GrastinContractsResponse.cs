using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "ContractList")]
    public class GrastinContractsResponse
    {
        [XmlElement("Contract")]
        public List<Contract> Contracts { get; set; }
    }

    [Serializable]
    public class Contract
    {
        [XmlElement("Id")]
        public string Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Idregion")]
        public string IdRegion { get; set; }
    }
}
