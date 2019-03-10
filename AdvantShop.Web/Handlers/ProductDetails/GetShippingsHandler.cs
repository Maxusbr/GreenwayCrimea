using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Orders;
using AdvantShop.Repository;
using AdvantShop.Repository.Currencies;
using AdvantShop.Shipping;
using AdvantShop.ViewModel.ProductDetails;
using System.Web;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Handlers.ProductDetails
{
    public class GetShippingsHandler
    {
        private readonly int _offerId;
        private readonly float _amount;
        private readonly string _customOptions;
        private readonly string _zip;

        public GetShippingsHandler(int offerId, float amount, string customOptions, string zip)
        {
            _offerId = offerId;
            _amount = amount;
            _customOptions = HttpUtility.UrlDecode(customOptions);
            _zip = zip;
        }

        public ShippingsViewModel Get()
        {
            if (OfferService.GetOffer(_offerId) == null ||
                SettingsDesign.ShowShippingsMethodsInDetails == SettingsDesign.eShowShippingsInDetails.Never)
            {
                return null;
            }

            var tempShopCart = new ShoppingCart
            {
                new ShoppingCartItem()
                {
                    Amount = _amount,
                    ShoppingCartType = ShoppingCartType.ShoppingCart,
                    OfferId = _offerId,
                    AttributesXml = _customOptions
                }
            };

            var currentZone = IpZoneContext.CurrentZone;
            //if (currentZone.Zip != _zip)
            //{
            //    currentZone.Zip = _zip;
            //    IpZoneContext.SetCustomerCookie(currentZone);
            //}
            var preOrder = new PreOrder()
            {
                CountryDest = currentZone.CountryName,
                CityDest = currentZone.City,
                RegionDest = currentZone.Region,
                Items = tempShopCart.Select(shpItem => new PreOrderItem(shpItem)).ToList(),
                ZipDest = _zip,
                Currency = CurrencyService.CurrentCurrency,
            };

            var shippingManager = new ShippingManager(preOrder, true);
            var shippingRates = shippingManager.GetOptions();

            var showZip = ShippingMethodService.GetAllShippingMethods(true).Where(x => x.ShowInDetails && x.DisplayIndex).Any();
            if (shippingRates.Count == 0)
            {
                if (_zip.IsNotEmpty() || !showZip)
                    return null;
                return new ShippingsViewModel()
                {
                    AdvancedObj = new { ShowZip = true }
                };
            }

            var model = new ShippingsViewModel()
            {
                Shippings =
                    shippingRates.OrderBy(item => item.Rate).Take(SettingsDesign.ShippingsMethodsInDetailsCount)
                        .Select(x => new ShippingItemModel()
                        {
                            Name = x.NameRate ?? x.Name,
                            DeliveryTime = x.DeliveryTime,
                            Rate = x.Rate == 0 ? x.ZeroPriceMessage : x.Rate.FormatPrice()
                        }).ToList(),
                AdvancedObj = null
                //AdvancedObj = new
                //{
                //    ShowZip = currentZone.Zip.IsNotEmpty() && showZip
                //}
            };

            return model;
        }
    }
}