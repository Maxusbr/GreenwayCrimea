//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Xml.Serialization;

namespace AdvantShop.Shipping.Sdek
{
    [Serializable]
    [XmlRoot("InfoReport", IsNullable = false)]
    public class SdekOrderInfoReport
    {
        [XmlElement("Order", Type = typeof(SdekOrderInfoReportOrder))]
        public SdekOrderInfoReportOrder[] Orders { get; set; }
    }

    [Serializable]
    public class SdekOrderInfoReportOrder
    {
        [XmlAttribute("Number")]
        public string Number { get; set; }

        [XmlAttribute("Date")]
        public string Date { get; set; }

        [XmlAttribute("DispatchNumber")]
        public int DispatchNumber { get; set; }

        [XmlAttribute("TariffTypeCode")]
        public int TariffTypeCode { get; set; }

        [XmlAttribute("Weight")]
        public float Weight { get; set; }

        [XmlAttribute("DeliverySum")]
        public float DeliverySum { get; set; }

        [XmlIgnore]
        private DateTime dateLastChange { get; set; }

        [XmlAttribute("DateLastChange")]
        public string DateLastChange
        {
            get { return dateLastChange == DateTime.MinValue ? string.Empty : dateLastChange.ToString(); }
            set { var dateTime = new DateTime(); if (DateTime.TryParse(value, out dateTime)) dateLastChange = dateTime; }
        }
        
        [XmlElement("SendCity", Type = typeof(SdekOrderInfoReportSendCity))]
        public SdekOrderInfoReportSendCity SendCity { get; set; }

        [XmlElement("RecCity", Type = typeof(SdekOrderInfoReportRecCity))]
        public SdekOrderInfoReportRecCity RecCity { get; set; }

        [XmlElement("AddedService", Type = typeof(SdekOrderInfoReportAddedService))]
        public SdekOrderInfoReportAddedService AddedService { get; set; }
    }

    [Serializable]
    public class SdekOrderInfoReportSendCity
    {
        [XmlAttribute("Code")]
        public string Code { get; set; }

        [XmlAttribute("PostCode")]
        public string PostCode { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }
    }

    [Serializable]
    public class SdekOrderInfoReportRecCity
    {
        [XmlAttribute("Code")]
        public string Code { get; set; }

        [XmlAttribute("PostCode")]
        public string PostCode { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }
    }

    [Serializable]
    public class SdekOrderInfoReportAddedService
    {
        [XmlAttribute("ServiceCode")]
        public int ServiceCode { get; set; }

        [XmlAttribute("Sum")]
        public float Sum { get; set; }
    }
}