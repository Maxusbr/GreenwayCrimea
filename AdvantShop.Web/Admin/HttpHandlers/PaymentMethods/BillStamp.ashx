<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.BillStamp" %>

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
    public class BillStamp : AdminHandler, IHttpHandler
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
                ReturnResult(context, new { error = true, msg = "Shipping method is not found" });
                return;
            }

            try
            {
                if (context.Request.Files.Count != 1)
                    return;

                var file = context.Request.Files[0];

                if (string.IsNullOrEmpty(file.FileName))
                {
                    context.Response.Write("{error:true, msg:'error'}");
                    return;
                }

                if (!file.FileName.Contains("."))
                {
                    context.Response.Write("{error:true, msg:'error'}");
                    return;
                }
                if (!FileHelpers.CheckFileExtension(file.FileName, EAdvantShopFileTypes.Image))
                {
                    context.Response.Write("{error:true, msg:'error'}");
                    return;
                }
                
                if (method.Parameters == null)
                    method.Parameters = new Dictionary<string, string>();

                var parameters = method.Parameters;

                if (!parameters.ContainsKey(BillTemplate.StampImageName))
                    parameters.Add(BillTemplate.StampImageName, "");

                if (!string.IsNullOrEmpty(parameters[BillTemplate.StampImageName]))
                    FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, parameters[BillTemplate.StampImageName]));

                var imgName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                file.SaveAs(FoldersHelper.GetPathAbsolut(FolderType.Pictures, imgName));
                
                parameters[BillTemplate.StampImageName] = imgName;
                method.Parameters = parameters;

                PaymentService.UpdatePaymentParams(method.PaymentMethodId, method.Parameters);

                ReturnResult(context, new
                {
                    error = false,
                    src = FoldersHelper.GetPath(FolderType.Pictures, imgName, false),
                    imgName = imgName
                });
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        private static void ReturnResult(HttpContext context, object result)
        {
            context.Response.Write(JsonConvert.SerializeObject(result));
        }
    }
}