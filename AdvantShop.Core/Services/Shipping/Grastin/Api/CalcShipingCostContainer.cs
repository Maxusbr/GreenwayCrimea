using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "File")]
    public class CalcShipingCostContainer : GrastinBaseObject
    {
        [XmlElement("Method")]
        public override EnApiMethod Method
        {
            get { return EnApiMethod.CalcShipingCost; }
            set { throw new NotSupportedException("Setting the Method property is not supported"); }
        }

        [XmlArray("Orders")]
        [XmlArrayItem("Order")]
        public List<CalcShipingCostOrder> Orders { get; set; }
    }

    public class CalcShipingCostOrder
    {
        /// <summary>
        /// Номер заказа. Он нужен для расшифровки ответа сервиса чтобы сопоставить суммы в ответе.
        /// </summary>
        [XmlAttribute("number")]
        public string Number { get; set; }

        /// <summary>
        /// Уникальный идентификатор региона в который происходит доставка заказа. Список регионов можно получить методом DeliveryRegion. По региону доставки принимается решение по какому правилу рассчитывать доставку заказа.
        /// Если регион доставки будет соответствовать “БоксБерри” значить доставка будет считаться по их тарифам.
        /// </summary>
        [XmlAttribute("idregion")]
        public string RegionId { get; set; }

        /// <summary>
        /// Вес заказа в граммах
        /// </summary>
        [XmlAttribute("weight")]
        public int Weight { get; set; }

        /// <summary>
        /// Сумма заказа
        /// </summary>
        [XmlIgnore]
        public float OrderSum { get; set; }

        [XmlAttribute("summa")]
        public string OrderSumForXml
        {
            get { return OrderSum.ToString("F2", CultureInfo.InvariantCulture); }
            set { }
        }

        /// <summary>
        /// Оценочная стоимость заказа
        /// </summary>
        [XmlIgnore]
        public float AssessedCost { get; set; }

        [XmlAttribute("assessedsumma")]
        public string AssessedCostForXml
        {
            get { return AssessedCost.ToString("F2", CultureInfo.InvariantCulture); }
            set { }
        }

        /// <summary>
        /// Признак что заказ крупногабаритный
        /// </summary>
        [XmlAttribute("bulky")]
        public int LargeSize { get; set; }

        /// <summary>
        /// Объем заказа в м3. Необходимо заполнять в случае крупногабаритного заказа.
        /// </summary>
        [XmlIgnore]
        public float Volume { get; set; }

        [XmlAttribute("volume")]
        public string VolumeForXml
        {
            get { return Volume.ToString("F2", CultureInfo.InvariantCulture); }
            set { }
        }

        /// <summary>
        /// Ширина заказа (см). Необходимо заполнять в случае крупногабаритного заказа.
        /// </summary>
        [XmlIgnore]
        public float Width { get; set; }

        [XmlAttribute("width")]
        public string WidthForXml
        {
            get { return Width.ToString("F2", CultureInfo.InvariantCulture); }
            set { }
        }

        /// <summary>
        /// Высота заказа (см). Необходимо заполнять в случае крупногабаритного заказа.
        /// </summary>
        [XmlIgnore]
        public float Height { get; set; }

        [XmlAttribute("height")]
        public string HeightForXml
        {
            get { return Height.ToString("F2", CultureInfo.InvariantCulture); }
            set { }
        }

        /// <summary>
        /// Длина заказа (см). Необходимо заполнять в случае крупногабаритного заказа.
        /// </summary>
        [XmlIgnore]
        public float Length { get; set; }

        [XmlAttribute("length")]
        public string LengthForXml
        {
            get { return Length.ToString("F2", CultureInfo.InvariantCulture); }
            set { }
        }

        /// <summary>
        /// Признак что заказ будет отправляться на пункт самовывоза 
        /// </summary>
        [XmlAttribute("selfpickup")]
        public bool IsSelfPickup { get; set; }

        /// <summary>
        /// Признак что заказ будет отправляться на пункт самовывоза работающий по областному тарифу
        /// </summary>
        [XmlAttribute("regional")]
        public bool IsRegional { get; set; }

        /// <summary>
        /// Признак что заказ будет отправляться в транспортную компанию 
        /// </summary>
        [XmlAttribute("transportcompany")]
        public bool IsTransportCompany { get; set; }

        /// <summary>
        /// Оплачиваемое расстояние в км.
        /// </summary>
        [XmlAttribute("paiddistance")]
        public int PaidDistance { get; set; }

        /// <summary>
        /// ID пункта самовывоза сторонней службы доставки
        /// </summary>
        [XmlAttribute("idpickup")]
        public string PickupId { get; set; }

        /// <summary>
        /// ID индекса сторонней службы доставки
        /// </summary>
        [XmlAttribute("idpostcode")]
        public string PostcodeId { get; set; }

    }
}
