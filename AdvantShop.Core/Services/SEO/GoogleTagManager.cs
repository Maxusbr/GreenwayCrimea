//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;

namespace AdvantShop.Core.Services.SEO
{
    public enum ePageType
    {
        other = 0,       // во всех остальных случаях,
        home,           // главная страница;
        category,       // категория товаров
        product,        // страница товара
        cart,           // страница корзины
        order,          // страницы оформления заказа
        purchase,       // страница покупки, где спасибо за заказ!!!;
        info,           // любые инфостраницы сайта для клиентов: о доставке, об оплате и т.д
        searchresults,  // страница поиска
        brand,          // категория товаров по бренду
        landing,
        //
        addToCart,
        buyOneClickForm,
        addToCompare,
        addToWishlist,
        sendFeedback,
        addResponse,
        getCallBack
    }

    public enum eClientType
    {
        guest,          // неавторизованный пользователь
        user,           // авторизованный пользователь
    }

    public class GoogleTagManager
    {
        public static string ContainerID
        {
            get { return SettingsSEO.GTMContainerID; }
        }

        public bool Enabled
        {
            get { return SettingsSEO.UseGTM; }
        }

        public ePageType PageType { get; set; }     // тип страницы 
        public int CatCurrentId { get; set; }       // ID текущий категории (показываем в категории и на странице товара).
        public string CatCurrentName { get; set; }  // название текущей категории (показываем в категории и на странице товара).
        public int CatParentId { get; set; }        // ID родительской  категории (показываем в категории и на странице товара).
        public string CatParentName { get; set; }   // название родительской категории (показываем в категории и на странице товара).
        public List<string> ProdIds { get; set; }   // список всех ID товаров на странице, при условии pageType  = category / cart / purchase / brand (Vladimir: Передаем туда артикулы)
        public string ProdId { get; set; }          // ID товара, при условии pageType = product (Vladimir: Передаем туда артикул)
        public string ProdName { get; set; }        // название товара, при условии pageType = product
        public float ProdValue { get; set; }        // стоимость товара, при условии pageType = product
        public float TotalValue { get; set; }       // стоимость всех товаров на странице с учетом количество, при условии pageType = cart / purchase

        public List<TransactionProduct> Products { get; set; }

        public eClientType ClientType               // тип клиента
        {             
            get { return Customers.CustomerContext.CurrentCustomer.RegistredUser ? eClientType.user : eClientType.guest; }
        }

        public string ClientId                      // ID авторизованного пользователя. (Vladimir: у нас id есть всегда, если будет мешать добавить условие type=eClientType.user)
        {
            get { return Customers.CustomerContext.CurrentCustomer.Id.ToString(); } 
        }        

        public Transaction Transaction { get; set; }


