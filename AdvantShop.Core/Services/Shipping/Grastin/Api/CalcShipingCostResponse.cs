using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    [XmlRoot(ElementName = "Orders")]
    public class CalcShipingCostResponse
    {
        [XmlElement("Order")]
        public List<CostResponse> CostsResponse { get; set; }
    }

    [Serializable]
    public class CostResponse
    {
        [XmlElement("number")]
        public string Number { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("error")]
        public string Error { get; set; }

        /// <summary>
        /// Стоимость доставки
        /// </summary>
        [XmlIgnore]
        public float ShippingCost { get; set; }

        [XmlElement("shippingcost")]
        public string ShippingCostForXml {
            get { return ShippingCost.ToString("F2", CultureInfo.InvariantCulture); }
            set { ShippingCost = float.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Стоимость доставки за оплачиваемое расстояние
        /// </summary>
        [XmlIgnore]
        public float ShippingCostDistance { get; set; }

        [XmlElement("shippingcostdistance")]
        public string ShippingCostDistanceForXml
        {
            get { return ShippingCostDistance.ToString("F2", CultureInfo.InvariantCulture); }
            set { ShippingCostDistance = float.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Комиссия
        /// </summary>
        [XmlIgnore]
        public float Commission { get; set; }

        [XmlElement("commission")]
        public string CommissionForXml
        {
            get { return Commission.ToString("F2", CultureInfo.InvariantCulture); }
            set { Commission = float.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Страховой запас
        /// </summary>
        [XmlIgnore]
        public float SafetyStock { get; set; }

        [XmlElement("safetystock")]
        public string SafetyStockForXml
        {
            get { return SafetyStock.ToString("F2", CultureInfo.InvariantCulture); }
            set { SafetyStock = float.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Дополнительная стоимость доставки
        /// </summary>
        [XmlIgnore]
        public float AdditionalShippingCosts { get; set; }

        [XmlElement("additionalshippingcosts")]
        public string AdditionalShippingCostsForXml
        {
            get { return AdditionalShippingCosts.ToString("F2", CultureInfo.InvariantCulture); }
            set { AdditionalShippingCosts = float.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Стоимость обработки заказа
        /// </summary>
        [XmlIgnore]
        public float OrderProcessing { get; set; }

        [XmlElement("orderprocessing")]
        public string OrderProcessingForXml
        {
            get { return OrderProcessing.ToString("F2", CultureInfo.InvariantCulture); }
            set { OrderProcessing = float.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Комиссия за быстрые деньги
        /// </summary>
        [XmlIgnore]
        public float CommissionFastMoney { get; set; }

        [XmlElement("commissionfastmoney")]
        public string CommissionFastMoneyForXml
        {
            get { return CommissionFastMoney.ToString("F2", CultureInfo.InvariantCulture); }
            set { CommissionFastMoney = float.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture); }
        }
    }
}
