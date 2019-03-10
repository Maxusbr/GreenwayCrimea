using AdvantShop.News;
using AdvantShop.Web.Admin.Models.News;

namespace AdvantShop.Web.Admin.Handlers.News
{
    public class DeleteNewsPicture
    {
        private readonly int _newsId;

        public DeleteNewsPicture(int? newsId)
        {
            _newsId = newsId != null ? newsId.Value : -1;
        }

        public UploadNewsPictureResult Execute()
        {
            NewsService.DeleteNewsImage(_newsId);
            return new UploadNewsPictureResult() { Result = true, Picture = "../images/nophoto_small.jpg" };
        }
    }
}
