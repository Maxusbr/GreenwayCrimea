using System;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "File")]
    public class RequestForIntakeContainer : GrastinBaseObject
    {
        [XmlElement("Method")]
        public override EnApiMethod Method
        {
            get { return EnApiMethod.RequestForIntake; }
            set { throw new NotSupportedException("Setting the Method property is not supported"); }
        }

        [XmlElement("IdRegion")]
        public string RegionId { get; set; }

        [XmlElement("Time")]
        public string Time { get; set; }

        [XmlElement("Volume")]
        public string Volume { get; set; }
    }
}
