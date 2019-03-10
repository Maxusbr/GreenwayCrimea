using AdvantShop.Catalog;
using AdvantShop.Module.SimaLand.Models;
using AdvantShop.Repository.Currencies;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Service
{
    public class UpdateBalanceService
    {
        public static object locker = new object();
        static int page = 1;
        static int req = 0;

        static bool usePrefix = PSLModuleSettings.UsePrefix;
        static string prefixText = PSLModuleSettings.PrefixText;

        public static void UpdateBalancePriceJob()
        {
            SimalandImportStatistic.Reset(SimalandImportStatistic.ProcessType.UpdateBalanceProducts);
            SimalandImportStatistic.Process = true;
            SlParseProductService.PrePareProducts();
            if (!SlParseProductService.ExistProductsToUpdate())
            {
                SimalandImportStatistic.Process = false;
                return;
            }
            SimalandImportStatistic.CurrentProcess = "Обновление цены и остатка";
            SimalandImportStatistic.PrePareTotalSlProductInShop = SimalandProductService.GetTotalCountSlProducts();

            var response = "";

            Task reqTask1 = new Task(ProductRequester);

            reqTask1.Start();

            reqTask1.Wait();
            reqTask1.Dispose();

            SlParseProductService.ExistProductIdAsNotAvailable();
            SimalandImportStatistic.SaveProductUpdateBalance();
            SimalandImportStatistic.Process = false;
        }

        private static void ProductRequester()
        {
            var slIds = SlParseProductService.GetSlProductIds();
            var query = "https://www.sima-land.ru/api/v5/item/{0}";
            var response = "";
            SimalandProduct.V5 slProduct = null;
            foreach (var slId in slIds)
            {
                response = ApiService.Request(string.Format(query, slId));
                slProduct = JsonConvert.DeserializeObject<SimalandProduct.V5>(response);
                SimalandImportStatistic.TotalUpdateProducts += UpdateBalancePriceWorker(slProduct);
            }
        }

        private static void Requester()
        {
            lock (locker)
            {
                req++;
            }
            var response = "";
            var query = "";
            List<Task> tasks = new List<Task>();
            do
            {
                if (SimalandImportStatistic.Process == false)
                {
                    break;
                }
                try
                {
                    var totalSlProducts = new List<SimalandProduct.V5>();
                    while (totalSlProducts.Count() < 25000)
                    {
                        lock (locker)
                        {
                            query = "https://www.sima-land.ru/api/v5/item?p=" + page;
                        }
                        response = ApiService.Request(query);
                        if (response != "not_found")
                        {
                            var slProducts = JsonConvert.DeserializeObject<List<SimalandProduct.V5>>(response);

                            totalSlProducts.AddRange(slProducts);

                           
                        }
                        lock (locker)
                        {
                            if (response == "not_found" || response == "error")
                                LogService.HistoryLog(response);
                            ++page;
                        }
                    }

                    if (tasks.Count > 2)
                    {
                        var index = Task.WaitAny(tasks.ToArray());
                        tasks.Remove(tasks[index]);
                    }
                    var tempTask = new Task(() => Worker(totalSlProducts));
                    tempTask.Start();
                    tasks.Add(tempTask);

                }
                catch (Exception ex)
                {
                    lock (locker)
                    {
                        LogService.ErrLog(ex.Message);
                    }
                }
                if (response == "not_found" || response == "error")
                {

                    lock (locker)
                    {
                        LogService.HistoryLog("requester decriment " + response.Substring(0,5));
                    }
                    break;
                }
            } while (true);
            lock (locker)
            {
                LogService.HistoryLog("requester decriment " + (response != "not_found").ToString());
                req--;
            }
        }

        private static void Worker(List<SimalandProduct.V5> slProducts)
        {
            var updateCount = 0;
            foreach (var slProduct in slProducts)
            {
                updateCount += UpdateBalancePriceWorker(slProduct);
            }
            lock (locker)
            {
                SimalandImportStatistic.TotalUpdateProducts += updateCount;
            }
        }

        public static int UpdateBalancePriceWorker(SimalandProduct.V5 sProduct)
        {
            try
            {
                Product product = null;
                if (sProduct.id == 0)
                    throw new Exception("SKU can not be empty");

                if (!SlParseProductService.ContainsToUpdate(sProduct.id))
                {
                    return 0;
                }
                var artNo = "";

                artNo = usePrefix ? sProduct.id.ToString() + "-" + prefixText : sProduct.id.ToString();

                product = ProductService.GetProduct(artNo);

                if (product == null)
                {
                    return 0;
                }

                var currency = CurrencyService.GetCurrencyByIso3("RUB");
                if (currency != null)
                    product.CurrencyID = currency.CurrencyId;
                else
                    throw new Exception("Currency not found");

                var price = sProduct.price;
                price = currency.Rate * price;
                OfferService.OfferFromFields(product, PriceService.SetPrice(price), price, sProduct.balance.TryParseInteger() == 0 ? 1000 : sProduct.balance.TryParseInteger());
                ProductService.UpdateProduct(product, false);
                
                SimalandProductService.InsertOrUpdateLink(sProduct.id, product.ProductId);
                SlParseProductService.Remove(product.ProductId);

                return 1;
            }
            catch (Exception ex)
            {
                LogService.ErrLog(ex.Message);
                return 0;
            }
        }
    }
}
