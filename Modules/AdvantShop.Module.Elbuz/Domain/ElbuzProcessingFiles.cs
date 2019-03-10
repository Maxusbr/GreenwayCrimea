//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FullSearch;
using AdvantShop.Repository.Currencies;
using AdvantShop.SEO;
using AdvantShop.Statistic;
using System.Web;

namespace AdvantShop.Module.Elbuz.Domain
{
    public class ElbuzProcessingFiles
    {
        /// <summary>
        /// Dictionary [Код товара в elbuz] и [артикул товара в бд]
        /// </summary>
        private Dictionary<string, string> productDictionary = new Dictionary<string, string>(); 

        /// <summary>
        /// Dictionsry [Код товара в elbuz] и [значение цвета]
        /// </summary>
        private Dictionary<string, string> colorDictionary = new Dictionary<string, string>();

        /// <summary>
        /// Dictionsry [Код товара в elbuz] и [значение размера]
        /// </summary>
        private Dictionary<string, string> sizeDictionary = new Dictionary<string, string>();

        private bool _typeSelectArtNo;

        public void ProcessData(string fullPath, bool disableProducts, bool disableCategories, bool typeSelectArtNo)
        {
            _typeSelectArtNo = typeSelectArtNo;
            var startAt = DateTime.Now;
            using (var streamReaderCount = new StreamReader(fullPath))
            {
                int count = 0;
                while (!streamReaderCount.EndOfStream)
                {
                    var s = streamReaderCount.ReadLine();
                    if (!string.IsNullOrEmpty(s) && (s.StartsWith("g\t") || s.StartsWith("p\t") || s.StartsWith("m\t") || s.StartsWith("pl\t")))
                        count++;
                }
                streamReaderCount.Close();
                CommonStatistic.TotalRow = count;
            }

            if (CommonStatistic.TotalRow == 0)
            {
                LogInvalidData("Invalid file format");
                CommonStatistic.IsRun = false;
                return;
            }
            
            if (disableCategories)
                ElbuzRepository.DisableAllCategories();

            try
            {
                ReadProductOptions(fullPath);
                ReadProducts(fullPath);
                ReadProductAttributes(fullPath);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            if (disableProducts)
            {
                ProductService.DisableAllProducts(startAt);
            }

            CommonStatistic.IsRun = false;

            new Task(() =>
            {
                CategoryService.RecalculateProductsCountManual();
                CategoryService.SetCategoryHierarchicallyEnabled(0);

                LuceneSearch.CreateAllIndexInBackground();
                ProductService.PreCalcProductParamsMassInBackground();
                CacheManager.Clean();

            }).Start();
        }

        private void ReadProductOptions(string fullPath)
        {
            using (var streamReader = new StreamReader(fullPath))
            {
                while (!streamReader.EndOfStream)
                {
                    if (!CommonStatistic.IsRun)
                    {
                        streamReader.Dispose();
                        return;
                    }

                    var temp = streamReader.ReadLine();

                    if (String.IsNullOrEmpty(temp) || !temp.StartsWith("af\t"))
                        continue;

                    var tempValue = temp.Split(new[] { "\t" }, StringSplitOptions.None);
                    if (tempValue[0] == "af")
                    {
                        ProcessColorSizeOptions(tempValue);
                    }
                }
                streamReader.Close();
            }
        }

        private void ReadProducts(string fullPath)
        {
            using (var streamReader = new StreamReader(fullPath))
            {
                var elbazcatToAdvcat = new Dictionary<int, int>();      // first = [Elbuz categoryId], second = [AdvantShop categoryId] 
                var elbazManufacturerToAdv = new Dictionary<int, int>();// first = [Elbuz manufacturerId], second = [AdvantShop brandId] 

                while (!streamReader.EndOfStream)
                {
                    if (!CommonStatistic.IsRun)
                    {
                        streamReader.Dispose();
                        return;
                    }

                    var temp = streamReader.ReadLine();

                    if (String.IsNullOrEmpty(temp))
                        continue;

                    var tempValue = temp.Split(new[] { "\t" }, StringSplitOptions.None);
                    if (tempValue.Length < 2)
                    {

                        LogInvalidData("InvalidData in row " + CommonStatistic.RowPosition);

                    }
                    else if (tempValue[0] == "g" && tempValue.Length >= 17)
                    {
                        if (ProcessCategory(tempValue, elbazcatToAdvcat))
                        {
                            CommonStatistic.TotalUpdateRow++;
                        }
                    }

                    else if (tempValue[0] == "p" && tempValue.Length >= 21)
                    {
                        ProcessProduct(tempValue, elbazcatToAdvcat, elbazManufacturerToAdv);
                    }

                    else if (tempValue[0] == "m")
                    {
                        ProcessManufacturer(tempValue, elbazManufacturerToAdv);
                    }
                    else
                    {
                        continue;
                    }
                    CommonStatistic.RowPosition++;

                }


                streamReader.Close();
            }
        }

        private void ReadProductAttributes(string fullPath)
        {
            using (var streamReader = new StreamReader(fullPath))
            {
                while (!streamReader.EndOfStream)
                {
                    if (!CommonStatistic.IsRun)
                    {
                        streamReader.Dispose();
                        return;
                    }

                    var temp = streamReader.ReadLine();

                    if (String.IsNullOrEmpty(temp))
                        continue;

                    var tempValue = temp.Split(new[] { "\t" }, StringSplitOptions.None);

                    if (tempValue[0] == "pl" && tempValue.Length >= 21)
                    {
                        ProcessProductPL(tempValue);
                        CommonStatistic.RowPosition++;
                    }
                }
                streamReader.Close();
            }
        }
        
        private void ProcessProduct(string[] temp, Dictionary<int, int> elbazcatToAdvcat, Dictionary<int, int> elbazManufacturerToAdv)
        {
            #region Product E-Trade Content Creator Description
            /*
           Если тип записи "p", тогда колонки означают следующее:
            2. Код товара (только числовое значение). *
            3. Код категории, в которой находится товар (только числовое значение). *
            4. Артикул производителя товара.
            5. Наименование товара. *
            6. Цена товара.
            7. Код поставщика.
            8. Код производителя.
            9. Тег meta_title для товара.
            10. Тег meta_description для товара.
            11. Тег meta_keywords для товара.
            12. Признак одного из статусов товара (Спецена, Распродажа, Новинка) = 0 или 1.
            13. Описание товара.
            14. Количество.
            15. Код описания товара в базе данных СС.
            16. Наименование производителя товара.
            17. Модель товара.
            18. Описание товара №2.
            19. Имя файла картинки товара №1.
            20. Имя файла картинки товара №2.
            21. Порядок (сортировка).
            22. "Статус наличия", символьное отображение наличия товара из учётной системы.
            23. Дополнительная цена на товар из учётной системы. Если дополнительных цен больше одной, тогда цены перечислены через точку с запятой (;), согласно порядка установленого в справочнике дополнительных цен.
            24. Признак о том, что товар необходимо экспортировать в файл XML для рекламных площадок (яндекс маркет и т.д.). Возможные значения: 1 или 0.
            25. SEO ссылка на товар (ЧПУ).
            26. Активность товара. Возможные значения: 1 или 0.
            27. Код товара UUID (только символьные значение). UUID - Universally Unique Identifier, уникальный 128-битный идентификатор.
            28. Вес товара.
            29. Гарантия на товар.
            30. Артикул товара (внутренний).
            31. Код информационного блока (каталога). Код указывает в каком информационном блоке (каталоге) находится товар. Используется для интеграции с CMS 1C-Битрикс, если главные категории товаров располагаются в отдельных информационных блоках (каталогах). Так же можно использовать данный код информационного блока (каталога) для создания мульти сайтового магазина, например на одном домене расположить одни категории, на другом поддомене другие категории.
            32. Код категории товара UUID (только символьные значение). UUID - Universally Unique Identifier, уникальный 128-битный идентификатор.
            33. Флаг товара "Л" - лучшая покупка.
            34. Флаг товара "СЦ" - специальная цена.
            35. Флаг товара "Р" - распродажа.
            36. Флаг товара "Н" - новинка.
            37. Артикул поставщика товара.
            38. Габарит (Размер).
            39. Наименование товара. Из программы ПЛИ экспортируются значения из поля "Наименование №1".
            40. Ид статуса наличия. Возможные значения: 1 - Есть, 2 - Нет, 3 - Под заказ.
            41. Стоимость доставки товара
            42. Цвет товара
            43. Единица измерения
            44. Дата обновления цены товара
            45. Код прайс-листа
            46. Цена (себестоимость)
            47. Флаг товара "ПС" - подтверждённый склад
            48. Рекомендованная цена
            49. Объём
            50. Старая цена
            51. Код родительского товара к которому привязан товар-атрибут.
            52. UUID код родительского товара к которому привязан товар-атрибут.
            * - обязательное заполнение значения для этого поля в файле CSV
         */
            #endregion

            try
            {
                string artNo;
                if (_typeSelectArtNo)
                {
                    artNo = temp[3].Trim();
                }
                else
                {
                    artNo = temp[1].Trim();
                }

                if (string.IsNullOrEmpty(artNo))
                {
                    LogInvalidData("Отсутствует артикул товара в выбранном поле импортируемого файла. Название: " + temp[4]);
                    return;
                }

                float price = 0;
                float.TryParse(temp[5], NumberStyles.Any, CultureInfo.InvariantCulture, out price);

                float shippingPrice = 0;
                float.TryParse(temp[40], NumberStyles.Any, CultureInfo.InvariantCulture, out shippingPrice);

                float supplyPrice = 0;
                float.TryParse(temp[45], NumberStyles.Any, CultureInfo.InvariantCulture, out supplyPrice);

                int amount = 0;
                int.TryParse(temp[13], out amount);

                int sortOrder = 0;
                int.TryParse(temp[20], out sortOrder);

                var urlPath = temp[24].IsNullOrEmpty()
                    ? UrlService.GetAvailableValidUrl(0, ParamType.Product, temp[1])
                    : temp[24];

                float weight = 0;
                float.TryParse(temp[27], out weight);

                Color color = null;
                if (!string.IsNullOrEmpty(temp[41]))
                {
                    color = ColorService.GetColor(temp[41]);
                    if (color == null)
                    {
                        color = new Color { ColorCode = "000", ColorName = temp[41], SortOrder = 0 };
                        color.ColorId = ColorService.AddColor(color);
                    }
                }
                else
                {
                    var productElbuzKey = temp[1];

                    if (colorDictionary.ContainsKey(productElbuzKey))
                    {
                        var colorName = colorDictionary[productElbuzKey];

                        color = ColorService.GetColor(colorName);
                        if (color == null)
                        {
                            color = new Color { ColorCode = "000", ColorName = colorName, SortOrder = 0 };
                            color.ColorId = ColorService.AddColor(color);
                        }
                    }
                }

                Size size = null;
                if (!string.IsNullOrEmpty(temp[37]))
                {
                    size = SizeService.GetSize(temp[37]);
                    if (size == null)
                    {
                        size = new Size { SizeName = temp[37], SortOrder = 0 };
                        size.SizeId = SizeService.AddSize(size);
                    }
                }
                else
                {
                    var productElbuzKey = temp[1];

                    if (sizeDictionary.ContainsKey(productElbuzKey))
                    {
                        var sizeName = sizeDictionary[productElbuzKey];

                        size = SizeService.GetSize(sizeName);
                        if (size == null)
                        {
                            size = new Size { SizeName = sizeName, SortOrder = 0 };
                            size.SizeId = SizeService.AddSize(size);
                        }
                    }
                }

                // add p
                productDictionary.Add(temp[1], temp[3]);

                var product = new Product
                {
                    ArtNo = artNo,
                    Name = temp[4],
                    Offers = new List<Offer>
                    {
                        new Offer
                        {
                            ArtNo = !string.IsNullOrWhiteSpace(temp[3]) ? temp[3] : temp[3] + "-" + temp[1],
                            BasePrice = price,
                            Amount = amount,
                            SupplyPrice = supplyPrice,
                            ColorID = color != null ? color.ColorId : (int?) null,
                            SizeID = size != null ? size.SizeId : (int?) null
                        }
                    },
                    Unit = temp[42],
                    ShippingPrice = shippingPrice,
                    Description = temp[17],
                    BriefDescription = temp[12],
                    HasMultiOffer = true,
                    Meta = new MetaInfo
                    {
                        Title = string.IsNullOrEmpty(temp[8])
                            ? SettingsSEO.ProductMetaTitle
                            : temp[8],
                        MetaDescription = string.IsNullOrEmpty(temp[9])
                            ? SettingsSEO.ProductMetaDescription
                            : temp[9],
                        MetaKeywords = string.IsNullOrEmpty(temp[10])
                            ? SettingsSEO.ProductMetaKeywords
                            : temp[10],
                        Type = MetaType.Product,
                        H1 = SettingsSEO.ProductMetaH1
                    },
                    Enabled = string.Equals(temp[25], "1"),
                    UrlPath = urlPath,
                    Weight = weight,
                    BestSeller = string.Equals(temp[32], "Л"),
                    OnSale = string.Equals(temp[34], "Р"),
                    New = string.Equals(temp[35], "Н"),
                    BrandId =
                        elbazManufacturerToAdv.ContainsKey(Convert.ToInt32(temp[7]))
                            ? elbazManufacturerToAdv[Convert.ToInt32(temp[7])]
                            : 0,
                    //MaxAmount = amount,
                    //MinAmount = 1,
                    Multiplicity = 1,
                    CurrencyID = CurrencyService.CurrentCurrency.CurrencyId
                };

                UpdateInsertProductWorker(product);
                if (product.ProductId != 0 && elbazcatToAdvcat.ContainsKey(Convert.ToInt32(temp[2])))
                {
                    //ProductService.DeleteAllProductLink(product.ProductId);
                    ProductService.AddProductLink(product.ProductId, elbazcatToAdvcat[Convert.ToInt32(temp[2])],
                        sortOrder, true);
                }
            }
            catch (Exception ex)
            {
                LogInvalidData(ex.Message + " Артикул: " + temp[3] + ", Название: " + temp[4]);
            }
        }

        private void ProcessProductPL(string[] temp)
        {
            #region Product E-Trade Content Creator Description
            /*
           Если тип записи "p", тогда колонки означают следующее:
            2. Код товара (только числовое значение). *
            3. Код категории, в которой находится товар (только числовое значение). *
            4. Артикул производителя товара.
            5. Наименование товара. *
            6. Цена товара.
            7. Код поставщика.
            8. Код производителя.
            9. Тег meta_title для товара.
            10. Тег meta_description для товара.
            11. Тег meta_keywords для товара.
            12. Признак одного из статусов товара (Спецена, Распродажа, Новинка) = 0 или 1.
            13. Описание товара.
            14. Количество.
            15. Код описания товара в базе данных СС.
            16. Наименование производителя товара.
            17. Модель товара.
            18. Описание товара №2.
            19. Имя файла картинки товара №1.
            20. Имя файла картинки товара №2.
            21. Порядок (сортировка).
            22. "Статус наличия", символьное отображение наличия товара из учётной системы.
            23. Дополнительная цена на товар из учётной системы. Если дополнительных цен больше одной, тогда цены перечислены через точку с запятой (;), согласно порядка установленого в справочнике дополнительных цен.
            24. Признак о том, что товар необходимо экспортировать в файл XML для рекламных площадок (яндекс маркет и т.д.). Возможные значения: 1 или 0.
            25. SEO ссылка на товар (ЧПУ).
            26. Активность товара. Возможные значения: 1 или 0.
            27. Код товара UUID (только символьные значение). UUID - Universally Unique Identifier, уникальный 128-битный идентификатор.
            28. Вес товара.
            29. Гарантия на товар.
            30. Артикул товара (внутренний).
            31. Код информационного блока (каталога). Код указывает в каком информационном блоке (каталоге) находится товар. Используется для интеграции с CMS 1C-Битрикс, если главные категории товаров располагаются в отдельных информационных блоках (каталогах). Так же можно использовать данный код информационного блока (каталога) для создания мульти сайтового магазина, например на одном домене расположить одни категории, на другом поддомене другие категории.
            32. Код категории товара UUID (только символьные значение). UUID - Universally Unique Identifier, уникальный 128-битный идентификатор.
            33. Флаг товара "Л" - лучшая покупка.
            34. Флаг товара "СЦ" - специальная цена.
            35. Флаг товара "Р" - распродажа.
            36. Флаг товара "Н" - новинка.
            37. Артикул поставщика товара.
            38. Габарит (Размер).
            39. Наименование товара. Из программы ПЛИ экспортируются значения из поля "Наименование №1".
            40. Ид статуса наличия. Возможные значения: 1 - Есть, 2 - Нет, 3 - Под заказ.
            41. Стоимость доставки товара
            42. Цвет товара
            43. Единица измерения
            44. Дата обновления цены товара
            45. Код прайс-листа
            46. Цена (себестоимость)
            47. Флаг товара "ПС" - подтверждённый склад
            48. Рекомендованная цена
            49. Объём
            50. Старая цена
            51. Код родительского товара к которому привязан товар-атрибут.
            52. UUID код родительского товара к которому привязан товар-атрибут.
            * - обязательное заполнение значения для этого поля в файле CSV
         */
            #endregion

            float price = 0;
            float.TryParse(temp[5], NumberStyles.Any, CultureInfo.InvariantCulture, out price);

            float shippingPrice = 0;
            float.TryParse(temp[40], NumberStyles.Any, CultureInfo.InvariantCulture, out shippingPrice);

            float supplyPrice = 0;
            float.TryParse(temp[45], NumberStyles.Any, CultureInfo.InvariantCulture, out supplyPrice);

            int amount = 0;
            int.TryParse(temp[13], out amount);
            

            var productElbuzKey = temp[1];

            Color color = null;
            if (colorDictionary.ContainsKey(productElbuzKey))
            {
                var colorName = colorDictionary[productElbuzKey];

                color = ColorService.GetColor(colorName);
                if (color == null)
                {
                    color = new Color { ColorCode = "000", ColorName = colorName, SortOrder = 0 };
                    color.ColorId = ColorService.AddColor(color);
                }
            }
            else if (!string.IsNullOrEmpty(temp[41]))
            {
                var colorName = temp[41];

                color = ColorService.GetColor(colorName);
                if (color == null)
                {
                    color = new Color { ColorCode = "000", ColorName = colorName, SortOrder = 0 };
                    color.ColorId = ColorService.AddColor(color);
                }
            }

            Size size = null;
            if (sizeDictionary.ContainsKey(productElbuzKey))
            {
                var sizeName = sizeDictionary[productElbuzKey];

                size = SizeService.GetSize(sizeName);
                if (size == null)
                {
                    size = new Size { SizeName = sizeName, SortOrder = 0 };
                    size.SizeId = SizeService.AddSize(size);
                }
            }

            var offer = new Offer
            {
                ArtNo = !string.IsNullOrWhiteSpace(temp[3]) ? temp[3] : temp[3] + "-" + temp[1],
                BasePrice = price,
                Amount = amount,
                SupplyPrice = supplyPrice,
                ColorID = color != null ? color.ColorId : (int?) null,
                SizeID = size != null ? size.SizeId : (int?) null
            };

            var o = OfferService.GetOffer(offer.ArtNo);
            if (o != null)
            {
                OfferService.UpdateOffer(offer);

                CommonStatistic.WriteLog("pl обновлен " + offer.ArtNo);
                CommonStatistic.TotalUpdateRow++;
            }
            else if (productDictionary.ContainsKey(temp[50])) // проверяем есть ли товар с таким ключем в словаре
            {
                var productArtNo = productDictionary[temp[50]];

                var product = ProductService.GetProduct(productArtNo);
                if (product != null)
                {
                    offer.ProductId = product.ProductId;
                    var needToUpdateProduct = false;

                    if (!product.HasMultiOffer)
                    {
                        product.HasMultiOffer = true;
                        needToUpdateProduct = true;
                    }

                    if (product.Offers.Any(x => x.ColorID == null && x.SizeID == null))
                    {
                        product.Offers = product.Offers.Where(x => x.ColorID != null || x.SizeID != null).ToList();
                        needToUpdateProduct = true;
                    }

                    if (product.Offers.Count <= 1 &&
                        product.Offers.Count(x => x.ColorID != null || x.SizeID != null) == 0)
                    {
                        product.HasMultiOffer = false;
                    }

                    if (needToUpdateProduct)
                        ProductService.UpdateProduct(product, false);
                    

                    var productOffer =
                        product.Offers.Find(
                            x => x.ArtNo == offer.ArtNo || (x.ColorID == offer.ColorID && x.SizeID == offer.SizeID));
                    
                    if (productOffer != null) // если у продукта уже есть такой offer, то обновляем остатки, иначе добавляем
                    {
                        productOffer.BasePrice = offer.BasePrice;
                        productOffer.Amount = offer.Amount;

                        OfferService.UpdateOffer(productOffer);

                        CommonStatistic.WriteLog("pl обновлен " + productOffer.ArtNo);
                        CommonStatistic.TotalUpdateRow++;
                    }
                    else
                    {
                        if (offer.ColorID != null || offer.SizeID != null)
                        {
                            OfferService.AddOffer(offer);

                            CommonStatistic.WriteLog("pl добавлен " + offer.ArtNo);
                            CommonStatistic.TotalAddRow++;
                        }
                        else
                        {
                            CommonStatistic.WriteLog("Ошибка pl " + offer.ArtNo + " не добавлен потому, что нет цвета размера");
                            CommonStatistic.TotalErrorRow++;
                        }
                    }
                }
            }
        }

        private static bool ProcessCategory(string[] temp, Dictionary<int, int> elbazcatToAdvcat)
        {
            #region Category E-Trade Content Creator Description
            /*
           Если тип записи "g", тогда колонки означают следующее:
            2. Код категории (только числовое значение). *
            3. Код родительской категории (только числовое значение). *
            4. Наименование категории. *
            5. Описание категории.
            6. Порядок (сортировка).
            7. Тег meta_title для категории.
            8. Тег meta_description для категории.
            9. Тег meta_keywords для категории.
            10. Имя файла картинки для категории.
            11. Полный путь категории в числовом виде (например 1/2/3).
            12. Уровень категории, в пределах дерева.
            13. Левый ключ (для использования метода хранения деревьев типа "Вложенные множества" - Nested Sets).
            14. Правый ключ (для использования метода хранения деревьев типа "Вложенные множества" - Nested Sets).
            15. Полный путь категории в символьном виде (например Категория / Подкатегория 1 / Подкатегория 2).
            16. SEO ссылка на категорию (ЧПУ).
            17. Активность категории. Возможные значения: 1 или 0.
            18. Код категории UUID (только символьное значение). UUID - Universally Unique Identifier, уникальный 128-битный идентификатор.
            19. Код родительской категории UUID (только символьное значение). UUID - Universally Unique Identifier, уникальный 128-битный идентификатор.
            20. Код информационного блока (каталога). Используется для интеграции с CMS 1C-Битрикс, если главные категории товаров располагаются в отдельных информационных блоках (каталогах). Так же можно использовать данный код информационного блока для создания мульти сайтового магазина, например на одном домене расположить одни категории, на другом поддомене другие категории.
            * - обязательное заполнение этого поля в файле CSV.
         */
            #endregion

            if (string.IsNullOrEmpty(temp[10]))
                return false;

            var categoryFullPath = string.Empty;
            var valid = true;

            foreach (var categoryIdElbuz in temp[10].Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (string.Equals(categoryIdElbuz, "0"))
                    continue;
                
                var cId = 0;

                if (!int.TryParse(categoryIdElbuz, out cId))
                    return false;

                if (elbazcatToAdvcat.ContainsKey(cId))
                {
                    var curCat = CategoryService.GetCategory(elbazcatToAdvcat[cId]);
                    if (curCat != null)
                    {
                        categoryFullPath += categoryFullPath.IsNotEmpty() ? " >> " : string.Empty;
                        categoryFullPath += string.Equals(categoryIdElbuz, temp[1])
                                                ? HttpUtility.HtmlDecode(temp[3])
                                                : curCat.Name;
                    }
                }
                else if (string.Equals(categoryIdElbuz, temp[1]))
                {
                    categoryFullPath += categoryFullPath.IsNotEmpty() ? " >> " : string.Empty;
                    categoryFullPath += HttpUtility.HtmlDecode(temp[3]);
                }
                else
                {
                    valid = false;
                    break;
                }
            }

            if (!valid && !string.IsNullOrEmpty(temp[14]))
            {
                categoryFullPath = temp[14].Replace(" / ", " >> ");
                valid = true;
            }

            if (!valid)
            {
                LogInvalidData("Ошибка в категории '" + temp[3] + "'. Возможно не найдена одна из подкатегорий.");
                return false;
            }

            var categoryId = CategoryService.SubParseAndCreateCategory("[" + categoryFullPath.Replace(";", ",") + "]", true);
            var category = CategoryService.GetCategory(categoryId);

            if (category == null)
            {
                LogInvalidData("Ошибка в категории '" + temp[3] + "'");
                return false;
            }

            var sortOrder = 0;
            Int32.TryParse(temp[5], out sortOrder);

            category.Name = HttpUtility.HtmlDecode(temp[3]).Replace(";", ",");
            category.Description = temp[4];

            if (sortOrder != 0)
                category.SortOrder = sortOrder;

            category.Meta = new MetaInfo
            {
                Title =
                    !string.IsNullOrEmpty(temp[6])
                        ? temp[6]
                        : SettingsSEO.CategoryMetaTitle,
                MetaDescription =
                    !string.IsNullOrEmpty(temp[7])
                        ? temp[7]
                        : SettingsSEO.CategoryMetaDescription,
                MetaKeywords =
                    !string.IsNullOrEmpty(temp[8])
                        ? temp[8]
                        : SettingsSEO.CategoryMetaKeywords,
                Type = MetaType.Category,
                H1 = SettingsSEO.CategoryMetaH1
            };

            category.UrlPath = !string.IsNullOrWhiteSpace(temp[15]) ? temp[15].Reduce(140) : category.UrlPath;
            category.Enabled = temp[16] == "1";

            CategoryService.UpdateCategory(category, true);

            elbazcatToAdvcat.Add(Convert.ToInt32(temp[1]), categoryId);

            return true;
        }

        private static void ProcessManufacturer(string[] temp, Dictionary<int, int> elbazManufacturerToAdv)
        {
            #region Manufacturer E-Trade Content Creator Description
            /*
           Если тип записи "m", тогда колонки означают следующее:
            2. Код производителя товара (только числовое значение). *
            3. Наименование производителя. *
            4. Имя файла картинки производителя.
            5. Тег meta_title для производителя.
            6. Тег meta_description для производителя.
            7. Тег meta_keywords для производителя.
            8. Ссылка на сайт производителя.
            9. SEO ссылка на производителя (ЧПУ).
            * - обязательное заполнение этого поля в файле CSV
         */
            #endregion

            Brand brand;
            var brandId = BrandService.GetBrandIdByName(temp[2]);
            if (brandId != 0)
            {
                brand = BrandService.GetBrandById(brandId);
                if (brand.BrandId != 0)
                {
                    CommonStatistic.WriteLog("Бренд обновлен " + brand.Name);
                    CommonStatistic.TotalUpdateRow++;
                }
            }
            else
            {
                brand = new Brand
                {
                    Name = temp[2],
                    Meta = new MetaInfo
                    {
                        Title =
                            !string.IsNullOrEmpty(temp[4])
                                ? temp[4]
                                : SettingsSEO.DefaultMetaTitle,
                        MetaDescription =
                            !string.IsNullOrEmpty(temp[5])
                                ? temp[5]
                                : SettingsSEO.DefaultMetaDescription,
                        MetaKeywords =
                            !string.IsNullOrEmpty(temp[6])
                                ? temp[6]
                                : SettingsSEO.DefaultMetaKeywords,
                        Type = MetaType.Brand,
                        H1 = "#BRAND_NAME#"
                    },
                    BrandSiteUrl = temp[7],
                    UrlPath = temp[8].IsNullOrEmpty()
                        ? UrlService.GetAvailableValidUrl(0, ParamType.Product, temp[2])
                        : temp[8],
                    Enabled = true

                };
                brand.BrandId = BrandService.AddBrand(brand);
                if (brand.BrandId != 0)
                {
                    CommonStatistic.WriteLog("Бренд добавлен " + brand.Name);
                    CommonStatistic.TotalAddRow++;
                }
            }

            if (brand.BrandId != 0)
            {
                elbazManufacturerToAdv.Add(Convert.ToInt32(temp[1]), brand.BrandId);
                return;
            }
            LogInvalidData("InvalidData in manufaturer:" + temp[2]);
            CommonStatistic.TotalErrorRow++;
        }

        /// <summary>
        /// af - означает что строка описывает список дополнительных полей для товаров (атрибутов)
        /// </summary>
        /// <param name="temp"></param>
        private void ProcessColorSizeOptions(string[] temp)
        {
            /*
             Обрабатываем опции (цвет и размер)
                1. Код товара (только числовое значение)
                2. Код товара UUID (только символьные значение). UUID - Universally Unique Identifier, уникальный 128-битный идентификатор
                3. Символьный идентификатор поля
                4. Название поля
                5. Значение поля для товара
                6. Примечание поля (описание)
                7. Код категории, в которой находится товар (только числовое значение)
                8. UUID код категории, в которой находится товар (только числовое значение)
             */

            if (String.IsNullOrWhiteSpace(temp[5]))
                return;
            var optionName = temp[4].ToLower();

            if (optionName == "цвет")
            {
                colorDictionary.Add(temp[1], temp[5]);
            }
            else if (optionName == "размер")
            {
                sizeDictionary.Add(temp[1], temp[5]);
            }
        }

        private static void UpdateInsertProductWorker(Product product)
        {
            bool addingNew;
            Product p = null;

            if (string.IsNullOrEmpty(product.ArtNo))
            {
                addingNew = true;
                // to do generate new ArtNO
                product.ArtNo = null;                
            }
            else
            {
                p = ProductService.GetProduct(product.ArtNo);
                addingNew = p == null;                
            }

            product.ModifiedBy = "elbuz";

            if (!addingNew)
            {
                p.New = product.New;
                p.BestSeller = product.BestSeller;
                p.OnSale = product.OnSale;
                p.Recomended = product.Recomended;
                p.ManufacturerWarranty = product.ManufacturerWarranty;
                p.Description = product.Description.IsNullOrEmpty() ? p.Description : product.Description;
                p.BriefDescription = product.BriefDescription.IsNullOrEmpty() ? p.BriefDescription : product.BriefDescription;
                p.Name = product.Name.IsNullOrEmpty() ? p.Name : product.Name;
                p.Unit = product.Unit.IsNullOrEmpty() ? p.Unit : product.Unit;
                p.ShippingPrice = product.ShippingPrice > 0f ? product.ShippingPrice : p.ShippingPrice;
                p.Meta = product.Meta != null
                    ? product.Meta = new MetaInfo()
                    {
                        Title = product.Meta.Title,
                        MetaDescription = product.Meta.MetaDescription,
                        MetaKeywords = product.Meta.MetaKeywords,
                        Type = MetaType.Product,
                        H1 = SettingsSEO.ProductMetaH1,
                        ObjId = p.ProductId
                    }
                    : p.Meta;
                p.Enabled = product.Enabled;
                p.Weight = product.Weight > 0f ? product.Weight : p.Weight;
                p.Multiplicity = product.Multiplicity > 0f ? product.Multiplicity : p.Multiplicity;
                p.BrandId = product.BrandId;

                int index = -1;
                if (product.Offers.Count > 0)
                {
                    if ((index = p.Offers.FindIndex(x => string.Equals(x.ArtNo, product.Offers.First().ArtNo))) > -1)
                    {
                        p.Offers[index] = product.Offers.First();
                        p.Offers[index].ProductId = p.ProductId;
                    }
                    else
                    {
                        p.Offers.Add(product.Offers.First());
                    }
                    p.HasMultiOffer = p.Offers.Count > 1 || p.Offers.Count(x => x.ColorID != null || x.SizeID != null) > 0;
                }

                if (p.Offers == null)
                    p.Offers = new List<Offer>();

                p.Offers[0].Main = !p.Offers.Any(item => item.Main);

                foreach (var offer in p.Offers)
                {
                    offer.ProductId = p.ProductId;
                }

                ProductService.UpdateProductByArtNo(p, false);

                if (p.ProductId == 0)
                {
                    CommonStatistic.WriteLog("Продукт не обновлен " + product.ArtNo + " offerArtno " +
                    (product.Offers != null && product.Offers.Count > 0 ? product.Offers[0].ArtNo : string.Empty));
                    CommonStatistic.TotalErrorRow++;
                    return;
                }

                CommonStatistic.WriteLog("Продукт обновлен " + product.ArtNo + " offerArtno " +
                    (product.Offers != null && product.Offers.Count > 0 ? product.Offers[0].ArtNo : string.Empty));
                CommonStatistic.TotalUpdateRow++;
            }
            else
            {
                product.Offers[0].Main = true;
                try
                {
                    ProductService.AddProduct(product, false);
                }
                catch (Exception ex)
                {
                    CommonStatistic.WriteLog("Ошибка " + product.ArtNo + " Error " + ex.InnerException + " " + ex.StackTrace);
                }

                if (product.ProductId != 0)
                {

                    CommonStatistic.WriteLog("Продукт добавлен " + product.ArtNo + " offerArtno " +
                                             product.Offers[0].ArtNo);
                    CommonStatistic.TotalAddRow++;
                }
                else
                {
                    CommonStatistic.WriteLog("Продукт не добавлен " + product.ArtNo + " offerArtno " +
                                             product.Offers[0].ArtNo);
                    CommonStatistic.TotalErrorRow++;
                }
            }

            if (CommonStatistic.TotalRow == CommonStatistic.RowPosition)
            {
                CommonStatistic.IsRun = false;
                LuceneSearch.CreateAllIndexInBackground();
                ProductService.PreCalcProductParamsMassInBackground();
            }
        }
        
        private static void LogInvalidData(string message)
        {
            CommonStatistic.WriteLog(message);
            CommonStatistic.TotalErrorRow++;
        }
    }
}