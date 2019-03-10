using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "SelfpickupBoxberryList")]
    public class GrastinBoxberrySelfPickupResponse
    {
        [XmlElement("SelfpickupBoxberry")]
        public List<SelfpickupBoxberry> SelfpickupsBoxberry { get; set; }
    }

    [Serializable]
    public class SelfpickupBoxberry
    {
        [XmlElement("Id")]
        public string Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("schedule")]
        public string Schedule { get; set; }

        [XmlElement("drivingdescription")]
        public string DrivingDescription { get; set; }

        [XmlElement("deliveryperiod")]
        public string DeliveryPeriod { get; set; }

        [XmlElement("fullprepayment")]
        public bool FullPrePayment { get; set; }

        [XmlElement("acquiring")]
        public bool Acquiring { get; set; }
    }
}
