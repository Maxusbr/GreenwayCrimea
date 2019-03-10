using System.Collections.Generic;

namespace AdvantShop.Module.YaBuying.Domain
{
    /* 
     * http://api.yandex.ru/market/partner/doc/dg/reference/post-order-status.xml
     * 
     */

    public class YaMarketOrderStatusRequest
    {
        public YaMarketOrderStatus order { get; set; }
    }

    public class YaMarketOrderStatus
    {
        public string id { get; set; }

        /// <summary>
        /// UNPAID — заказ оформлен, но еще не оплачен (если выбрана оплата при оформлении)
        /// PROCESSING — заказ можно выполнять
        /// CANCELLED — заказ отменен.
        /// </summary>
        public string status { get; set; }


        /// <summary>
        /// Дополнительный параметр для нового статуса заказа (подстатус). 
        /// Если для статуса не существует подстатусов, то поле не выдается. Возможные подстатусы для статуса CANCELLED:
        /// RESERVATION_EXPIRED — покупатель не завершил оформление зарезервированного заказа вовремя;
        /// USER_NOT_PAID — покупатель не оплатил заказ (для типа оплаты PREPAID);
        /// USER_UNREACHABLE — не удалось связаться с покупателем;
        /// USER_CHANGED_MIND — покупатель отменил заказ по собственным причинам;
        /// USER_REFUSED_DELIVERY — покупателя не устраивают условия доставки;
        /// USER_REFUSED_PRODUCT — покупателю не подошел товар;
        /// SHOP_FAILED — магазин не может выполнить заказ;
        /// USER_REFUSED_QUALITY — покупателя не устраивает качество товара;
        /// REPLACING_ORDER — покупатель изменяет состав заказа;
        /// PROCESSING_EXPIRED — магазин не обработал заказ вовремя.
        /// </summary>
        public string substatus { get; set; }

        public string creationDate { get; set; }
        
        /// <summary>
        /// RUR — российский рубль; UAH — украинская гривна
        /// </summary>
        public string currency { get; set; }


        /// <summary>
        /// Общая сумма заказа в валюте заказа без учета доставки
        /// </summary>
        public string itemsTotal { get; set; }

        /// <summary>
        /// бщая сумма заказа в валюте заказа с учетом доставки
        /// </summary>
        public string total { get; set; }

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

        public YaMarketBuyer buyer { get; set; }
        
    }

    public class YaMarketBuyer
    {
        public string id { get; set; }

        public string firstName { get; set; }

        public string phone { get; set; }

        public string email { get; set; }

        public string lastName { get; set; }

        public string middleName { get; set; }

    }

    public class YaStatus
    {
        public YaStatusOrder order { get; set; }
    }

    public class YaStatusOrder
    {
        public string status { get; set; }
        public string substatus { get; set; }
    }
}