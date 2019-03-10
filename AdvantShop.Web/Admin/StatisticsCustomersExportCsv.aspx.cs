//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.ExportImport;
using AdvantShop.FilePath;
using AdvantShop.Helpers;

using AdvantShop.Saas;
using AdvantShop.Statistic;
using CsvHelper;
using Resources;

namespace Admin
{
    public partial class StatisticsCustomersExportCsv : AdvantShopAdminPage
    {
        private readonly string _strFilePath;
        private string _strFullPath;
        private const string StrFileName = "StatisticsCustomers";
        private const string StrFileExt = ".csv";
        protected string ExtStrFileName;
        private string _csvEnconing;
        private string _csvSeparator;
        private string _csvAddressSeparator;
        private int? _csvGroup;
        private DateTime? _csvDateFrom;
        private DateTime? _csvDateTo;

        public StatisticsCustomersExportCsv()
        {
            _strFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            FileHelpers.CreateDirectory(_strFilePath);
            var firstFile = Directory.GetFiles(_strFilePath).FirstOrDefault(f => f.Contains(StrFileName));
            _strFullPath = firstFile;
            ExtStrFileName = Path.GetFileName(firstFile);

            if (!string.IsNullOrWhiteSpace(_strFullPath)) return;
            ExtStrFileName = (StrFileName + StrFileExt).FileNamePlusDate();
            _strFullPath = _strFilePath + ExtStrFileName;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            CommonHelper.DisableBrowserCache();
            divAction.Visible = !CommonStatistic.IsRun;
            divbtnAction.Visible = !CommonStatistic.IsRun;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveExcel)
            {
                mainDiv.Visible = false;
                notInTariff.Visible = true;
            }

