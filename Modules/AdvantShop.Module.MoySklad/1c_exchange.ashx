<%@ WebHandler Language="C#" Class="_1c_exchange" %>

using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.SQL;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.FullSearch;
using AdvantShop.Helpers;
using AdvantShop.Diagnostics;
using AdvantShop.Configuration;
using AdvantShop.Module.MoySklad;
using AdvantShop.Repository.Currencies;
using AdvantShop.Saas;
using AdvantShop.Security;
using AdvantShop.Orders;


public class _1c_exchange : IHttpHandler
{
    private readonly bool ModuleEnabled = ModulesRepository.IsActiveModule(MoySklad.GetModuleStringId()) && ModulesRepository.IsInstallModule(MoySklad.GetModuleStringId());
    private readonly Guid InternetUserGuid = new Guid("1E684980-E140-49B9-830B-C462D4A84041");//Для тех кто перешел с 4.0 со старой базой
    private static Regex rg = new Regex("[^a-zA-z0-9_-]+", RegexOptions.Singleline);
    private static readonly string FileNameOrders = "from.xml";
    private static readonly string FileNameOrderIds = "idssend.txt";
    private readonly int CountSendServiceOrders = ModuleSettingsProvider.GetSettingValue<int>("MoySkladCSSO", MoySklad.GetModuleStringId()); //100;
    private readonly string[] ListNoloadProp = ModuleSettingsProvider.GetSettingValue<string>("MoySkladPropNoLoad", MoySklad.GetModuleStringId()).Split(new string[] { "[;]" }, StringSplitOptions.RemoveEmptyEntries);

    private readonly string PropWeight = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNamePropWeight", MoySklad.GetModuleStringId());
    private readonly string PropSize = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNamePropSize", MoySklad.GetModuleStringId());
    private readonly string PropBrand = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNamePropBrand", MoySklad.GetModuleStringId());
    private readonly string PropDiscount = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNamePropDiscount", MoySklad.GetModuleStringId());
    private readonly string PropGtin = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNamePropGtin", MoySklad.GetModuleStringId());
    private readonly string PropBarCode = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNameBarCode", MoySklad.GetModuleStringId());

    private readonly string CharactColor = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNameCharactColor", MoySklad.GetModuleStringId());
    private readonly string CharactSize = ModuleSettingsProvider.GetSettingValue<string>("MoySkladNameCharactSize", MoySklad.GetModuleStringId());
    private readonly bool UseZip = ModuleSettingsProvider.GetSettingValue<bool>("UseZip", MoySklad.GetModuleStringId());
    private object OrderSync = new object();

    private static readonly string _moduleName = MoySklad.GetModuleStringId();

    private static Regex _invalidXMLChars = new Regex(@"(?<![\uD800-\uDBFF])[\uDC00-\uDFFF]|[\uD800-\uDBFF](?![\uDC00-\uDFFF])|[\x00-\x08\x0B\x0C\x0E-\x1F\x7F-\x9F\uFEFF\uFFFE\uFFFF]", RegexOptions.Compiled);

    private readonly bool IsWriteLog = false;

    public void ProcessRequest(HttpContext context)
    {
        //try
        //{
        if (ModuleEnabled)
        {
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = Encoding.GetEncoding("windows-1251");

            var tempstrex = string.Empty;
            bool isAuthorization = false;

            if (CustomerContext.CurrentCustomer.IsAdmin)
            {
                isAuthorization = true;
                tempstrex = "cookie";
            }
            if (!isAuthorization)
            {
                var authorization = context.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authorization) && authorization.Contains("Basic "))
                {
                    var userdate = Encoding.ASCII.GetString(Convert.FromBase64String(authorization.Replace("Basic ", string.Empty)));
                    var charIndex = userdate.IndexOf(':');
                    if (charIndex > 0)
                    {
                        var userName = userdate.Substring(0, charIndex);
                        var userPassword = userdate.Substring(charIndex + 1, userdate.Length - charIndex - 1);
                        if (AuthorizeService.SignIn(userName, userPassword, false, false))//if (userName.Equals(login) && userPassword.Equals(password))
                        {
                            if (CustomerService.GetCustomerByEmailAndPassword(userName, userPassword, false).IsAdmin)
                            {
                                isAuthorization = true;
                                tempstrex = "Basic";
                            }
                        }
                    }
                }
            }

