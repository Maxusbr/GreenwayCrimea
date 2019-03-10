using System.Linq;
using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Products
{
    public class ChangeRelatedProductSortOrder
    {
        private readonly int _productId;
        private readonly RelatedType _type;
        private readonly int _relProductId;
        private readonly int? _prevProductId;
        private readonly int? _nextProductId;

        public ChangeRelatedProductSortOrder(int productId, RelatedType type, int relProductId, int? prevProductId, int? nextProductId)
        {
            _productId = productId;
            _type = type;
            _relProductId = relProductId;
            _prevProductId = prevProductId;
            _nextProductId = nextProductId;
        }

        public bool Execute()
        {
            //var photo = PhotoService.GetPhoto(_photoId);
            //if (photo == null)
            //    return false;

            //Photo prevPhoto = null;
            //Photo nextPhoto = null;

            //if (_prevPhotoId != null)
            //    prevPhoto = PhotoService.GetPhoto(_prevPhotoId.Value);

            //if (_nextPhotoId != null)
            //    nextPhoto = PhotoService.GetPhoto(_nextPhotoId.Value);

            //if (prevPhoto == null && nextPhoto == null)
            //    return false;

            //if (prevPhoto != null && nextPhoto != null)
            //{
            //    if (nextPhoto.PhotoSortOrder - prevPhoto.PhotoSortOrder > 1)
            //    {
            //        photo.PhotoSortOrder = prevPhoto.PhotoSortOrder + 1;
            //        PhotoService.UpdatePhoto(photo);
            //    }
            //    else
            //    {
            //        UpdateSortOrderForAll(photo, prevPhoto, nextPhoto);
            //    }
            //}
            //else
            //{
            //    UpdateSortOrderForAll(photo, prevPhoto, nextPhoto);
            //}

            return true;
        }

        private void UpdateSortOrderForAll(Photo photo, Photo prevPhoto, Photo nextPhoto)
        {
            //var photos =
            //    PhotoService.GetPhotos(_productId, PhotoType.Product)
            //        .Where(x => x.PhotoId != _photoId)
            //        .OrderBy(x => x.PhotoSortOrder)
            //        .ToList();


            //if (prevPhoto != null)
            //{
            //    var index = photos.FindIndex(x => x.PhotoId == prevPhoto.PhotoId);
            //    photos.Insert(index + 1, photo);
            //}
            //else if (nextPhoto != null)
            //{
            //    var index = photos.FindIndex(x => x.PhotoId == nextPhoto.PhotoId);
            //    photos.Insert(index > 0 ? index - 1 : 0, photo);
            //}

            //for (int i = 0; i < photos.Count; i++)
            //{
            //    photos[i].PhotoSortOrder = i * 10 + 10;
            //    PhotoService.UpdatePhoto(photos[i]);
            //}
        }
    }
}
