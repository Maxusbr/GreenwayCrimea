using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "SelfpickupList")]
    public class GrastinSelfPickupResponse
    {
        [XmlElement("Selfpickup")]
        public List<Selfpickup> Selfpickups { get; set; }
    }

    [Serializable]
    public class Selfpickup
    {
        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("timetable")]
        public string TimeTable { get; set; }

        [XmlElement("linkdrivingdescription")]
        public string LinkDrivingDescription { get; set; }

        [XmlElement("drivingdescription")]
        public string DrivingDescription { get; set; }

        [XmlElement("phone")]
        public string Phone { get; set; }

        [XmlElement("city")]
        public string City { get; set; }

        [XmlElement("paymentcard")]
        public bool AcceptsPaymentCards { get; set; }

        [XmlElement("regional")]
        public bool RegionalPoint { get; set; }

        [XmlElement("largesize")]
        public bool IssuesLargeSize { get; set; }

        [XmlElement("onlylargesize")]
        public bool IssuesOnlyLargeSize { get; set; }
    }
}
