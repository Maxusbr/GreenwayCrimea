using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Module.YaBuying.Domain
{
    /* 
     * http://api.yandex.ru/market/partner/doc/dg/reference/post-order-accept.xml
     * 
     */

    public class YaMarketOrderRequest
    {
        public YaMarketOrder order { get; set; }
    }

    public class YaMarketOrder
    {
        public string id { get; set; }

        /// <summary>
        /// RUR — российский рубль; UAH — украинская гривна
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// PREPAID — предоплата;POSTPAID — постоплата при получении заказа. Необязательный параметр.
        /// </summary>
        public string paymentType { get; set; }

        /// <summary>
        /// Возможные значения для типа оплаты PREPAID:
        /// YANDEX — оплата при оформлении (только для России); SHOP_PREPAID — предоплата напрямую магазину (только для Украины).
        /// Возможные значения для типа оплаты POSTPAID:
        /// CASH_ON_DELIVERY — наличный расчет при получении заказа;CARD_ON_DELIVERY — оплата банковской картой при получении заказа.
        /// Необязательный параметр.
        /// </summary>
        public string paymentMethod { get; set; }


        public bool fake { get; set; }
        public List<YaMarketOrderItem> items { get; set; }
        public string notes { get; set; }
        public YaMarketOrderDelivery delivery { get; set; }
    }

    public class YaMarketOrderItem
    {
        public int feedId { get; set; }
        public string offerId { get; set; }
        public string feedСategoryId { get; set; }
        public string offerName { get; set; }
        public float price { get; set; }
        public float count { get; set; }
    }

    public class YaMarketOrderDelivery
    {
        public string id { get; set; }

        public string type { get; set; }

        public string serviceName { get; set; }
        
        //[JsonIgnore]
        public float? price { get; set; }

        public YaMarketDate dates { get; set; }

        public YaMarketRegion region { get; set; }

        public YaMarketAddress address { get; set; }

        public int outletId { get; set; }
    }

    public class YaMarketOrderResponse
    {
        public YaMarketOrderAccept order { get; set; }
    }

    public class YaMarketOrderAccept
    {
        public bool accepted { get; set; }

        /// <summary>
        /// Идентификатор заказа, присвоенный магазином. Указывается, если заказ принят.
        /// </summary>
        public string id { get; set; }

        ///// <summary>
        ///// Причина отклонения заказа. Указывается в случае отклонения заказа (значение false атрибута accepted). Возможные значения:
        ///// OUT_OF_DATE — информация по заказу устарела либо магазин не отправляет заказы в указанный регион.
        ///// </summary>
        //public string reason { get; set; }
    }


    public class YaOrder
    {
        public int MarketOrderId { get; set; }
        public int OrderId { get; set; }

        public string Status { get; set; }
    }
}