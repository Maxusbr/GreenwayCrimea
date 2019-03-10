using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "File")]
    public class GrastinOrderCourier : GrastinBaseObject
    {
        [XmlElement("Method")]
        public override EnApiMethod Method
        {
            get { return EnApiMethod.NewOrderCourier; }
            set { throw new NotSupportedException("Setting the Method property is not supported"); }
        }

        [XmlArray("Orders")]
        [XmlArrayItem("Order")]
        public List<GrastinOrder> Orders { get; set; }
    }

    [Serializable]
    public class GrastinOrder
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
        [XmlAttribute("phone1")]
        public string Phone1 { get; set; }

        /// <summary>
        /// Номер стационарного телефона
        /// </summary>
        [XmlAttribute("phone2")]
        public string Phone2 { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        [XmlAttribute("email")]
        public string Email { get; set; }

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

        [XmlAttribute("assessedsumma")]
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
        /// Начало желаемого времени доставки
        /// </summary>
        [XmlAttribute("shippingtimefrom")]
        public string DeiveryTimeFrom { get; set; }

        /// <summary>
        /// Окончание желаемого времени доставки
        /// </summary>
        [XmlAttribute("shippingtimefor")]
        public string DeiveryTimeTo { get; set; }

        /// <summary>
        /// Код услуги доставки
        /// </summary>
        [XmlAttribute("service")]
        public EnCourierService Service { get; set; }

        /// <summary>
        /// Количество мест
        /// </summary>
        [XmlAttribute("seats")]
        public int Seats { get; set; }

        /// <summary>
        /// УИД офиса транспортной компании
        /// </summary>
        [XmlAttribute("tc_office")]
        public string OfficeId { get; set; }

        /// <summary>
        /// Юр. лицо
        /// </summary>
        [XmlIgnore]
        public EnTypeRecipient? TypeRecipient { get; set; }

        [XmlAttribute("tc_typerecipient")]
        public string TypeRecipientForXml
        {
            get { return TypeRecipient.HasValue ? ((XmlEnumAttribute)typeof(EnTypeRecipient).GetMember(TypeRecipient.ToString())[0].GetCustomAttributes(typeof(XmlEnumAttribute), false).First()).Name : null; }
            set { }
        }

        /// <summary>
        /// Почтовый индекс получателя
        /// </summary>
        [XmlAttribute("tc_postcode")]
        public string Index { get; set; }

        /// <summary>
        /// Адрес получателя
        /// </summary>
        [XmlAttribute("tc_address")]
        public string AddressForTransportCompany { get; set; }

        /// <summary>
        /// ФИО получателя
        /// </summary>
        [XmlAttribute("tc_fullname")]
        public string BuyerForTransportCompany { get; set; }

        /// <summary>
        /// Телефон получателя
        /// </summary>
        [XmlAttribute("tc_phone")]
        public string PhoneForTransportCompany { get; set; }

        /// <summary>
        /// Паспортные данные получателя
        /// </summary>
        [XmlAttribute("tc_passport")]
        public string Passport { get; set; }

        /// <summary>
        /// Организация
        /// </summary>
        [XmlAttribute("tc_organization")]
        public string Organization { get; set; }

        /// <summary>
        /// ИНН организации
        /// </summary>
        [XmlAttribute("tc_INN")]
        public string Inn { get; set; }

        /// <summary>
        /// КПП организации
        /// </summary>
        [XmlAttribute("tc_KPP")]
        public string Kpp { get; set; }

        [XmlElement("good")]
        public List<GrastinProduct> Products { get; set; }
    }

    public enum EnTypeRecipient
    {
        /// <summary>
        /// Физическое лицо
        /// </summary>
        [Localize("Физическое лицо")]
        [XmlEnum(Name = "1")]
        Individual = 1,

        /// <summary>
        /// Юридическое лицо
        /// </summary>
        [Localize("Юридическое лицо")]
        [XmlEnum(Name = "2")]
        Organization = 2
    }

    public enum EnCourierService
    {
        /// <summary>
        /// Доставка без оплаты
        /// </summary>
        [Localize("Доставка без оплаты")]
        [XmlEnum(Name = "1")]
        DeliverWithoutPayment = 1,

        /// <summary>
        /// Доставка с оплатой
        /// </summary>
        [Localize("Доставка с оплатой")]
        [XmlEnum(Name = "2")]
        DeliveryWithPayment = 2,

        /// <summary>
        /// Доставка с кассовым обслуживанием
        /// </summary>
        [Localize("Доставка с кассовым обслуживанием")]
        [XmlEnum(Name = "3")]
        DeliveryWithCashServices = 3,

        /// <summary>
        /// Обмен/забор товара
        /// </summary>
        [Localize("Обмен/забор товара")]
        [XmlEnum(Name = "4")]
        ExchangeReturnOfGoods = 4,

        /// <summary>
        /// Самовывоз без оплаты
        /// </summary>
        [Localize("Самовывоз без оплаты")]
        [XmlEnum(Name = "5")]
        PickupWithoutPaying = 5,

        /// <summary>
        /// Самовывоз с оплатой
        /// </summary>
        [Localize("Самовывоз с оплатой")]
        [XmlEnum(Name = "6")]
        PickupWithPayment = 6,

        /// <summary>
        /// Самовывоз с кассовым обслуживанием
        /// </summary>
        [Localize("Самовывоз с кассовым обслуживанием")]
        [XmlEnum(Name = "7")]
        PickupWithCashServices = 7,

        /// <summary>
        /// Большой доставка без оплаты
        /// </summary>
        [Localize("Большой доставка без оплаты")]
        [XmlEnum(Name = "8")]
        GreatDeliveryWithoutPayment = 8,

        /// <summary>
        /// Большой доставка и забор наличных
        /// </summary>
        [Localize("Большой доставка и забор наличных")]
        [XmlEnum(Name = "9")]
        GreatDeliveryWithPayment = 9,

        /// <summary>
        /// Большой доставка с кассовым обслуживанием
        /// </summary>
        [Localize("Большой доставка с кассовым обслуживанием")]
        [XmlEnum(Name = "10")]
        GreatDeliveryWithCashServices = 10,

        /// <summary>
        /// Обмен/забор товара на самовывозе
        /// </summary>
        [Localize("Обмен/забор товара на самовывозе")]
        [XmlEnum(Name = "11")]
        ExchangeReturnOfGoodsOnPickup = 11,

        /// <summary>
        /// Транспортная компания
        /// </summary>
        [Localize("Транспортная компания")]
        [XmlEnum(Name = "12")]
        TransportCompany = 12,

        ///// <summary>
        ///// Почтовая доставка
        /////// </summary>
        //[Localize("Почтовая доставка")]
        //[XmlEnum(Name = "13")]
        //PostalShipping = 13,

        ///// <summary>
        ///// Посылка онлайн
        ///// </summary>
        //[Localize("Посылка онлайн")]
        //[XmlEnum(Name = "14")]
        //PackageOnline = 14,

        ///// <summary>
        ///// Курьер онлайн
        ///// </summary>
        //[Localize("Курьер онлайн")]
        //[XmlEnum(Name = "15")]
        //CourierOnline = 15,

        /// <summary>
        /// Самовывоз с оплатой картой
        /// </summary>
        [Localize("Самовывоз с оплатой картой")]
        [XmlEnum(Name = "16")]
        PickupWithCreditCard = 16,

        ///// <summary>
        ///// Забор товара у поставщика (закупки)
        ///// </summary>
        //[Localize("Забор товара у поставщика (закупки)")]
        //[XmlEnum(Name = "17")]
        //TakingOfGoodsFromSupplier = 17,

        ///// <summary>
        ///// Забор БОЛЬШОЙ товара у поставщика  (закупки)
        ///// </summary>
        //[Localize("Забор БОЛЬШОЙ товара у поставщика  (закупки)")]
        //[XmlEnum(Name = "18")]
        //TakingOfBigGoodsFromSupplier = 18,

        /// <summary>
        /// Доставка с оплатой картой
        /// </summary>
        [Localize("Доставка с оплатой картой")]
        [XmlEnum(Name = "19")]
        DeliverWithCreditCard = 19,

    }
}
