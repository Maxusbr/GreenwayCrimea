using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "Orders")]
    public class OrderInfoResponse
    {
        [XmlElement("Order")]
        public List<OrdersInfo> OrdersInfoResponse { get; set; }
    }

    [Serializable]
    public class OrdersInfo
    {
        [XmlElement("Number")]
        public string Number { get; set; }

        /// <summary>
        /// Статус заказа
        /// </summary>
        [XmlElement("Status")]
        public string Status { get; set; }

        /// <summary>
        /// Статус заказа у партнёрской службы доставки
        /// </summary>
        [XmlElement("SecondStatus")]
        public string SecondStatus { get; set; }

        [XmlElement("StatusDateTime")]
        public string StatusDateTimeXml { get; set; }

        [XmlIgnore]
        public DateTime StatusDateTime
        {
            get
            {
                return string.IsNullOrEmpty(StatusDateTimeXml)
                    ? DateTime.MinValue
                    : DateTime.ParseExact(StatusDateTimeXml, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
            }
        }

        [XmlElement("DateDelivery")]
        public string DateDeliveryXml { get; set; }

        [XmlIgnore]
        public DateTime DateDelivery
        {
            get
            {
                return string.IsNullOrEmpty(DateDeliveryXml)
                    ? DateTime.MinValue
                    : DateTime.ParseExact(DateDeliveryXml, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Почтовый идентификатор. Для заказов Почта России выводится почтовый идентификатор для отслеживания посылки.
        /// </summary>
        [XmlElement("TrackingNumber")]
        public string TrackingNumber { get; set; }
    }
}
