//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.ExportImport.Excel;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Saas;
using AdvantShop.Statistic;
using Resources;

namespace Admin
{
    public partial class Admin_ExportOrdersExcel : AdvantShopAdminPage
    {
        private readonly string _strFilePath;
        private readonly string _strFullPath;
        public string NotDoPost = "";
        public string Link = "";
        private const string StrFileName = "orders.csv";

        public Admin_ExportOrdersExcel()
        {
            _strFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            FileHelpers.CreateDirectory(_strFilePath);
            _strFullPath = string.Format("{0}{1}", _strFilePath, StrFileName);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ExportOrdersExcel_Title));

            if ((SaasDataService.IsSaasEnabled) && (!SaasDataService.CurrentSaasData.HaveExcel))
            {
                mainDiv.Visible = false;
                notInTariff.Visible = true;
            }

            lError.Visible = false;

            if (IsPostBack) return;
            OutDiv.Visible = CommonStatistic.IsRun;
            linkCancel.Visible = CommonStatistic.IsRun;
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (CommonStatistic.IsRun)
            {
                lError.Visible = true;
                lError.Text = Resource.Admin_Restrict_Action_In_demo;
                return;
            }
            var paging = new SqlPaging
            {
                TableName = "[Order].[Order]"
            };
            paging.AddField(new Field { Name = "*" });

            if (chkStatus.Checked)
            {
                paging.AddField(new Field
                {
                    Name = "OrderStatusID",
                    NotInQuery = true,
                    Filter = new EqualFieldFilter { ParamName = "@OrderStatusID", Value = ddlStatus.SelectedValue }
                });
            }

            if (chkDate.Checked)
            {
                var filter = new DateTimeRangeFieldFilter { ParamName = "@RDate" };
                var dateFrom = txtDateFrom.Text.TryParseDateTime();
                filter.From = dateFrom != DateTime.MinValue ? dateFrom : new DateTime(2000, 1, 1);

                var dateTo = txtDateTo.Text.TryParseDateTime();
                filter.To = dateTo != DateTime.MinValue ? dateTo.AddDays(1) : new DateTime(3000, 1, 1);
                paging.AddField(new Field { Name = "OrderDate", NotInQuery = true, Filter = filter });
            }
            var ordersCount = paging.TotalRowsCount;

            if (ordersCount == 0) return;
            CommonStatistic.Init();
            CommonStatistic.IsRun = true;
            CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
            CommonStatistic.CurrentProcessName = Resource.Admin_ExportOrdersExcel_DownloadOrders;
            CommonStatistic.TotalRow = ordersCount;

            linkCancel.Visible = true;
            OutDiv.Visible = true;
            btnDownload.Visible = false;
            pnSearch.Visible = false;

            try
            {
                // Directory
                if (!Directory.Exists(_strFilePath))
                    Directory.CreateDirectory(_strFilePath);

                CommonStatistic.StartNew(() => Save(paging));
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                lError.Text = ex.Message;
                lError.Visible = true;
                CommonStatistic.IsRun = false;
            }
        }

        protected void Save(SqlPaging paging)
        {
            var orders = paging.GetCustomData("*", "", OrderService.GetOrderFromReader);
            ExcelExport.MultiOrder(orders, _strFullPath, "UTF-8");
            CommonStatistic.IsRun = false;
        }

        protected void linkCancel_Click(object sender, EventArgs e)
        {
            CommonStatistic.IsRun = false;
            CommonStatistic.Init();
        }

        protected void btnAsyncLoad_Click(object sender, EventArgs e)
        {
            linkCancel.Visible = false;
            NotDoPost = "true";
            var flag = true;
            while (flag)
            {
                try
                {
                    using (var f = File.Open(_strFullPath, FileMode.Open))
                    {
                        flag = false;
                        f.Close();
                    }
                }
                catch (Exception)
                {
                    flag = true;
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (File.Exists(_strFullPath))
            {
                var f = new FileInfo(_strFullPath);
                const double size = 0;
                double sizeM = (Double)f.Length / 1048576; //1024 * 1024

                string sizeMesage;
                if ((int)sizeM > 0)
                {
                    sizeMesage = ((int)sizeM) + " MB";
                }
                else
                {
                    double sizeK = (Double)f.Length / 1024;
                    if ((int)sizeK > 0)
                    {
                        sizeMesage = ((int)sizeK) + " KB";
                    }
                    else
                    {
                        sizeMesage = ((int)size) + " B";
                    }
                }

                Link = "<a href='" + FoldersHelper.GetPath(FolderType.PriceTemp, StrFileName, false) + "' >" +
                       Resource.Admin_ExportOrdersExcel_DownloadFile +
                       "</a><span> " + Resource.Admin_ExportOrdersExcel_FileSize + ": " + sizeMesage + "</span>";
                Link += "<span>" + ", " +
                        AdvantShop.Localization.Culture.ConvertDate(File.GetLastWriteTime(_strFullPath)) + "</span>";
            }
            else
            {
                Link = "<span>" + Resource.Admin_ExportOrdersExcel_NotExistDownloadFile + "</span>";
            }
        }

        protected void sdsStatus_Init(object sender, EventArgs e)
        {
            sdsStatus.ConnectionString = Connection.GetConnectionString();
        }
    }
}