<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.UploadPhoto" %>

using System;
using System.Web;
using AdvantShop.Catalog;
using System.Drawing;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using AdvantShop.Trial;

namespace Admin.HttpHandlers
{
    public class UploadPhoto : AdminHandler, IHttpHandler
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

            if (SaasDataService.IsSaasEnabled)
            {
                int maxPhotoCount = SaasDataService.CurrentSaasData.PhotosCount;
                if (PhotoService.GetCountPhotos(SQLDataHelper.GetInt(context.Request["productid"]), PhotoType.Product) >= maxPhotoCount)
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

                try
                {
                    var productId = SQLDataHelper.GetInt(context.Request["productid"]);
                    var tempName =
                        PhotoService.AddPhoto(new Photo(0, productId, PhotoType.Product)
                            {
                                Description = context.Request["description"],
                                OriginName = pf.FileName
                            });
                    if (string.IsNullOrWhiteSpace(tempName)) continue;
                    using (Image image = Image.FromStream(pf.InputStream, true))
                    {
                        FileHelpers.SaveProductImageUseCompress(tempName, image);
                    }
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