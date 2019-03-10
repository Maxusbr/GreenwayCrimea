using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using AdvantShop.Module.SimaLand.Models;

namespace AdvantShop.Module.SimaLand.Service
{
    public class SimalandImportStatistic
    {

        public enum ProcessType
        {
            Other,
            ParseProducts,
            UpdateBalanceProducts,
            ParseCategories
        }

        public static int PrePareTotalSlProductInShop { get; set; }
        public static int TotalProcessedProducts { get; set; }
        public static int TotalProductError { get; set; }
        public static int TotalAddProducts { get; set; }
        public static int TotalUpdateProducts { get; set; }
        public static bool Process { get; set; }
        public static ProcessType Type { get; set; }

        //public static bool ParseCategoriesProcess { get; set; }
        public static int PrePareCategoriesInSimaLand { get; set; }
        public static int TotalCountCategoriesProcess{ get; set; }

        public static string CurrentProcess { get; set; }
        public static Stopwatch Timer { get; set; }

        public static void Reset(ProcessType type)
        {
            TotalProcessedProducts = 0;
            TotalProductError = 0;
            TotalAddProducts = 0;
            TotalUpdateProducts = 0;
            Process = false;
            PrePareCategoriesInSimaLand = 0;
            TotalCountCategoriesProcess = 0;
            Timer = Stopwatch.StartNew();
            Type = type;
            CurrentProcess = SetTypeName(type);
        }

        private static string SetTypeName(ProcessType type)
        {
            switch (type)
            {
                case ProcessType.ParseProducts:
                    PrePareTotalSlProductInShop = GetTotalCountProducts();
                    return "Загрузка товаров";
                case ProcessType.UpdateBalanceProducts:
                    return "Обновление остатков товаров";
                case ProcessType.ParseCategories:
                    return "Парсинг категорий";
                default:
                    return "";
            }
        }

        //public static void CategoryReset()
        //{
        //    //ParseCategoriesProcess = false;
        //    Timer = Stopwatch.StartNew();
        //}

        public static void SaveCategoryParsingResult(string text = "")
        {
            var nl = Environment.NewLine;
            var msg = "Всего активных категорий на Sima-land: " + PrePareCategoriesInSimaLand;
            msg += nl + "Категорий обработано: " + TotalCountCategoriesProcess;
            msg += nl + text;
            LogService.HistoryLog(msg);
        }

        public static void SaveProductParsingResult(string text = "")
        {
            var nl = Environment.NewLine;
            var msg = "Всего активных товаров на Sima-land: " + TotalProcessedProducts;
            msg += nl + "Товаров обработано: " + (TotalAddProducts + TotalUpdateProducts);
            msg += nl + "Товаров добавлено: " + TotalAddProducts;
            msg += nl + "Товаров обновлено: " + TotalUpdateProducts;
            msg += nl + text;
            LogService.HistoryLog(msg);
        }

        public static void SaveProductUpdateBalance(string text = "")
        {
            var nl = Environment.NewLine;
            var msg = "Всего товаров Sima-land в магазине: " + PrePareTotalSlProductInShop;
            msg += nl + "Товаров обновлено: " + TotalUpdateProducts;
            msg += nl + "Обновление заняло: " + Timer.Elapsed;
            msg += nl + text;
            LogService.HistoryLog(msg);
            Timer.Stop();
        }

        private static int GetTotalCountProducts()
        {
            var response = ApiService.Request("https://www.sima-land.ru/api/v3/item/");
            return JsonConvert.DeserializeObject<ResponseProduct>(response)._meta.totalCount;
        }

    }
}
