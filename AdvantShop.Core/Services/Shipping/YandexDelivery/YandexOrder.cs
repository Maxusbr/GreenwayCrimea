using System.Collections.Generic;

namespace AdvantShop.Shipping.ShippingYandexDelivery
{
    public class YandexOrder
    {
        public string SecreKey { get; set; }
        public int ClientId { get; set; }
        public int SenderId { get; set; }
        public string recipient_first_name { get; set; }
        public string recipient_middle_name { get; set; }
        public string recipient_last_name { get; set; }
        public string recipient_phone { get; set; }
        public string recipient_email { get; set; }
        public string recipient_comment { get; set; }

        public int order_weight { get; set; }
        public int order_length { get; set; }
        public int order_width { get; set; }
        public int order_height { get; set; }
        public string order_num { get; set; }

        public int order_delivery_cost { get; set; }

        public string deliverypoint_city { get; set; }
        public int delivery_delivery { get; set; }

        public int delivery_direction { get; set; }
        public int tariff_id { get; set; }

        public List<YandexOrderitem> order_items { get; set; }
    }

    public class YandexOrderitem
    {
        public string orderitem_article { get; set; }
        public string orderitem_name { get; set; }
        public string orderitem_quantity { get; set; }
        public string orderitem_cost { get; set; }

    }
}