using System;
using System.IO;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.ExportImport.Excel;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Statistic;
using AdvantShop.Web.Admin.ViewModels.Analytics;

namespace AdvantShop.Web.Admin.Handlers.Analytics
{
    public class ExportOrdersHandler
    {
        private readonly ExportOrdersModel _settings;
        private readonly string _strFilePath;
        private readonly string _strFullPath;
        private const string _filePrefix = "StatisticsOrders";
        private const string _fileExt = ".csv";
        private readonly string _fileName;

        public ExportOrdersHandler(ExportOrdersModel settings)
        {
            _settings = settings;

            _strFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            FileHelpers.CreateDirectory(_strFilePath);
            _fileName = (_filePrefix + _fileExt).FileNamePlusDate();
            _strFullPath = string.Format("{0}{1}", _strFilePath, _fileName);
        }

        public void Execute()
        {
            if (CommonStatistic.IsRun)
            {
                return;
            }
            var paging = new SqlPaging
            {
                //TableName = "[Order].[Order]"
                TableName = "[Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].OrderId = [OrderCustomer].[OrderId]"
            };
            paging.AddField(new Field { Name = "*" });

            if (_settings.UseStatus)
            {
                paging.AddField(new Field
                {
                    Name = "OrderStatusID",
                    NotInQuery = true,
                    Filter = new EqualFieldFilter { ParamName = "@OrderStatusID", Value = _settings.Status.ToString() }
                });
            }

            if (_settings.UsePaid)
            {
                paging.AddField(new Field
                {
                    Name = "PaymentDate",
                    NotInQuery = true,
                    Filter = new NullFieldFilter { ParamName = "@PaymentDate", Null = !_settings.Paid }
                });
            }
            if (_settings.UseShipping)
            {
                paging.AddField(new Field
                {
                    Name = "ShippingMethodID",
                    NotInQuery = true,
                    Filter = new EqualFieldFilter { ParamName = "@ShippingMethodID", Value = _settings.Shipping.ToString() }
                });
            }
            if (_settings.UseCity)
            {
                paging.AddField(new Field
                {
                    Name = "City",
                    NotInQuery = true,
                    Filter = new EqualFieldFilter { ParamName = "@City", Value = _settings.City }
                });
            }
            if (_settings.UseSum)
            {
                var filter = new RangeFieldFilter { ParamName = "@Sum" };
                filter.From = _settings.OrderSumFrom;

                filter.To = _settings.OrderSumTo;
                paging.AddField(new Field { Name = "Sum", NotInQuery = true, Filter = filter });
            }

            if (_settings.UseDate)
            {
                var filter = new DateTimeRangeFieldFilter { ParamName = "@RDate" };
                var dateFrom = _settings.DateFrom;
                filter.From = dateFrom != DateTime.MinValue ? dateFrom : new DateTime(2000, 1, 1);

                var dateTo = _settings.DateTo;
                filter.To = dateTo != DateTime.MinValue ? dateTo.AddDays(1) : new DateTime(3000, 1, 1);
                paging.AddField(new Field { Name = "OrderDate", NotInQuery = true, Filter = filter });
            }
            var ordersCount = paging.TotalRowsCount;

            //if (ordersCount == 0) return;
            CommonStatistic.Init();
            CommonStatistic.IsRun = true;
            CommonStatistic.CurrentProcess = "analytics/exportorders";
            CommonStatistic.CurrentProcessName = LocalizationService.GetResource("Admin.Analytics.ExportOrders.ProcessName");
            CommonStatistic.TotalRow = ordersCount;

            try
            {
                // Directory
                if (!Directory.Exists(_strFilePath))
                    Directory.CreateDirectory(_strFilePath);

                foreach (var item in Directory.GetFiles(_strFilePath).Where(f => f.Contains(_filePrefix)))
                {
                    FileHelpers.DeleteFile(item);
                }

                CommonStatistic.FileName = UrlService.GetAbsoluteLink(FoldersHelper.PhotoFoldersPath[FolderType.PriceTemp] + _fileName);

                CommonStatistic.StartNew(() => Save(paging));
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex.Message, ex);
                CommonStatistic.IsRun = false;
            }
        }

        protected void Save(SqlPaging paging)
        {
            var orders = paging.GetCustomData("*", "", OrderService.GetOrderFromReader);
            ExcelExport.MultiOrder(orders, _strFullPath, _settings.Encoding);

            CommonStatistic.IsRun = false;
        }
    }
}
