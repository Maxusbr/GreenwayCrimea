//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Web;
using System.Text.RegularExpressions;

namespace AdvantShop.SEO
{
    public static class Crawler
    {
        public static bool IsCrawler(HttpRequest request)
        {
            // set next line to "bool isCrawler = false; to use this to deny certain bots 
            bool isCrawler = request.Browser.Crawler;
            // Microsoft doesn't properly detect several crawlers 
            if (!isCrawler)
            {
                // put any additional known crawlers in the Regex below 
                // you can also use this list to deny certain bots instead, if desired: 
                // just set bool isCrawler = false; for first line in method  
                // and only have the ones you want to deny in the following Regex list 
                var regEx = new Regex("Slurp|slurp|ask|Ask|Teoma|teoma|Yandex|Googlebot|StackRambler|Aport|Slurp|MSNBot|ia_archiver|Mail.Ru");
                if (!string.IsNullOrEmpty(request.UserAgent)) isCrawler = regEx.Match(request.UserAgent).Success;
            }
            return isCrawler;
        }
    }
}