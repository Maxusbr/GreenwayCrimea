using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "PostcodeBoxberryList")]
    public class GrastinBoxberryPostCodeResponse
    {
        [XmlElement("PostcodeBoxberry")]
        public List<PostcodeBoxberry> PostcodesBoxberry { get; set; }
    }

    [Serializable]
    public class PostcodeBoxberry
    {
        [XmlElement("Id")]
        public string Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }
    }
}
