using System;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "File")]
    public class GrastinTransportCompanyOfficeList : GrastinBaseObject
    {
        [XmlElement("Method")]
        public override EnApiMethod Method
        {
            get { return EnApiMethod.TransportCompanyOfficeList; }
            set { throw new NotSupportedException("Setting the Method property is not supported"); }
        }
    }
}
