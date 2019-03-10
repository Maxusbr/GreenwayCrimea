using AdvantShop.Diagnostics;
using AdvantShop.Module.SimaLand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;
using AdvantShop.Module.SimaLand.Service;

namespace AdvantShop.Module.SimaLand.Service
{
    public class ApiService
    {
        public static string ApiSimaLand = "https://www.sima-land.ru/api/v3/";

        public static string Token = PSLModuleSettings.JWT;

        public static string Request(string query)
        {
            lock (UpdateBalanceService.locker)
            {
                LimitRequestService.CheckLimit();
            }
            var result = "";

            try
            {
                using (var wb = new WebClient())
                {
                    wb.Headers.Add(HttpRequestHeader.Authorization, Token);
                    wb.Headers.Add(HttpRequestHeader.Accept, "application/json");
                    wb.Encoding = Encoding.UTF8;
                    result = wb.DownloadString(query);                    
                }
            }
            catch (Exception ex)
            {
                var wex = SimalandImport.GetNestedException<WebException>(ex);

                if (wex == null) { throw; }

                var response = wex.Response as HttpWebResponse;

                if (response == null || response.StatusCode != HttpStatusCode.NotFound)
                {
                    LogService.ErrLog(ex.Message + " " + query);
                    return "not_found";
                }
                Debug.Log.Error(ex);
                result = "error";
            }

            return result;
        }

        //for api v5
        public static object SignIn(SlLoginModel slUser)
        {
            try
            {
                var json = JsonConvert.SerializeObject(slUser);

                var body = Encoding.UTF8.GetBytes(json);
                var request = (HttpWebRequest)WebRequest.Create("https://www.sima-land.ru/api/v5/signin");

                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = body.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Close();
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    PSLModuleSettings.JWT = response.Headers.Get("Authorization");
                    response.Close();
                }

                return new { error = false, message = "Вы успешно авторизованый" };
            }
            catch (Exception ex)
            {
                return new { error = true, message = ex.Message };
            }
        }

    }
}
