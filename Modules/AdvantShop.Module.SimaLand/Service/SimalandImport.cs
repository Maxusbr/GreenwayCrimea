using System;
using System.Linq;
using AdvantShop.Module.SimaLand.Models;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Repository.Currencies;
using System.Net;
using System.Drawing;
using AdvantShop.Helpers;
using AdvantShop.Core.Modules;
using System.Data;
using System.Threading;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Saas;

namespace AdvantShop.Module.SimaLand.Service
{
    public class SimalandImport
    {
        public static void UpdateInsertProductWorker(SimalandProduct sProduct, int catId)
        {
            Product advProduct = null;
            AdvProduct product = null;

            var productSaveDb = false;
            if (!ModulesRepository.IsActiveModule(SimaLand.ModuleStringId))
            {
                SimalandImportStatistic.Process = false;
            }
            try
            {
                if (!AdvCategoryService.IsExistCategory(catId))
                {
                    throw new CategoryIsNullException("Категории нет в списке категорий магазина");
                }
                bool addingNew;

                

                if (sProduct.id == 0)
                    throw new Exception("SKU can not be empty");

                var artNo = PSLModuleSettings.UsePrefix ? sProduct.id.ToString() + "-" + PSLModuleSettings.PrefixText : sProduct.id.ToString();

                advProduct = ProductService.GetProduct(artNo);
                product = advProduct != null ? new AdvProduct(advProduct) : null;
                advProduct = null;

                addingNew = product == null;


                if (addingNew && SaasDataService.IsSaasEnabled && ProductService.GetProductsCount() >= SaasDataService.CurrentSaasData.ProductsCount)
                {
                    return;
                }

                if (addingNew)
                {
                    product = new AdvProduct { ArtNo = string.IsNullOrEmpty(artNo) ? null : artNo, Multiplicity = 1, CurrencyID = SettingsCatalog.DefaultCurrency.CurrencyId, Discount = new Discount(0, 0)};
                }


                product.Enabled = !Convert.ToBoolean(sProduct.is_disabled);

                var currency = CurrencyService.GetCurrencyByIso3(sProduct.currency);
                if (currency != null)
                    product.CurrencyID = currency.CurrencyId;
                else
                    throw new Exception("Currency not found");


                product.Length = sProduct.box_depth != null ? (float)(sProduct.box_depth * 10) : sProduct.depth * 10;
                product.Height = sProduct.box_height != null ? (float)(sProduct.box_height * 10) : sProduct.height * 10;
                product.Width = sProduct.box_width != null ? (float)(sProduct.box_width * 10) : sProduct.width * 10;
                product.Weight = sProduct.weight != 0 ? sProduct.weight / 1000 : sProduct.weight;
                var price = sProduct.price_max > 0 ? sProduct.price_max : sProduct.price;
                price = currency.Rate * price;

                var balance = PSLModuleSettings.AlwaysAvailable ? 1000f : (sProduct.balance.TryParseInteger() == 0 ? 1000f : sProduct.balance.TryParseInteger());

                OfferService.OfferFromFields(product, PriceService.SetPrice(price), price, balance);

                if (PSLModuleSettings.MinMax)
                {
                    product.MinAmount = (float)sProduct.min_qty;
                    product.MaxAmount = (float)sProduct.max_qty;
                }

                if (PSLModuleSettings.ImportDiscount)
                {
                    if (sProduct.has_discount.TryParseBoolean() && sProduct.discountPercent != null)
                    {
                        product.Discount = new Discount((float)sProduct.discountPercent, 0);
                    }
                }

                if (PSLModuleSettings.DownloadMarkers)
                {
                    product.New = sProduct.isNovelty;
                    product.BestSeller = Convert.ToBoolean(sProduct.is_hit);

                    if (sProduct.offer != null && sProduct.offer.is_disabled != 1)
                    {
                        product.OnSale = true;
                    }
                    else
                    {
                        product.OnSale = false;
                    }
                }

                product.Meta.ObjId = product.ProductId;

                if (sProduct.trademark != null)
                {
                    var bID = AdvBrandService.InsertOrGetBrand(sProduct.trademark);
                    if (bID != 0)
                    {
                        product.BrandId = bID;
                    }
                }
                product.ModifiedBy = SimaLand.ModuleStringId;

                /* new method */
                product.CheckAttributes(sProduct);

                if (!addingNew)
                {
                    ProductService.UpdateProduct(product, false);
                    SimalandProductService.InsertOrUpdateLink(sProduct.id, product.ProductId);
                    SimalandImportStatistic.TotalProcessedProducts++;
                    SimalandImportStatistic.TotalUpdateProducts++;
                    productSaveDb = true;
                    if (product.MainCategory == null)
                    {
                        ProductService.AddProductLink(product.ProductId, catId, 0, true, true);
                    }
                }
                else
                {
                    if (!(SaasDataService.IsSaasEnabled && ProductService.GetProductsCount() >= SaasDataService.CurrentSaasData.ProductsCount))
                    {
                        var productId = product.ProductId = ProductService.AddProduct(product, false);
                        SimalandProductService.InsertOrUpdateLink(sProduct.id, productId);
                        SimalandImportStatistic.TotalProcessedProducts++;
                        SimalandImportStatistic.TotalAddProducts++;
                        productSaveDb = true;
                        ProductService.AddProductLink(productId, catId, 0, true, true);

                    }
                }

                /* PROPERTY */
                if (addingNew || !PSLModuleSettings.NotUpdateProperty)
                {
                    PropertyService.DeleteProductProperties(product.ProductId);
                    AdvPropertiesService.CheckProperties(sProduct, product.ProductId);
                }
                /* PROPERTY */

                if (PSLModuleSettings.ThreePayTwo)
                {
                    var addTPT = sProduct.has_3_pay_2_action.HasValue ? Convert.ToBoolean(sProduct.has_3_pay_2_action.Value) : false;

                    if (addTPT)
                    {
                        var tpttag = product.Tags.Select(x => x).Where(x => x.UrlPath == "three-pay-two");
                        if (tpttag.Count() == 0)
                        {
                            ThreePayTwo(product.ProductId);
                        }
                    }
                    else
                    {
                        var tags = product.Tags;
                        tags = tags.Select(x => x).Where(x => x.UrlPath != "three-pay-two").ToList();
                        TagService.DeleteMap(product.ProductId, ETagType.Product);
                        if (tags != null && tags.Count != 0)
                        {
                            foreach (var item in tags)
                            {
                                var tag = TagService.Get(item.Name);
                                item.Id = tag == null ? TagService.Add(item) : tag.Id;
                                TagService.AddMap(product.ProductId, item.Id, ETagType.Product, 0);
                            }
                        }
                    }
                }

                if (PSLModuleSettings.MTGift)
                {
                    var addMTG = sProduct.hasGift.TryParseBoolean();
                    if (addMTG)
                    {
                        TagMarkerService.MTGiftAdd(product.ProductId);
                    }
                    else
                    {
                        TagMarkerService.MTGiftRemove(product);
                    }
                }

                if (PSLModuleSettings.DwnlImageType != DownloadImageType.NoPhoto && SimalandProductService.ReloadImages && !addingNew)
                {
                    PhotoService.DeleteProductPhotos(product.ProductId);
                }

                if (PSLModuleSettings.DwnlImageType != DownloadImageType.NoPhoto && !AdvProductService.HasPhoto(product.ProductId))
                {
                    if (PSLModuleSettings.DwnlImageType == DownloadImageType.AllPhoto)
                    {
                        downloadimage(sProduct.id.ToString(), product.ProductId, sProduct.photoIndexes);
                    }
                    else
                    {
                        downloadimage(sProduct.id.ToString(), product.ProductId);
                    }
                }

                if (PSLModuleSettings.AddedProductToChildCategory)
                {
                    if (sProduct.categories.Length > 0)
                    {
                        AdvProductService.LinkCategories(product.ProductId, sProduct.categories);
                    }
                }
                product = null;
            }
            catch (CategoryIsNullException ex)
            {
                if (!productSaveDb)
                {
                    SimalandImportStatistic.TotalProductError++;
                }
                throw new CategoryIsNullException(ex.Message);
            }
            catch (ThreadAbortException ex)
            {
                if (!productSaveDb)
                {
                    SimalandImportStatistic.TotalProductError++;
                }
                var msg = "";
                if ((int)ex.ExceptionState == 0)
                {
                    msg = " id: " + sProduct.id + " sid: " + sProduct.sid + " totalcount: " + SimalandImportStatistic.TotalProcessedProducts + " product reset \"" + ex.Message + "\"";
                    LogService.ErrLog(msg);
                    Thread.ResetAbort();
                }
                else
                {
                    msg = " id: " + sProduct.id + " sid: " + sProduct.sid + " totalcount: " + SimalandImportStatistic.TotalProcessedProducts + " product abort \"" + ex.Message + "\"";
                    LogService.ErrLog(msg);
                    Thread.ResetAbort();
                }
                SimalandImportStatistic.TotalProductError++;
            }
            catch (Exception ex)
            {
                if (!productSaveDb)
                {
                    SimalandImportStatistic.TotalProductError++;
                }
                var msg = "sProduct.id: " + sProduct.id  + "-" + (product != null ? product.ProductId.ToString() : "null") + " Message: " + ex.Message +  "\r\n"  + ex.StackTrace;
                LogService.ErrLog(msg);
                SimalandImportStatistic.TotalProductError++;
            }
            finally
            {
                if (!SimalandImportStatistic.Process)
                {
                    LogService.HistoryLog("Загрузка прервана пользователем");
                    throw new BreakTaskException("Загрузка прервана пользователем");
                }
            }
        }



