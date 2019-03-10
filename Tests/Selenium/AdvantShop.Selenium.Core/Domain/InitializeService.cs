using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Protractor;
using System.Xml;
using System.Globalization;
using AdvantShop.Selenium.Core.SQL;

namespace AdvantShop.SeleniumTest.Core
{
    [Flags]
    public enum ClearType
    {
        None = 0,
        Basic = 1,
        Catalog = 2,
        Customers = 4,
        Orders = 8,
        CRM = 16,
        CMS = 32,
        Payment = 64,
        Shipping = 128,
        ExportFeed = 256,
        Settings = 512,
        Total = 1024,
        SettingsSearch = 2048,
        Tasks = 4096,
        Bonuses = 8192,
        Taxes = 16384,
        SettingsProductsPerPage = 32768,
    }

    public static class InitializeService
    {
        private static string _etalonConnectionString;
        private static string _testConnectionString;

        static string _etalonDatabase;
        static string _testDatabase;
        static string _backupPath;
        static string _databasePath;

        private static string projectPath;

        static string _baseScreenshotsPath;

        static string _logFile;


        static string _siteurl;

        static InitializeService()
        {
            string dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            projectPath = new DirectoryInfo(dllPath).Parent.Parent.FullName + "\\";

            ReadConfig();
        }


        public static void InitBrowser(out IWebDriver driver, out string baseURL, out string baseScrinshotsPath, out string logFile, bool useNgDriver = true)
        {

            baseURL = _siteurl;
            baseScrinshotsPath = _baseScreenshotsPath.Trim('\\') + "\\";
            logFile = _logFile;

            var options = new FirefoxOptions();
            //options.AddArgument("no-sandbox");

            if (useNgDriver)
            {
                driver = new NgWebDriver(new FirefoxDriver(options), "html", baseScrinshotsPath);
            }
            else
            {
                driver = new FirefoxDriver(options);
            }

            driver.Manage().Window.Maximize();

            //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(10);
        }

