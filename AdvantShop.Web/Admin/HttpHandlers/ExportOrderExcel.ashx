<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.ExportOrderExcel" %>

using System;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Diagnostics;
using AdvantShop.ExportImport.Excel;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using System.Linq;
using System.Text.RegularExpressions;

namespace Admin.HttpHandlers
{
    public class ExportOrderExcel : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var order = OrderService.GetOrder(SQLDataHelper.GetInt(context.Request["OrderID"]));
            order.OrderItems = order.OrderItems.OrderByDescending(x => x.Name).ToList();
            if(order.OrderItems.Count > 1)
            {
                var temp = order.OrderItems[order.OrderItems.Count - 1];
                var orderItems = order.OrderItems.Where(x => x.OrderItemID != temp.OrderItemID).OrderByDescending(x => x.Name).ToList();
                order.OrderItems.Clear();
                order.OrderItems.Add(temp);
                order.OrderItems.AddRange(orderItems.ToArray());
            }
            if (order != null)
            {
                try
                {
                    string strPath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
                    FileHelpers.CreateDirectory(strPath);

                    Regex regex = new Regex(@"\W*", RegexOptions.Compiled);
                    var matches = regex.Matches(order.Number);
                    var filename = string.Format("Order{0}.xlsx", ReplaceRegexSymbols(order.Number, matches));
                    var templatePath = context.Server.MapPath(ExcelExport.templateSingleOrder);
                    ExcelExport.SingleOrder(templatePath, strPath + filename, order);

                    CommonHelper.WriteResponseXlsx(strPath + filename, filename);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Error on creating xls document");
                }
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("Error on creating xls document");
            }
        }

        private string ReplaceRegexSymbols(string str, MatchCollection matches)
        {
            foreach (Match item in matches)
            {
                if(!string.IsNullOrEmpty(item.Value))
                    str = str.Replace(item.Value, string.Empty);
            }
            return str;
        }
    }
}