            if (isAuthorization)
            {
                if (("sale".Equals(context.Request["type"], StringComparison.InvariantCultureIgnoreCase)
                        && "checkauth".Equals(context.Request["mode"], StringComparison.InvariantCultureIgnoreCase))
                    || ("catalog".Equals(context.Request["type"], StringComparison.InvariantCultureIgnoreCase)
                        && "checkauth".Equals(context.Request["mode"], StringComparison.InvariantCultureIgnoreCase)))
                {
                    ReturnCookieAuth(context);
                }
                else if ("sale".Equals(context.Request["type"], StringComparison.InvariantCultureIgnoreCase)
                    && "success".Equals(context.Request["mode"], StringComparison.InvariantCultureIgnoreCase))
                {
                    CheckOrders();
                }
                else if ("sale".Equals(context.Request["type"], StringComparison.InvariantCultureIgnoreCase)
                    && "query".Equals(context.Request["mode"], StringComparison.InvariantCultureIgnoreCase))
                {
                    ReturnOrders(context);
                }
                else if (("sale".Equals(context.Request["type"], StringComparison.InvariantCultureIgnoreCase)
                        && "init".Equals(context.Request["mode"], StringComparison.InvariantCultureIgnoreCase))
                    || ("catalog".Equals(context.Request["type"], StringComparison.InvariantCultureIgnoreCase)
                        && "init".Equals(context.Request["mode"], StringComparison.InvariantCultureIgnoreCase)))
                {
                    ReturnParametersInputFiles(context);
                }
                else if (("sale".Equals(context.Request["type"], StringComparison.InvariantCultureIgnoreCase)
                        && "file".Equals(context.Request["mode"], StringComparison.InvariantCultureIgnoreCase))
                    || ("catalog".Equals(context.Request["type"], StringComparison.InvariantCultureIgnoreCase)
                        && "file".Equals(context.Request["mode"], StringComparison.InvariantCultureIgnoreCase)))
                {
                    SaveInputFile(context);
                }
                else if ("catalog".Equals(context.Request["type"], StringComparison.InvariantCultureIgnoreCase)
                        && "import".Equals(context.Request["mode"], StringComparison.InvariantCultureIgnoreCase))
                {
                    ParsingFile(context);
                }
                else
                {
                    Debug.LogError(string.Format("Moy sklad: {0}{1}", tempstrex, context.Request.Url));
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
            }
        }
        else
        {
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
        }
        //}
        //    catch (Exception ex)
        //    {
        //        context.Response.Write(string.Format("failure\n{0}", ex.Message));
        //        Debug.LogError(ex, ex.Message);
        //    }
    }

    private void ReturnCookieAuth(HttpContext context)
    {
        context.Response.Write("success\n");
        context.Response.Write(string.Format("{0}\n", System.Web.Security.FormsAuthentication.FormsCookieName));
        context.Response.Write(string.Format("{0}\n",
            context.Response.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName].Value));
    }

    private void CheckOrders()
    {
        lock (OrderSync)
        {
            var strIds = File.ReadAllText(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\" + FileNameOrderIds);
            foreach (var id in strIds.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                MoySklad.SetSendService(int.Parse(id), false);

            FileHelpers.DeleteFile(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\" + FileNameOrderIds);
        }
    }

    private void ReturnOrders(HttpContext context)
    {
        lock (OrderSync)
        {
            context.Response.ContentType = "text/xml";
            FileHelpers.CreateDirectory(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\");
            FileHelpers.DeleteFile(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\" + FileNameOrders);

            var listIds = GenerateFileCMLOrders(FileNameOrders);
            context.Response.WriteFile(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\" + FileNameOrders, false);

            var strIds = new StringBuilder(string.Empty);
            listIds.ForEach(id => strIds.AppendFormat("{0};", id));

            FileHelpers.DeleteFile(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\" + FileNameOrderIds);
            File.WriteAllText(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\" + FileNameOrderIds,
                strIds.ToString());
        }
    }

    private void ReturnParametersInputFiles(HttpContext context)
    {
        context.Response.Write(string.Format("zip={0}\n", UseZip ? "yes" : "no"));
        context.Response.Write(string.Format("file_limit={0}\n", 4 * 1024 * 1024));

        if (!MoySklad.ImportStatisticMoySkladRequest.IsRun)
        {
            FileHelpers.CreateDirectory(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\");

            FileHelpers.DeleteDirectory(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\syncfiles\");
            FileHelpers.CreateDirectory(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\syncfiles\");

            FileHelpers.DeleteDirectory(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\lastsyncfiles\");
            FileHelpers.CreateDirectory(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\lastsyncfiles\");

            MoySklad.ClearProductSuncMoysklad();
            MoySklad.ClearOfferSuncMoysklad();
        }
    }

    private void SaveInputFile(HttpContext context)
    {
        if (!MoySklad.ImportStatisticMoySkladRequest.IsRun)
        {
            string filename = context.Request["filename"];

            FileHelpers.CreateDirectory(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\");
            FileHelpers.CreateDirectory(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\syncfiles\");
            FileHelpers.CreateDirectory(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\lastsyncfiles\");

            // файл может отправляться частями
            //FileHelpers.DeleteFile(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\syncfiles\" + filename);

            int bufferSize = 255;
            byte[] buffer = new byte[bufferSize];
            int countRead = 1;
            using (
                var wr = new FileStream(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\syncfiles\" + filename,
                    FileMode.Append))
            {
                while (countRead > 0)
                {
                    countRead = context.Request.InputStream.Read(buffer, 0, bufferSize);
                    if (countRead == 0)
                        break;
                    wr.Write(buffer, 0, countRead);
                }
            }

            var extension = VirtualPathUtility.GetExtension(filename);
            if (!string.IsNullOrEmpty(extension) && extension.ToLower() == ".zip")
            {
                if (FileHelpers.UnZipFile(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\syncfiles\" + filename))
                {
                    if (File.Exists(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\syncfiles\" + filename))
                        File.Copy(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\syncfiles\" + filename,
                            SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\lastsyncfiles\" + filename, true);

                    FileHelpers.DeleteFile(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\syncfiles\" + filename);
                }
            }

            MoySklad.ImportStatisticMoySkladRequest.IsCompleted = true;

            context.Response.Write("success\n");
        }
        else
        {
            context.Response.Write("failure\nСервис занят обработкой файла.");
        }
    }

    private void ParsingFile(HttpContext context)
    {
        if (!MoySklad.ImportStatisticMoySkladRequest.IsRun)
        {
            if (MoySklad.ImportStatisticMoySkladRequest.IsCompleted)
            {
                string filename = context.Request["filename"];
                if (File.Exists(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\syncfiles\" + filename))
                {
                    //var listFiles = new List<string>();
                    //listFiles.Add(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\syncfiles\" + filename);
                    var file = SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\syncfiles\" + filename;

                    //------------------------

                    //foreach (var file in listFiles)
                    //{
                    //if (!string.IsNullOrEmpty(file) && File.Exists(file))
                    //{
                    if (Path.GetFileName(file).Contains("import"))
                    {
                        MoySklad.ImportStatisticMoySkladRequest.Init(SettingsGeneral.AbsolutePath +
                                                                     @"\App_Data\filesmysklad\import-log.txt");
                        MoySklad.ImportStatisticMoySkladRequest.IsRun = true;

                        var tr = new Thread(ProcessingImportFile) { Name = "ProcessingImportFile" };
                        MoySklad.ImportStatisticMoySkladRequest.ThreadImport = tr;
                        tr.Start(file);

                        context.Response.Write(
                            string.Format("progress\n.Обработано {0} из {1}. Добавлено {2}. Обновлено {3}. Ошибок {4}.",
                                MoySklad.ImportStatisticMoySkladRequest.RowPosition,
                                MoySklad.ImportStatisticMoySkladRequest.TotalRow,
                                MoySklad.ImportStatisticMoySkladRequest.TotalAddRow,
                                MoySklad.ImportStatisticMoySkladRequest.TotalUpdateRow,
                                MoySklad.ImportStatisticMoySkladRequest.TotalErrorRow));
                    }
                    else if (Path.GetFileName(file).Contains("offers"))
                    {
                        MoySklad.ImportStatisticMoySkladRequest.Init(SettingsGeneral.AbsolutePath +
                                                                     @"\App_Data\filesmysklad\offers-log.txt");
                        MoySklad.ImportStatisticMoySkladRequest.IsRun = true;

                        var tr = new Thread(ProcessingOffersFile) { Name = "ProcessingOffersFile" };
                        MoySklad.ImportStatisticMoySkladRequest.ThreadImport = tr;
                        tr.Start(file);

                        context.Response.Write(
                            string.Format("progress\n.Обработано {0} из {1}. Добавлено {2}. Обновлено {3}. Ошибок {4}.",
                                MoySklad.ImportStatisticMoySkladRequest.RowPosition,
                                MoySklad.ImportStatisticMoySkladRequest.TotalRow,
                                MoySklad.ImportStatisticMoySkladRequest.TotalAddRow,
                                MoySklad.ImportStatisticMoySkladRequest.TotalUpdateRow,
                                MoySklad.ImportStatisticMoySkladRequest.TotalErrorRow));
                    }
                    else
                    {
                        context.Response.Write(string.Format("failure\nФайл \"{0}\" не обработан.",
                            Path.GetFileName(file)));
                    }
                }
                else
                {
                    //context.Response.Write(string.Format("failure\nФайл \"{0}\" не найден.", Path.GetFileName(filename)));
                    context.Response.Write(MoySklad.ImportStatisticMoySkladRequest.IsFaild
                                            ? string.Format("failure\nФайл \"{0}\" не найден.", Path.GetFileName(filename))
                                            : "success\n");

                }
            }
            else
            {
                context.Response.Write(MoySklad.ImportStatisticMoySkladRequest.IsFaild
                    ? "failure\nФайл обработан c ошибкой."
                    : "success\n");

                MoySklad.ImportStatisticMoySkladRequest.IsCompleted = true;
            }
        }
        else
            context.Response.Write(
                string.Format("progress\n.Обработано {0} из {1}. Добавлено {2}. Обновлено {3}. Ошибок {4}.",
                    MoySklad.ImportStatisticMoySkladRequest.RowPosition,
                    MoySklad.ImportStatisticMoySkladRequest.TotalRow, MoySklad.ImportStatisticMoySkladRequest.TotalAddRow,
                    MoySklad.ImportStatisticMoySkladRequest.TotalUpdateRow,
                    MoySklad.ImportStatisticMoySkladRequest.TotalErrorRow));
    }

    private void WriteLog(string message)
    {
        if (IsWriteLog)
            MoySklad.ImportStatisticMoySkladRequest.WriteLog(string.Format("{0} - {1}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss:FFFF"), message));
    }

    /// <summary>
    /// Обработка файла с ценами
    /// </summary>
    /// <param name="data">Путь к файлу</param>
    private void ProcessingOffersFile(object data)
    {
        try
        {
            var moySklad = new MoySklad();

            if (moySklad.UpdateOnlyProducts)
            {
                WriteLog("Включена опция обновлять только продукты. Остатки не обновляются");
                MoySklad.ImportStatisticMoySkladRequest.IsRun = false;
                return;
            }

            string file = (string)data;
            var listTypePrice = new Dictionary<string, string>();
            var retailPriceName = ModuleSettingsProvider.GetSettingValue<string>("MoySkladRetailPriceName", MoySklad.GetModuleStringId());
            retailPriceName = string.IsNullOrWhiteSpace(retailPriceName) ? "розничная" : retailPriceName.ToLower();


            WriteLog("Загрузка файла начата");

            var xDoc = XDocument.Parse(GetFileText(file));
            var eltypeprice =
                xDoc.XPathSelectElement("//КоммерческаяИнформация/ПакетПредложений/ТипыЦен");
            foreach (var elprice in eltypeprice.Elements("ТипЦены"))
            {
                listTypePrice.Add(elprice.Element("Ид").Value, elprice.Element("Наименование").Value.ToLower());
            }

            var elements = xDoc.XPathSelectElements("//КоммерческаяИнформация/ПакетПредложений/Предложения/Предложение");
            MoySklad.ImportStatisticMoySkladRequest.TotalRow = elements.Count();
            var colors = ColorService.GetAllColors();
            var sizes = SizeService.GetAllSizes();

            foreach (var element in elements)
            {
                XElement temp = null;
                Product product = null;

                string initArt = string.Empty;
                string initName = string.Empty;

                string offerMoyskladId = null;
                Offer offer = null;

                if(element.Element("Артикул") != null && !string.IsNullOrEmpty(element.Element("Артикул").Value))
                {
                    initArt = element.Element("Артикул").Value;
                }

                if (element.Element("Ид") != null)
                {
                    var ids = element.Element("Ид").Value.Split(new char[] { '#' }, 2);
                    product = MoySklad.GetProductByMoyskladId(ids[0]);
                    offerMoyskladId = ids.Length > 1 ? ids[1] : ids[0];
                    offer = MoySklad.GetOfferByMoyskladId(offerMoyskladId);
                }

                if (element.Element("Наименование") != null && product == null)
                {
                    //product = MoySklad.GetProductByName(element.Element("Наименование").Value);
                    initName = element.Element("Наименование").Value;
                }

                if (product != null)
                {
                    string nameSize = !string.IsNullOrEmpty(CharactSize) ? CharactSize : "Размер";
                    string nameColor = !string.IsNullOrEmpty(CharactColor) ? CharactColor : "Цвет";
                    int? valueSizeId = null;
                    int? valueColorId = null;
                    bool errCharact = false;

                    var elcharacts = element.Element("ХарактеристикиТовара");
                    if (elcharacts != null)
                    {
                        foreach (var elcharact in elcharacts.Elements("ХарактеристикаТовара"))
                        {
                            var nameCharact = elcharact.Element("Наименование").Value;
                            if (nameCharact.Equals(nameSize, StringComparison.InvariantCultureIgnoreCase) ||
                                nameCharact.Equals(nameColor, StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (nameCharact.Equals(nameSize, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    var valSize = elcharact.Element("Значение").Value;
                                    var size = sizes.FirstOrDefault(s => s.SizeName.Equals(valSize, StringComparison.InvariantCultureIgnoreCase));
                                    valueSizeId = size != null ? size.SizeId : (int?)null;

                                    if (!valueSizeId.HasValue)
                                    {
                                        valueSizeId = SizeService.AddSize(new Size
                                        {
                                            SizeName = valSize,
                                            SortOrder = 0
                                        });
                                        sizes = SizeService.GetAllSizes();
                                    }

                                }
                                else if (nameCharact.Equals(nameColor, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    var valColor = elcharact.Element("Значение").Value;
                                    var color = colors.FirstOrDefault(c => c.ColorName.Equals(valColor, StringComparison.InvariantCultureIgnoreCase));
                                    valueColorId = color != null ? color.ColorId : (int?)null;

                                    if (!valueColorId.HasValue)
                                    {
                                        valueColorId = ColorService.AddColor(new Color
                                        {
                                            ColorName = valColor,
                                            ColorCode = "#000000",
                                            SortOrder = 0
                                        });
                                        colors = ColorService.GetAllColors();
                                    }
                                }
                            }
                            else
                            {
                                errCharact = true;
                            }
                        }
                    }

                    //if (!errCharact)
                    //{
                    if (offer == null)
                    {
                        offer =
                            product.Offers.FirstOrDefault(
                                o => o.ColorID == valueColorId && o.SizeID == valueSizeId);

                        if (offer != null && offer.OfferId > 0 && !string.IsNullOrEmpty(offerMoyskladId))
                            MoySklad.AddOfferExternalId(offerMoyskladId, offer.OfferId);
                    }

                    if (offer == null)
                        offer = new Offer()
                        {
                            ArtNo =
                                GetAvailableValidArtNoOffer(
                                    !string.IsNullOrEmpty(initArt)
                                        ? initArt
                                        : (!string.IsNullOrEmpty(offerMoyskladId) ? offerMoyskladId : product.ArtNo),
                                    productId: product.ProductId),

                            Main = !OfferService.GetProductOffers(product.ProductId).Any(),
                            ProductId = product.ProductId
                        };

                    offer.ColorID = valueColorId;
                    offer.SizeID = valueSizeId;


                    if (!moySklad.DontChangeOfferArtnoToProductArtno &&
                        offer.OfferId > 0 &&
                        !offer.ArtNo.Equals(product.ArtNo, StringComparison.InvariantCultureIgnoreCase) &&
                        !OfferService.IsArtNoExist(product.ArtNo, offer.OfferId) &&
                        OfferService.GetProductOffers(product.ProductId).Count == 1)
                    {
                        offer.ArtNo = product.ArtNo;
                    }

                    var format = new CultureInfo(CultureInfo.InvariantCulture.LCID);
                    format.NumberFormat.NumberDecimalSeparator = ".";

                    var elprices = element.Element("Цены");
                    if (elprices != null)
                    {
                        foreach (var elprice in elprices.Elements("Цена"))
                        {
                            var temptypeprice = elprice.Element("ИдТипаЦены").Value;

                            if (listTypePrice.ContainsKey(temptypeprice))
                                if (string.Equals(listTypePrice[temptypeprice], retailPriceName, StringComparison.InvariantCultureIgnoreCase) || // "розничная"
                                    string.Equals(listTypePrice[temptypeprice], "закупочная", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    float price = 0F;
                                    string codeVal = string.Empty;
                                    float coeff = 1F;

                                    temp = elprice.Element("ЦенаЗаЕдиницу");
                                    if (temp != null && !string.IsNullOrEmpty(temp.Value))
                                        price = float.Parse(temp.Value, format);

                                    temp = elprice.Element("Валюта");
                                    if (temp != null && !string.IsNullOrEmpty(temp.Value))
                                        codeVal = temp.Value;

                                    temp = elprice.Element("Коэффициент");
                                    if (temp != null && !string.IsNullOrEmpty(temp.Value))
                                        coeff = float.Parse(temp.Value, format);

                                    price = (float)Math.Round(price * coeff, 2);

                                    if (string.Equals(listTypePrice[temptypeprice], retailPriceName, StringComparison.InvariantCultureIgnoreCase)) // "розничная"
                                    {
                                        if (!moySklad.NotUpdatePrice)
                                        {
                                            offer.BasePrice = price;
                                        }

                                        var currency = new Currency();
                                        var allCurrencies = CurrencyService.GetAllCurrencies(true);
                                        int moduleSettingCurrency = ModuleSettingsProvider.GetSettingValue<int>("MoySkladImportCurrencyInProdyuct", _moduleName);

                                        currency = allCurrencies.FirstOrDefault(c => c.CurrencyId == moduleSettingCurrency) ??
                                                allCurrencies.FirstOrDefault(c => c.NumIso3.ToString() == codeVal) ??
                                                allCurrencies.FirstOrDefault(c => c.Iso3.ToString() == codeVal) ??
                                                CurrencyService.CurrentCurrency;

                                        product.CurrencyID = currency.CurrencyId;
                                    }
                                    else if (string.Equals(listTypePrice[temptypeprice], "закупочная", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        offer.SupplyPrice = price;
                                    }
                                }
                        }
                    }

                    temp = element.Element("Количество");
                    if (temp != null && !string.IsNullOrEmpty(temp.Value) && (!moySklad.NotUpdateAmount || offer.OfferId == 0))
                    {
                        float amount = temp.Value.TryParseFloat(); //(int)Math.Round(decimal.Parse(temp.Value, format));
                        if (amount < 0F)
                            amount = 0F;
                        offer.Amount = amount;
                    }

                    if (offer.Amount == 0 && moySklad.AvailablePreOrder)
                    {
                        product.AllowPreOrder = true;
                    }

                    if (offer.ColorID.HasValue || offer.SizeID.HasValue)
                    {
                        var wrongOffer = product.Offers.FirstOrDefault(o => !o.ColorID.HasValue && !o.SizeID.HasValue);
                        if (wrongOffer != null && wrongOffer.ArtNo == offer.ArtNo)
                        {
                            product.Offers.RemoveAll(o => o.OfferId == wrongOffer.OfferId);
                            product.Offers.Add(offer);
                        }
                        else if (wrongOffer != null)
                        {
                            product.Offers.RemoveAll(o => o.OfferId == wrongOffer.OfferId);
                            OfferService.DeleteOffer(wrongOffer.OfferId);
                        }
                    }
                    else if (product.Offers.Any(o => o.ColorID.HasValue || o.SizeID.HasValue))
                    {
                        // не добавлять оффер без модификации, если у товара есть офферы с модификациями
                        offer = null;
                    }

                    if (offer != null)
                    {
                        if (product.Offers.All(o => !o.Main))
                        {
                            offer.Main = true;
                        }

                        if (offer.OfferId <= 0)
                        {
                            if (!moySklad.DeleteOffersWithZeroAmount || offer.Amount > 0)
                            {
                                offer.OfferId = OfferService.AddOffer(offer);
                                product.Offers.Add(offer);
                                if (!string.IsNullOrEmpty(offerMoyskladId))
                                    MoySklad.AddOfferExternalId(offerMoyskladId, offer.OfferId);
                            }
                        }
                        else
                        {
                            product.Offers.RemoveAll(o => o.OfferId == offer.OfferId);
                            if (!moySklad.DeleteOffersWithZeroAmount || offer.Amount > 0)
                            {
                                product.Offers.Add(offer);
                            }
                            //OfferService.UpdateOffer(offer);
                        }

                        if (offer.OfferId > 0 && (!moySklad.DeleteOffersWithZeroAmount || offer.Amount > 0))
                            MoySklad.AddOfferSuncMoysklad(offer.OfferId);

                        if (!product.HasMultiOffer &&
                            (offer.ColorID.HasValue || offer.SizeID.HasValue || !offer.Main))
                        {
                            product.HasMultiOffer = true;
                        }
                        else if (product.HasMultiOffer && !offer.ColorID.HasValue && !offer.SizeID.HasValue &&
                                    OfferService.GetProductOffers(product.ProductId).Count <= 1)
                        {
                            product.HasMultiOffer = false;
                        }
                        //обновляем HasMultiOffer и Валюту товара
                        ProductService.UpdateProduct(product, false);
                    }
                    MoySklad.ImportStatisticMoySkladRequest.TotalUpdateRow++;
                    WriteLog(string.Format("Загружена цена и остатки товара ({0}).", product.ArtNo));
                    //}
                    //else
                    //{
                    //    WriteLog("Неверные характеристики товара. Могут присутствовать только цвет и/или размер.");
                    //    MoySklad.ImportStatisticMoySkladRequest.TotalErrorRow++;
                    //}
                }
                else
                {
                    WriteLog(string.Format("Не найден товар с артикулом \"{0}\", названием \"{1}\"", initArt, initName));
                    MoySklad.ImportStatisticMoySkladRequest.TotalErrorRow++;
                }
                MoySklad.ImportStatisticMoySkladRequest.RowPosition++;
            }

            WriteLog("Загрузка файла закончена");

            if (File.Exists(file))
                File.Copy(file, SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\lastsyncfiles\" + Path.GetFileName(file), true);

            FileHelpers.DeleteFile(file);

            int coutFilesInDir = Directory.GetFiles(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\syncfiles\").Count(fileInDir => Path.GetFileName(fileInDir).Contains("offers"));
            if (coutFilesInDir == 0)
            {
                if (moySklad.IsDeleteOfferNotSuncMoysklad)
                {
                    MoySklad.DeleteOfferNotSuncMoysklad();
                    WriteLog("Удаление не пришедших цен произведено");
                }


                if (moySklad.DeleteOffersWithZeroAmount)
                {
                    MoySklad.DeleteOffersWithZeroAmoount();
                    WriteLog("Удаление модификаций с 0 наличием завершено");
                }



                MoySklad.ClearOfferSuncMoysklad();
                ProductService.PreCalcProductParamsMassInBackground();
                WriteLog("Очистка списка пришедших цен");
            }
        }
        catch (Exception ex)
        {
            MoySklad.ImportStatisticMoySkladRequest.IsFaild = true;
            MoySklad.ImportStatisticMoySkladRequest.TotalErrorRow++;
            WriteLog(string.Format("{0} {1} {2} {3}", ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.Message : string.Empty, ex.InnerException != null ? ex.InnerException.StackTrace : string.Empty));
        }
        MoySklad.ImportStatisticMoySkladRequest.IsRun = false;
    }

    private string GetAvailableValidArtNoOffer(string artNo, int offerId = 0, int productId = 0)
    {
        var temp = artNo;
        int j = 1;
        while (j < 99)
        {
            var offerExists = OfferService.IsArtNoExist(temp.Trim(), offerId);
            if(!offerExists)
                return temp;

            temp = string.Format("{0}-{1}", MoySklad.TrimArtNo(artNo, MoySklad.MaxLenArt - 3), j++);
        }

        if (productId != 0)
        {
            temp = productId.ToString();
            j = 1;
            while (j < 99)
            {
                var offerExists = OfferService.IsArtNoExist(temp.Trim(), offerId);
                if (!offerExists)
                    return temp;

                temp = string.Format("{0}-{1}", productId, j++);
            }
        }

        return Guid.NewGuid().ToString();
    }

    private string GetAvailableValidArtNo(string artNo, int productId = 0)
    {
        var temp = artNo;
        int j = 1;
        while ( j < 99)
        {
            var id = ProductService.GetProductId(temp.Trim());
            if(id == 0 || id == productId)
                return temp;

            temp = string.Format("{0}-{1}", MoySklad.TrimArtNo(artNo, MoySklad.MaxLenArt - 3), j++);
        }
        return Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Обработка файла с данными по товарам
    /// </summary>
    /// <param name="data">Путь к файлу</param>
    private void ProcessingImportFile(object data)
    {
        //try
        //{
        var moySklad = new MoySklad();

        string file = (string)data;
        var listGroupCategory = new Dictionary<string, int>();
        var listProperty = new Dictionary<string, string>();

        WriteLog("Загрузка файла начата");

        //using (var streamReader = new StreamReader(file, true))
        //{
        var syncProperty = moySklad.SyncProperty;
        var syncDescription = moySklad.SyncDescription;

        var xDoc = XDocument.Parse(GetFileText(file));//XDocument.Load(streamReader);
        var elGroup = xDoc.XPathSelectElement("//КоммерческаяИнформация/Классификатор/Группы");
        GetGroup(ref listGroupCategory, elGroup, 0, moySklad.IsNewCategoryEnabled);

        if (syncProperty != MoySklad.EnSyncProperty.None)
        {
            if (xDoc.XPathSelectElement("//КоммерческаяИнформация/Каталог/Товары") != null)
                foreach (
                    var propElement in
                        xDoc.XPathSelectElements("//КоммерческаяИнформация/Классификатор/Свойства/Свойство"))
                {
                    var tempId = propElement.Element("Ид");
                    var tempType = propElement.Element("Наименование");
                    if (tempType != null && tempId != null && !string.IsNullOrWhiteSpace(tempType.Value) &&
                        !string.IsNullOrWhiteSpace(tempId.Value))
                        listProperty.Add(tempId.Value, tempType.Value);
                }
        }

        var elements = xDoc.XPathSelectElements("//КоммерческаяИнформация/Каталог/Товары/Товар");
        MoySklad.ImportStatisticMoySkladRequest.TotalRow = elements.Count();

        foreach (var element in elements)
        {
            //if (SaasDataService.IsSaasEnabled && ProductService.GetProductsCount() >= SaasDataService.CurrentSaasData.ProductsCount)
            //{
            //    WriteLog("Превышен лимит товаров по Вашему тарифному плану.");
            //    break;
            //}

            bool addingNew;
            Product product = null;
            string tempstr = null;

            var moyskladId = element.Element("Ид") != null ? element.Element("Ид").Value : null;
            var artNo = element.Element("Артикул") != null ? MoySklad.TrimArtNo(element.Element("Артикул").Value) : null;
            var productId = MoySklad.GetProductIdByMoyskladId(moyskladId);

            if (string.IsNullOrEmpty(moyskladId))
            {
                if (element.Element("Наименование") != null)
                {
                    //product = MoySklad.GetProductByName(element.Element("Наименование").Value);
                    addingNew = product == null;
                }
                else
                    addingNew = true;
            }
            else
            {
                //В МойСклад артикул не уникален
                //if (!string.IsNullOrEmpty(artNo) && productId <= 0)
                //{
                //    productId = ProductService.GetProductId(artNo);
                //    if (productId > 0)
                //        MoySklad.AddProductExternalId(moyskladId, productId);
                //}
                if (productId <= 0)
                {
                    productId = !string.IsNullOrEmpty(artNo) ? ProductService.GetProductId(artNo) : 0;
                    if (productId > 0)
                        MoySklad.AddProductExternalId(moyskladId, productId);
                    else
                    {
                        productId = ProductService.GetProductId(MoySklad.TrimArtNo(moyskladId));
                        if (productId > 0)
                            MoySklad.AddProductExternalId(moyskladId, productId);
                    }
                }

                product = ProductService.GetProduct(productId);

                if (product != null)
                {
                    product.ModifiedBy = "moysklad";
                }
                addingNew = product == null;
            }

            if (addingNew)
            {
                product = new Product()
                {
                    Multiplicity = 1,
                    Enabled =
                        (moySklad.UpdateEnableProduct == MoySklad.EnUpdateEnableProduct.OnlyNew ||
                         moySklad.UpdateEnableProduct == MoySklad.EnUpdateEnableProduct.Always),
                    ModifiedBy = "moysklad",
                    CurrencyID = CurrencyService.CurrentCurrency.CurrencyId
                };
            }
            else if (moySklad.UpdateEnableProduct == MoySklad.EnUpdateEnableProduct.Always)
            {
                product.Enabled = true;
            }

            if (!string.IsNullOrEmpty(artNo))
                artNo = GetAvailableValidArtNo(artNo, product.ProductId);
            else
                artNo = GetAvailableValidArtNo(moyskladId, product.ProductId);


            if (addingNew)
            {
                product.ArtNo = string.IsNullOrEmpty(artNo) ? null : artNo;
            }
            else
            {
                if (!string.IsNullOrEmpty(artNo))
                    product.ArtNo = artNo;
                else
                {
                    MoySklad.ImportStatisticMoySkladRequest.TotalErrorRow++;
                    WriteLog(string.Format("Артикул для текущего товара не может быть пустым ({0}).", moyskladId));
                    continue;
                }
            }

            tempstr = element.Element("Наименование").Value;
            product.Name = tempstr;
            var rewurl = tempstr.Trim();
            if (addingNew)
                product.UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Product,
                    rg.Replace(StringHelper.Translit((rewurl.IsNotEmpty() ? rewurl : product.ArtNo != null ? product.ArtNo.Replace(" ", "-") : string.Empty)), "-"));


            if (syncDescription != MoySklad.EnSyncDescription.None && element.Element("ЗначенияРеквизитов") != null)
                foreach (var elemdesc in element.Element("ЗначенияРеквизитов").Elements("ЗначениеРеквизита"))
                    if (string.Equals(elemdesc.Element("Наименование").Value, "полное наименование",
                                      StringComparison.InvariantCultureIgnoreCase) &&
                        elemdesc.Element("Значение") != null)
                    {
                        if ((syncDescription == MoySklad.EnSyncDescription.AddNew && addingNew) || syncDescription == MoySklad.EnSyncDescription.Always)
                            product.Description = elemdesc.Element("Значение").Value;
                    }

            if (syncDescription != MoySklad.EnSyncDescription.None && element.Element("Описание") != null)
                if ((syncDescription == MoySklad.EnSyncDescription.AddNew && addingNew) || syncDescription == MoySklad.EnSyncDescription.Always)
                    product.Description = element.Element("Описание").Value;

            //if (product.Offers.Count == 0)
            //    product.Offers.Add(new Offer());
            if (element.Element("БазоваяЕдиница") != null)
            {
                tempstr = element.Element("БазоваяЕдиница").Value;
                product.Unit = tempstr;
            }

            if (!addingNew)
            {
                //ProductService.EnableDynamicProductLinkRecalc();
                ProductService.UpdateProduct(product, false);
                //ProductService.DisableDynamicProductLinkRecalc();
                MoySklad.ImportStatisticMoySkladRequest.TotalUpdateRow++;
                WriteLog(string.Format("Обновлен основными данными товар ({0}).", product.ArtNo));
            }
            else
            {

                ProductService.AddProduct(product, false);

                if (product.ProductId > 0)
                    MoySklad.AddProductExternalId(moyskladId, product.ProductId);

                MoySklad.ImportStatisticMoySkladRequest.TotalAddRow++;
                WriteLog(string.Format("Добавлен с основными данными товар ({0}).", product.ArtNo));
            }

            if (product.ProductId > 0)
            {
                MoySklad.AddProductSuncMoysklad(product.ProductId);


                var elg = element.Element("Группы");
                if (elg != null)
                    foreach (var elid in elg.Elements("Ид"))
                        if (listGroupCategory.ContainsKey(elid.Value))
                        {
                            var cat = listGroupCategory[elid.Value];
                            if (cat > 0)
                            {
                                //ProductService.EnableDynamicProductLinkRecalc();
                                ProductService.AddProductLink(product.ProductId, cat, 0, false);
                                //ProductService.DisableDynamicProductLinkRecalc();
                                //ProductService.SetProductHierarchicallyEnabled(product.ProductId);
                                //CategoryService.SetCategoryHierarchicallyEnabled(cat);
                                WriteLog(string.Format("Товар ({0}) добавлен в категорию (id {1}).", product.ArtNo, cat));
                            }
                        }

                var elg2 = element.Element("ЗначенияСвойств");
                if (syncProperty != MoySklad.EnSyncProperty.None && elg2 != null)
                {
                    if (syncProperty == MoySklad.EnSyncProperty.OneToOne)
                    {
                        PropertyService.DeleteProductProperties(product.ProductId);
                        WriteLog(string.Format("Удалены свойства товар ({0}).", product.ArtNo));
                    }

                    var updateProduct = false;

                    foreach (var elid in elg2.Elements("ЗначенияСвойства"))
                    {
                        var tempType = elid.Element("Ид");
                        var tempValue = elid.Element("Значение");


                        if (tempType != null && tempValue != null
                            && !string.IsNullOrWhiteSpace(tempType.Value) && listProperty.ContainsKey(tempType.Value)
                            && !ListNoloadProp.Any(p => p.Equals(listProperty[tempType.Value], StringComparison.InvariantCultureIgnoreCase)))
                        {
                            if (PropWeight.Equals(listProperty[tempType.Value], StringComparison.InvariantCultureIgnoreCase))
                            {
                                float weight = 0F;
                                if (!string.IsNullOrWhiteSpace(tempValue.Value) && float.TryParse(tempValue.Value.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, out weight))
                                {
                                    if (weight >= 0F)
                                    {
                                        product.Weight = weight;
                                        updateProduct = true;
                                        WriteLog(string.Format("Товару ({0}) присвоен вес.", product.ArtNo));
                                    }
                                }
                            }
                            else if (PropSize.Equals(listProperty[tempType.Value], StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (!string.IsNullOrWhiteSpace(tempValue.Value))
                                {
                                    var vals = tempValue.Value.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (vals.Length == 3 && vals.Count(v => v.IsInt()) == 3)
                                    {
                                        product.Length = vals[0].TryParseInt();
                                        product.Width = vals[1].TryParseInt();
                                        product.Height = vals[2].TryParseInt();
                                        updateProduct = true;
                                        WriteLog(string.Format("Товару ({0}) присвоены размеры.", product.ArtNo));
                                    }
                                }
                            }
                            else if (PropBrand.Equals(listProperty[tempType.Value], StringComparison.InvariantCultureIgnoreCase))
                            {
                                updateProduct = true;
                                if (!string.IsNullOrWhiteSpace(tempValue.Value))
                                {
                                    if ((product.BrandId = BrandService.GetBrandIdByName(tempValue.Value)) == 0)
                                    {
                                        product.BrandId = BrandService.AddBrand(new Brand
                                        {
                                            Enabled = true,
                                            Name = tempValue.Value,
                                            Description = tempValue.Value,
                                            UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Brand, tempValue.Value),
                                            Meta = null
                                        });
                                        WriteLog(string.Format("Создан бренд ({0}).", tempValue.Value));
                                        WriteLog(string.Format("Товару ({0}) указан бренд ({1}).", product.ArtNo, tempValue.Value));
                                    }
                                    else
                                    {
                                        WriteLog(string.Format("Товару ({0}) указан бренд ({1}).", product.ArtNo, tempValue.Value));
                                    }
                                }
                                else
                                {
                                    product.BrandId = 0;
                                    WriteLog(string.Format("У товара ({0}) удален бренд.", product.ArtNo));
                                }
                            }
                            else if (PropDiscount.Equals(listProperty[tempType.Value], StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (!string.IsNullOrWhiteSpace(tempValue.Value) && tempValue.Value.IsDecimal())
                                {
                                    var discount = tempValue.Value.TryParseFloat();

                                    if (discount >= 0F && discount <= 100F)
                                    {
                                        product.Discount = new Discount(discount, 0);
                                        updateProduct = true;
                                        WriteLog(string.Format("Товару ({0}) присвоена скидка.", product.ArtNo));
                                    }
                                }
                            }
                            else if (PropGtin != null && PropGtin.Equals(listProperty[tempType.Value], StringComparison.InvariantCultureIgnoreCase))
                            {
                                updateProduct = true;
                                if (!string.IsNullOrWhiteSpace(tempValue.Value))
                                {
                                    product.Gtin = tempValue.Value.Trim();
                                    WriteLog(string.Format("Товару ({0}) указан gtin ({1}).", product.ArtNo, tempValue.Value));
                                }
                                else
                                {
                                    product.Gtin = "";
                                    WriteLog(string.Format("У товара ({0}) удален gtin.", product.ArtNo));
                                }
                            }
                            else if (PropBarCode != null && PropBarCode.Equals(listProperty[tempType.Value], StringComparison.InvariantCultureIgnoreCase))
                            {
                                updateProduct = true;
                                if (!string.IsNullOrWhiteSpace(tempValue.Value))
                                {
                                    product.BarCode = tempValue.Value.Trim();
                                    WriteLog(string.Format("Товару ({0}) указан штрих код ({1}).", product.ArtNo, tempValue.Value));
                                }
                                else
                                {
                                    product.BarCode = "";
                                    WriteLog(string.Format("У товара ({0}) удален штрих код.", product.ArtNo));
                                }
                            }
                            else if (!string.IsNullOrWhiteSpace(tempValue.Value))
                            {
                                // inside stored procedure not thread save/ do save mode by logic 
                                SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_ParseProductProperty]",
                                                              CommandType.StoredProcedure,
                                                              new SqlParameter("@nameProperty", listProperty[tempType.Value]),
                                                              new SqlParameter("@propertyValue", tempValue.Value),
                                                              new SqlParameter("@rangeValue", tempValue.Value.TryParseFloat()),
                                                              new SqlParameter("@productId", product.ProductId),
                                                              new SqlParameter("@sort", (object)0));

                                WriteLog(string.Format("Товару ({0}) указано свойство (\"{1}\" - \"{2}\").", product.ArtNo, listProperty[tempType.Value], tempValue.Value));
                            }
                        }
                    }

                    if (updateProduct)
                        ProductService.UpdateProduct(product, false);
                }
            }
            else
            {
                WriteLog(string.Format("Product.Id = 0 с артикулом \"{0}\", названием \"{1}\"", product.ArtNo, product.Name));
                MoySklad.ImportStatisticMoySkladRequest.TotalErrorRow++;
            }
            MoySklad.ImportStatisticMoySkladRequest.RowPosition++;
        }
        //}
        WriteLog("Загрузка файла закончена");

        if (File.Exists(file))
            File.Copy(file, SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\lastsyncfiles\" + Path.GetFileName(file), true);

        FileHelpers.DeleteFile(file);

        int coutFilesInDir = Directory.GetFiles(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\syncfiles\").Count(fileInDir => Path.GetFileName(fileInDir).Contains("import"));
        if (coutFilesInDir == 0)
        {
            if (moySklad.IsSetNoEnableProductNotSuncMoysklad)
            {
                MoySklad.ProductNoEnabledNotSuncMoysklad();
                WriteLog("Отключение не пришедших товаров произведено");
            }

            MoySklad.ClearProductSuncMoysklad();
            WriteLog("Очистка списка пришедших товаров");
        }

        CategoryService.RecalculateProductsCountManual();
        CategoryService.SetCategoryHierarchicallyEnabled(0);
        LuceneSearch.CreateAllIndexInBackground();
        ProductService.PreCalcProductParamsMassInBackground();
        WriteLog("Пресчет, индексация произведены");
        //}
        //    catch (Exception ex)
        //    {
        //        MoySklad.ImportStatisticMoySkladRequest.IsFaild = true;
        //        MoySklad.ImportStatisticMoySkladRequest.TotalErrorRow++;
        //        WriteLog(string.Format("{0} {1} {2} {3}", ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.Message : string.Empty, ex.InnerException != null ? ex.InnerException.StackTrace : string.Empty));
        //    }
        MoySklad.ImportStatisticMoySkladRequest.IsRun = false;
    }

    /// <summary>
    /// Генерация файла CommercML с заказами
    /// </summary>
    /// <param name="filename">Название файла</param>
    /// <returns>Идентификаторы заказов сохраненных в файл</returns>
    private List<int> GenerateFileCMLOrders(string filename)
    {
        var res = new List<int>();
        using (var streamWriter = new StreamWriter(SettingsGeneral.AbsolutePath + @"\App_Data\filesmysklad\" + filename, false, Encoding.GetEncoding("windows-1251")))
        {
            XDocument xDoc = new XDocument(new XDeclaration("1.0", "windows-1251", string.Empty));
            //xDoc.Declaration.Version = "1.0";
            //xDoc.Declaration.Encoding = "windows-1251";
            var kominfo = new XElement("КоммерческаяИнформация");
            kominfo.Add(
                new XAttribute("ДатаФормирования", DateTime.Now.ToString("yyyy-MM-dd")),
                new XAttribute("ВерсияСхемы", "2.05"));

            var format = new CultureInfo(CultureInfo.InvariantCulture.LCID);
            format.NumberFormat.NumberDecimalSeparator = ".";

            var moySklad = new MoySklad();
            var orderPrefix = moySklad.OrderPrefix ?? "";

            foreach (var order in MoySklad.GetOrdersSendService(CountSendServiceOrders))
            {
                if (order.OrderItems == null || order.OrderCurrency == null)
                {
                    res.Add(order.OrderID);
                    continue;
                }

                var doc = new XElement("Документ");
                doc.Add(new XElement("Ид", orderPrefix + order.OrderID));
                doc.Add(new XElement("Номер", orderPrefix + order.OrderID));
                doc.Add(new XElement("Дата", order.OrderDate.ToString("yyyy-MM-dd")));
                doc.Add(new XElement("ХозОперация", "Заказ товара"));
                doc.Add(new XElement("Роль", "Продавец"));
                doc.Add(new XElement("Валюта",
                                     order.OrderCurrency.CurrencyNumCode != 0
                                         ? order.OrderCurrency.CurrencyNumCode.ToString(CultureInfo.InvariantCulture)
                                         : order.OrderCurrency.CurrencyCode));
                doc.Add(new XElement("Курс", order.OrderCurrency.CurrencyValue.ToString(format)));
                doc.Add(new XElement("Сумма", Math.Round(order.Sum, 2).ToString(format)));

                var users = new XElement("Контрагенты");

                if (order.OrderCustomer != null)
                {
                    var user = new XElement("Контрагент");
                    user.Add(order.OrderCustomer.CustomerID != InternetUserGuid
                        ? new XElement("Ид", MoySklad.GuidToString(order.OrderCustomer.CustomerID))
                        : new XElement("Ид", string.Format("advuser{0}", order.OrderID)));

                    user.Add(new XElement("Наименование",
                                          string.Format("{0} {1}", order.OrderCustomer.FirstName,
                                                        order.OrderCustomer.LastName)));
                    user.Add(new XElement("Роль", "Покупатель"));
                    user.Add(new XElement("ПолноеНаименование",
                                          string.Format("{0} {1}", order.OrderCustomer.FirstName,
                                                        order.OrderCustomer.LastName)));
                    user.Add(new XElement("Фамилия", order.OrderCustomer.LastName));
                    user.Add(new XElement("Имя", order.OrderCustomer.FirstName));
                    user.Add(new XElement("АдресРегистрации", new XElement("Представление", MoySklad.AddressToLined(order.OrderCustomer))));
                    user.Add(new XElement("Контакты",
                                          new XElement("Контакт",
                                                       new XElement("Тип", "Телефон"),
                                                       new XElement("Значение", order.OrderCustomer.Phone)),
                                          new XElement("Контакт",
                                                       new XElement("Тип", "Почта"),
                                                       new XElement("Значение", order.OrderCustomer.Email))));

                    users.Add(user);
                }

                doc.Add(users);

                doc.Add(new XElement("Время", order.OrderDate.ToString("HH:mm:ss")));
                doc.Add(new XElement("Комментарий",
                    "Заказ: " + order.Number + 
                    ". Доставка: " + order.ArchivedShippingName + ". " +
                    ". Оплата: " + order.PaymentMethodName + ".  Комментарий: " +
                    order.CustomerComment ?? "нет"));

                if (order.Taxes != null)
                {
                    var taxes = new XElement("Налоги");
                    foreach (var tax in order.Taxes)
                    {
                        taxes.Add(new XElement("Налог",
                                               new XElement("Наименование", tax.Name),
                                               new XElement("УчтеноВСумме", tax.ShowInPrice.ToString().ToLower()),
                                               new XElement("Сумма", Math.Round(tax.Sum, 2).ToString(format))));
                    }
                    doc.Add(taxes);
                }

                var tovari = new XElement("Товары");

                if (order.ShippingCost > 0)
                {
                    tovari.Add(new XElement("Товар",
                        new XElement("Ид",
                            string.Format("ORDER_DELIVERY_{0}{1}", 
                                order.ShippingMethodId, 
                                order.OrderPickPoint != null && order.OrderPickPoint.PickPointAddress != null 
                                    ? order.OrderPickPoint.PickPointId 
                                    : "")),
                        new XElement("Наименование",
                            string.Format("Доставка \"{0} {1}\"",
                                order.ArchivedShippingName,
                                order.OrderPickPoint != null && order.OrderPickPoint.PickPointAddress != null
                                    ? order.OrderPickPoint.PickPointAddress
                                    : "")),
                        new XElement("БазоваяЕдиница", "шт",
                            new XAttribute("МеждународноеСокращение", "PCE"),
                            new XAttribute("НаименованиеПолное", "Штука"),
                            new XAttribute("Код", "796")),
                        new XElement("ЦенаЗаЕдиницу",
                            Math.Round(order.ShippingCost, 2).ToString(format)),
                        new XElement("Количество", "1"),
                        new XElement("Сумма", Math.Round(order.ShippingCost, 2).ToString(format)),
                        new XElement("ЗначенияРеквизитов",
                            new XElement("ЗначениеРеквизита",
                                new XElement("Наименование", "ВидНоменклатуры"),
                                new XElement("Значение", "Услуга")),
                            new XElement("ЗначениеРеквизита",
                                new XElement("Наименование", "ТипНоменклатуры"),
                                new XElement("Значение", "Услуга")))));
                }

                if (order.PaymentCost > 0)
                {
                    tovari.Add(new XElement("Товар",
                                            new XElement("Ид",
                                                         string.Format("ORDER_PAYMENT_{0}", order.PaymentMethodId)),
                                            new XElement("Наименование",
                                                         string.Format("Комиссия метода оплаты \"{0}\"", order.PaymentMethodName)),
                                            new XElement("БазоваяЕдиница", "шт",
                                                         new XAttribute("МеждународноеСокращение", "PCE"),
                                                         new XAttribute("НаименованиеПолное", "Штука"),
                                                         new XAttribute("Код", "796")),
                                            new XElement("ЦенаЗаЕдиницу",
                                                         Math.Round(order.PaymentCost, 2).ToString(format)),
                                            new XElement("Количество", "1"),
                                            new XElement("Сумма", Math.Round(order.PaymentCost, 2).ToString(format)),
                                            new XElement("ЗначенияРеквизитов",
                                                         new XElement("ЗначениеРеквизита",
                                                                      new XElement("Наименование", "ВидНоменклатуры"),
                                                                      new XElement("Значение", "Услуга")),
                                                         new XElement("ЗначениеРеквизита",
                                                                      new XElement("Наименование", "ТипНоменклатуры"),
                                                                      new XElement("Значение", "Услуга")))));
                }

                float discountCouponSum = 0;
                if (order.Coupon != null)
                {
                    switch (order.Coupon.Type)
                    {
                        case CouponType.Fixed:
                            var productsPrice =
                                order.OrderItems.Where(p => p.IsCouponApplied).Sum(p => p.Price * p.Amount);
                            discountCouponSum = productsPrice >= order.Coupon.Value ? order.Coupon.Value : productsPrice;
                            discountCouponSum = (float)Math.Round(discountCouponSum / order.OrderItems.Where(p => p.IsCouponApplied).Sum(p => p.Amount), 2);
                            break;
                    }
                }

                foreach (var item in order.OrderItems)
                {
                    var tovar = new XElement("Товар");
                    var priceT = Math.Round(item.Price / (1 - (order.GroupDiscount / 100)), 2);

                    var moyskladId = item.ProductID != null ? MoySklad.GetMoyskladIdByProductId((int)item.ProductID) : null;
                    if (string.IsNullOrEmpty(moyskladId))
                    {
                        moyskladId =  item.ArtNo.IsNotEmpty() ? item.ArtNo : string.Format("Adv-{0}-{1}", order.OrderID, item.OrderItemID);
                    }
                    else
                    {
                        var temp = MoySklad.GetMoyskladIdByOrderItemId(item.OrderItemID);
                        if (!string.IsNullOrEmpty(temp) && !moyskladId.Equals(temp, StringComparison.InvariantCultureIgnoreCase))
                        {
                            moyskladId = string.Format("{0}#{1}", moyskladId, temp);
                        }
                    }


                    tovar.Add(new XElement("Ид", moyskladId),
                              new XElement("Наименование",
                                           string.Format("{0}{2}{1}", item.Name, RenderSelectedOptions(item.SelectedOptions), RenderColorAndSize(item.Color, item.Size))),
                              //new XElement("БазоваяЕдиница", "шт", new XAttribute("МеждународноеСокращение", ""), new XAttribute("НаименованиеПолное", "Штука"), new XAttribute("Код", "")),
                              new XElement("ЦенаЗаЕдиницу", Math.Round(priceT, 2).ToString(format)),
                              new XElement("Количество", item.Amount),
                              new XElement("Сумма", Math.Round(priceT * item.Amount, 2).ToString(format)),
                              new XElement("ДополнительныеЗначенияРеквизитов",
                                           new XElement("ЗначениеРеквизита",
                                                        new XElement("Наименование", "ВидНоменклатуры"),
                                                        new XElement("Значение", "Товар")),
                                           new XElement("ЗначениеРеквизита",
                                                        new XElement("Наименование", "ТипНоменклатуры"),
                                                        new XElement("Значение", "Товар"))));

                    if (order.OrderDiscount > 0 || order.GroupDiscount > 0 || order.Coupon != null || order.BonusCost > 0)
                    {
                        var discounts = new XElement("Скидки");
                        bool disAdd = false;

                        if (order.GroupDiscount > 0 && disAdd == false)
                        {
                            //var priceT = Math.Round(item.Price / (1 - (order.GroupDiscount / 100)), 2);
                            discounts.Add(new XElement("Скидка",
                                                       new XElement("Наименование",
                                                                    string.Format("Скидка \"{0}\"", order.GroupName)),
                                                       new XElement("Сумма",
                                                                    Math.Round(order.GroupDiscount * priceT / 100, 2).ToString(
                                                                        format)),
                                                       new XElement("Процент", order.GroupDiscount.ToString(format)),
                                                       new XElement("УчтеноВСумме", "false")));

                            disAdd = true;
                        }

                        if (order.OrderDiscount > 0 && disAdd == false)
                        {
                            discounts.Add(new XElement("Скидка",
                                                       new XElement("Наименование",
                                                                    "Скидка иcходя из общей стоимости заказа"),
                                                       new XElement("Сумма",
                                                                    Math.Round(order.OrderDiscount * item.Price / 100, 2).
                                                                        ToString(format)),
                                                       new XElement("Процент", order.OrderDiscount.ToString(format)),
                                                       new XElement("УчтеноВСумме", "false")));

                            disAdd = true;
                        }

                        if (order.Coupon != null && item.IsCouponApplied && disAdd == false)
                        {
                            float discount = 0;
                            switch (order.Coupon.Type)
                            {
                                case CouponType.Fixed:
                                    discount = discountCouponSum;
                                    break;
                                case CouponType.Percent:
                                    discount = order.Coupon.Value * item.Price / 100;
                                    break;
                            }

                            discounts.Add(new XElement("Скидка",
                                                       new XElement("Наименование",
                                                                    string.Format("Скидка по купону \"{0}\"",
                                                                                  order.Coupon.Code)),
                                                       new XElement("Сумма", Math.Round(discount, 2).ToString(format)),
                                                       order.Coupon.Type == CouponType.Percent
                                                           ? new XElement("Процент", order.Coupon.Value.ToString(format))
                                                           : null,
                                                       new XElement("УчтеноВСумме", "false")));

                            disAdd = true;
                        }

                        if(order.BonusCost > 0 && order.BonusCardNumber != null && disAdd == false)
                        {
                            discounts.Add(new XElement("Скидка",
                                                       new XElement("Наименование",
                                                                    string.Format("Скидка по бонусной карте \"{0}\"",
                                                                                  order.BonusCardNumber)),
                                                       new XElement("Сумма", Math.Round(order.BonusCost, 2).ToString(format)),
                                                       new XElement("УчтеноВСумме", "false")));

                            disAdd = true;
                        }

                        tovar.Add(discounts);
                    }

                    tovari.Add(tovar);
                }
                doc.Add(tovari);

                var rekviz = new XElement("ЗначенияРеквизитов");

                rekviz.Add(new XElement("ЗначениеРеквизита",
                                        new XElement("Наименование", "Метод оплаты"),
                                        new XElement("Значение", order.PaymentMethodName)));

                rekviz.Add(new XElement("ЗначениеРеквизита",
                                        new XElement("Наименование", "Заказ оплачен"),
                                        new XElement("Значение", order.Payed ? "true" : "false")));

                rekviz.Add(new XElement("ЗначениеРеквизита",
                                        new XElement("Наименование", "Доставка разрешена"),
                                        new XElement("Значение", "false")));

                rekviz.Add(new XElement("ЗначениеРеквизита",
                                        new XElement("Наименование", "Отменен"),
                                        new XElement("Значение",
                                                     order.OrderStatus.IsCanceled
                                                         ? "true"
                                                         : "false")));

                rekviz.Add(new XElement("ЗначениеРеквизита",
                                        new XElement("Наименование", "Финальный статус"),
                                        new XElement("Значение", "false")));

                rekviz.Add(new XElement("ЗначениеРеквизита",
                                        new XElement("Наименование", "Статус заказа"),
                                        new XElement("Значение", order.OrderStatus.StatusName)));

                if (order.PaymentDate.HasValue)
                    rekviz.Add(new XElement("ЗначениеРеквизита",
                                            new XElement("Наименование", "Дата оплаты"),
                                            new XElement("Значение", order.PaymentDate.Value.ToString("yyyy-MM-dd HH:mm:ss"))));

                if (order.OrderCustomer != null)
                {
                    rekviz.Add(new XElement("ЗначениеРеквизита",
                                            new XElement("Наименование", "Адрес плательщика"),
                                            new XElement("Значение", MoySklad.ContactToLined(order.OrderCustomer))));
                }

                if (order.OrderCustomer != null)
                {
                    rekviz.Add(new XElement("ЗначениеРеквизита",
                                            new XElement("Наименование", "Адрес доставки"),
                                            new XElement("Значение", MoySklad.ContactToLined(order.OrderCustomer))));
                }

                doc.Add(rekviz);

                kominfo.Add(doc);

                res.Add(order.OrderID);
            }

            xDoc.Add(kominfo);
            xDoc.Save(streamWriter);
        }

        return res;
    }

    protected string RenderColorAndSize(string color, string size)
    {
        if (!string.IsNullOrEmpty(color) || !string.IsNullOrEmpty(size))
        {
            if (!string.IsNullOrEmpty(color) && !string.IsNullOrEmpty(size))
                return string.Format(" ({0}, {1})", color, size);
            else if (!string.IsNullOrEmpty(color))
                return string.Format(" ({0})", color);
            else if (!string.IsNullOrEmpty(size))
                return string.Format(" ({0})", size);
        }
        return string.Empty;
    }

    protected string RenderSelectedOptions(IList<EvaluatedCustomOptions> evlist)
    {
        var html = new StringBuilder();
        if (evlist != null && evlist.Count > 0)
        {
            html.Append(" (");

            foreach (EvaluatedCustomOptions ev in evlist)
            {
                html.Append(string.Format("{0}: {1},", ev.CustomOptionTitle, ev.OptionTitle));
            }

            html.Append(")");
        }
        return html.ToString();
    }

    //protected void GetContragent(ref XElement element, int orderId, OrderContact contact, OrderCustomer customer, string role, PaymentDetails paymentDetails)
    //{
    //    element.Add(new XElement("Ид", contact.OrderContactId));
    //    element.Add(new XElement("Наименование", contact.Name));
    //    element.Add(new XElement("Роль", role));
    //    element.Add(new XElement("ПолноеНаименование", contact.Name));
    //    //element.Add(new XElement("Фамилия", string.Empty));
    //    //element.Add(new XElement("Имя", contact.Name));

    //    var address = new XElement("АдресРегистрации");
    //    address.Add(new XElement("Представление", string.Format("{0}, {1}, {2}, {3}, {4}", contact.Country, contact.Zip, contact.Zone,
    //                                           contact.City, contact.Address)));
    //    address.Add(new XElement("АдресноеПоле",
    //        new XElement("Тип", "Почтовый индекс"), new XElement("Значение", contact.Zip)));
    //    address.Add(new XElement("АдресноеПоле",
    //        new XElement("Тип", "Страна"), new XElement("Значение", contact.Country)));
    //    address.Add(new XElement("АдресноеПоле",
    //        new XElement("Тип", "Регион"), new XElement("Значение", contact.Zone)));
    //    address.Add(new XElement("АдресноеПоле",
    //        new XElement("Тип", "Город"), new XElement("Значение", contact.City)));
    //    element.Add(address);

    //    //var contacts = new XElement("Контакты");
    //    //contacts.Add(new XElement("Контакт",
    //    //    new XElement("Тип", "Телефон"), new XElement("Значение", contact.Telephone)));

    //    //element.Add(contacts);

    //    if (customer != null)
    //        element.Add(new XElement("Представители",
    //            new XElement("Представитель",
    //                new XElement("Контрагент",
    //                    new XElement("Отношение", "Контактное лицо"),
    //                    new XElement("Ид", customer.CustomerID != InternetUserGuid ? MoySklad.GuidToString(customer.CustomerID) : string.Format("advuser{0}", orderId)),
    //                    new XElement("Наименование", string.Format("{0} {1}", customer.FirstName, customer.LastName)),
    //                    new XElement("Имя", customer.FirstName),
    //                    new XElement("Фамилия", customer.LastName)))));
    //    else
    //        element.Add(new XElement("Представители",
    //            new XElement("Представитель",
    //                new XElement("Контрагент",
    //                    new XElement("Отношение", "Контактное лицо"),
    //                    new XElement("Ид", string.Format("{0}#{1}", contact.OrderContactId, contact.Name)),
    //                    new XElement("Наименование", contact.Name)))));
    //}

    protected void GetGroup(ref Dictionary<string, int> listGroupCategory, XElement element, int parentCatId, bool createNewCategoryEnabled)
    {
        if (element != null)
        {
            foreach (var elementGorup in element.Elements("Группа"))
            {
                var id = elementGorup.Element("Ид").Value;
                var name = elementGorup.Element("Наименование").Value;
                var cat = CategoryService.GetChildCategoryIdByName(parentCatId, name);
                if (!cat.HasValue)
                {
                    cat = CategoryService.AddCategory(new Category
                    {
                        Name = name,
                        ParentCategoryId = parentCatId,
                        //Picture = string.Empty,
                        SortOrder = 0,
                        Enabled = createNewCategoryEnabled,
                        DisplayChildProducts = false,
                        UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Category, name),
                        DisplayStyle = ECategoryDisplayStyle.Tile,
                        //Meta = MetaInfoService.GetDefaultMetaInfo(MetaType.Category, name.HtmlEncode())
                    }, false);
                }

                listGroupCategory.Add(id, cat.Value);
                GetGroup(ref listGroupCategory, elementGorup.Element("Группы"), cat.Value, createNewCategoryEnabled);
            }
        }
    }

    private string GetFileText(string file)
    {
        var text = "";

        using (var streamReader = new StreamReader(file, true))
            text = streamReader.ReadToEnd();

        return string.IsNullOrEmpty(text) ? "" : _invalidXMLChars.Replace(text, "");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}