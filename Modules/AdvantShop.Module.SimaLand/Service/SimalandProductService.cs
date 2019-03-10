using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AdvantShop.Diagnostics;
using AdvantShop.Module.SimaLand.Models;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.FullSearch;
using AdvantShop.Core.Modules;
using System.Data;

namespace AdvantShop.Module.SimaLand.Service
{
    public class SimalandProductService
    {

        public static List<SimalandCertificate> Certificates = new List<SimalandCertificate> { };
        public static bool ReloadImages = false;

        public static void ParseProducts()
        {
            SimalandCategoryService.pc();
            SimalandImportStatistic.Reset(SimalandImportStatistic.ProcessType.ParseProducts);
            LimitRequestService.Init();
            SimalandImportStatistic.Process = true;
            LogService.HistoryLog("Запущена загрузка товаров");

            Certificates = JsonConvert.DeserializeObject<CertificateItems>(ApiService.Request(@"https://www.sima-land.ru/api/v3/certificate-type/")).items;
            ReloadImages = PSLModuleSettings.ReloadImages;
            PSLModuleSettings.ReloadImages = false;
            try
            {
                SimalandCategoryService.ClearNullCategoryId();
                var ids = SimalandCategoryService.GetLinkCategory();
                var sw = Stopwatch.StartNew();
                foreach (var id in ids)
                {
                    try
                    {
                        ParseProductsByCategory(id);
                        if (!SimalandImportStatistic.Process)
                        {
                            PSLModuleSettings.ElapsedTimeParseProducts = sw.Elapsed.ToString();
                            PSLModuleSettings.LastUpdateProducts = DateTime.Now.ToShortDateString();
                            SimalandImportStatistic.Process = false;
                            sw.Stop();
                            LogService.HistoryLog("Загрузка товаров прервана. Process = " + SimalandImportStatistic.Process);
                            return;
                        }
                    }
                    catch (ThreadAbortException ex)
                    {
                        var msg = "";
                        if ((int)ex.ExceptionState == 0)
                        {
                            msg = id + " Category reset " + ex.Message + ex.ExceptionState;
                            LogService.ErrLog(msg);
                            Thread.ResetAbort();
                        }
                        else
                        {
                            msg = id + " Category abort " + ex.Message + ex.ExceptionState;
                            LogService.ErrLog(msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        var msg = id + " Category " + ex.Message + ex.StackTrace;
                        LogService.ErrLog(msg);
                    }
                }
                sw.Stop();
                PSLModuleSettings.ElapsedTimeParseProducts = sw.Elapsed.ToString();
                PSLModuleSettings.LastUpdateProducts = DateTime.Now.ToShortDateString();
                SimalandImportStatistic.Process = false;
                LogService.HistoryLog("Загрузка товаров успешно завершена");
                SimalandImportStatistic.SaveProductParsingResult("Загрузка товаров заняла: " + sw.Elapsed.ToString());

                if (PSLModuleSettings.AlwaysAvailable)
                {
                    SLProductAvailable();
                    LogService.HistoryLog("Все товары Sima-land в наличии");
                }

                CategoryService.RecalculateProductsCountManual();
                SimalandCategoryService.AdvCategoryWithNoProductAsHidden();
            }
            catch (ThreadAbortException ex)
            {
                var msg = "";
                if ((int)ex.ExceptionState == 0)
                {
                    msg = "Parse products reset" + ex.Message + ex.ExceptionState;
                    LogService.ErrLog(msg);
                    Thread.ResetAbort();
                }
                else
                {
                    msg = "Parse products abort" + ex.Message + ex.ExceptionState;
                    LogService.ErrLog(msg);
                }

            }
            catch (Exception ex)
            {
                Diagnostics.Debug.Log.Error(ex);
            }
        }

        private static void ParseProductsByCategory(int categoryId)
        {
            var advCategoryId = SimalandCategoryService.GetAdvCategoryBySlCatId(categoryId);
            if (!AdvCategoryService.IsExistCategory(advCategoryId))
            {
                throw new CategoryIsNullException("Категории нет в списке категорий магазина");
            }
            if (!PSLModuleSettings.AlwaysAvailable)
            {
                AdvProductService.SetNotAvailableProducts(advCategoryId);
            }
            var query = ApiService.ApiSimaLand + "item?category_id=" + categoryId + "&has_balance=1&per_page=100&expand=description,categories,attrs"; //only active product
            var response = ApiService.Request(query);
            var products = DeserializeProduct(response);
            if (products != null)
            {
                foreach (var product in products.items)
                {
                    try
                    {
                        if (PSLModuleSettings.AddPriceInRange)
                        {
                            if (!(product.price >= PSLModuleSettings.fromPriceRange && product.price <= PSLModuleSettings.toPriceRange))
                            {
                                continue;
                            }
                        }
                        SimalandImport.UpdateInsertProductWorker(product, advCategoryId);
                    }
                    catch (CategoryIsNullException ex)
                    {
                        return;
                    }
                    catch (BreakTaskException ex)
                    {
                        if (!SimalandImportStatistic.Process)
                        {

                            PSLModuleSettings.StopMessage = ex.Message;
                            return;
                        }
                    }
                    catch (ThreadAbortException ex)
                    {
                        var msg = "";
                        if ((int)ex.ExceptionState == 0)
                        {
                            msg = product.id + " products.items  reset " + ex.Message + ex.ExceptionState + " query:" + query;
                            LogService.ErrLog(msg);
                            Thread.ResetAbort();
                        }
                        else
                        {
                            msg = product.id + " products.items abort" + ex.Message + ex.ExceptionState + " query:" + query;
                            LogService.ErrLog(msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        var msg = product.id + " products.items " + ex.Message + " query:" + query + ex.StackTrace;
                        LogService.ErrLog(msg);
                    }
                }
            }
            if (products != null && products._meta  != null && products._meta.pageCount > 1)
            {
                var next_page = products._links.next.href ?? "";
                for (int i = 2; i <= products._meta.pageCount; i++)
                {
                    try
                    {
                        response = ApiService.Request(next_page);
                        products = DeserializeProduct(response);
                        foreach (var product in products.items)
                        {
                            try
                            {
                                if (PSLModuleSettings.AddPriceInRange)
                                {
                                    if (!(product.price >= PSLModuleSettings.fromPriceRange && product.price <= PSLModuleSettings.toPriceRange))
                                    {
                                        continue;
                                    }
                                }
                                SimalandImport.UpdateInsertProductWorker(product, advCategoryId);
                                next_page = i < products._meta.pageCount ? products._links.next.href : "";
                            }
                            catch (CategoryIsNullException ex)
                            {
                                return;
                            }
                            catch (BreakTaskException ex)
                            {
                                if (true)
                                {
                                    break;
                                }
                            }
                            catch (ThreadAbortException ex)
                            {
                                var msg = "";
                                if ((int)ex.ExceptionState == 0)
                                {
                                    msg = product.id + " products.items reset " + ex.Message + ex.ExceptionState;
                                    LogService.ErrLog(msg);
                                    Thread.ResetAbort();
                                }
                                else
                                {
                                    msg = product.id + " products.items abort" + ex.Message + ex.ExceptionState;
                                    LogService.ErrLog(msg);
                                }
                            }
                            catch (Exception ex)
                            {
                                var msg = product.id + " products.items " + ex.Message + ex.StackTrace;
                                LogService.ErrLog(msg);
                            }
                        }
                    }
                    catch (ThreadAbortException ex)
                    {

                        var msg = "";
                        if ((int)ex.ExceptionState == 0)
                        {
                            msg = i + " products._meta.pageCount reset " + ex.Message + ex.ExceptionState;
                            LogService.ErrLog(msg);
                            Thread.ResetAbort();
                        }
                        else
                        {
                            msg = i + " products._meta.pageCount abort" + ex.Message + ex.ExceptionState;
                            LogService.ErrLog(msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        var msg = i + " products._meta.pageCount " + ex.Message;
                        LogService.ErrLog(msg);
                    }

                }
                LimitRequestService.Stop();
            }
        }

        private static ResponseProduct DeserializeProduct(string obj)
        {
            ResponseProduct result = null;
            try
            {
                if (!string.IsNullOrEmpty(obj))
                {
                    result = JsonConvert.DeserializeObject<ResponseProduct>(obj);
                }
            }
            catch (Exception ex)
            {
                var msg = "DeserializeProduct error" + " ex.message is '" + ex.Message + "'";
                LogService.ErrLog(msg);
            }
            return result;
        }

        public static void InsertOrUpdateLink(int slProductId, int advproductId = 0)
        {
            var query = @"if (SELECT COUNT(*) FROM Module.SimalandProducts where SlProductId = "+slProductId+@") = 0
                             INSERT INTO Module.SimalandProducts (ProductId, SlProductId) VALUES ("+advproductId+","+slProductId+@")
                            else
                             UPDATE Module.SimalandProducts SET UpdateDate = GETDATE() WHERE SlProductId = " + slProductId;
            ModuleService.Query(query);
        }

        public static string GetArtNo(int slProductId)
        {
            var query = @"SELECT ArtNo FROM Catalog.Product
                            INNER JOIN Module.SimalandProducts ON SimalandProducts.ProductId = Product.ProductId
                            where slProductId = " + slProductId;
            return ModulesRepository.ModuleExecuteScalar<string>(query, CommandType.Text);
        }

        public static int GetTotalCountSlProducts()
        {
            var query = @"SELECT COUNT(*) FROM Catalog.Product
                            INNER JOIN Module.SimalandProducts ON SimalandProducts.ProductId = Product.ProductId";
            return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text);
        }

        public static bool ParseProductOnPage()
        {
            var query = @"SELECT COUNT(*) FROM Module." + ModuleTables.SimalandParseProduct;

            var count = ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text);

            var httpQuery = "https://www.sima-land.ru/api/v3/item?per_page=100";

            return count > DeserializeProduct(ApiService.Request(httpQuery))._meta.pageCount;           
        }

        public static void SLProductAvailable()
        {
            var query = @"UPDATE Catalog.Offer 
                            SET Amount = 1000 
                            WHERE ProductId in (SELECT Product.ProductId 
						                            FROM Catalog.Product
					                            INNER JOIN [Module].[SimalandProducts] 
						                            ON [SimalandProducts].ProductId = Product.ProductId)";

            ModuleService.Query(query);
        }
    }
}
