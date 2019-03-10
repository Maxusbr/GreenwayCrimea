using System;
using System.IO;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using AdvantShop.Shipping.Sdek;

namespace AdvantShop.Web.Admin.Handlers.Orders.Shippings
{
    public class SdekOrderPrintForm
    {
        private readonly int _orderId;

        public SdekOrderPrintForm(int orderId)
        {
            _orderId = orderId;
        }

        public Tuple<string, string> Execute()
        {
            var order = OrderService.GetOrder(_orderId);
            if (order == null)
                return null;
            
            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shippingMethod.ShippingType != "Sdek")
                return null;
            
            try
            {
                var fileName = (new Sdek(shippingMethod, null)).PrintFormOrder(order);

                var fullFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp, fileName);

                if (!File.Exists(fullFilePath))
                    return null;

                return new Tuple<string, string>(fullFilePath, fileName);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return null;
        }
    }
}
