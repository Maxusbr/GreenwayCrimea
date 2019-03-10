using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Taxes;

namespace AdvantShop.Core.Services.Shipping.YandexDelivery
{
    public class YandexDeliveryService
    {
        public static string MakeRequest(string url, string postDataString)
        {
            string responseData = null;

            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            webRequest.Accept = "text/json";
            webRequest.Timeout =
                HttpContext.Current != null &&
                HttpContext.Current.Request.Url.ToString().Contains("/api/")
                    ? 3000
                    : 8000;

            var bytes = Encoding.UTF8.GetBytes(postDataString);
            webRequest.ContentLength = bytes.Length;

            using (var requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }

            try
            {
                using (var response = webRequest.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                        using (var reader = new StreamReader(stream))
                        {
                            responseData = reader.ReadToEnd();
                        }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return responseData;
        }

        public static string GetSign(Dictionary<string, object> dict, string secretkey)
        {
            var preparedData = "";

            var items = dict.Where(d=> d.Value != null).OrderBy(d => d.Key).Select(d => d.Value);

            foreach (var item in items)
            {
                if (item is Array)
                {
                    foreach (var subItem in (Array)item)
                    {
                        var subOrderItem = subItem as OrderItem;
                        if (subOrderItem != null)
                        {
                            preparedData += Slugify(subOrderItem.ArtNo.Replace(",", ".")) +
                                            subOrderItem.Price.ToString("F2").Replace(",", ".") + Slugify(subOrderItem.Name) +
                                            subOrderItem.Amount.ToString("F2").Replace(",", ".");
                            var vatRate = GetTaxType(subOrderItem.TaxType);
                            if (vatRate > 0)
                                preparedData += vatRate.ToString();
                        }
                    }
                }
                else
                {
                    preparedData += item;
                }
            }

            return (preparedData + secretkey).Md5(false, Encoding.UTF8);
        }

        public static string GetOrderItems(IList<OrderItem> items)
        {
            var result = "";

            for (var i = 0; i < items.Count; i++)
            {
                result += (result != "" ? "&" : "") +
                          string.Format(
                              "order_items[{0}][orderitem_article]={1}&order_items[{0}][orderitem_cost]={2}&order_items[{0}][orderitem_name]={3}&order_items[{0}][orderitem_quantity]={4}",
                              i, 
                              HttpUtility.UrlEncode(items[i].ArtNo.Replace(",", ".")),
                              items[i].Price.ToString("F2").Replace(",", "."), 
                              HttpUtility.UrlEncode(items[i].Name),
                              items[i].Amount.ToString("F2").Replace(",", "."));

                var vatRate = GetTaxType(items[i].TaxType);
                if (vatRate > 0)
                    result += string.Format("&order_items[{0}][orderitem_vat_value]={1}", i, vatRate);
            }

            return result;
        }


        public static string Slugify(string str)
        {
            //string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()) + "+";

            //foreach (char c in invalid)
            //{
            //    str = str.Replace(c.ToString(), "");
            //}
            //return System.Web.HttpUtility.UrlEncode(str);
            return str;
        }

        /*
        Ставка НДС 18% 1
        Ставка НДС 10% 2
        Ставка НДС расч. 18/118. 3
        Ставка НДС расч. 10/110 4
        Ставка НДС 0% 5
        НДС не облагается 6 
        */
        private static int GetTaxType(TaxType? taxType)
        {
            if (taxType == null)
                return 0;

            if (taxType.Value == TaxType.Without)
                return 6;

            if (taxType.Value == TaxType.Zero)
                return 5;

            if (taxType.Value == TaxType.Ten)
                return 2;

            if (taxType.Value == TaxType.Eighteen)
                return 1;

            return 0;
        }
    }
}
