using System;
using System.Linq;
using System.Text;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Module.AbandonedCarts.Domain
{
    [Serializable]
    public class AbandonedCartTemplate
    {
        private string _customerEmail;
        private string _firstName;
        private string _basket;
        private string _basketUrl;
        
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int SendingTime { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool Active { get; set; }


        public void Register(Customer customer, ShoppingCart shoppingCart)
        {
            _customerEmail = customer.EMail;
            _firstName = customer.FirstName;
            _basket = RenderBasketHtml(customer, shoppingCart);
            _basketUrl = SettingsMain.SiteUrl.Trim('/') + "/cart?products=" +
                         shoppingCart.Select(p => p.OfferId + "-" + p.Amount).AggregateString(";") +
                         "&utm_source=email&utm_medium=abandonedcart&utm_campaign=email_" + this.Id;
        }

        public void BuildMail()
        {
            var logo = SettingsMain.LogoImageName.IsNotEmpty()
                           ? String.Format("<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" />",
                                           FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, false),
                                           SettingsMain.ShopName)
                           : string.Empty;

            Subject = Subject != null ? FormatString(Subject) : string.Empty;
            Body = Body != null ? FormatString(Body).Replace("#LOGO#", logo) : string.Empty;
        }

        private string FormatString(string formatedStr)
        {
            formatedStr = formatedStr.Replace("#SHOPNAME#", SettingsMain.ShopName);
            formatedStr = formatedStr.Replace("#SHOPURL#", SettingsMain.SiteUrl);
            formatedStr = formatedStr.Replace("#BASKETURL#", _basketUrl);
            formatedStr = formatedStr.Replace("#EMAIL#", _customerEmail);
            formatedStr = formatedStr.Replace("#NAME#", _firstName);
            formatedStr = formatedStr.Replace("#PRODUCTS#", _basket);

            return formatedStr;
        }

        private string RenderBasketHtml(Customer customer, ShoppingCart shoppingCart)
        {
            if (!shoppingCart.HasItems)
                return string.Empty;

            var sb = new StringBuilder();

            //TODO РЕСУРСЫ  
            sb.Append("<table style=\'width:100%;\' cellspacing=\'0\' cellpadding=\'2\'>");
            sb.Append("<tr>");
            sb.AppendFormat("<td style=\'width:100px; text-align: center;\'>&nbsp;</td>");
            sb.AppendFormat("<td>{0}</td>", /*Resource.Client_OrderConfirmation_Name*/ "Наименование");
            sb.AppendFormat("<td style=\'width:90px; text-align:center;\'>{0}</td>", /*Resource.Client_OrderConfirmation_Price*/ "Цена");
            sb.AppendFormat("<td style=\'width:80px; text-align:center;\'>{0}</td>", /*Resource.Client_OrderConfirmation_Count*/ "Количество");
            sb.AppendFormat("<td style=\'width:90px; text-align:center;\'>{0}</td>",/* Resource.Client_OrderConfirmation_Cost*/ "Стоимость");
            sb.Append("</tr>");
            
            foreach (var item in shoppingCart.Where(x => x.Offer != null && x.Offer.Product != null))
            {
                var photo = item.Offer.Photo;

                float price = 0;

                if (item.IsGift)
                {
                    price = 0;
                }
                else
                {
                    //var discount = PriceService.GetFinalDiscount(item.Offer.Product.Discount, customer.CustomerGroup, item.Offer.ProductId);
                    var customOptionPrice = CustomOptionsService.GetCustomOptionPrice(item.Offer.RoundedPrice, item.AttributesXml, item.Offer.Product.Currency.Rate);

                    price = PriceService.GetFinalPrice(item.Offer, customer.CustomerGroup, customOptionPrice); //PriceService.GetFinalPrice(item.Offer.RoundedPrice + customOptionPrice, discount);
                }
                
                sb.Append("<tr>");
                if (photo != null)
                {
                    sb.AppendFormat("<td style=\'text-align: center;\'><img src='{0}' /></td>",
                        FoldersHelper.GetImageProductPath(ProductImageType.XSmall, photo.PhotoName, false));
                }
                else
                {
                    sb.AppendFormat("<td>&nbsp;</td>");
                }

                sb.AppendFormat("<td>{0}{1}{2}</td>", item.Offer.Product.Name,
                    item.Offer.Color != null ? "<div>" + SettingsCatalog.ColorsHeader + ": " + item.Offer.Color + "</div>" : "",
                    item.Offer.Size != null  ? "<div>" + SettingsCatalog.SizesHeader + ": " + item.Offer.Size + "</div>" : "");

                sb.AppendFormat("<td style=\'text-align: center;\'>{0}</td>", price.FormatPrice());
                sb.AppendFormat("<td style=\'text-align: center;\'>{0}</td>", item.Amount);
                sb.AppendFormat("<td style=\'text-align: center;\'>{0}</td>", (price * item.Amount).FormatPrice());
                sb.Append("</tr>");
            }

            sb.Append("</table>");

            return sb.ToString();
        }
    }
}