            //hrefAgaint.Visible = false;
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ExportExcel_Title));

            if (!IsPostBack)
            {
                ddlEncoding.Items.Clear();
                foreach (var enumItem in (EncodingsEnum[])Enum.GetValues(typeof(EncodingsEnum)))
                {
                    ddlEncoding.Items.Add(new ListItem(enumItem.ToString(), enumItem.StrName()));
                }

                ddlSeparetors.Items.Clear();
                foreach (var enumItem in (SeparatorsEnum[])Enum.GetValues(typeof(SeparatorsEnum)))
                {
                    ddlSeparetors.Items.Add(new ListItem(enumItem.Localize(), enumItem.StrName()));
                }
                ddlSeparetors.SelectedValue = SeparatorsEnum.SemicolonSeparated.StrName();

                ddlGroup.Items.Clear();
                ddlGroup.Items.Add(new ListItem("Все", ""));
                foreach (var group in CustomerGroupService.GetCustomerGroupList())
                {
                    ddlGroup.Items.Add(new ListItem(group.GroupName, group.CustomerGroupId.ToString()));
                }

            }

            MsgErr(true);
            OutDiv.Visible = CommonStatistic.IsRun;
            linkCancel.Visible = CommonStatistic.IsRun;
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (lError.Visible) return;
            if (CommonStatistic.IsRun) return;

            //delete old
            foreach (var item in Directory.GetFiles(_strFilePath).Where(f => f.Contains(StrFileName)))
            {
                FileHelpers.DeleteFile(item);
            }

            divAction.Visible = false;
            divbtnAction.Visible = false;

            _csvEnconing = ddlEncoding.SelectedValue;
            _csvSeparator = ddlSeparetors.SelectedValue == SeparatorsEnum.Custom.StrName() ? txtCustomSeparator.Text : ddlSeparetors.SelectedValue;

            _csvGroup = ddlGroup.SelectedValue.TryParseInt(true);

            _csvAddressSeparator = txtAddressSeparator.Text;

            if (!string.IsNullOrEmpty(txtDateFrom.Text))
            {
                DateTime? d = null;
                try
                {
                    d = DateTime.Parse(txtDateFrom.Text);
                }
                catch (Exception)
                {
                }
                if (d.HasValue)
                {
                    var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 0, 0, 0, 0);
                    _csvDateFrom = dt;
                }
            }

            if (!string.IsNullOrEmpty(txtDateTo.Text))
            {
                DateTime? d = null;
                try
                {
                    d = DateTime.Parse(txtDateTo.Text);
                }
                catch (Exception)
                {
                }
                if (d.HasValue)
                {
                    var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 23, 59, 59, 99);
                    _csvDateTo = dt;
                }
            }

            CommonStatistic.Init();
            CommonStatistic.IsRun = true;
            CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
            CommonStatistic.CurrentProcessName = lblSubHead.Text;
            linkCancel.Visible = true;
            OutDiv.Visible = true;
            btnDownload.Visible = false;

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
                    SaveCustomersStatisticToCsv(_strFullPath, _csvEnconing, _csvSeparator, _csvAddressSeparator, _csvGroup, _csvDateFrom, _csvDateTo);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    CommonStatistic.WriteLog(ex.Message);
                }
                CommonStatistic.IsRun = false;
            });
        }

        private void SaveCustomersStatisticToCsv(string strFullPath, string csvEnconing, string csvSeparator, string csvAddressSeparator, int? csvGroup, DateTime? csvDateFrom, DateTime? csvDateTo)
        {
            var sqlParameters = new List<SqlParameter>();
            var paramsss = (csvDateFrom.HasValue ? " AND [RegistrationDateTime] >= @DateFrom" : "") +
                          (csvDateTo.HasValue ? " AND [RegistrationDateTime] <= @DateTo" : "") +
                          (csvGroup.HasValue ? " AND [CustomerGroupId] = @GroupId" : "");
            paramsss = paramsss.Trim(" AND".ToCharArray());

            var cmd = string.Format("SELECT [Customer].[CustomerID], [Email], [FirstName], [LastName], [Patronymic], [Phone], [RegistrationDateTime], [Rating], [GroupName]," +

                                    "(SELECT Top(1) Country  FROM [Customers].[Contact] Where [Contact].[CustomerID] = [Customer].[CustomerID]) As Country, " +
                                    "(SELECT Top(1) City  FROM [Customers].[Contact] Where [Contact].[CustomerID] = [Customer].[CustomerID]) As City, " +

                                    "(SELECT COUNT([Order].[OrderID]) FROM [Order].[Order] INNER JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] WHERE [PaymentDate] IS NOT NULL AND [OrderCustomer].[CustomerID]=[Customer].[CustomerID]) AS CountPay, " +
                                    "(SELECT ISNULL(SUM([Order].[Sum]),0) FROM [Order].[Order] INNER JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] WHERE [PaymentDate] IS NOT NULL AND [OrderCustomer].[CustomerID]=[Customer].[CustomerID]) AS SumPay, " +
                                    "(SELECT COUNT([Order].[OrderID]) FROM [Order].[Order] INNER JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] WHERE [OrderCustomer].[CustomerID]=[Customer].[CustomerID]) AS CountOrders, " +
                                    "(SELECT ISNULL(SUM([Order].[Sum]),0) FROM [Order].[Order] INNER JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] WHERE [OrderCustomer].[CustomerID]=[Customer].[CustomerID]) AS SumOrders " +
                                    "FROM [Customers].[Customer] INNER JOIN [Customers].[CustomerGroup] ON [Customer].[CustomerGroupId] = [CustomerGroup].[CustomerGroupId] {0} {1}",
                                    csvDateFrom.HasValue || csvDateTo.HasValue || csvGroup.HasValue ? " Where " : "", paramsss);
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
                var columns = new[]
                {
                    Resource.Admin_ExportField_Email,
                    Resource.Admin_ExportField_Name,
                    Resource.Admin_ExportField_Phone,
                    Resource.Admin_ExportField_Country,
                    Resource.Admin_ExportField_City,
                    Resource.Admin_ExportField_CustomerGroup,
                    Resource.Admin_ExportField_OrdersCount,
                    Resource.Admin_ExportField_OrdersSum,
                    Resource.Admin_ExportField_PaiedOrdersCount,
                    Resource.Admin_ExportField_PaiedOrdersSum,
                    Resource.Admin_ExportField_RegDate,
                    Resource.Admin_ExportField_Rating,
                };

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

        protected void linkCancel_Click(object sender, EventArgs e)
        {
            CommonStatistic.IsRun = false;
            CommonStatistic.Init();
            linkCancel.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (CommonStatistic.IsRun)
            {
                ltLink.Text = string.Empty;
                return;
            }
            if (File.Exists(_strFullPath))
            {
                var f = new FileInfo(_strFullPath);
                double sizeM = (double)f.Length / 1048576; //1024 * 1024

                string sizeMesage;
                if ((int)sizeM > 0)
                {
                    sizeMesage = ((int)sizeM) + " MB";
                }
                else
                {
                    double sizeK = (double)f.Length / 1024;
                    if ((int)sizeK > 0)
                    {
                        sizeMesage = ((int)sizeK) + " KB";
                    }
                    else
                    {
                        sizeMesage = (f.Length) + " B";
                    }
                }

                var temp = @"<a href='" + UrlService.GetAbsoluteLink("content/price_temp/" + ExtStrFileName) + @"' {0}>" +
                           Resource.Admin_ExportExcel_DownloadFile + @"</a>";

                //spanMessage.Text
                var t = @"<span> " + Resource.Admin_ExportExcel_FileSize + @": " + sizeMesage + @"</span>" + @"<span>, " + AdvantShop.Localization.Culture.ConvertDate(File.GetLastWriteTime(_strFullPath)) + @"</span>";
                ltLink.Text = string.Format(temp, "") + t;
            }
            else
            {
                ltLink.Text = @"<span>" + Resource.Admin_ExportExcel_NotExistDownloadFile + @"</span>";
            }
        }

        private void MsgErr(bool clean)
        {
            if (clean)
            {
                lError.Visible = false;
                lError.Text = string.Empty;
            }
            else
            {
                lError.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            lError.Visible = true;
            lError.Text = @"<br/>" + messageText;
        }

        public class ExportCustomerDto {
            public string Email { get;set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Phone { get; set; }
            public string Country { get; set; }
            public string City { get; set; }
            public string GroupName { get; set; }
            public int CountOrders { get; set; }
            public float SumOrders { get; set; }
            public int CountPay { get; set; }
            public float SumPay { get; set; }
            public DateTime RegistrationDateTime { get; set; }
            public int Rating { get; set; }
        }
    }
}