//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Xml.Serialization;

namespace AdvantShop.Shipping.Sdek
{
    [Serializable]
    [XmlRoot("StatusReport", IsNullable = false)]
    public class SdekOrderStatusInfo
    {
        [XmlAttribute("DateFirst")]
        public DateTime DateFirst { get; set; }

        [XmlAttribute("DateLast")]
        public DateTime DateLast { get; set; }

        [XmlAttribute("ErrorCode")]
        public string ErrorCode { get; set; }

        [XmlAttribute("Msg")]
        public string Msg { get; set; }

        [XmlElement("Order", Type = typeof(SdekStatusInfoOrder), IsNullable = true)]
        public SdekStatusInfoOrder[] Orders { get; set; }
        
    }

    [Serializable]
    public class SdekStatusInfoOrder
    {
        [XmlAttribute("ActNumber")]
        public string ActNumber { get; set; }

        [XmlAttribute("Number")]
        public string Number { get; set; }

        [XmlAttribute("DispatchNumber")]
        public string DispatchNumber { get; set; }

        [XmlAttribute("Msg")]
        public string Msg { get; set; }

        [XmlIgnore]
        private DateTime deliveryDate { get; set; }

        [XmlAttribute("DeliveryDate")]
        public string DeliveryDate
        {
            get { return deliveryDate == DateTime.MinValue ? string.Empty : deliveryDate.ToString(); }
            set { if (!value.Equals("")) deliveryDate = DateTime.Parse(value); }
        }

        [XmlAttribute("RecipientName")]
        public string RecipientName { get; set; }

        [XmlAttribute("ReturnDispatchNumber")]
        public int ReturnDispatchNumber { get; set; }

        [XmlElement("Status", Type = typeof(SdekStatusInfoOrderStatus))]
        public SdekStatusInfoOrderStatus Status { get; set; }

        [XmlElement("Reason", Type = typeof(SdekStatusInfoOrderReason))]
        public SdekStatusInfoOrderReason Reason { get; set; }

        [XmlElement("DelayReason", Type = typeof(SdekStatusInfoOrderDelayReason))]
        public SdekStatusInfoOrderDelayReason DelayReason { get; set; }

        [XmlElement("Call", Type = typeof(SdekStatusInfoOrderCall))]
        public SdekStatusInfoOrderCall Call { get; set; }
         
    }

    [Serializable]
    public class SdekStatusInfoOrderStatus
    {
        [XmlIgnore]
        private DateTime date { get; set; }
        [XmlAttribute("Date")]
        public string Date
        {
            get { return date == DateTime.MinValue ? string.Empty : date.ToString(); }
            set { if (!value.Equals("")) date = DateTime.Parse(value); }
        }

        [XmlAttribute("Code")]
        public string Code { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }

        [XmlAttribute("CityCode")]
        public int CityCode { get; set; }

        [XmlAttribute("CityName")]
        public string CityName { get; set; }

        [XmlElement("State", Type = typeof(SdekStatusInfoOrderStatusState))]
        public SdekStatusInfoOrderStatusState[] State { get; set; }
    }

    [Serializable]
    public class SdekStatusInfoOrderStatusState
    {
        [XmlIgnore]
        private DateTime date { get; set; }
        [XmlAttribute("Date")]
        public string Date
        {
            get { return date == DateTime.MinValue ? string.Empty : date.ToString(); }
            set { if (!value.Equals("")) date = DateTime.Parse(value); }
        }

        [XmlAttribute("Code")]
        public string Code { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }

        [XmlAttribute("CityCode")]
        public int CityCode { get; set; }

        [XmlAttribute("CityName")]
        public string CityName { get; set; }
    }

    [Serializable]
    public class SdekStatusInfoOrderReason
    {
        [XmlIgnore]
        private DateTime date { get; set; }
        [XmlAttribute("Date")]
        public string Date
        {
            get { return date == DateTime.MinValue ? string.Empty : date.ToString(); }
            set { if (!value.Equals("")) date = DateTime.Parse(value); }
        }

