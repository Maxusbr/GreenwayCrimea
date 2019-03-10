using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Web.Admin.Models.OrdersEdit;

namespace AdvantShop.Web.Admin.Handlers.Orders
{
    public class GetOrderItems
    {
        private readonly int _orderId;
        private readonly string _sorting;
        private readonly string _sortingType;
        private readonly UrlHelper _urlHelper;

        public GetOrderItems(int orderId, string sorting, string sortingType)
        {
            _orderId = orderId;
            _sorting = sorting;
            _sortingType = sortingType;
            _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        public List<OrderItemModel> Execute()
        {
            var orderItems = new List<OrderItemModel>();

            var currency = OrderService.GetOrderCurrency(_orderId) ?? CurrencyService.CurrentCurrency;

            foreach (var item in OrderService.GetOrderItems(_orderId))
            {
                var p = item.ProductID != null ? ProductService.GetProduct(item.ProductID.Value) : null;

                var orderItem = new OrderItemModel()
                {
                    OrderItemId = item.OrderItemID,
                    OrderId = item.OrderID,
                    ImageSrc = item.Photo.ImageSrcSmall(),
                    ArtNo = item.ArtNo,
                    Name = item.Name,
                    ProductLink = p != null ? _urlHelper.Action("Edit", "Product", new { id = p.ProductId }) : null,

                    Color = !string.IsNullOrEmpty(item.Color) ? SettingsCatalog.ColorsHeader + ": " + item.Color : "",
                    Size = !string.IsNullOrEmpty(item.Size) ? SettingsCatalog.SizesHeader + ": " + item.Size : "",
                    CustomOptions = item.SelectedOptions != null ? RenderCustomOptions(item.SelectedOptions) : null,

                    Price = item.Price,
                    Amount = item.Amount,
                    Cost = (item.Price*item.Amount).FormatPrice(currency),
                    Width = item.Width,
                    Height = item.Height,
                    Length = item.Length
                };

                var offer = OfferService.GetOffer(item.ArtNo);
                if (offer == null || item.Amount > offer.Amount)
                {
                    var amount = offer == null ? 0 : offer.Amount;

                    orderItem.Available = false;
                    orderItem.AvailableText = string.Format(LocalizationService.GetResource("Admin.Orders.GetOrderItems.AvailableLimit"), amount);
                }
                else if (item.Amount <= offer.Amount)
                {
                    orderItem.Available = true;
                    orderItem.AvailableText = LocalizationService.GetResource("Admin.Orders.GetOrderItems.Available");
                }


                orderItems.Add(orderItem);
            }

            if (!string.IsNullOrEmpty(_sorting))
            {
                bool orderBy = !(_sortingType != null && _sortingType.ToLower() == "desc");
                    
                switch (_sorting.ToLower())
                {
                    case "name":
                        orderItems = orderBy
                            ? orderItems.OrderBy(x => x.Name).ToList()
                            : orderItems.OrderByDescending(x => x.Name).ToList();
                        break;

                    case "price":
                        orderItems = orderBy
                            ? orderItems.OrderBy(x => x.Price).ToList()
                            : orderItems.OrderByDescending(x => x.Price).ToList();
                        break;

                    case "amount":
                        orderItems = orderBy
                            ? orderItems.OrderBy(x => x.Amount).ToList()
                            : orderItems.OrderByDescending(x => x.Amount).ToList();
                        break;

                    case "available":
                        orderItems = orderBy
                            ? orderItems.OrderBy(x => x.Available).ToList()
                            : orderItems.OrderByDescending(x => x.Available).ToList();
                        break;

                    case "cost":
                        orderItems = orderBy
                            ? orderItems.OrderBy(x => x.Price * x.Amount).ToList()
                            : orderItems.OrderByDescending(x => x.Price * x.Amount).ToList();
                        break;
                }
            }

            return orderItems;
        }

        public static string RenderCustomOptions(List<EvaluatedCustomOptions> evlist)
        {
            if (evlist == null || evlist.Count == 0)
                return "";

            var html = new StringBuilder("");
            foreach (var ev in evlist)
            {
                html.AppendFormat(
                    "<div class=\"orderitem-option\"><span class=\"orderitem-option-name\">{0}:</span> <span class=\"orderitem-option-value\">{1} {2}</span></div>",
                    ev.CustomOptionTitle, ev.OptionTitle, ev.FormatPrice);
            }
            html.Append("");

            return html.ToString();
        }
    }
}