        public string RenderCounter()
        {
            if (!Enabled)
                return string.Empty;

            var sendData = new StringBuilder();
            sendData.Append("<script>dataLayer = [{");

            sendData.AppendFormat("'pageType' : '{0}', ", PageType.ToString());
            sendData.AppendFormat("'clientType' : '{0}', ", ClientType.ToString());
            if (ClientType == eClientType.user)
            {
                sendData.AppendFormat("'clientId' : '{0}', ", ClientId);
            }

            if (PageType == ePageType.category || PageType == ePageType.product)
            {
                sendData.AppendFormat("'catCurrentId' : '{0}', ", CatCurrentId);
                sendData.AppendFormat("'catCurrentName' : '{0}', ", HttpUtility.HtmlEncode(CatCurrentName));
            }

            if (PageType == ePageType.category)
            {
                sendData.AppendFormat("'catParentId' : '{0}', ", CatParentId);
                sendData.AppendFormat("'catParentName' : '{0}', ", HttpUtility.HtmlEncode(CatParentName));
            }

            if (PageType == ePageType.product)
            {
                sendData.AppendFormat("'prodId' : '{0}',", HttpUtility.HtmlEncode(ProdId));
                sendData.AppendFormat("'prodName' : '{0}', ", HttpUtility.HtmlEncode(ProdName));
                sendData.AppendFormat("'prodValue' : '{0}', ", ProdValue);
            }

            if ((PageType == ePageType.category || PageType == ePageType.brand || PageType == ePageType.searchresults 
                || PageType == ePageType.purchase) && ProdIds != null && ProdIds.Any())
            {
                sendData.AppendFormat("'prodIds' : [{0}], ", ProdIds.Select(id => "'" + HttpUtility.HtmlEncode(id) + "'").AggregateString(","));
            }

            if ((PageType == ePageType.cart || PageType == ePageType.order) && ProdIds != null && ProdIds.Any())
            {
                string product = null;
                foreach (var item in Products)
                {
                    product += "{'sku':'" + item.SKU + "', 'name':'" + item.Name + "', 'category':'" + item.Category + "', 'price':" + item.Price + ", 'quantity':" + item.Quantity + " }" +
                        (Products.Last() == item ? string.Empty : ",");
                }
                sendData.AppendFormat("'prodIds' : [{0}], 'products' : [{1}],", ProdIds.Select(id => "'" + HttpUtility.HtmlEncode(id) + "'").AggregateString(","), product ?? "null");
            }

            if (PageType == ePageType.cart || PageType == ePageType.purchase)
            {
                sendData.AppendFormat("'totalValue' : '{0}', ", TotalValue);
            }

            if (PageType == ePageType.purchase && Transaction != null && Transaction.TransactionProducts != null && Transaction.TransactionProducts.Any())
            {
                sendData.AppendFormat("'transactionId' : '{0}', ", Transaction.TransactionId);
                sendData.AppendFormat("'transactionAffiliation' : '{0}', ", Transaction.TransactionAffiliation);
                sendData.AppendFormat("'transactionTotal' : {0}, ", Transaction.TransactionTotal.ToString().Replace(",", "."));
                sendData.AppendFormat("'transactionShipping' : {0}, ", Transaction.TransactionShipping.ToString().Replace(",", "."));
                sendData.AppendFormat("'transactionProducts' : [{0}], ",
                                      Transaction.TransactionProducts.Select(
                                          p =>
                                          string.Format(
                                              "{{'sku':'{0}', 'name':'{1}', 'category':'{2}', 'price':{3}, 'quantity':{4}}}",
                                              HttpUtility.HtmlEncode(p.SKU), HttpUtility.HtmlEncode(p.Name), HttpUtility.HtmlEncode(p.Category), p.Price.ToString().Replace(",", "."), p.Quantity.ToString().Replace(",", ".")
                                              )
                                          ).AggregateString(",")
                    );
                sendData.AppendFormat("'advantshop_Shipping' : {0}, ", Transaction.Advantshop_Shipping.ToString().Replace(",", "."));
                sendData.AppendFormat("'advantshop_Purchase' : {0}", Transaction.Advantshop_Purchase.ToString().Replace(",", "."));
            }

            sendData.Append("}];</script>\n");

            sendData.AppendFormat(@"<!-- Google Tag Manager -->
<noscript><iframe src=""//www.googletagmanager.com/ns.html?id={0}""
height=""0"" width=""0"" style=""display:none;visibility:hidden""></iframe></noscript>
<script>(function(w,d,s,l,i){{w[l]=w[l]||[];w[l].push({{'gtm.start':
new Date().getTime(),event:'gtm.js'}});var f=d.getElementsByTagName(s)[0],
j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src=
'//www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);
}})(window,document,'script','dataLayer','{0}');</script>
<!-- End Google Tag Manager -->", ContainerID);

            return sendData.ToString();
        }

        public void CreateTransaction(Order order)
        {
            if (!Enabled)
                return;

            PageType = ePageType.purchase;
            TotalValue = order.OrderItems.Sum(item => item.Price * item.Amount);

            Transaction = new Transaction()
            {
                TransactionAffiliation = SettingsMain.ShopName,
                TransactionId = order.OrderID,
                TransactionTotal = order.Sum - order.ShippingCost,
                TransactionShipping = order.ShippingCost,
                TransactionProducts =
                    new List<TransactionProduct>(
                        order.OrderItems.Select(
                            item =>
                                new TransactionProduct()
                                {
                                    Name = item.Name,
                                    Price = item.Price,
                                    Quantity = item.Amount,
                                    SKU = item.ArtNo,
                                    Category =
                                        CategoryService.GetCategory(
                                            ProductService.GetFirstCategoryIdByProductId((int) item.ProductID)).Name
                                })),
                Advantshop_Shipping = order.ShippingCost,
                Advantshop_Purchase = order.OrderItems.Sum(x => x.SupplyPrice)
            };
        }
    }

    public class Transaction
    {
        public int TransactionId { get; set; }                  // номер транзакции (заказа)
        public string TransactionAffiliation { get; set; }      // название магазина
        public float TransactionTotal { get; set; }             // сумма итого без стоимости доставки
        public float TransactionShipping { get; set; }          // сумма доставки
        public List<TransactionProduct> TransactionProducts { get; set; }
        public float Advantshop_Shipping { get; set; }
        public float Advantshop_Purchase { get; set; }
    }

    public class TransactionProduct
    {
        public string SKU { get; set; }         // артикул товара
        public string Name { get; set; }        // название 
        public string Category { get; set; }    // имя категории
        public float Price { get; set; }        // стоимость единицы товара
        public float Quantity { get; set; }     // количество товаров в покупке
    }

}