using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Module.MoySklad.Domain
{
    public class MoySkladExportService
    {
        public static IEnumerable<MoySkladExportFeedCsvProduct> GetProducts()
        {
            return SQLDataAccess.ExecuteReadIEnumerable(
                "SELECT * FROM [Catalog].[Product] " +
                "Left Join [Catalog].[Photo] ON [Photo].[ObjId] = [Product].[ProductID] AND Type = 'Product' AND Photo.[Main] = 1 ",
                CommandType.Text,
                 reader =>
                 {
                     var product = ProductService.GetProductFromReader(reader);

                     var categoryId = ProductService.GetFirstCategoryIdByProductId(product.ProductId);
                     var categories = "";
                     if (categoryId > 0)
                     {
                         foreach (var cat in CategoryService.GetParentCategories(categoryId).Reverse())
                         {
                             categories += (categories != "" ? "/" : "") + cat.Name.Replace("/", "#/");
                         }
                     }

                     string currencyCode = "";
                     var currency = CurrencyService.GetCurrency(product.CurrencyID, true);
                     if (currency != null && currency.Iso3 != "RUB")
                         currencyCode = currency.Symbol;


                     var productCsv = new MoySkladExportFeedCsvProduct
                     {
                         ProductId = product.ProductId,
                         //ArtNo = product.ArtNo,
                         Name = product.Name,
                         UrlPath = product.UrlPath,
                         Enabled = product.Enabled ? "+" : "-",
                         Unit = product.Unit,
                         ShippingPrice = (product.ShippingPrice ?? 0).ToString("F2"),
                         Discount = product.Discount.Percent.ToString("F2"),
                         Weight = product.Weight.ToString("F2"),
                         Size = product.Length + "x" + product.Width + "x" + product.Height,
                         BriefDescription = product.BriefDescription,
                         Description = product.Description,
                         Currency = currencyCode,
                         OrderByRequest = product.AllowPreOrder ? "+" : "-",

                         Category =  categories,
                         Offers = product.Offers,
                         HasMultiOffer = product.HasMultiOffer
                     };

                     var offer = product.Offers.FirstOrDefault(x => x.Main) ?? new Offer();

                     productCsv.ArtNo = offer.ArtNo ?? string.Empty;

                     var externalCode = MoySklad.GetMoyskladIdByProductId(productCsv.ProductId);
                     if (!string.IsNullOrWhiteSpace(externalCode))
                     {
                         productCsv.ExternalCode = externalCode;
                     }
                     else
                     {
                         // В Внешний код идут только те артикулы, которые уникальны для проудукта и офера и офер один
                         var offerByProductArtno = OfferService.GetOffer(product.ArtNo);
                         productCsv.ExternalCode = offerByProductArtno == null || !product.HasMultiOffer ? product.ArtNo : "";
                     }

                     productCsv.Price = offer.BasePrice.ToString("F2");
                     productCsv.PurchasePrice = offer.SupplyPrice.ToString("F2");
                     productCsv.Amount = offer.Amount.ToString(CultureInfo.InvariantCulture);
                     productCsv.MultiOffer = string.Empty;

                     return productCsv;
                 });
        }
    }
}
