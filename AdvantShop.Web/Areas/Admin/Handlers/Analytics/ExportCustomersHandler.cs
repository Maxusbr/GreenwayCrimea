using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;

using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using AdvantShop.Statistic;
using AdvantShop.Core.SQL;
using AdvantShop.FilePath;
using System.Data;
using System.Text;
using CsvHelper;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.ExportImport;
using AdvantShop.Web.Admin.ViewModels.Analytics;
using AdvantShop.Saas;
using AdvantShop.Customers;

namespace AdvantShop.Web.Admin.Handlers.Analytics
{
    public class ExportCustomersHandler
    {
        private readonly ExportCustomersModel _settings;

        private readonly string _strFilePath;
        private string _strFullPath;

        private const string StrFileName = "StatisticsCustomers";
        private const string StrFileExt = ".csv";

        protected string ExtStrFileName;


        public ExportCustomersHandler(ExportCustomersModel settings)
        {
            _settings = settings;

            _strFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            FileHelpers.CreateDirectory(_strFilePath);
            var firstFile = Directory.GetFiles(_strFilePath).FirstOrDefault(f => f.Contains(StrFileName));
            _strFullPath = firstFile;
            ExtStrFileName = Path.GetFileName(firstFile);

            if (!string.IsNullOrWhiteSpace(_strFullPath)) return;
            ExtStrFileName = (StrFileName + StrFileExt).FileNamePlusDate();
            _strFullPath = _strFilePath + ExtStrFileName;
        }

        public void Execute()
        {

            if (CommonStatistic.IsRun)
            {
                return;
            }

            foreach (var item in Directory.GetFiles(_strFilePath).Where(f => f.Contains(StrFileName)))
            {
                FileHelpers.DeleteFile(item);
            }

            CommonStatistic.Init();
            CommonStatistic.IsRun = true;
            CommonStatistic.CurrentProcess = "analytics/exportcustomers";
            CommonStatistic.CurrentProcessName = "Выгрузка покупателей";

            // Directory
            foreach (var file in Directory.GetFiles(_strFilePath).Where(f => f.Contains(StrFileName)).ToList())
            {
                FileHelpers.DeleteFile(file);
            }

            ExtStrFileName = (StrFileName + StrFileExt).FileNamePlusDate();
            _strFullPath = _strFilePath + ExtStrFileName;
            FileHelpers.CreateDirectory(_strFilePath);

            var ctx = HttpContext.Current;
            CommonStatistic.StartNew(() =>
            {
                try
                {
                    HttpContext.Current = ctx;
                    SaveCustomersStatisticToCsv(
                        _strFullPath,
                        _settings.Encoding,
                        _settings.ColumnSeparator,
                        _settings.PropertySeparator,
                        _settings.Group == -1 ? null : (int?)_settings.Group,
                        _settings.DateFrom == DateTime.MinValue ? null : (DateTime?)_settings.DateFrom,
                        _settings.DateTo == DateTime.MinValue ? null : (DateTime?)_settings.DateTo);
                }
                catch (Exception ex)
                {
                    //Debug.Log.Error(ex);
                    CommonStatistic.WriteLog(ex.Message);
                }
                CommonStatistic.FileName = UrlService.GetAbsoluteLink(FoldersHelper.PhotoFoldersPath[FolderType.PriceTemp] + ExtStrFileName);
                CommonStatistic.IsRun = false;
            });
        }


