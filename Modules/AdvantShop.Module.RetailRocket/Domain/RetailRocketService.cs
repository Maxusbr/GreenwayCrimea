//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Newtonsoft.Json;

namespace AdvantShop.Module.RetailRocket.Domain
{
    public enum EMainPageProductsType
    {
        NotDisplay = 0,
        Popular = 1,
        Personal = 2
    }

    public enum ECategoryProductsType
    {
        NotDisplay = 0,
        Category = 1,
        Personal = 2
    }

    public class RetailRocketService
    {
        private const string ApiUrl = "http://api.retailrocket.ru/api/1.0/Recomendation/";

        #region Help method

        private static T MakeRequest<T>(string url, string data = null) where T : class
        {
            try
            {
                var request = WebRequest.Create(ApiUrl + url) as HttpWebRequest;
                request.Timeout = 250;
                request.Method = "GET";
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "text/json";

                if (data.IsNotEmpty())
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

                if (responseContent.Contains("Error"))
                {
                    Debug.Log.Error("Retail Rocket Error: " + responseContent);
                    return null;
                }

                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            catch (WebException ex)
            {
                using (var eResponse = ex.Response)
                {
                    if (eResponse != null)
                    {
                        using (var eStream = eResponse.GetResponseStream())
                            if (eStream != null)
                                using (var reader = new StreamReader(eStream))
                                {
                                    var error = reader.ReadToEnd();
                                    Debug.Log.Error(error);
                                }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return null;
        }

        private static List<ProductModel> GetProducts(List<string> ids)
        {
            var offerIds = ids.Take(RRSettings.Limit).Select(x => SQLDataHelper.GetInt(x)).Where(x => x != 0).ToList();
            var productIds = ProductService.GetProductIdsByOfferIds(offerIds);

            var products = ProductService.GetProductsByIds(productIds);
            return products;
        } 


        #endregion

        #region Public methods

        public static List<ProductModel> GetProductRecomendations(int offerId)
        {
            var ids = MakeRequest<List<string>>(string.Format("ItemToItems/{0}/{1}", RRSettings.PartnerId, offerId));
            if (ids == null || ids.Count == 0)
                return null;

            return GetProducts(ids);
        }

        public static List<ProductModel> GetProductUpSellRecomendations(int offerId)
        {
            var ids = MakeRequest<List<string>>(string.Format("UpSellItemToItems/{0}/{1}", RRSettings.PartnerId, offerId));
            if (ids == null || ids.Count == 0)
                return null;

            return GetProducts(ids);
        }

        public static List<ProductModel> GetProductCrossSellRecomendations(int offerId)
        {
            var ids = MakeRequest<List<string>>(string.Format("CrossSellItemToItems/{0}/{1}", RRSettings.PartnerId, offerId));
            if (ids == null || ids.Count == 0)
                return null;

            return GetProducts(ids);
        }

        public static List<int> GetRecomendationsByCategory(int categoryId)
        {
            var ids = MakeRequest<List<string>>(string.Format("CategoryToItems/{0}/{1}", RRSettings.PartnerId, categoryId));
            if (ids == null || ids.Count == 0)
                return null;

            var offerIds = ids.Take(RRSettings.Limit).Select(x => SQLDataHelper.GetInt(x)).Where(x => x != 0).ToList();
            return ProductService.GetProductIdsByOfferIds(offerIds);
        }

        public static List<int> GetMainPageProducts()
        {
            var ids = MakeRequest<List<string>>(string.Format("ItemsToMain/{0}/", RRSettings.PartnerId));
            if (ids == null || ids.Count == 0)
                return null;

            var offerIds = ids.Take(RRSettings.Limit).Select(x => SQLDataHelper.GetInt(x)).Where(x => x != 0).ToList();
            return ProductService.GetProductIdsByOfferIds(offerIds);
        }

        public static List<int> GetCartRecomendations(string rrpusid)
        {
            var ids = MakeRequest<List<string>>(string.Format("PersonalRecommendation/{0}/?rrUserId={1}", RRSettings.PartnerId, rrpusid));
            if (ids == null || ids.Count == 0)
                return null;

            var offerIds = ids.Take(RRSettings.Limit).Select(x => SQLDataHelper.GetInt(x)).Where(x => x != 0).ToList();
            return ProductService.GetProductIdsByOfferIds(offerIds);
        }

        public static List<int> GetShoppingCartRecomendations(List<int> offerIds)
        {
            var ids = MakeRequest<List<string>>(string.Format("CrossSellItemToItems/{0}/{1}", RRSettings.PartnerId, String.Join(",", offerIds)));
            if (ids == null || ids.Count == 0)
                return null;

            var recomOfferIds = ids.Take(RRSettings.Limit).Select(x => SQLDataHelper.GetInt(x)).Where(x => x != 0).ToList();
            return ProductService.GetProductIdsByOfferIds(recomOfferIds);
        }

        #endregion
    }
}