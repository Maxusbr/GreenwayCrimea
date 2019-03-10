//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using Newtonsoft.Json;
using AdvantShop.Diagnostics;
using AdvantShop.ExportImport;
using System.Web;
using AdvantShop.FilePath;
using AdvantShop.Configuration;
using System.Net;
using System.IO;
using System.Text;

namespace AdvantShop.Module.Rees46.Domain
{
    #region ObjectFields

    public class TrackEvents
    {
        public int? Id { get; set; }
        public bool? Stock { get; set; }
        public int? Amount { get; set; }
        public string Price { get; set; }
        public string Name { get; set; }
        public int[] Categories { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public TypeEvent? Type { get; set; }
        public string RecommendedBy { get; set; }
        public string Order { get; set; }
        public string OrderPrice { get; set; }
        public ProductRees46[] Products { get; set; }
    }

    public enum TypeEvent
    {
        [StringName("Отсутствует")]
        none,

        [StringName("Просмотр товара")]
        view,

        [StringName("Добавление товара в корзину")]
        cart,

        [StringName("Удаление товара из корзины")]
        remove_from_cart,

        [StringName("Оформление заказа")]
        purchase,
    }

    public class ProductRees46
    {
        public int Id { get; set; }
        public string Price { get; set; }
        public float Amount { get; set; }
    }
    /// <summary>
    /// Рекомендации
    /// </summary>
    public enum Recomender
    {
        /// <summary>
        /// Не выводить
        /// </summary>
        [StringName("Не выводить")]
        none,

        /// <summary>
        /// Вас это заинтересует
        /// </summary>
        [StringName("Вас это заинтересует")]
        interesting,

        /// <summary>
        /// С этим товаром покупают
        /// </summary>
        [StringName("С этим товаром покупают")]
        also_bought,

        /// <summary>
        /// Похожие товары
        /// </summary>
        [StringName("Похожие товары")]
        similar,

        /// <summary>
        /// Популярные товары
        /// </summary>
        [StringName("Популярные товары")]
        popular,

        /// <summary>
        /// Посмотрите также
        /// </summary>
        [StringName("Посмотрите также")]
        see_also,

        /// <summary>
        /// Вы недавно смотрели
        /// </summary>
        [StringName("Вы недавно смотрели")]
        recently_viewed,

        /// <summary>
        /// Прямо сейчас покупают
        /// </summary>
        [StringName("Прямо сейчас покупают")]
        buying_now,
    }

    public class RegCustomers
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Country_Code { get; set; }
        public string City { get; set; }
        public string Shopify { get; set; }
    }

    public class RegShop
    {
        public string Api_Key { get; set; }
        public string Api_Secret { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public int Category { get; set; }
        public string Yml_File_Url { get; set; }
        public int Cms_Id { get; set; }
        public int Geo_Law { get; set; }
    }

    public class ResponsesApi
    {
        public ResponsesApi()
        {
            status = false;
        }
        public bool status { get; set; }

        public string data { get; set; }
        [JsonProperty(PropertyName = "error")]
        public string message { get; set; }
    }

    public class RequestInCAP
    {
        public string ShopName { get; set; }
        public string Email { get; set; }
        public string ShopKey { get; set; }
    }

    public class RegistrationCustomers
    {
        [JsonProperty(PropertyName = "api_key")]
        public string ApiKey { get; set; }

        [JsonProperty(PropertyName = "api_secret")]
        public string ApiSecret { get; set; }
    }

    public class RegistrationShop
    {
        [JsonProperty(PropertyName = "shop_key")]
        public string ShopKey { get; set; }

        [JsonProperty(PropertyName = "shop_secret")]
        public string SecretKey { get; set; }
    }

    #endregion

    public class Rees46Service
    {
        #region Fields

        private const string UrlRees46 = "https://rees46.com";

        private const string FormatStr = 
@"<div class='rees46-recommender {0}'></div> 
<script>
window.addEventListener('load', function load () {{
   window.removeEventListener('load', load);

   try {{
     r46('recommend', '{0}', {{{1}}}, 
            function(data) {{
            getRees46Products('{0}', {4}, data, '{2}', '{3}', '{5}');
        }});
    r46('add_css', 'recommendations');
  }} catch(e) {{ }}
}});
</script>";

