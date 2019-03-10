using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    public abstract class GrastinBaseObject
    {
        [XmlElement("API")]
        public string ApiKey { get; set; }

        public abstract EnApiMethod Method { get; set; }
    }

    public enum EnApiMethod
    {
        [XmlEnum(Name = "selfpickup")]
        [EnumMember(Value = "selfpickup")]
        SelfPickup,

        [XmlEnum(Name = "newordercourier")]
        [EnumMember(Value = "newordercourier")]
        NewOrderCourier,

        [XmlEnum(Name = "newordermail")]
        [EnumMember(Value = "newordermail")]
        NewOrderRussianPost,

        [XmlEnum(Name = "neworderboxberry")]
        [EnumMember(Value = "neworderboxberry")]
        NewOrderBoxberry,

        [XmlEnum(Name = "neworderhermes")]
        [EnumMember(Value = "neworderhermes")]
        NewOrderHermes,

        [XmlEnum(Name = "warehouse")]
        [EnumMember(Value = "warehouse")]
        Warehouse,

        [XmlEnum(Name = "boxberryselfpickup")]
        [EnumMember(Value = "boxberryselfpickup")]
        BoxberrySelfPickup,

        [XmlEnum(Name = "boxberrypostcode")]
        [EnumMember(Value = "boxberrypostcode")]
        BoxberryPostCode,

        [XmlEnum(Name = "hermesselfpickup")]
        [EnumMember(Value = "hermesselfpickup")]
        HermesSelfPickup,

        [XmlEnum(Name = "tcofficelist")]
        [EnumMember(Value = "tcofficelist")]
        TransportCompanyOfficeList,

        [XmlEnum(Name = "deliveryregion")]
        [EnumMember(Value = "deliveryregion")]
        DeliveryRegions,

        [XmlEnum(Name = "requestforintake")]
        [EnumMember(Value = "requestforintake")]
        RequestForIntake,

        [XmlEnum(Name = "contractlist")]
        [EnumMember(Value = "contractlist")]
        GetContractList,

        [XmlEnum(Name = "printactofreceiving")]
        [EnumMember(Value = "printactofreceiving")]
        GetAct,

        [XmlEnum(Name = "printmark")]
        [EnumMember(Value = "printmark")]
        GetMark,

        [XmlEnum(Name = "calcshipingcost")]
        [EnumMember(Value = "calcshipingcost")]
        CalcShipingCost,

        [XmlEnum(Name = "orderinformation")]
        [EnumMember(Value = "orderinformation")]
        OrderInfo,
    }
}
