
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdvantShop.Module.ReturnCustomer.Models;
using AdvantShop.Helpers;
using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.FilePath;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Catalog;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Orders;

namespace AdvantShop.Module.ReturnCustomer.Service
{
    public class RCService
    {
        public static bool NonQuery(string query, params SqlParameter[] parameters)
        {
            try
            {
                if (parameters != null)
                {
                    ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text, parameters);
                }
                else
                {
                    ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text);
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return false;
            }
        }

        public static bool Install()
        {
            var install = RCSettings.SetDefaultSettings();

            if (!ModulesRepository.IsExistsModuleTable("Module", "ReturnCustomerRecords"))
            {
                install &= NonQuery(@"CREATE TABLE [Module].[ReturnCustomerRecords]
                                    (
                                        [CustomerID] [uniqueidentifier] NOT NULL,
                                        [LastActionDate] [datetime] NOT NULL,
                                        [ExpirationDate] [datetime] NOT NULL,
                                        [LastSendingDate] [nvarchar](30) NULL,
                                        [IsNotNeedChecked] [bit] NULL,
                                        [WaitingVisit] [bit] NOT NULL
                                    )");
            }

            return install;
        }

        public static bool UnInstall()
        {
            var isDropped = true;
            if (ModulesRepository.IsExistsModuleTable("Module", "ReturnCustomerRecords"))
            {
                isDropped &= NonQuery("DROP TABLE [Module].[ReturnCustomerRecords]");
            }

            return isDropped;
        }

        public static bool Update()
        {
            var isUpdated = true;
            if (ModulesRepository.IsExistsModuleTable("Module", "ReturnCustomerRecords"))
            {
                if (IsExistColumn("LastSendingDate"))
                {
                    isUpdated &= NonQuery("ALTER TABLE [Module].[ReturnCustomerRecords] ALTER COLUMN [LastSendingDate] [nvarchar](30)");
                }
                else
                {
                    isUpdated &= NonQuery("ALTER TABLE [Module].[ReturnCustomerRecords] ADD [LastSendingDate] [nvarchar](30) NULL");
                }
            }

            return isUpdated;
        }

        private static bool IsExistColumn(string columnName)
        {
            return ModulesRepository.ModuleExecuteScalar<int>(
                "IF COLUMNPROPERTY(OBJECT_ID('[Module].[ReturnCustomerRecords]'), '" + columnName + "', 'AllowsNull') IS NOT NULL SELECT 1 ELSE SELECT 0",
                CommandType.Text) > 0;
        }
        //--------------------------------------------------------------------------------------------

        public static ReturnCustomerRecord GetReturnCustomerRecord(Guid customerId)
        {
            return ModulesRepository.ModuleExecuteReadOne<ReturnCustomerRecord>(
                    "SELECT * FROM [Module].[ReturnCustomerRecords] WHERE [CustomerID] = @CustomerID",
                        CommandType.Text,
                        reader => new ReturnCustomerRecord
                        {
                            CustomerID = SQLDataHelper.GetGuid(reader, "CustomerID"),
                            LastActionDate = SQLDataHelper.GetDateTime(reader, "LastActionDate"),
                            ExpirationDate = SQLDataHelper.GetDateTime(reader, "ExpirationDate"),
                            LastSendingDate = SQLDataHelper.GetString(reader, "LastSendingDate"),
                            IsNotNeedChecked = SQLDataHelper.GetBoolean(reader, "IsNotNeedChecked"),
                            WaitingVisit = SQLDataHelper.GetBoolean(reader, "WaitingVisit")
                        },
                        new SqlParameter("@CustomerID", customerId));
        }

        public static List<ReturnCustomerRecord> GetReturnCustomerRecords()
        {
            return ModulesRepository.ModuleExecuteReadList<ReturnCustomerRecord>(
                    "SELECT * FROM [Module].[ReturnCustomerRecords]",
                        CommandType.Text,
                        reader => new ReturnCustomerRecord
                        {
                            CustomerID = SQLDataHelper.GetGuid(reader, "CustomerID"),
                            LastActionDate = SQLDataHelper.GetDateTime(reader, "LastActionDate"),
                            ExpirationDate = SQLDataHelper.GetDateTime(reader, "ExpirationDate"),
                            LastSendingDate = SQLDataHelper.GetString(reader, "LastSendingDate"),
                            IsNotNeedChecked = SQLDataHelper.GetBoolean(reader, "IsNotNeedChecked"),
                            WaitingVisit = SQLDataHelper.GetBoolean(reader, "WaitingVisit")
                        });
        }

        public static void AddReturnCustomerRecord(ReturnCustomerRecord returnCustomerRecord)
        {
            NonQuery("INSERT INTO [Module].[ReturnCustomerRecords] ([CustomerID], [LastActionDate], [ExpirationDate], [LastSendingDate], [IsNotNeedChecked], [WaitingVisit]) " +
                     "VALUES (@CustomerID, @LastActionDate, @ExpirationDate, @LastSendingDate, @IsNotNeedChecked, @WaitingVisit)",
                        new SqlParameter("@CustomerID", returnCustomerRecord.CustomerID),
                        new SqlParameter("@LastActionDate", returnCustomerRecord.LastActionDate),
                        new SqlParameter("@ExpirationDate", returnCustomerRecord.ExpirationDate),
                        new SqlParameter("@LastSendingDate", returnCustomerRecord.LastSendingDate ?? "-"),
                        new SqlParameter("@IsNotNeedChecked", returnCustomerRecord.IsNotNeedChecked ?? (object)DBNull.Value),
                        new SqlParameter("@WaitingVisit", returnCustomerRecord.WaitingVisit));
        }

        public static void UpdateReturnCustomerRecord(ReturnCustomerRecord returnCustomerRecord)
        {
            NonQuery("UPDATE [Module].[ReturnCustomerRecords] SET [LastActionDate] = @LastActionDate, [ExpirationDate] = @ExpirationDate, [LastSendingDate] = @LastSendingDate, [IsNotNeedChecked] = @IsNotNeedChecked, [WaitingVisit] = @WaitingVisit WHERE [CustomerID] = @CustomerID",
                    new SqlParameter("@CustomerID", returnCustomerRecord.CustomerID),
                    new SqlParameter("@LastActionDate", returnCustomerRecord.LastActionDate),
                    new SqlParameter("@ExpirationDate", returnCustomerRecord.ExpirationDate),
                    new SqlParameter("@LastSendingDate", returnCustomerRecord.LastSendingDate ?? "-"),
                    new SqlParameter("@IsNotNeedChecked", returnCustomerRecord.IsNotNeedChecked ?? (object)DBNull.Value),
                    new SqlParameter("@WaitingVisit", returnCustomerRecord.WaitingVisit));
        }

        public static void DeleteReturnCustomerRecord(Guid customerID)
        {
            NonQuery("DELETE FROM [Module].[ReturnCustomerRecords] WHERE [CustomerID] = @CustomerID",
                    new SqlParameter("@CustomerID", customerID));
        }

        public static void SetWaitingVisitReturnCustomerRecord(Guid customerID, bool waitingVisit)
        {
            NonQuery("UPDATE [Module].[ReturnCustomerRecords] SET [WaitingVisit] = @WaitingVisit WHERE [CustomerID] = @CustomerID",
                    new SqlParameter("@CustomerID", customerID),
                    new SqlParameter("@WaitingVisit", waitingVisit));
        }

        public static int GetLastOrderIdByCustomerId(Guid customerID)
        {
            return ModulesRepository.ModuleExecuteScalar<int>(
                "SELECT TOP(1) [OrderId] FROM [Order].[OrderCustomer] WHERE [CustomerID] = @CustomerID",
                CommandType.Text,
                new SqlParameter("@CustomerID", customerID));
        }

        public static DateTime GetOrderDateByOrderId(int orderId)
        {
            return ModulesRepository.ModuleExecuteScalar<DateTime>(
                "SELECT [OrderDate] FROM [Order].[Order] WHERE [OrderId] = @OrderID",
                CommandType.Text,
                new SqlParameter("@OrderID", orderId));
        }

        public static List<int> GetLastAddedEnabledProductsIds(int quantity)
        {
            return ModulesRepository.ModuleExecuteReadList<int>(
                "SELECT TOP(@Quantity) [ProductId] FROM [Catalog].[Product] WHERE [Enabled] = 1 ORDER BY [DateAdded] DESC",
                CommandType.Text,
                reader => SQLDataHelper.GetInt(reader, "ProductId"),
                new SqlParameter("@Quantity", quantity));
        }

        public static string GetEmailByCustomerID(Guid customerID)
        {
            return ModulesRepository.ModuleExecuteScalar<string>(
                "SELECT [Email] FROM [Customers].[Customer] WHERE [CustomerID] = @CustomerID",
                CommandType.Text,
                new SqlParameter("@CustomerID", customerID));
        }

        public static DateTime GetProductDateAdded(int productId)
        {
            return ModulesRepository.ModuleExecuteScalar<DateTime>(
                "SELECT [DateAdded] FROM [Catalog].[Product] WHERE [ProductId] = @ProductID",
                CommandType.Text,
                new SqlParameter("@ProductID", productId));
        }

        public static DateTime GetRecentlyViewDateProduct(Guid customerId, int productId)
        {
            return ModulesRepository.ModuleExecuteScalar<DateTime>(
                "SELECT [ViewDate] FROM [Customers].[RecentlyViewsData] WHERE [CustomerID] = @CustomerID AND [ProductId] = @ProductID",
                CommandType.Text,
                new SqlParameter("@ProductID", productId),
                new SqlParameter("@CustomerID", customerId));
        }

        public static string GetProductArtNoById(int productId)
        {
            return ModulesRepository.ModuleExecuteScalar<string>(
                "SELECT [ArtNo] FROM [Catalog].[Product] WHERE [ProductId] = @ProductID",
                CommandType.Text,
                new SqlParameter("@ProductID", productId));
        }

        public static List<int> GetViewLastProductsByCustomer(Guid customerId, int rowsCount)
        {
            return ModulesRepository.ModuleExecuteReadColumn<int>(
                        "SELECT TOP(10) [ProductID] FROM [Customers].[RecentlyViewsData] WHERE [CustomerID] = @CustomerID",
                        CommandType.Text,
                        "ProductID",
                        new SqlParameter("@CustomerID", customerId),
                        new SqlParameter("@RowsCount", rowsCount));
        }
        //--------------------------------------------------------------------------------------------

        /* если разница во времени в минутах между текущим временем и временем последнего действия пользователя >= timeExpiration, 
         отсылаем письмо (если не было заказа) */
        public const byte timeExpiration = 15; 
        private const byte productsViewedCount = 30; // кол-во последних просмотренных товаров

        public static void CheckUserActions()
        {
            var returnCustomerRecords = GetReturnCustomerRecords();
            
            foreach(var returnCustomerRecord in returnCustomerRecords)
            {
                if(returnCustomerRecord.IsNotNeedChecked.HasValue && returnCustomerRecord.IsNotNeedChecked.Value)
                {
                    continue;
                }

                var _timeExpiration = new TimeSpan(0, 0, timeExpiration, 0, 0);
                if (DateTime.Now - returnCustomerRecord.LastActionDate >= _timeExpiration)
                {
                    SendMailToCustomer(returnCustomerRecord.CustomerID);
                }
            }

            ClearReturnCustomerRecords();
        }

        public static void ClearReturnCustomerRecords()
        {
            var records = GetReturnCustomerRecords();
            foreach(var record in records)
            {
                if (DateTime.Now >= record.ExpirationDate)
                {
                    //DeleteReturnCustomerRecord(record.CustomerID);
                    SetWaitingVisitReturnCustomerRecord(record.CustomerID, true);
                }
            }
        }

        public static void SendMailToCustomer(Guid customerID)
        {
            var customer = Customers.CustomerService.GetCustomer(customerID);
            if (customer == null || customer.CustomerRole == Customers.Role.Administrator || customer.CustomerRole == Customers.Role.Moderator)
            {
                return;
            }

            var returnCustomerRecord = GetReturnCustomerRecord(customerID);
            if(returnCustomerRecord == null)
            {
                return;
            }

            try
            {
                if (RCSettings.DisabledMailsList.Contains(customer.EMail))
                {
                    DeleteReturnCustomerRecord(customer.Id);
                    return;
                }

                var lastProductsViewed = Customers.RecentlyViewService.LoadViewDataByCustomer(customer.Id, productsViewedCount)
                    .Where(product => IsCorrectProduct(product)).Take(3).ToList();

                var alternativeMessage = lastProductsViewed == null || lastProductsViewed.Count < 1;

                var mailBody = !alternativeMessage ? RCSettings.MessageText : RCSettings.AlternativeMessageText;
                mailBody = mailBody.Replace("#STORE_NAME#", SettingsMain.ShopName);
                mailBody = mailBody.Replace("#USERNAME#", string.Format("{0} {1}", customer.FirstName, customer.LastName));

                if (!alternativeMessage)
                {
                    var productsViewed = string.Empty;
                    if (lastProductsViewed != null)
                    {
                        productsViewed = GenerateProductsViewedItemsHtml(lastProductsViewed);
                    }

                    mailBody = mailBody.Replace("#VIEWEDPRODUCTS#", productsViewed);
                }

                var mailSubject = !alternativeMessage ? RCSettings.MessageSubject : RCSettings.AlternativeMessageSubject;

                var logo = SettingsMain.LogoImageName.IsNotEmpty()
                                           ? string.Format("<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" />",
                                                           SettingsMain.SiteUrl.Trim('/') + '/' +
                                                           FoldersHelper.GetPathRelative(FolderType.Pictures, SettingsMain.LogoImageName, false),
                                                           SettingsMain.ShopName)
                                           : string.Empty;

                mailBody = mailBody.Replace("#LOGO#", logo);
                mailBody = mailBody.Replace("#MAIN_PHONE#", SettingsMain.Phone);

                ModulesService.SendModuleMail(customer.Id, mailSubject, mailBody, customer.EMail, true);

                var daysInterval = RCSettings.DaysInterval;
                var expirationDate = DateTime.Now.AddDays(daysInterval);

                returnCustomerRecord.ExpirationDate = expirationDate;
                returnCustomerRecord.IsNotNeedChecked = true;
                returnCustomerRecord.LastSendingDate = DateTime.Now.ToString("dd.MM.yy HH:mm");

                var log = new RCLog();
                log.Write(string.Format("{0}: Отправлено сообщение пользователю с email: {1}. Следующая отправка не ранее след. даты: {2}",
                    DateTime.Now.ToString("[dd.MM.yy HH:mm]"),
                    customer.EMail,
                    returnCustomerRecord.ExpirationDate.ToString("[dd.MM.yy HH:mm]")));

                UpdateReturnCustomerRecord(returnCustomerRecord);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(string.Format("Return customer {0} check user actions error: {1}", returnCustomerRecord.CustomerID, ex.Message));
            }
        }

        public static bool IsCorrectProduct(ProductModel productModel)
        {
            var product = ProductService.GetProduct(productModel.ProductId);
            if(product == null || !product.Enabled || !product.CategoryEnabled)
            {
                return false;
            }
            
            var timeSpan = DateTime.Now - GetProductDateAdded(product.ProductId);
            if (timeSpan.Days < 1)
            {
                return false;
            }

            var mainOffer = product.Offers.Where(offer => offer.Main).FirstOrDefault();
            if(mainOffer == null)
            {
                return false;
            }

            if(mainOffer.Amount < 1)
            {
                return false;
            }

            return true;
        }

        public static void UpdateRecordsWhenDoOrderAdded(IOrder order)
        {
            if (order == null) { return; }

            var customer = order.GetOrderCustomer();
            if(customer == null) { return; }

            var record = GetReturnCustomerRecord(customer.CustomerID);
            if(record == null) { return; }

            var daysInterval = RCSettings.DaysInterval;
            var expirationDate = DateTime.Now.AddDays(daysInterval);

            record.ExpirationDate = expirationDate;
            record.IsNotNeedChecked = true;

            UpdateReturnCustomerRecord(record);
        }

        public static string GenerateProductsViewedItemsHtml(List<ProductModel> productViewedItems)
        {
            var productViewedItemsHtml = new StringBuilder();
            
            productViewedItemsHtml.Append("<table class='orders-table' style='border-collapse: collapse; width: 100%;'>");
            productViewedItemsHtml.Append("<tr class='orders-table-header'>");
            productViewedItemsHtml.AppendFormat("<th class='photo' style='border-bottom: 1px solid #e3e3e3; padding: 20px 5px 20px 0; text-align: left;'>{0}</th>", LocalizationService.GetResource("Core.Orders.Order.Letter.Goods"));
            productViewedItemsHtml.Append("<th class='name' style='border-bottom: 1px solid #e3e3e3; padding: 20px 0; text-align: left; width: 50%;'></th>");
            productViewedItemsHtml.AppendFormat("<th class='price' style='border-bottom: 1px solid #e3e3e3; padding: 20px 5px 20px 0; text-align: center;'>{0}</th>", LocalizationService.GetResource("Core.Orders.Order.Letter.Price"));
            productViewedItemsHtml.Append("</tr>");
            
            foreach (var item in productViewedItems)
            {
                productViewedItemsHtml.Append("<tr>");

                Photo photo;
                if ((photo = item.Photo) != null)
                {
                    productViewedItemsHtml.AppendFormat(
                        "<td class='photo' style='border-bottom: 1px solid #e3e3e3; margin-right: 15px; padding: 20px 5px 20px 0; padding-left: 20px; text-align: left; width:{1}px;'><img style='border:none;display:block;outline:none;text-decoration:none;max-width:100%;height:auto;' src='{0}' /></td>",
                        FoldersHelper.GetImageProductPath(ProductImageType.XSmall, photo.PhotoName, false), SettingsPictureSize.XSmallProductImageWidth);
                }
                else
                {
                    productViewedItemsHtml.AppendFormat("<td class='photo' style='border-bottom: 1px solid #e3e3e3; margin-right: 15px; padding: 20px 5px 20px 0; padding-left: 20px; text-align: left; width:{0}px;'></td>", SettingsPictureSize.XSmallProductImageWidth);
                }
                
                var product = ProductService.GetProduct(item.ProductId) ?? null;
                
                if (product != null)
                {
                    productViewedItemsHtml.AppendFormat("<td class='name' style='border-bottom: 1px solid #e3e3e3; padding: 20px 5px 20px 0; text-align: left; min-width:150px; width: 50%;'>" +
                                                        "<div class='description' style='display: inline-block;'>" +
                                                            "<div class='prod-name' style='font-size: 14px; margin-bottom: 5px;'><a href='{0}' class='cs-link' style='color: #0764c3; text-decoration: none;'>{1}</a></div> " +
                                                        "</div>" +
                                                "</td>",
                                                product != null ? SettingsMain.SiteUrl.Trim('/') + "/" + UrlService.GetLink(ParamType.Product, product.UrlPath, product.ProductId) : "",
                                                item.Name);

                    var mainOffer = product.Offers.Where(offer => offer.Main).FirstOrDefault();
                    if(mainOffer != null)
                    {
                        productViewedItemsHtml.AppendFormat("<td class='price' style='border-bottom: 1px solid #e3e3e3; padding: 20px 5px 20px 0; text-align: center; white-space: nowrap;'>{0}</td>", mainOffer.RoundedPrice.FormatPrice(AdvantShop.Repository.Currencies.CurrencyService.CurrentCurrency));
                    }
                }
                else
                {
                    productViewedItemsHtml.AppendFormat("<td>&nbsp;</td>");
                    productViewedItemsHtml.AppendFormat("<td>&nbsp;</td>");
                }

                productViewedItemsHtml.Append("</tr>");
            }

            productViewedItemsHtml.Append("</table>");

            return productViewedItemsHtml.ToString();
        }
    }
}