        private static void ThreePayTwo(int productId)
        {
            var tag = TagService.GetByUrl("three-pay-two");
            if (tag == null)
            {
                var tpt = new Tag()
                {
                    Name = "3 по цене 2",
                    Enabled = true,
                    UrlPath = "three-pay-two"
                };
                var tId = TagService.Add(tpt);
                tag = TagService.Get(tId);
            }
            var existTag = TagService.Gets(productId, ETagType.Product).Select(x => x).Where(t => t.UrlPath == "three-pay-two");
            if (existTag.Count() == 0)
                TagService.AddMap(productId, tag.Id, ETagType.Product, 0);
        }

        private static bool downloadimage(string artno, int productId, int[] photoIndexes = null)
        {
            LimitRequestService.CheckLimit();

            if (photoIndexes == null)
            {
                photoIndexes = new int[] { 0 };
            }
            var linkToErr = "";
            try
            {
                foreach (var photoIndex in photoIndexes)
                {
                    var link = string.Format("https://cdn.sima-land.ru/items/{0}/{1}/700-nw.jpg", artno, photoIndex);
                    linkToErr = link;
                    WebRequest getImage = WebRequest.Create(link);
                    var imageResponse = getImage.GetResponse();
                    var slImage = imageResponse.GetResponseStream();

                    var tempName =
                        PhotoService.AddPhoto(new Photo(0, productId, PhotoType.Product)
                        {
                            OriginName = "photo_simaland.jpg"
                        });
                    using (Image image = Image.FromStream(slImage, true))
                    {
                        FileHelpers.SaveProductImageUseCompress(tempName, image, true);
                    }
                    ProductService.PreCalcProductParams(productId);
                    DeleteOriginalPicture(productId);
                }
            }
            catch (WebException ex)
            {
                WebExceptionStatus status = ex.Status;
                if (status == WebExceptionStatus.ProtocolError)
                {
                    //LogService.ErrLog("downloadimage " + ex.Message + " " + link);
                }
            }
            catch (Exception ex)
            {
                var wex = GetNestedException<WebException>(ex);

                if (wex == null) { throw; }

                var response = wex.Response as HttpWebResponse;

                if (response == null || response.StatusCode != HttpStatusCode.Forbidden)
                {
                    LogService.ErrLog("downloadimage " + ex.Message + " " + linkToErr);
                }
            }
            return true;
        }

        private static void DeleteOriginalPicture(int productId)
        {
            try
            {
                var query = @"SELECT Photo.PhotoId FROM catalog.photo
                            where ObjId = " + productId + " and type='Product'";
                var fileName = ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text) + "_original.jpg";
                var pathToFile = System.Web.Hosting.HostingEnvironment.MapPath("~/pictures/product/original/") + fileName;

                System.IO.File.Delete(pathToFile);
            }
            catch (Exception)
            {

            }
        }

        public static T GetNestedException<T>(Exception ex) where T : Exception
        {
            if (ex == null) { return null; }

            var tEx = ex as T;
            if (tEx != null) { return tEx; }

            return GetNestedException<T>(ex.InnerException);
        }

    }
}