        private const string FormatTrack = 
@"<script>
window.addEventListener('load', function load () {{
    window.removeEventListener('load', load);

    r46('track', '{0}', {1});
}});
</script>";

        #endregion

        #region Public methods

        public static string GetRecomender(Recomender recom, int? offerId = null, int? categoryId = null,
                                            List<int> cartIds = null, string title = "", string relatedType = "", int productCount = 0)
        {
            var parammetrs = "";

            switch (recom)
            {
                case Recomender.interesting:
                case Recomender.also_bought:
                    if (offerId != null)
                        parammetrs = "item: " + offerId;
                    break;
                case Recomender.similar:
                case Recomender.buying_now:
                    if (offerId != null)
                        parammetrs = "item: " + offerId;
                    if (cartIds != null && cartIds.Count > 0)
                        parammetrs += (parammetrs != "" ? ",\n" : "\n") + "cart: [" + string.Join(",", cartIds) + "]";
                    break;

                case Recomender.popular:
                    if (categoryId != null)
                        parammetrs = "category: " + categoryId;
                    break;

                case Recomender.see_also:
                    if (cartIds != null && cartIds.Count > 0)
                        parammetrs = "cart: [" + string.Join(",", cartIds) + "]";
                    break;

                case Recomender.none:
                    return string.Empty;
            }

            if (Rees46Settings.Limit > 0)
            {
                parammetrs += (parammetrs != "" ? ",\n" : "\n") + "limit: " + Rees46Settings.Limit;
            }

            if (string.IsNullOrEmpty(title))
                title = recom.StrName();

            return string.Format(FormatStr, recom, parammetrs, title, relatedType, offerId != null ? offerId.Value : 0, productCount != 0 ? productCount.ToString() : "");
        }

        public static string TrackView(TrackEvents obj, TypeEvent type)
        {
            if (TypeEvent.purchase == type)
            {
                var str = "[";
                foreach (var item in obj.Products)
                {
                    str += "{ id: " + item.Id + ", price: " + item.Price + ", amount: " + item.Amount + "}" + (item == obj.Products.Last() ? "]" : ",");
                }
                return string.Format(FormatTrack,type, "{products: " + str + ", order: '" + obj.Order + "', order_price: " + obj.OrderPrice + "}");
            }
            else
            {
                return string.Format(FormatTrack, type, "{id: " + obj.Id + ", stock: " + obj.Stock.ToString().ToLower() + ", price: " + obj.Price +
                    ", name: '" + obj.Name + "', categories: [" + string.Join(",",obj.Categories) + "], image: '" + obj.Image + "', url: '" + obj.Url + "'}");
            }
        }

        #endregion

        #region Registration

