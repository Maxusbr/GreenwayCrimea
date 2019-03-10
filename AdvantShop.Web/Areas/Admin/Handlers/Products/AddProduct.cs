using System.Collections.Generic;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Repository.Currencies;
using AdvantShop.Trial;
using AdvantShop.Saas;
using AdvantShop.Core;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.Products
{
    public class AddProduct : AbstractCommandHandler<int>
    {
        private string _name;
        private int _categoryId;

        public AddProduct(string name, int categoryId)
        {
            _name = name ?? string.Empty;
            _categoryId = categoryId;
        }

        protected override void Validate()
        {
            if (string.IsNullOrWhiteSpace(_name))
            {
                throw new BlException("Укажите название товара");
            }

            if (SaasDataService.IsSaasEnabled && ProductService.GetProductsCount() >= SaasDataService.CurrentSaasData.ProductsCount)
            {
                throw new BlException(T("Admin.Product.AddProductHandler.Error.ExceedingSaasValue"));
            }
        }

        protected override int Handle()
        {
            var productId = 0;
            
            var url = StringHelper.TransformUrl(StringHelper.Translit(_name));

            var product = new Product()
            {
                Name = _name,
                ArtNo = null,
                UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Product, string.IsNullOrEmpty(url) ? _name : url),
                Enabled = true,
                CurrencyID = CurrencyService.CurrentCurrency.CurrencyId,
                Multiplicity = 1,
                Offers = new List<Offer>
                    {
                        new Offer()
                        {
                            ArtNo = null,
                            Amount = 1,
                            BasePrice = 0,
                            Main = true,
                        }
                    },
                Meta = null,
                ModifiedBy = CustomerContext.CustomerId.ToString()
            };

            productId = ProductService.AddProduct(product, true);

            if (productId != 0 && _categoryId != 0 && _categoryId != CategoryService.DefaultNonCategoryId)
            {
                ProductService.EnableDynamicProductLinkRecalc();
                ProductService.AddProductLink(productId, _categoryId, 0, true);
                ProductService.DisableDynamicProductLinkRecalc();
                ProductService.SetProductHierarchicallyEnabled(product.ProductId);
            }

            TrialService.TrackEvent(TrialEvents.AddProduct, string.Empty);
            TrialService.TrackEvent(ETrackEvent.Trial_AddProduct);

            return productId;
        }
    }
}
