using System.Collections.Generic;
using System.Linq;
using AdvantShop.Orders;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.ViewModel.Catalog;

namespace AdvantShop.Handlers.Catalog
{
    public class WishListHandler
    {
        public ProductViewModel Get()
        {

            var products = new List<ProductModel>();

            foreach (var item in ShoppingCartService.CurrentWishlist)
            {
                var product = item.Offer.Product;
                var photos = product.ProductPhotos;
                
                var productModel = new ProductModel()
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
                    Discount = product.Discount.Percent,
                    DiscountAmount = product.Discount.Amount,
                    Photo = item.Offer.Photo,
                    CurrencyValue = product.Currency.Rate,
                    CountPhoto = item.Offer.ColorID != null ? photos.Count(x => x.ColorID == item.Offer.ColorID) : photos.Count,
                };

                products.Add(productModel);
            }

            var model = new ProductViewModel(products)
            {
               DisplayPhotoPreviews = false           
            };

            return model;
        }
    }
}