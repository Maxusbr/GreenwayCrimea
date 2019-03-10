using AdvantShop.Catalog;
using AdvantShop.Core.Caching;
using AdvantShop.Web.Admin.Models.Categories;

namespace AdvantShop.Web.Admin.Handlers.UploadPictures
{
    public class DeletePictureHandler
    {
        private readonly int _puctureId;

        public DeletePictureHandler(int puctureId)
        {
            _puctureId = puctureId;
        }

        public UploadCategoryPictureResult Execute()
        {
            var photo = PhotoService.GetPhoto(_puctureId);
            PhotoService.DeletePhotoWithPath(photo.Type, photo.PhotoName);

            if (photo.Type.ToString().Contains("Category") && photo.ObjId != 0)
            {
                CacheManager.RemoveByPattern(CacheNames.Category);                
            }
            else if (photo.Type == PhotoType.News && photo.ObjId != 0)
            {
                CacheManager.RemoveByPattern(CacheNames.News);
            }


            return new UploadCategoryPictureResult() { Result = true, Picture = "../images/nophoto_xsmall.jpg" };
        }
    }
}
