using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Customers;
using AdvantShop.Module.GoogleImagesSearch.Domain;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using System;
using System.Linq;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;

namespace AdvantShop.Module.GoogleImagesSearch.Controllers
{

    [AdminAuth, Module(Type = "GoogleImagesSearchModule")]
    public partial class GoogleImagesSearchController : ModuleController
    {
        private const int ResultsPerPage = 8;

        public enum enumObjTypes
        {
            Product = 0,
            Category = 1,
            Brand = 2
        }

        public JsonResult SearchImages(string term, int page = 0)
        {
            if (string.IsNullOrWhiteSpace(term))
                return ErrorResult("Введите название товара");

            if (!CustomerContext.CurrentCustomer.IsAdmin && !(CustomerContext.CurrentCustomer.IsModerator 
                && CustomerContext.CurrentCustomer.HasRoleAction(RoleAction.Catalog)))
            {
                return ErrorResult("Доступ запрещен");
            }

            if (string.IsNullOrEmpty(GoogleImagesSearchSettings.CSEngineId) || 
                string.IsNullOrEmpty(GoogleImagesSearchSettings.ApiKey))
            {
                return ErrorResult("Пожалуйста, настройте модуль \"Поиск фотографий для товара\"");
            }

            using (var wc = new WebClient { Encoding = Encoding.UTF8 })
            {
                string res;
                try
                {
                    res = wc.DownloadString(
                        string.Format(
                            "https://www.googleapis.com/customsearch/v1?q={0}&cx={1}&searchType=image&key={2}&num={3}&start={4}",
                            HttpUtility.UrlEncode(term),
                            HttpUtility.UrlEncode(GoogleImagesSearchSettings.CSEngineId),
                            HttpUtility.UrlEncode(GoogleImagesSearchSettings.ApiKey),
                            ResultsPerPage,
                            page * ResultsPerPage + 1
                            ));
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    return ErrorResult("Не удалось получить ответ от Google Custom Search API. Проверьте настройки модуля \"Поиск фотографий для товара\".");
                }

                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleResponse>(res);
                if (response == null)
                {
                    return ErrorResult("Возникла непредвиденная ошибка. Проверьте настройки Google Custom Search API.");
                }

                if (response.error != null && !string.IsNullOrEmpty(response.error.message))
                {
                    return ErrorResult(response.error.message);
                }

                if (response.items == null || response.items.Count == 0)
                {
                    return ErrorResult("Поиск не дал результатов. Попробуйте переименовать товар.");
                }

                response.items = response.items.Where(x => FileHelpers.CheckFileExtension(x.link, EAdvantShopFileTypes.Image)).ToList();

                return Json(response);
            }
        }

        public JsonResult SearchImagesById(int objId, PhotoType type, int page = 0)
        {
            string term = string.Empty;

            switch (type)
            {
                case PhotoType.Product:
                    var product = ProductService.GetProduct(objId);
                    term = product != null ? product.Name : null;
                    break;
                case PhotoType.CategoryIcon:
                case PhotoType.CategorySmall:
                case PhotoType.CategoryBig:
                    var category = CategoryService.GetCategory(objId);
                    term = category != null ? category.Name : null;
                    break;

                case PhotoType.Brand:
                    var brand = BrandService.GetBrandById(objId);
                    term = brand != null ? brand.Name : null;
                    break;
                case PhotoType.News:
                    var news = News.NewsService.GetNewsById(objId);
                    term = news != null ? news.Title : null;
                    break;
                default:
                    Debug.Log.Error("Wrong type for module GoogleImageSearch");
                    return ErrorResult("Wrong type for module GoogleImageSearch");
            }

            return SearchImages(term, page);
        }

        private JsonResult ErrorResult(string message)
        {
            return Json(new GoogleResponse { error = new GoogleError { message = message } });
        }
    }
}