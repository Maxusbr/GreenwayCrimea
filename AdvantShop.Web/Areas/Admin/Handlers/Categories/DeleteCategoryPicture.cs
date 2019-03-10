using AdvantShop.Catalog;
using AdvantShop.Web.Admin.Models.Categories;

namespace AdvantShop.Web.Admin.Handlers.Categories
{
    public class DeleteCategoryPictureHandler
    {
        private readonly int _puctureId;

        public DeleteCategoryPictureHandler(int puctureId)
        {
            _puctureId = puctureId;
        }

        public UploadCategoryPictureResult Execute()
        {
            var photo = PhotoService.GetPhoto(_puctureId);
            PhotoService.DeletePhotoWithPath(photo.Type, photo.PhotoName);

            if (photo.ObjId != 0)
            {                
                CategoryService.ClearCategoryCache(photo.ObjId);
            }

            var nophoto = string.Empty;

            switch (photo.Type)
            {
                case PhotoType.CategoryIcon:
                    nophoto = "../images/nophoto_xsmall.jpg";
                    break;

                case PhotoType.CategorySmall:
                    nophoto = "../images/nophoto_small.jpg";
                    break;
                case PhotoType.CategoryBig:
                    nophoto = "../images/nophoto_big.jpg";
                    break;
            }

            return new UploadCategoryPictureResult() { Result = true, Picture = nophoto };
        }
    }
}
