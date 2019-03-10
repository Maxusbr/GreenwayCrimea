<%@ WebHandler Language="C#" Class=" Admin.HttpHandlers.Catalog.UploadColorPicture" %>

using System;
using System.Web;
using AdvantShop.Catalog;
using System.Drawing;
using AdvantShop.Configuration;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.FilePath;
using AdvantShop.Helpers;

namespace Admin.HttpHandlers.Catalog
{
    public class UploadColorPicture : AdminHandler, IHttpHandler
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
                    
                    
                    var colorid = SQLDataHelper.GetInt(context.Request["colorid"]);
                    PhotoService.DeletePhotos(colorid, PhotoType.Color);
                    
                    var tempName = PhotoService.AddPhoto(new Photo(0, colorid, PhotoType.Color)
                            {
                                OriginName = pf.FileName
                            });
                   
                    if (string.IsNullOrWhiteSpace(tempName)) continue;
                    using (Image image = Image.FromStream(pf.InputStream))
                    {
                        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetImageColorPathAbsolut(ColorImageType.Catalog, tempName), SettingsPictureSize.ColorIconWidthCatalog, SettingsPictureSize.ColorIconHeightCatalog, image);
                        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetImageColorPathAbsolut(ColorImageType.Details, tempName), SettingsPictureSize.ColorIconWidthDetails, SettingsPictureSize.ColorIconHeightDetails, image);
                    }
                }
                catch (Exception ex)
                {
                    Msg(context, ex.Message + " at UploadColorimage");
                    AdvantShop.Diagnostics.Debug.Log.Error(ex.Message + " at UploadColorimage", ex);
                    return;
                }
            }

            context.Response.Write("{error:'', msg:'success'}");
        }
    }
}