        [XmlAttribute("Code")]
        public string Code { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }
    }

    [Serializable]
    public class SdekStatusInfoOrderDelayReason
    {
        [XmlIgnore]
        private DateTime date { get; set; }
        [XmlAttribute("Date")]
        public string Date
        {
            get { return date == DateTime.MinValue ? string.Empty : date.ToString(); }
            set { if (!value.Equals("")) date = DateTime.Parse(value); }
        }

        [XmlAttribute("Code")]
        public string Code { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }
    }

    [Serializable]
    public class SdekStatusInfoOrderCall
    {
        [XmlElement("CallGood", Type = typeof(SdekStatusInfoOrderCallGoodGood))]
        public SdekStatusInfoOrderCallGoodGood CallGood { get; set; }

        [XmlElement("CallFail", Type = typeof(SdekStatusInfoOrderCallFailFail))]
        public SdekStatusInfoOrderCallFailFail CallFail { get; set; }

        [XmlElement("CallDelay", Type = typeof(SdekStatusInfoOrderCallDelayDelay))]
        public SdekStatusInfoOrderCallDelayDelay CallDelay { get; set; }

    }

    [Serializable]
    public class SdekStatusInfoOrderCallGoodGood
    {
        [XmlElement("Good", Type = typeof(SdekStatusInfoOrderCallGood))]
        public SdekStatusInfoOrderCallGood[] Good { get; set; }
    }

    [Serializable]
    public class SdekStatusInfoOrderCallFailFail
    {
        [XmlElement("Fail", Type = typeof(SdekStatusInfoOrderCallFail))]
        public SdekStatusInfoOrderCallFail[] Fail { get; set; }
    }

    [Serializable]
    public class SdekStatusInfoOrderCallDelayDelay
    {
        [XmlElement("Delay", Type = typeof(SdekStatusInfoOrderCallDelay))]
        public SdekStatusInfoOrderCallDelay[] Delay { get; set; }
    }

    [Serializable]
    public class SdekStatusInfoOrderCallGood
    {
        [XmlIgnore]
        private DateTime date { get; set; }
        [XmlAttribute("Date")]
        public string Date
        {
            get { return date == DateTime.MinValue ? string.Empty : date.ToString(); }
            set { if (!value.Equals("")) date = DateTime.Parse(value); }
        }
        
        [XmlIgnore]
        private DateTime dateDeliv { get; set; }
        [XmlAttribute("DateDeliv")]
        public string DateDeliv
        {
            get { return dateDeliv == DateTime.MinValue ? string.Empty : dateDeliv.ToString(); }
            set { if (!value.Equals("")) dateDeliv = DateTime.Parse(value); }
        }
    }

    [Serializable]
    public class SdekStatusInfoOrderCallFail
    {
        [XmlIgnore]
        private DateTime date { get; set; }
        [XmlAttribute("Date")]
        public string Date
        {
            get { return date == DateTime.MinValue ? string.Empty : date.ToString(); }
            set { if (!value.Equals("")) date = DateTime.Parse(value); }
        }

        [XmlAttribute("ReasonCode")]
        public string ReasonCode { get; set; }

        [XmlAttribute("ReasonDescription")]
        public string ReasonDescription { get; set; }
    }

    [Serializable]
    public class SdekStatusInfoOrderCallDelay
    {
        [XmlIgnore]
        private DateTime date { get; set; }
        [XmlAttribute("Date")]
        public string Date
        {
            get { return date == DateTime.MinValue ? string.Empty : date.ToString(); }
            set { if (!value.Equals("")) date = DateTime.Parse(value); }
        }

        [XmlIgnore]
        private DateTime dateNext { get; set; }
        [XmlAttribute("DateNext")]
        public string DateNext
        {
            get { return dateNext == DateTime.MinValue ? string.Empty : dateNext.ToString(); }
            set { if (!value.Equals("")) dateNext = DateTime.Parse(value); }
        }
    }
}