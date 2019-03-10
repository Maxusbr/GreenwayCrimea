using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Orders;
using AdvantShop.ViewModel.Compare;

namespace AdvantShop.Handlers.Compare
{
    public class CompareProductsHandler
    {
        
        public CompareProductsViewModel Get()
        {
            var products = new List<CompareProductModel>();

            foreach (var item in ShoppingCartService.CurrentCompare)
            {
                var product = item.Offer.Product;

                var productModel = new CompareProductModel()
                {
                    ProductId = product.ProductId,
                    OfferId = item.OfferId,
                    UrlPath = product.UrlPath,
                    Name = product.Name,
                    ArtNo = product.ArtNo,
                    BriefDescription = product.BriefDescription,
                    Enabled = product.Enabled,
                    Bestseller = product.BestSeller,
                    Sales = product.OnSale,
                    Recomend = product.Recomended,
                    New = product.New,
                    AllowPreorder = product.AllowPreOrder,
                    Amount = item.Offer.Amount,
                    Multiplicity = product.Multiplicity,
                    BasePrice = item.Offer.BasePrice,
                    Photo = item.Offer.Photo,
                    CurrencyValue = product.Currency.Rate,
                    Discount = item.Discount.Percent,
                    DiscountAmount = item.Discount.Amount
                };

                products.Add(productModel);
            }

            var model = new CompareProductsViewModel(products, PropertyService.GetPropertyNamesByCompareCart());

            return model;
        }
    }
}