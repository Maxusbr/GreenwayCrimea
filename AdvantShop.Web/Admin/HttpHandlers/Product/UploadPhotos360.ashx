<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Product.UploadPhotos360" %>

using System;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using AdvantShop.Trial;

namespace Admin.HttpHandlers.Product
{
    public class UploadPhotos360 : AdminHandler, IHttpHandler
    {
        static void Msg(HttpContext context, string msg)
        {
            context.Response.Write("{error:'" + msg + "', msg:'error'}");
        }

        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            context.Response.ContentType = "text/html";

            var productId = 0;

            if (!int.TryParse(context.Request["productid"], out productId))
            {
                context.Response.Write("{error:'invalid ProductId', msg:'error'}");
                return;
            }

            if (SaasDataService.IsSaasEnabled)
            {
                int maxPhotoCount = SaasDataService.CurrentSaasData.PhotosCount;
                if (PhotoService.GetCountPhotos(productId, PhotoType.Product) >= maxPhotoCount)
                {
                    Msg(context, Resources.Resource.Admin_UploadPhoto_MaxReached + maxPhotoCount);
                    return;
                }
            }

            if (context.Request.Files.Count < 1)
            {
                context.Response.Write("{error:'no file', msg:'error'}");
                return;
            }

            for (int i = 0; i < context.Request.Files.Count; i++)
            {

                HttpPostedFile pf = context.Request.Files[i];
                if (string.IsNullOrEmpty(pf.FileName))
                {
                    context.Response.Write("{error:'no file', msg:'error'}");
                    return;
                }

                if (!pf.FileName.Contains("."))
                {
                    context.Response.Write("{error:'no file extension', msg:'error'}");
                    return;
                }
                if (!FileHelpers.CheckFileExtension(pf.FileName, EAdvantShopFileTypes.Image))
                {
                    context.Response.Write("{error:'wrong extension', msg:'error'}");
                    return;
                }

                var fileName = pf.FileName.Substring(pf.FileName.LastIndexOf("\\")+1);

                try
                {
                    var photoId = PhotoService.AddPhotoWithOrignName(new Photo(0, productId, PhotoType.Product360) { OriginName = pf.FileName, PhotoName = fileName, Main = false });

                    if (photoId == 0)
                    {
                        continue;
                    }

                    var filePath = FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Rotate, string.Empty) + productId + "/";

                    FileHelpers.CreateDirectory(filePath);
                    FileHelpers.SaveFile(filePath + fileName, pf.InputStream);

                    ProductService.PreCalcProductParams(productId);
                }
                catch (Exception ex)
                {
                    Msg(context, ex.Message + " at Uploadimage");
                    Debug.Log.Error(ex.Message + " at Uploadimage", ex);
                    return;
                }
            }
            

            TrialService.TrackEvent(TrialEvents.AddProductPhoto, "");
            context.Response.Write("{error:'', msg:'success'}");
            
        }
    }
}