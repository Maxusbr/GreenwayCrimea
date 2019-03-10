using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "File")]
    public class RussianPostOrderContainer : GrastinBaseObject
    {
        [XmlElement("Method")]
        public override EnApiMethod Method
        {
            get { return EnApiMethod.NewOrderRussianPost; }
            set { throw new NotSupportedException("Setting the Method property is not supported"); }
        }

        [XmlArray("Orders")]
        [XmlArrayItem("Order")]
        public List<RussianPostOrder> Orders { get; set; }
    }

    public class RussianPostOrder
    {
        /// <summary>
        /// Номер заказа в Вашей системе с префиксом
        /// </summary>
        [XmlAttribute("number")]
        public string Number { get; set; }

        /// <summary>
        /// Комментарий по доставке
        /// </summary>
        [XmlAttribute("comment")]
        public string Comment { get; set; }

        /// <summary>
        /// ФИО покупателя
        /// </summary>
        [XmlAttribute("buyer")]
        public string Buyer { get; set; }

        /// <summary>
        /// Номер мобильного телефона
        /// </summary>
        [XmlAttribute("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        [XmlAttribute("email")]
        public string Email { get; set; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        [XmlAttribute("zipcode")]
        public string Index { get; set; }

        /// <summary>
        /// Регион
        /// </summary>
        [XmlAttribute("region")]
        public string Region { get; set; }

        /// <summary>
        /// Район
        /// </summary>
        [XmlAttribute("district")]
        public string District { get; set; }

        /// <summary>
        /// Город
        /// </summary>
        [XmlAttribute("city")]
        public string City { get; set; }

        /// <summary>
        /// Адрес доставки
        /// </summary>
        [XmlAttribute("address")]
        public string Address { get; set; }

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

        [XmlAttribute("value")]
        public string AssessedCostForXml
        {
            get { return AssessedCost.ToString("F2", CultureInfo.InvariantCulture); }
            set { }
        }

        /// <summary>
        /// Тестовый режим
        /// </summary>
        [XmlIgnore]
        public bool IsTest { get; set; }

        [XmlAttribute("test")]
        public string IsTestForXml
        {
            get { return IsTest ? "yes" : null; }
            set { }
        }

        /// <summary>
        /// Склад приёма заказа
        /// </summary>
        [XmlAttribute("takewarehouse")]
        public string TakeWarehouse { get; set; }

        /// <summary>
        /// Вид груза
        /// </summary>
        [XmlAttribute("cargotype")]
        public string CargoType { get; set; }

        /// <summary>
        /// Штрихкод
        /// </summary>
        [XmlAttribute("barcode")]
        public string BarCode { get; set; }

        /// <summary>
        /// Наименование сайта для вывода в маршрутный лист курьера
        /// </summary>
        [XmlAttribute("sitename")]
        public string SiteName { get; set; }

        /// <summary>
        /// Дата доставки
        /// </summary>
        [XmlIgnore]
        public DateTime DeiveryDate { get; set; }

        [XmlAttribute("shippingdate")]
        public string DeiveryDateForXml
        {
            get { return DeiveryDate.ToString("ddMMyyyy"); }
            set { }
        }

        /// <summary>
        /// Код услуги доставки
        /// </summary>
        [XmlAttribute("service")]
        public EnRussianPostService Service { get; set; }

        /// <summary>
        /// Наложенный платеж
        /// </summary>
        [XmlIgnore]
        public bool CashOnDelivery { get; set; }

        [XmlAttribute("cod")]
        public string CashOnDeliveryForXml
        {
            get { return IsTest ? "yes" : "no"; }
            set { }
        }

        [XmlElement("good")]
        public List<GrastinProduct> Products { get; set; }
    }


    public enum EnRussianPostService
    {
        /// <summary>
        /// Почтовая доставка
        /// </summary>
        [Localize("Почтовая доставка")]
        [XmlEnum(Name = "13")]
        PostalShipping = 13,

        /// <summary>
        /// Посылка онлайн
        /// </summary>
        [Localize("Посылка онлайн")]
        [XmlEnum(Name = "14")]
        PackageOnline = 14,

        /// <summary>
        /// Курьер онлайн
        /// </summary>
        [Localize("Курьер онлайн")]
        [XmlEnum(Name = "15")]
        CourierOnline = 15,
    }
}
