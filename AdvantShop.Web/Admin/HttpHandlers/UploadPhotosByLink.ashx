<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.UploadPhotosByLink" %>

using System;
using System.IO;
using System.Linq;
using System.Web;
using AdvantShop.Catalog;
using System.Drawing;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using AdvantShop.Trial;

namespace Admin.HttpHandlers
{
    public class UploadPhotosByLink : AdminHandler, IHttpHandler
    {
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
                    context.Response.Write(0);
                    return;
                }
            }

            var productId = SQLDataHelper.GetInt(context.Request["productid"]);
            if (!ProductService.IsExists(productId))
            {
                context.Response.Write(0);
                return;
            }
            
            if (context.Request["links"].IsNullOrEmpty())
            {
                context.Response.Write(0);
                return;
            }

            var photosCount = 0;
            foreach (var link in context.Request["links"].Split(','))
            {
                if (string.IsNullOrEmpty(link))
                {
                    continue;
                }

                if (!FileHelpers.CheckFileExtension(link, EAdvantShopFileTypes.Image))
                {
                    continue;
                }

                try
                {
                    FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));

                    var photoName = link.Md5() + Path.GetExtension(link).Split('?').FirstOrDefault();
                    var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);

                    if (string.IsNullOrWhiteSpace(link) || PhotoService.IsProductHaveThisPhotoByName(productId, photoName) ||
                        !FileHelpers.DownloadRemoteImageFile(link, photoFullName))
                    {
                        continue;
                    }
                    
                    
                    var tempName = PhotoService.AddPhoto(new Photo(0, productId, PhotoType.Product) { OriginName = photoName });
                    
                    if (string.IsNullOrWhiteSpace(tempName)) continue;
                    
                    using (Image image = Image.FromFile(photoFullName))
                    {
                        FileHelpers.SaveProductImageUseCompress(tempName, image);
                        photosCount++;
                    }
                    FileHelpers.DeleteFile(photoFullName);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex.Message + " at Uploadimage", ex);
                }
            }

            ProductService.PreCalcProductParams(productId);
            TrialService.TrackEvent(TrialEvents.AddProductPhoto, "");
            context.Response.Write(photosCount);
        }
    }
}