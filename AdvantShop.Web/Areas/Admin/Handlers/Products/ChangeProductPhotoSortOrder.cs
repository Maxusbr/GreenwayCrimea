using System.Linq;
using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Products
{
    public class ChangeProductPhotoSortOrder
    {
        private readonly int _productId;
        private readonly int _photoId;
        private readonly int? _prevPhotoId;
        private readonly int? _nextPhotoId;

        public ChangeProductPhotoSortOrder(int productId, int photoId, int? prevPhotoId, int? nextPhotoId)
        {
            _productId = productId;
            _photoId = photoId;
            _prevPhotoId = prevPhotoId;
            _nextPhotoId = nextPhotoId;
        }

        public bool Execute()
        {
            var photo = PhotoService.GetPhoto<ProductPhoto>(_photoId, PhotoType.Product);
            if (photo == null)
                return false;

            ProductPhoto prevPhoto = null;
            ProductPhoto nextPhoto = null;

            if (_prevPhotoId != null)
                prevPhoto = PhotoService.GetPhoto<ProductPhoto>(_prevPhotoId.Value, PhotoType.Product);

            if (_nextPhotoId != null)
                nextPhoto = PhotoService.GetPhoto<ProductPhoto>(_nextPhotoId.Value, PhotoType.Product);

            if (prevPhoto == null && nextPhoto == null)
                return false;

            if (prevPhoto != null && nextPhoto != null)
            {
                if (nextPhoto.PhotoSortOrder - prevPhoto.PhotoSortOrder > 1)
                {
                    photo.PhotoSortOrder = prevPhoto.PhotoSortOrder + 1;
                    PhotoService.UpdatePhoto(photo);
                }
                else
                {
                    UpdateSortOrderForAll(photo, prevPhoto, nextPhoto);
                }
            }
            else
            {
                UpdateSortOrderForAll(photo, prevPhoto, nextPhoto);
            }
            
            return true;
        }

        private void UpdateSortOrderForAll(ProductPhoto photo, ProductPhoto prevPhoto, ProductPhoto nextPhoto)
        {
            var photos =
                PhotoService.GetPhotos<ProductPhoto>(_productId, PhotoType.Product)
                    .Where(x => x.PhotoId != _photoId)
                    .ToList();


            if (prevPhoto != null)
            {
                var index = photos.FindIndex(x => x.PhotoId == prevPhoto.PhotoId);
                photos.Insert(index + 1, photo);
            }
            else if (nextPhoto != null)
            {
                var index = photos.FindIndex(x => x.PhotoId == nextPhoto.PhotoId);
                photos.Insert(index > 0 ? index - 1 : 0, photo);
            }

            for (int i = 0; i < photos.Count; i++)
            {
                photos[i].PhotoSortOrder = i * 10 + 10;
                PhotoService.UpdatePhoto(photos[i]);
            }

            if (photos.Any() && !photos[0].Main)
            {
                PhotoService.SetProductMainPhoto(photos[0].PhotoId);
            }
        }
    }
}
