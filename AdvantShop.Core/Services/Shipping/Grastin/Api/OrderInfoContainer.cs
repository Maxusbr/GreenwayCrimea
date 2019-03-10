using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "File")]
    public class OrderInfoContainer : GrastinBaseObject
    {
        [XmlElement("Method")]
        public override EnApiMethod Method
        {
            get { return EnApiMethod.OrderInfo; }
            set { throw new NotSupportedException("Setting the Method property is not supported"); }
        }

        [XmlArray("Orders")]
        [XmlArrayItem("Order")]
        public List<string> Orders { get; set; }
    }
}
