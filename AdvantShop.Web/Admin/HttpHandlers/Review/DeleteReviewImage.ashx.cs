using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;

namespace AdvantShop.Admin.HttpHandlers.Review
{
    public class DeleteReviewImage : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
            {
                return;
            }
            context.Response.ContentType = "application/json";
            int reviewId = context.Request["reviewid"].TryParseInt();
            if (reviewId != 0)
            {
                PhotoService.DeletePhotos(reviewId, PhotoType.Review);
            }
        }
    }
}