        private void SaveCustomersStatisticToCsv(string strFullPath, string csvEnconing, string csvSeparator, string csvAddressSeparator, int? csvGroup, DateTime? csvDateFrom, DateTime? csvDateTo)
        {
            var sqlParameters = new List<SqlParameter>();
            var paramsss = (csvDateFrom.HasValue ? " AND [RegistrationDateTime] >= @DateFrom" : "") +
                          (csvDateTo.HasValue ? " AND [RegistrationDateTime] <= @DateTo" : "") +
                          (csvGroup.HasValue ? " AND [CustomerGroup].[CustomerGroupId] = @GroupId" : "");
            paramsss = paramsss.Trim(" AND".ToCharArray());

            var cmd = string.Format("SELECT [Customer].[CustomerID], [Email], [FirstName], [LastName], [Phone], [Patronymic], [RegistrationDateTime], [Rating], [GroupName], " +

                                    "(SELECT Top(1) Country  FROM [Customers].[Contact] Where [Contact].[CustomerID] = [Customer].[CustomerID]) As Country, " +
                                    "(SELECT Top(1) City  FROM [Customers].[Contact] Where [Contact].[CustomerID] = [Customer].[CustomerID]) As City, " +

                                    "(SELECT COUNT([Order].[OrderID]) FROM [Order].[Order] INNER JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] WHERE [PaymentDate] IS NOT NULL AND [OrderCustomer].[CustomerID]=[Customer].[CustomerID]) AS CountPay, " +
                                    "(SELECT ISNULL(SUM([Order].[Sum]),0) FROM [Order].[Order] INNER JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] WHERE [PaymentDate] IS NOT NULL AND [OrderCustomer].[CustomerID]=[Customer].[CustomerID]) AS SumPay, " +
                                    "(SELECT COUNT([Order].[OrderID]) FROM [Order].[Order] INNER JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] WHERE [OrderCustomer].[CustomerID]=[Customer].[CustomerID]) AS CountOrders, " +
                                    "(SELECT ISNULL(SUM([Order].[Sum]),0) FROM [Order].[Order] INNER JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] WHERE [OrderCustomer].[CustomerID]=[Customer].[CustomerID]) AS SumOrders " +
                                    "FROM [Customers].[Customer] INNER JOIN [Customers].[CustomerGroup] ON [Customer].[CustomerGroupId] = [CustomerGroup].[CustomerGroupId] {0} {1}",
                                    csvDateFrom.HasValue || csvDateTo.HasValue || csvGroup.HasValue ? " Where [Customer].[CustomerRole] = 0 AND " : " Where [Customer].[CustomerRole] = 0", paramsss);
            if (csvGroup.HasValue)
            {
                sqlParameters.Add(new SqlParameter("@GroupId", csvGroup.Value));
            }
            if (csvDateFrom.HasValue)
            {
                sqlParameters.Add(new SqlParameter("@DateFrom", csvDateFrom.Value));
            }
            if (csvDateTo.HasValue)
            {
                sqlParameters.Add(new SqlParameter("@DateTo", csvDateTo.Value));
            }

            var data = SQLDataAccess.ExecuteTable(cmd, CommandType.Text, sqlParameters.ToArray());

            CommonStatistic.TotalRow = data.Rows.Count;

            using (var writer = InitWriter(strFullPath, csvEnconing, csvSeparator))
            {
                var columns = new List<string>
                {
                    LocalizationService.GetResource("Admin.ExportField.Email"),
                    LocalizationService.GetResource("Admin.ExportField.Name"),
                    LocalizationService.GetResource("Admin.ExportField.Phone"),
                    LocalizationService.GetResource("Admin.ExportField.Country"),
                    LocalizationService.GetResource("Admin.ExportField.City"),
                    LocalizationService.GetResource("Admin.ExportField.CustomerGroup"),
                    LocalizationService.GetResource("Admin.ExportField.OrdersCount"),
                    LocalizationService.GetResource("Admin.ExportField.OrdersSum"),
                    LocalizationService.GetResource("Admin.ExportField.PaiedOrdersCount"),
                    LocalizationService.GetResource("Admin.ExportField.PaiedOrdersSum"),
                    LocalizationService.GetResource("Admin.ExportField.RegDate"),
                    LocalizationService.GetResource("Admin.ExportField.Rating")
                };

                var additionalFields = new List<CustomerField>();
                if (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.CustomerAdditionFields))
                {
                    additionalFields = CustomerFieldService.GetCustomerFields(true);
                    foreach (var field in additionalFields)
                    {
                        columns.Add(field.Name);
                    }
                }

                foreach (var item in columns)
                    writer.WriteField(item);

                writer.NextRecord();

                for (int row = 0; row < data.Rows.Count; row++)
                {
                    if (!CommonStatistic.IsRun) return;

                    writer.WriteField(data.Rows[row]["Email"]);
                    writer.WriteField(string.Format("{0} {1} {2}", data.Rows[row]["FirstName"], data.Rows[row]["LastName"], data.Rows[row]["Patronymic"]));
                    writer.WriteField(data.Rows[row]["Phone"]);

                    writer.WriteField(data.Rows[row]["Country"]);
                    writer.WriteField(data.Rows[row]["City"]);
                    writer.WriteField(data.Rows[row]["GroupName"]);

                    //writer.WriteField(string.Join(csvAddressSeparator,
                    //    CustomerService.GetCustomerContacts(SQLDataHelper.GetGuid(data.Rows[row]["CustomerID"]))
                    //        .Select(CustomerService.ConvertToLinedAddress)));

                    writer.WriteField(SQLDataHelper.GetInt(data.Rows[row]["CountOrders"]).ToString());
                    writer.WriteField(SQLDataHelper.GetFloat(data.Rows[row]["SumOrders"]).ToString("F2"));
                    writer.WriteField(SQLDataHelper.GetInt(data.Rows[row]["CountPay"]).ToString());
                    writer.WriteField(SQLDataHelper.GetFloat(data.Rows[row]["SumPay"]).ToString("F2"));

                    writer.WriteField(SQLDataHelper.GetDateTime(data.Rows[row]["RegistrationDateTime"]).ToString());
                    writer.WriteField(SQLDataHelper.GetInt(data.Rows[row]["Rating"]).ToString());

                    if (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.CustomerAdditionFields))
                    {
                        var values = CustomerFieldService.GetCustomerFieldsWithValue((Guid)data.Rows[row]["CustomerID"]);
                        foreach (var field in additionalFields)
                        {
                            var customerFieldValue = values.FirstOrDefault(item => item.Id == field.Id);
                            writer.WriteField(customerFieldValue != null ? customerFieldValue.Value : string.Empty);
                        }
                    }

                    writer.NextRecord();

                    CommonStatistic.RowPosition++;
                }
            }
        }

        private CsvWriter InitWriter(string strFullPath, string csvEnconing, string csvSeparator)
        {
            var writer = new CsvWriter(new StreamWriter(strFullPath, false, Encoding.GetEncoding(csvEnconing)));
            writer.Configuration.Delimiter = csvSeparator;

            return writer;
        }


    }
}
