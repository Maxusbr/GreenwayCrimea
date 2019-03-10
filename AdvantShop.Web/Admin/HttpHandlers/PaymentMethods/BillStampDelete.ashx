<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.BillStampDelete" %>

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Payment;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Order
{
    public class BillStampDelete : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            context.Response.ContentType = "text/html";

            var methodId = context.Request["methodId"].TryParseInt();
            var method = PaymentService.GetPaymentMethod(methodId);

            if (method == null)
            {
                ReturnResult(context, new { result = false, message = "Shipping method is not found" });
                return;
            }

            try
            {
                if (method.Parameters == null)
                    method.Parameters = new Dictionary<string, string>();

                if (!method.Parameters.ContainsKey(BillTemplate.StampImageName))
                    method.Parameters.Add(BillTemplate.StampImageName, "");

                if (!string.IsNullOrEmpty(method.Parameters[BillTemplate.StampImageName]))
                    FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, method.Parameters[BillTemplate.StampImageName]));
                    
                var parameters = method.Parameters;
                parameters[BillTemplate.StampImageName] = "";

                method.Parameters = parameters;

                PaymentService.UpdatePaymentParams(method.PaymentMethodId, method.Parameters);

                ReturnResult(context, new {error = true});
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        private static void ReturnResult(HttpContext context, object result)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { result }));
        }
    }
}