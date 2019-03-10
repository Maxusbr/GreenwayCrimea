using System;
using System.Web;
using AdvantShop.Catalog;
using System.Drawing;
using AdvantShop.Configuration;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;

namespace AdvantShop.Admin.HttpHandlers.Review
{
    public class UploadReviewImage : AdminHandler, IHttpHandler
    {
        static void Msg(HttpContext context, string msg)
        {
            context.Response.Write("{error:'" + msg + "', msg:'error'}");
        }

        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
            {
                return;
            }

            context.Response.ContentType = "text/html";

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
                    var reviewId = SQLDataHelper.GetInt(context.Request["reviewid"]);
                    PhotoService.DeletePhotos(reviewId, PhotoType.Review);

                    var photoName = PhotoService.AddPhoto(new Photo(0, reviewId, PhotoType.Review)
                    {
                        OriginName = pf.FileName
                    });

                    if (string.IsNullOrWhiteSpace(photoName)) continue;
                    using (Image image = Image.FromStream(pf.InputStream))
                    {
                        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.ReviewImage, photoName), SettingsPictureSize.ReviewImageWidth, SettingsPictureSize.ReviewImageHeight, image);
                    }
                }
                catch (Exception ex)
                {
                    Msg(context, ex.Message + " at UploadReviewImage");
                    Debug.Log.Error(ex.Message + " at UploadReviewImage", ex);
                    return;
                }
            }

            context.Response.Write("{error:'', msg:'success'}");
        }
    }
}