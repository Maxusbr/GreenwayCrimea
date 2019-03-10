namespace AdvantShop.Shipping.ShippingYandexDelivery
{
    public class YandexDeliveryOption : BaseShippingOption
    {
        public string Direction { get; set; }
        public new string DeliveryId { get; set; }
        public string TariffId { get; set; }

        public YandexDeliveryOption()
        {
        }

        public override string Id
        {
            get { return MethodId + "_" + (Name + MethodId + TariffId).GetHashCode(); }
        }

        public YandexDeliveryOption(ShippingMethod method, YandexDeliveryListItem item)
            : base(method)
        {
            Name = item.Delivery.name.ToLower().Contains("почта") ? item.Delivery.name + " (" + item.TariffName + ")" :  "Курьер " + item.Delivery.name;
            Rate = item.CostWithRules;
            DeliveryTime = !item.Days.Contains("дн") ? item.Days + " дн" : "";
            DeliveryId = item.Delivery.id;
            Direction = item.Direction;
            TariffId = item.TariffId;
        }
    }
}