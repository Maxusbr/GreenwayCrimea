//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Saas;
using AdvantShop.Statistic;

using CsvHelper;
using CsvHelper.Configuration;
using Resources;

using AdvantShop.Core.Services.Catalog;
using AdvantShop.ExportImport;
using System.Web.UI.WebControls;
using AdvantShop.Core.Services.Orders;

namespace Admin
{
    public partial class StatisticsOrdersExportCsv : AdvantShopAdminPage
    {
        private readonly string _strFilePath;
        private string _strFullPath;
        public string NotDoPost = "";
        public string Link = "";
        private const string StrFileName = "StatisticsOrders";
        private const string StrFileExt = ".csv";
        protected string ExtStrFileName;

        public StatisticsOrdersExportCsv()
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
            pnSearch.Visible = !CommonStatistic.IsRun;
            divbtnAction.Visible = !CommonStatistic.IsRun;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ExportOrdersExcel_Title));
			
            if (!IsPostBack)
            {
                ddlEncoding.Items.Clear();
                foreach (var enumItem in (EncodingsEnum[])Enum.GetValues(typeof(EncodingsEnum)))
                {
                    ddlEncoding.Items.Add(new ListItem(enumItem.ToString(), enumItem.StrName()));
                }
            }

            lError.Visible = false;

            if (IsPostBack) return;
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

            linkCancel.Visible = true;
            OutDiv.Visible = true;
            btnDownload.Visible = false;
            pnSearch.Visible = false;


            var paging = new SqlPaging { TableName = "[Order].[Order]" };

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

            ExtStrFileName = (StrFileName + StrFileExt).FileNamePlusDate();
            _strFullPath = _strFilePath + ExtStrFileName;

            try
            {
                // Directory
                if (!Directory.Exists(_strFilePath))
                    Directory.CreateDirectory(_strFilePath);

                var ctx = HttpContext.Current;
                CommonStatistic.StartNew(() =>
                {
                    try
                    {
                        HttpContext.Current = ctx;
                        SaveOrdersStatisticToCsv(paging.GetCustomData("*", "", OrderService.GetOrderFromReader));
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                        CommonStatistic.WriteLog(ex.Message);
                    }
                    CommonStatistic.IsRun = false;
                });

            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                lError.Text = ex.Message;
                lError.Visible = true;
                CommonStatistic.IsRun = false;
            }
        }

        protected void linkCancel_Click(object sender, EventArgs e)
        {
            CommonStatistic.IsRun = false;
            CommonStatistic.Init();
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
                var sizeMesage = FileHelpers.FileSize(_strFullPath);
                var temp = @"<a href='" + UrlService.GetAbsoluteLink("content/price_temp/" + ExtStrFileName) + @"' {0}>" +
                           Resource.Admin_ExportExcel_DownloadFile + @"</a>";

                var t = @"<span> " + Resource.Admin_ExportExcel_FileSize + @": " + sizeMesage + @"</span>" + @"<span>, " + AdvantShop.Localization.Culture.ConvertDate(File.GetLastWriteTime(_strFullPath)) + @"</span>";
                ltLink.Text = string.Format(temp, "") + t;
            }
            else
            {
                ltLink.Text = @"<span>" + Resource.Admin_ExportExcel_NotExistDownloadFile + @"</span>";
            }
        }

        protected void sdsStatus_Init(object sender, EventArgs e)
        {
            sdsStatus.ConnectionString = Connection.GetConnectionString();
        }

        private void SaveOrdersStatisticToCsv(List<Order> orders)
        {
            CommonStatistic.TotalRow = orders.Count;
            
            using (var writer = new CsvWriter(new StreamWriter(_strFullPath, false, Encoding.GetEncoding(ddlEncoding.SelectedValue)), new CsvConfiguration { Delimiter = ";" }))
            {
                var columns = new[]
                {
                    Resource.Admin_ExportField_OrderID,
                    Resource.Admin_ExportField_Status,
                    Resource.Admin_ExportField_OrderDate,
                    Resource.Admin_ExportField_Name,
                    Resource.Admin_ExportField_Email,
                    Resource.Admin_ExportField_Phone,
                    Resource.Admin_ExportField_OrderedItems,
                    Resource.Admin_ExportField_Total,
                    Resource.Admin_ExportField_Currency,
                    Resource.Admin_ExportField_Tax,
                    Resource.Admin_ExportField_Cost,
                    Resource.Admin_ExportField_Profit,
                    Resource.Admin_ExportField_Payment,
                    Resource.Admin_ExportField_Shipping,
                    Resource.Admin_ExportField_ShippingAddress,
                    Resource.Admin_ExportField_CustomerComment,
                    Resource.Admin_ExportField_AdminComment,
                    Resource.Admin_ExportField_StatusComment,
                    Resource.Admin_ExportField_Payed,                    
                };

                foreach (var item in columns)
                    writer.WriteField(item);

                writer.NextRecord();

                foreach (var order in orders)
                {
                    if (!CommonStatistic.IsRun) return;
                    
                    writer.WriteField(order.Number);
                    writer.WriteField(order.OrderStatus != null ? order.OrderStatus.StatusName : "Неизвестный");

                    writer.WriteField(order.OrderDate);
                    if (order.OrderCustomer != null)
                    {
                        writer.WriteField(order.OrderCustomer.LastName + " " + order.OrderCustomer.FirstName);
                        writer.WriteField(order.OrderCustomer.Email ?? string.Empty);
                        writer.WriteField(order.OrderCustomer.Phone ?? string.Empty);
                    }
                    else
                    {
                        writer.WriteField("Неизвестный");
                        writer.WriteField(string.Empty);
                        writer.WriteField(string.Empty);
                    }

                    if (order.OrderCurrency != null)
                    {
                        writer.WriteField(RenderOrderedItems(order.OrderItems) ?? string.Empty);
                        writer.WriteField(PriceService.RoundPrice(order.Sum, order.OrderCurrency));
                        writer.WriteField(order.OrderCurrency.CurrencySymbol);
                        writer.WriteField(PriceService.RoundPrice(order.TaxCost, order.OrderCurrency));
                        float totalCost = order.OrderItems.Sum(oi => oi.SupplyPrice * oi.Amount);
                        writer.WriteField(PriceService.RoundPrice(totalCost, order.OrderCurrency));
                        writer.WriteField(PriceService.RoundPrice(order.Sum - order.ShippingCost - order.TaxCost - totalCost, order.OrderCurrency));
                        writer.WriteField(order.PaymentMethodName);
                        writer.WriteField(order.ArchivedShippingName + " - " + PriceService.RoundPrice(order.ShippingCost, order.OrderCurrency));
                        writer.WriteField(order.OrderCustomer != null
                            ? new List<string>
                                {
                                    order.OrderCustomer.Zip,
                                    order.OrderCustomer.Country,
                                    order.OrderCustomer.Region,
                                    order.OrderCustomer.City,
                                    order.OrderCustomer.GetCustomerAddress(),
                                    order.OrderCustomer.CustomField1,
                                    order.OrderCustomer.CustomField2,
                                    order.OrderCustomer.CustomField3
                                }.Where(s => s.IsNotEmpty()).AggregateString(", ")
                            : string.Empty);
                        writer.WriteField(order.CustomerComment ?? string.Empty);
                        writer.WriteField(order.AdminOrderComment ?? string.Empty);
                        writer.WriteField(order.StatusComment);
                        writer.WriteField(order.Payed ? Resource.Admin_ExportField_Yes : Resource.Admin_ExportField_No);
                    }
                    writer.NextRecord();

                    CommonStatistic.RowPosition++;
                }
            }
        }

        private static string RenderOrderedItems(IEnumerable<OrderItem> items)
        {
            var res = new StringBuilder();

            foreach (OrderItem orderItem in items)
            {
                res.AppendFormat("[{0} - {1} - {2}{3}{4}], ", orderItem.ArtNo, orderItem.Name, orderItem.Amount,
                    Resources.Resource.Admin_ExportOrdersExcel_Pieces, RenderSelectedOptions(orderItem.SelectedOptions, orderItem.Color, orderItem.Size));//orderItem.SelectedOptions != null && orderItem.SelectedOptions.Count > 0 ? RenderSelectedOptions(orderItem.SelectedOptions, orderItem.Color, orderItem.Size) : string.Empty);
            }

            return res.ToString().TrimEnd(new[] { ',', ' ' });
        }

        private static string RenderSelectedOptions(IList<EvaluatedCustomOptions> evlist, string color, string size)
        {
            if((evlist == null | evlist.Count == 0) && color.IsNullOrEmpty() && size.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var html = new StringBuilder();
            html.Append(" (");

            if (!string.IsNullOrEmpty(color))
                html.Append(SettingsCatalog.ColorsHeader + ": " + color + (!string.IsNullOrEmpty(size) || (evlist != null && evlist.Count > 0)? "," : string.Empty));

            if (!string.IsNullOrEmpty(size))
                html.Append(SettingsCatalog.SizesHeader + ": " + size + (evlist != null && evlist.Count > 0 ? "," : string.Empty));

            if(evlist != null && evlist.Count > 0)
            {
                foreach (EvaluatedCustomOptions ev in evlist)
                {
                    html.Append(string.Format("{0}: {1},", ev.CustomOptionTitle, ev.OptionTitle));
                }
            }

            html.Append(")");
            return html.ToString();
        }

    }
}