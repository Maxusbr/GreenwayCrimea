using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "Errors")]
    public class GrastinErrorResponse
    {
        [XmlElement("Error")]
        public List<string> Errors { get; set; }
    }
}