        public static ResponsesApi Registration(RegCustomers data)
        {
            var dataContent = JsonConvert.SerializeObject(data).ToLower().Replace("null", "\"\"");
            ResponsesApi result = MakeRequest("/api/customers", dataContent, contentType: "application/json");
            if (result.status == false || result.data.IsNullOrEmpty())
            {
                result = SetMessageError("Не удалось добавить пользователя, проверьте ваши данные.");
                return result;
            }
            var customerData = new RegistrationCustomers();
            try
            {
                customerData = JsonConvert.DeserializeObject<RegistrationCustomers>(result.data);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return SetMessageError("Не удалось получить данные пользователя");
            }

            var dataShop = new RegShop();
            dataShop.Api_Key = customerData.ApiKey;
            dataShop.Api_Secret = customerData.ApiSecret;
            dataShop.Url = SettingsMain.SiteUrl;
            dataShop.Name = SettingsMain.ShopName;
            dataShop.Category = 0;//Другое
            dataShop.Cms_Id = 11;//Advantshop
            //Ищем есть ли выгрузки для Rees46
            var allFeeds = ExportFeedService.GetExportFeeds();
            if (allFeeds != null && allFeeds.Count > 0)
            {
                allFeeds = allFeeds.Where(x => x.Type == EExportFeedType.YandexMarket && x.Name.ToLower().Contains("rees46")).ToList();
                if (allFeeds.Count > 0)
                {
                    var currentFeed = ExportFeedSettingsProvider.GetSettings(allFeeds.First().Id);
                    dataShop.Yml_File_Url = SettingsMain.SiteUrl + "/" + currentFeed.FileFullName;
                }
            }
            if (allFeeds.Count == 0)
            {
                var exportFeedId = ExportFeedService.AddExportFeed(new ExportFeed
                {
                    Name = "Выгрузка для Rees46",
                    Type = EExportFeedType.YandexMarket,
                    Description = "Выгрузка для Rees46"
                });
                ExportFeedService.InsertCategory(exportFeedId, 0);
                ExportFeedSettingsProvider.SetSettings(exportFeedId, new ExportFeedYandexOptions
                {
                    PriceMargin = 0,
                    FileName = System.IO.File.Exists(SettingsGeneral.AbsolutePath + "/export/rees46.yml") ? "export/rees46" + exportFeedId : "export/rees46",
                    FileExtention = ExportFeedYandex.AvailableFileExtentions[1],
                    CompanyName = "#STORE_NAME#",
                    ShopName = "#STORE_NAME#",
                    ProductDescriptionType = "short",
                    Currency = ExportFeedYandex.AvailableCurrencies[0],
                    GlobalDeliveryCost = "[]",
                    LocalDeliveryOption = "{\"Cost\":null,\"Days\":\"\",\"OrderBefore\":\"\"}"
                });
                dataShop.Yml_File_Url = SettingsMain.SiteUrl + "/export/rees46.yml";
            }

            dataContent = JsonConvert.SerializeObject(dataShop).ToLower();
            result = MakeRequest("/api/shops", dataContent, contentType: "application/json");
            if (result.status == false || result.data.IsNullOrEmpty())
            {
                result = SetMessageError("Не удалось создать магазин для пользовалетя.");
                return result;
            }
            var shopData = new RegistrationShop();
            try
            {
                shopData = JsonConvert.DeserializeObject<RegistrationShop>(result.data);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return SetMessageError("Не удалось получить данные магазина");
            }

            Rees46Settings.ShopKey = shopData.ShopKey;
            Rees46Settings.SecretKey = shopData.SecretKey;
            //Отправка данных в капу
            //var dataInCAP = new RequestInCAP();
            //dataInCAP.ShopName = SettingsMain.SiteUrl;
            //dataInCAP.Email = data.Email;
            //dataInCAP.ShopKey = Rees46Settings.ShopKey;
            //dataContent = JsonConvert.SerializeObject(dataInCAP).ToLower();
            //MakeRequest("", dataContent, contentType: "application/json");
            return result;
        }

        private static ResponsesApi MakeRequest(string url, string data = null, string method = "POST", string contentType = "application/x-www-form-urlencoded")
        {

            string requestUrl = string.Format(UrlRees46 + url, method == "GET" ? "?" + data : string.Empty);
            var result = new ResponsesApi();
            try
            {
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Method = method;
                request.Timeout = 600000;
                request.ContentType = contentType;

                if (data.IsNotEmpty() && method == "POST")
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    request.ContentLength = bytes.Length;

                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                    }
                }

                var responseContent = "";
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream))
                            {
                                responseContent = reader.ReadToEnd();
                            }
                    }
                }
                if (!string.IsNullOrEmpty(responseContent) && (responseContent.Contains("api_key") || responseContent.Contains("shop_key")))
                {
                    result.status = true;
                    result.data = responseContent;
                }
                return result;
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    Debug.Log.Error(ex);
                    return result;
                }

                using (var stream = ex.Response.GetResponseStream())
                {
                    if (stream != null)
                        using (var reader = new StreamReader(stream))
                        {
                            var temp = reader.ReadToEnd();
                            if (temp.IsNotEmpty() && temp.Length < 150)
                            {
                                result = JsonConvert.DeserializeObject<ResponsesApi>(temp);
                                result.message =
                                    result.message.Replace("#<ActiveRecord::RecordInvalid:", string.Empty)
                                        .Replace(">", string.Empty);
                            }
                            return result;
                        }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return result;
        }

        private static ResponsesApi SetMessageError(string message)
        {
            var result = new ResponsesApi
            {
                message = message,
                status = false
            };
            return result;
        }

        #endregion
    }
}