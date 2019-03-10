
using System;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "File")]
    public class GrastinBoxberryPostCode : GrastinBaseObject
    {
        [XmlElement("Method")]
        public override EnApiMethod Method
        {
            get { return EnApiMethod.BoxberryPostCode; }
            set { throw new NotSupportedException("Setting the Method property is not supported"); }
        }

        [XmlElement("City")]
        public string FilterCity { get; set; }
    }
}
