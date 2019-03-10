using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "File")]
    public class ActContainer : GrastinBaseObject
    {
        [XmlElement("Method")]
        public override EnApiMethod Method
        {
            get { return EnApiMethod.GetAct; }
            set { throw new NotSupportedException("Setting the Method property is not supported"); }
        }

        [XmlElement("IdContract")]
        public string ContractId { get; set; }

        //[XmlElement("number")]
        //public string OrderNumber { get; set; }

        [XmlArray("Orders")]
        [XmlArrayItem("Order")]
        public List<ActOrder> Orders { get; set; }
    }

    [Serializable]
    public class ActOrder
    {
        [XmlAttribute("number")]
        public string OrderNumber { get; set; }

        [XmlAttribute("seats")]
        public int Seats { get; set; }
    }
}