        public static void ClearData(ClearType type)
        {
            if (type.HasFlag(ClearType.Basic) || type.HasFlag(ClearType.Total) || type.HasFlag(ClearType.Catalog))
            {
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.product", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.category where categoryid <> 0", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.property", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.propertygroup", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.PropertyValue", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.ProductCategories", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.ProductPropertyValue", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.PropertyGroupCategory", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.RelatedCategories", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.RelatedProducts", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.RelatedPropertyValues", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.color", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.size", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.tag", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.brand", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.coupon", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.options", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.customoptions", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.ProductGifts", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.photo", connectionString: _testConnectionString);

                SQLDataAccess2.ExecuteNonQuery("delete from cms.review", connectionString: _testConnectionString);

                SQLDataAccess2.ExecuteNonQuery("delete from catalog.productlist", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.productext", connectionString: _testConnectionString);
                
                SQLDataAccess2.ExecuteNonQuery("delete from seo.metainfo where type in ('product', 'category', 'brand', 'tag')", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.photo where type in ('product', 'category', 'brand')", connectionString: _testConnectionString);
            }

            if (type.HasFlag(ClearType.Basic) || type.HasFlag(ClearType.Total) || type.HasFlag(ClearType.Customers))
            {
                SQLDataAccess2.ExecuteNonQuery("delete from Customers.Task", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from Customers.ViewedTask", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from Customers.TaskGroup", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from Customers.ManagerTask", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [Bonus].[Transaction]", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.customer", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.clientcode", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.contact", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.CustomerCertificate", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.CustomerCoupon", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.CustomerGroup", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.CustomerRoleAction", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.Departments", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.RecentlyViewsData", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.OpenIdLinkCustomer", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.RoleAction", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.SmsNotifications", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.Subscription", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.CustomerField", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.CustomerFieldValue", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.CustomerFieldValuesMap", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.ManagerRole", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.Country", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.Region", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.City", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.Managers", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.CustomerSegment", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.CustomerSegment_Customer", connectionString: _testConnectionString);
            }


            if (type.HasFlag(ClearType.Basic) || type.HasFlag(ClearType.Total) || type.HasFlag(ClearType.Orders))
            {
                SQLDataAccess2.ExecuteNonQuery("delete from [order].Certificate", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].[Order]", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].OrderContact", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].OrderCurrency", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].OrderCustomer", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].OrderItems", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].OrderPaymentInfo", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].PaymentDetails", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].OrderPickPoint", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].OrderPriceDiscount", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].OrderCustomOptions", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].OrderHistory", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].StatusHistory", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].OrderTax", connectionString: _testConnectionString);

                SQLDataAccess2.ExecuteNonQuery("delete from [order].DeletedOrders", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].OrderByRequest", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].OrderConfirmation", connectionString: _testConnectionString);

                SQLDataAccess2.ExecuteNonQuery("delete from [order].OrderStatus", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].OrderSource", connectionString: _testConnectionString);

            }

            if (type.HasFlag(ClearType.Basic) || type.HasFlag(ClearType.Total) || type.HasFlag(ClearType.CRM))
            {
                SQLDataAccess2.ExecuteNonQuery("delete from [order].Lead", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].LeadItem", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].LeadCurrency", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].LeadEvent", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from crm.BizProcessRule", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from crm.DealStatus", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].OrderSource", connectionString: _testConnectionString);

                SQLDataAccess2.ExecuteNonQuery("delete from [order].[Order]", connectionString: _testConnectionString);
            }

            if (type.HasFlag(ClearType.Basic) || type.HasFlag(ClearType.Total) || type.HasFlag(ClearType.CMS))
            {
                SQLDataAccess2.ExecuteNonQuery("delete from cms.carousel", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from cms.menu", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from cms.staticpage", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from cms.staticblock", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from Settings.News", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from Settings.NewsCategory", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from voice.Answer", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from voice.VoiceTheme", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.photo", connectionString: _testConnectionString);

                SQLDataAccess2.ExecuteNonQuery("delete from catalog.photo where type in ('carousel', 'staticpage', 'menu')", connectionString: _testConnectionString);
            
            }

            if (type.HasFlag(ClearType.Basic) || type.HasFlag(ClearType.Total) || type.HasFlag(ClearType.Payment))
            {
                SQLDataAccess2.ExecuteNonQuery("delete from [order].PaymentMethod", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].PaymentParam", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].PaymentCity", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].PaymentCountry", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from Settings.GiftCertificatePayments", connectionString: _testConnectionString);

            }

            if (type.HasFlag(ClearType.Basic) || type.HasFlag(ClearType.Total) || type.HasFlag(ClearType.Shipping))
            {
                SQLDataAccess2.ExecuteNonQuery("delete from [order].ShippingCache", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].ShippingCityExcluded", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].ShippingCountry", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].ShippingMethod", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].ShippingParam", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].ShippingPayments", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from [order].ShippingCity", connectionString: _testConnectionString);
            }

            if (type.HasFlag(ClearType.Basic) || type.HasFlag(ClearType.Total) || type.HasFlag(ClearType.ExportFeed))
            {
                SQLDataAccess2.ExecuteNonQuery("delete from settings.ExportFeed", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from settings.ExportFeedSelectedCategories", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from settings.ExportFeedSelectedProducts", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from settings.ExportFeedSettings", connectionString: _testConnectionString);
            }

            if (type.HasFlag(ClearType.Basic) || type.HasFlag(ClearType.Total) || type.HasFlag(ClearType.Settings))
            {
                //SQLDataAccess2.ExecuteNonQuery("delete from settings.ProfitPlan", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from settings.Redirect", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from settings.Reseller", connectionString: _testConnectionString);
                //SQLDataAccess2.ExecuteNonQuery("delete from settings.TemplateSettings", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from settings.SettingsSearch", connectionString: _testConnectionString);
            }

            if (type.HasFlag(ClearType.Basic) || type.HasFlag(ClearType.Total) || type.HasFlag(ClearType.SettingsSearch))
            {
                SQLDataAccess2.ExecuteNonQuery("delete from settings.SettingsSearch", connectionString: _testConnectionString);
            }

            if (type.HasFlag(ClearType.Basic) || type.HasFlag(ClearType.Total) || type.HasFlag(ClearType.Tasks))
            {
                SQLDataAccess2.ExecuteNonQuery("delete from Customers.Task", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from Customers.TaskGroup", connectionString: _testConnectionString);
            }

            if (type.HasFlag(ClearType.Basic) || type.HasFlag(ClearType.Total) || type.HasFlag(ClearType.Bonuses))
            {
                SQLDataAccess2.ExecuteNonQuery("delete from Bonus.Card", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from Bonus.Grade", connectionString: _testConnectionString);
                  SQLDataAccess2.ExecuteNonQuery("delete from Bonus.AdditionBonus", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from Bonus.SmsTemplate", connectionString: _testConnectionString);
            }

            if (type.HasFlag(ClearType.Basic) || type.HasFlag(ClearType.Total) || type.HasFlag(ClearType.Taxes))
            {
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.tax", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from settings.settings where Name = 'DefaultTaxId'", connectionString: _testConnectionString);
            }

            if (type.HasFlag(ClearType.Basic) || type.HasFlag(ClearType.Total) || type.HasFlag(ClearType.SettingsProductsPerPage))
            {
                SQLDataAccess2.ExecuteNonQuery("delete from settings.settings where Name = 'ProductsPerPage'", connectionString: _testConnectionString);
            }


            if (type.HasFlag(ClearType.Total))
            {
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.currency", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from catalog.tax", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from settings.GiftCertificateTaxes", connectionString: _testConnectionString);

                SQLDataAccess2.ExecuteNonQuery("delete from customers.city", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.region", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from customers.country", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from dbo.DownloadableContent", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from dbo.Modules", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from dbo.ModuleSettings", connectionString: _testConnectionString);

                SQLDataAccess2.ExecuteNonQuery("delete from dbo.SaasData", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from dbo.Modules", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from dbo.OrderSource", connectionString: _testConnectionString);
                SQLDataAccess2.ExecuteNonQuery("delete from dbo.OrderStatus", connectionString: _testConnectionString);
            }

            Reindex();

        }

        private static void Reindex()
        {
            SQLDataAccess2.ExecuteNonQuery("[Catalog].[SetCategoryHierarchicallyEnabled]", new { CatParent = 0 }, CommandType.StoredProcedure, connectionString: _testConnectionString);

            SQLDataAccess2.ExecuteNonQuery("[Catalog].[PreCalcProductParamsMass]",
                new
                {
                    ModerateReviews = SQLDataAccess2.ExecuteScalar<bool>("Select value from settings.settings where name ='ModerateReviewed'", connectionString: _testConnectionString),
                    OnlyAvailable = SQLDataAccess2.ExecuteScalar<bool>("Select value from settings.settings where name ='ShowOnlyAvalible'", connectionString: _testConnectionString)
                }, CommandType.StoredProcedure, connectionString: _testConnectionString);

            SQLDataAccess2.ExecuteNonQuery("[Catalog].[sp_RecalculateProductsCount]",
                new
                {
                    UseAmount = SQLDataAccess2.ExecuteScalar<bool>("Select value from settings.settings where name ='ShowOnlyAvalible'", connectionString: _testConnectionString)
                }, CommandType.StoredProcedure, connectionString: _testConnectionString);
        }

        private static void SetLicKey()
        {
            SQLDataAccess2.ExecuteNonQuery("UPDATE Settings.Settings set value = '8b40c4f4-322e-4926-ad39-d2f6d6cd10c1' where name='lickey'", connectionString: _testConnectionString);
        }

        public static void LoadData(params string[] files)
        {

            foreach (var file in files)
            {
                DataTable csvData = new DataTable();
                string value;

                using (TextReader textReader = new StreamReader(projectPath + file))
                {
                    using (var reader = new CsvHelper.CsvReader(textReader, new CsvHelper.Configuration.Configuration() { HasHeaderRecord = false, Delimiter = ";" }))
                    {
                        int counter = 0;
                        //reading headers
                        if (reader.Read())
                        {
                            for (int i = 0; reader.TryGetField<string>(i, out value); i++)
                            {
                                if (value.Contains("[GUID]"))
                                {
                                    value = value.Replace("[GUID]", "");
                                    DataColumn datecolumn = new DataColumn(value, typeof(Guid));
                                    csvData.Columns.Add(datecolumn);
                                }
                                else if (value.Contains("[DATE]"))
                                {
                                    value = value.Replace("[DATE]", "");
                                    DataColumn datecolumn = new DataColumn(value, typeof(DateTime));
                                    csvData.Columns.Add(datecolumn);
                                }
                                else
                                {
                                    DataColumn datecolumn = new DataColumn(value);
                                    csvData.Columns.Add(datecolumn);
                                }

                                counter++;
                            }
                        }

                        var line = new List<string>();
                        //reading data
                        while (reader.Read())
                        {
                            object[] fieldData = new object[counter];
                            for (int i = 0; reader.TryGetField<string>(i, out value); i++)
                            {
                                Guid tempGuid;
                                DateTime tempDate;
                                if (Guid.TryParse(value, out tempGuid))
                                {
                                    fieldData[i] = tempGuid;
                                }
                                else if (DateTime.TryParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempDate))
                                {
                                    fieldData[i] = tempDate;
                                }
                                else if (DateTime.TryParseExact(value, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempDate))
                                {
                                    fieldData[i] = tempDate;
                                }
                                else if (DateTime.TryParseExact(value, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempDate))
                                {
                                    fieldData[i] = tempDate;
                                }
                                else
                                {
                                    switch (value)
                                    {
                                        case "NULL":
                                            fieldData[i] = null;
                                            break;

                                        case "[TODAY]":
                                            fieldData[i] = DateTime.Today.ToString();
                                            break;

                                        case "[TOMORROW]":
                                            fieldData[i] = DateTime.Today.AddDays(1).ToString();
                                            break;

                                        case "[YESTERDAY]":
                                            fieldData[i] = DateTime.Today.AddDays(-1).ToString();
                                            break;

                                        case "[MONTH_AGO]":
                                            fieldData[i] = DateTime.Today.AddMonths(-1).ToString();
                                            break;

                                        default:
                                            fieldData[i] = value;
                                            break;
                                    }
                                }
                            }
                            csvData.Rows.Add(fieldData);
                        }
                    }
                }

                string tableName = new FileInfo(file).Name.Replace(".csv", "");

                ExecuteCsvDataToSql(tableName, csvData);
            }

            SetLicKey();
            Reindex();
            ClearCache();
            ReindexLucene();
        }

        private static void ClearCache()
        {

            var response = (new System.Net.WebClient()).DownloadString(_siteurl.Trim('/') + "/tools/clearcache.ashx");
            if (response != "ok")
                throw new Exception("Cant clear cache:" + response);
        }


        private static void ReindexLucene()
        {
            var response = (new System.Net.WebClient()).DownloadString(_siteurl.Trim('/') + "/tools/ReindexLucene.ashx");
            if (response != "ok")
                throw new Exception("Cant reindex lucene:" + response);
        }


        private static void ExecuteCsvDataToSql(string tablenane, DataTable data)
        {
            using (SqlConnection dbConnection = new SqlConnection(_testConnectionString))
            {
                dbConnection.Open();
                using (SqlBulkCopy s = new SqlBulkCopy(dbConnection, SqlBulkCopyOptions.KeepIdentity, null))
                {
                    s.DestinationTableName = tablenane;
                    foreach (var column in data.Columns)
                    {
                        s.ColumnMappings.Add(column.ToString(), column.ToString());
                    }
                    s.WriteToServer(data);
                }
                dbConnection.Close();
            }
        }


        public static void RollBackDatabase()
        {
            //#if DEBUG
            //return;
            //#endif
            DbHelper.BackupDatabase(_etalonConnectionString, _etalonDatabase, _backupPath + "etalon.bak");

            if (DbHelper.ExistDatabase(_testConnectionString))
            {
                DbHelper.DropDatabase(_testConnectionString);
            }
            DbHelper.RestoreDatabase(_testConnectionString, _testDatabase, _backupPath + "etalon.bak", _databasePath);

        }


        private static void ReadConfig()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(projectPath + "app.config");

            XmlNode nodeListsettings = xmldoc.SelectNodes("/configuration/appSettings")[0];

            foreach (XmlNode node in nodeListsettings)
            {
                switch (node.Attributes["key"].Value)
                {
                    case "EtalonDatabaseName":
                        _etalonDatabase = node.Attributes["value"].Value;
                        break;

                    case "TestDatabaseName":
                        _testDatabase = node.Attributes["value"].Value;
                        break;

                    case "BackupPath":
                        _backupPath = node.Attributes["value"].Value;
                        break;

                    case "DatabasePath":
                        _databasePath = node.Attributes["value"].Value;
                        break;

                    case "SiteUrl":
                        _siteurl = node.Attributes["value"].Value;
                        break;

                    case "ScreenshotsPath":
                        _baseScreenshotsPath = node.Attributes["value"].Value;
                        break;

                    case "LogFile":
                        _logFile = node.Attributes["value"].Value;
                        break;



                }
            }

            XmlNode nodeListConnections = xmldoc.SelectNodes("/configuration/connectionStrings")[0];

            foreach (XmlNode node in nodeListConnections)
            {
                switch (node.Attributes["name"].Value)
                {
                    case "etalon":
                        _etalonConnectionString = node.Attributes["connectionString"].Value;
                        break;
                    case "AdvantConnectionString":
                        _testConnectionString = node.Attributes["connectionString"].Value;
                        break;

                }
            }

        }

    }
}
