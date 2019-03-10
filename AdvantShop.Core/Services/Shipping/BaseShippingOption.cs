using AdvantShop.Orders;

namespace AdvantShop.Shipping
{
    public class BaseShippingOption : AbstractShippingOption
    {
        public BaseShippingOption()
        {
        }

        public BaseShippingOption(ShippingMethod method)
        {
            MethodId = method.ShippingMethodId;
            Name = method.Name;
            Desc = method.Description;
            DisplayCustomFields = method.DisplayCustomFields;
            DisplayIndex = method.DisplayIndex;
            IconName = method.IconFileName != null
                ? ShippingIcons.GetShippingIcon(method.ShippingType, method.IconFileName.PhotoName, method.Name)
                : null;
            ShowInDetails = method.ShowInDetails;
            ZeroPriceMessage = method.ZeroPriceMessage;
            TaxType = method.TaxType;
            ShippingType = method.ShippingType;
        }

        public virtual void Update(BaseShippingOption option)
        {
           
        }

        public virtual OrderPickPoint GetOrderPickPoint()
        {
            return null;
        }

        public bool IsCustom { get; set; }
    }
}