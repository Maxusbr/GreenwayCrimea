<%@ WebHandler Language="C#" Class=" Admin.HttpHandlers.Catalog.DeleteColorPicture" %>

using System;
using System.Web;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;


namespace Admin.HttpHandlers.Catalog
{
    public class DeleteColorPicture : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
            {
                return;
            }
            context.Response.ContentType = "application/json";
            int colorid = context.Request["colorid"].TryParseInt();
            if (colorid != 0)
            {
                PhotoService.DeletePhotos(colorid, PhotoType.Color);
            }
        }
    }
}