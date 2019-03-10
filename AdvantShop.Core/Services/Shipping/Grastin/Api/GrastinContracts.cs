using System;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "File")]
    public class GrastinContracts : GrastinBaseObject
    {
        [XmlElement("Method")]
        public override EnApiMethod Method
        {
            get { return EnApiMethod.GetContractList; }
            set { throw new NotSupportedException("Setting the Method property is not supported"); }
        }
    }
}
