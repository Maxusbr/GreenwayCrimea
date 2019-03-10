using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.CMS;
using AdvantShop.Core.Services.InplaceEditor;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Trial;
using AdvantShop.CMS;
using System.Drawing;

namespace AdvantShop.Handlers.Inplace
{
    public class InplaceImageHandler
    {
        public List<object> Execute(List<HttpPostedFileBase> files, ImageInplaceField field,
                                    ImageInplaceCommands command, int? colorId, string additionalData, int id, int objId)
        {
            var result = new List<object>();
            var customer = CustomerContext.CurrentCustomer;
            
            switch (field)
            {
                case ImageInplaceField.Logo:
                    if (!IsAuth(RoleAction.Settings))
                        return null;

                    switch (command)
                    {
                        case ImageInplaceCommands.Add:
                        case ImageInplaceCommands.Update:
                            result.Add(UpdateLogo(files[0]));
                            TrialService.TrackEvent(TrialEvents.ChangeLogo, "");
                            TrialService.TrackEvent(ETrackEvent.Trial_ChangeLogo);
                            break;

                        case ImageInplaceCommands.Delete:
                            result.Add(DeleteLogo());
                            break;
                        default:
                            return null;
                    }
                    break;

                case ImageInplaceField.Brand:
                    if (!IsAuth(RoleAction.Catalog))
                        return null;

                    switch (command)
                    {
                        case ImageInplaceCommands.Add:
                        case ImageInplaceCommands.Update:
                            result.Add(UpdateBrandImage(id, files[0]));
                            break;

                        case ImageInplaceCommands.Delete:
                            result.Add(DeleteBrandImage(id));
                            break;
                    }
                    break;

                case ImageInplaceField.News:
                    if (!IsAuth(RoleAction.Crm))
                        return null;

                    switch (command)
                    {
                        case ImageInplaceCommands.Add:
                        case ImageInplaceCommands.Update:
                            result.Add(UpdateNewsImage(id, files[0]));
                            break;

                        case ImageInplaceCommands.Delete:
                            result.Add(DeleteNewsImage(id));
                            break;
                    }
                    break;

                case ImageInplaceField.Carousel:
                    if (!IsAuth(RoleAction.Catalog))
                        return null;

                    switch (command)
                    {
                        case ImageInplaceCommands.Add:
                            for (var i = 0; i < files.Count; i++)
                                result.Add(AddCarouselImage(files[i]));

                            TrialService.TrackEvent(TrialEvents.AddCarousel, "");
                            TrialService.TrackEvent(ETrackEvent.Trial_AddCarousel);
                            break;

                        case ImageInplaceCommands.Update:
                            result.Add(UpdateCarouselImage(id, files[0]));
                            break;

                        case ImageInplaceCommands.Delete:
                            result.Add(DeleteCarouselImage(id));
                            break;
                        default:
                            return null;
                    }
                    break;

                case ImageInplaceField.CategorySmall:
                case ImageInplaceField.CategoryBig:
                    if (!IsAuth(RoleAction.Catalog))
                        return null;

                    var categoryType = field == ImageInplaceField.CategorySmall
                        ? PhotoType.CategorySmall
                        : PhotoType.CategoryBig;

                    switch (command)
                    {
                        case ImageInplaceCommands.Add:
                            result.Add(AddCategoryImage(id, categoryType, files[0]));
                            break;

                        case ImageInplaceCommands.Update:
                            result.Add(AddCategoryImage(id, categoryType, files[0]));
                            break;

                        case ImageInplaceCommands.Delete:
                            result.Add(DeleteCategoryImage(id, categoryType));
                            break;
                        default:
                            return null;
                    }
                    break;

                case ImageInplaceField.Product:
                    if (!IsAuth(RoleAction.Catalog))
                        return null;

                    var productImageType = (ProductImageType)Enum.Parse(typeof(ProductImageType), additionalData);

                    if (colorId == 0)
                        colorId = null;

                    switch (command)
                    {
                        case ImageInplaceCommands.Add:
                            for (var i = 0; i < files.Count; i++)
                                result.Add(AddProductImage(objId, colorId, productImageType, files[i]));
                            break;

                        case ImageInplaceCommands.Update:
                            result.Add(UpdateProductImage(id, objId, colorId, productImageType, files[0]));
                            break;

                        case ImageInplaceCommands.Delete:
                            if (id > 0)
                            {
                                PhotoService.DeleteProductPhoto(id);
                            }

                            var photoPathReturn = string.Empty;
                            var mainPhoto = PhotoService.GetMainProductPhoto(objId, colorId);

                            if (mainPhoto != null)
                            {
                                photoPathReturn = FoldersHelper.GetImageProductPath(productImageType,
                                    mainPhoto.PhotoName, false);
                            }
                            else
                            {
                                photoPathReturn = "images/nophoto_" + productImageType.ToString().ToLower() + ".jpg";
                            }

                            result.Add(ReturnItem(photoPathReturn, mainPhoto != null ? mainPhoto.PhotoId : 0));
                            break;

                        default:
                            return null;
                    }

                    ProductService.PreCalcProductParams(objId);
                    break;

                case ImageInplaceField.Review:
                    if (!IsAuth(RoleAction.Catalog))
                        return null;

                    switch (command)
                    {
                        case ImageInplaceCommands.Add:
                        case ImageInplaceCommands.Update:
                            PhotoService.DeletePhotos(id, PhotoType.Review);

                            var photo = new Photo(0, id, PhotoType.Review)
                            {
                                OriginName = files[0].FileName
                            };

                            var photoName = PhotoService.AddPhoto(photo);

                            using (Image image = Image.FromStream(files[0].InputStream))
                            {
                                FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.ReviewImage, photoName), SettingsPictureSize.ReviewImageWidth, SettingsPictureSize.ReviewImageHeight, image);
                            }

                            result.Add(ReturnItem(FoldersHelper.GetPath(FolderType.ReviewImage, photoName, false), id));
                            break;
                        case ImageInplaceCommands.Delete:
                            PhotoService.DeletePhotos(id, PhotoType.Review);
                            result.Add(ReturnItem("images/nophoto_user.jpg", id));
                            break;
                        default:
                            return null;
                    }
                    break;
                default:
                    return null;
            }

            return result;
        }

        #region Help methods

        private object UpdateLogo(HttpPostedFileBase file)
        {
            FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
            FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsMain.LogoImageName));
            var newFile = file.FileName.FileNamePlusDate("logo");
            SettingsMain.LogoImageName = newFile;
            file.SaveAs(FoldersHelper.GetPathAbsolut(FolderType.Pictures, newFile));

            return ReturnItem(FoldersHelper.GetPath(FolderType.Pictures, newFile, false), null);
        }

        private object DeleteLogo()
        {
            FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsMain.LogoImageName));
            SettingsMain.LogoImageName = string.Empty;

            return ReturnItem("images/nophoto-logo.png", null);
        }


        private object UpdateBrandImage(int id, HttpPostedFileBase file)
        {
            PhotoService.DeletePhotos(id, PhotoType.Brand);

            var tempName = PhotoService.AddPhoto(new Photo(0, id, PhotoType.Brand) { OriginName = file.FileName });
            if (!string.IsNullOrWhiteSpace(tempName))
            {
                using (var image = System.Drawing.Image.FromStream(file.InputStream))
                {
                    FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.BrandLogo, tempName),
                        SettingsPictureSize.BrandLogoWidth,
                        SettingsPictureSize.BrandLogoHeight, image);
                }
            }


            return ReturnItem(FoldersHelper.GetPath(FolderType.BrandLogo, tempName, false), null);
        }

        private object DeleteBrandImage(int id)
        {
            BrandService.DeleteBrandLogo(id);
            return ReturnItem("images/nophoto_xsmall.jpg", null);
        }

        private object UpdateNewsImage(int id, HttpPostedFileBase file)
        {

            PhotoService.DeletePhotos(id, PhotoType.News);

            var tempName = PhotoService.AddPhoto(new Photo(0, id, PhotoType.News) { OriginName = file.FileName });
            if (!string.IsNullOrWhiteSpace(tempName))
            {
                using (var image = System.Drawing.Image.FromStream(file.InputStream))
                    FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.News, tempName),
                        SettingsPictureSize.NewsImageWidth, SettingsPictureSize.NewsImageHeight, image);
            }


            return ReturnItem(FoldersHelper.GetPath(FolderType.News, tempName, false), null);
        }

        private object DeleteNewsImage(int id)
        {
            AdvantShop.News.NewsService.DeleteNewsImage(id);
            return ReturnItem("images/nophoto_small.jpg", null);
        }


        private object AddCarouselImage(HttpPostedFileBase file)
        {
            var carousel = new Carousel() { Url = "#", SortOrder = CarouselService.GetMaxSortOrder(), Enabled = true, DisplayInMobile = true, DisplayInOneColumn = true, DisplayInTwoColumns = true  };

            int id = CarouselService.AddCarousel(carousel);

            return UpdateCarouselImage(id, file);
        }

        private object UpdateCarouselImage(int id, HttpPostedFileBase file)
        {

            PhotoService.DeletePhotos(id, PhotoType.Carousel);

            var tempName = PhotoService.AddPhoto(new Photo(0, id, PhotoType.Carousel) { OriginName = file.FileName });
            if (!string.IsNullOrWhiteSpace(tempName))
            {
                using (var image = System.Drawing.Image.FromStream(file.InputStream))
                    FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.Carousel, tempName),
                        SettingsPictureSize.CarouselBigWidth, SettingsPictureSize.CarouselBigHeight, image);
            }


            return ReturnItem(FoldersHelper.GetPath(FolderType.Carousel, tempName, false), id);
        }

        private object DeleteCarouselImage(int id)
        {
            CarouselService.DeleteCarousel(id);

            return ReturnItem("images/nophoto_carousel_" + SettingsMain.Language + ".jpg", 0);
        }

        private object AddCategoryImage(int id, PhotoType photoType, HttpPostedFileBase file)
        {

            object result;

            PhotoService.DeletePhotos(id, photoType);

            var tempName = PhotoService.AddPhoto(new Photo(0, id, photoType) { OriginName = file.FileName });

            using (System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream))
            {
                switch (photoType)
                {
                    case PhotoType.CategoryBig:
                        FileHelpers.SaveResizePhotoFile(
                            FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Big, tempName),
                            SettingsPictureSize.BigCategoryImageWidth, SettingsPictureSize.BigCategoryImageHeight, image);
                        result = ReturnItem(FoldersHelper.GetImageCategoryPath(CategoryImageType.Big, tempName, false), id);
                        break;

                    case PhotoType.CategorySmall:
                        FileHelpers.SaveResizePhotoFile(
                            FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, tempName),
                            SettingsPictureSize.SmallCategoryImageWidth, SettingsPictureSize.SmallCategoryImageHeight,
                            image);
                        result = ReturnItem(FoldersHelper.GetImageCategoryPath(CategoryImageType.Small, tempName, false), id);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            return result;
        }

        private object DeleteCategoryImage(int id, PhotoType photoType)
        {
            PhotoService.DeletePhotos(id, photoType);

            return ReturnItem(photoType == PhotoType.CategorySmall ? "images/nophoto_small.jpg" : "images/nophoto_big.jpg", id);

        }

        private object AddProductImage(int productId, int? colorId, ProductImageType productImageType,
            HttpPostedFileBase file, int photoSortOrder = 0)
        {

            var photo = new Photo(0, productId, PhotoType.Product)
            {
                OriginName = file.FileName,
                ColorID = colorId,
                PhotoSortOrder = photoSortOrder
            };

            var tempName = PhotoService.AddPhoto(photo);

            using (System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream, true))
            {
                FileHelpers.SaveProductImageUseCompress(tempName, image);
            }

            return ReturnItem(FoldersHelper.GetImageProductPath(productImageType, tempName, false), photo.PhotoId);
        }

        private object UpdateProductImage(int id, int productId, int? colorId, ProductImageType productImageType,
            HttpPostedFileBase file)
        {

            var photo = PhotoService.GetPhoto(id);

            var photoSortOrder = 0;

            if (photo != null)
            {
                photoSortOrder = photo.PhotoSortOrder;
            }

            PhotoService.DeleteProductPhoto(id);

            return AddProductImage(productId, colorId, productImageType, file, photoSortOrder);
        }

        private object ReturnItem(string filename, int? id)
        {
            return new
            {
                id = id,
                filename = filename
            };
        }

        private static bool IsAuth(RoleAction roleKey)
        {
            var customer = CustomerContext.CurrentCustomer;

            return customer.IsAdmin ||
                   customer.CustomerRole == Role.Moderator && customer.HasRoleAction(roleKey) ||
                   customer.IsVirtual ||
                   TrialService.IsTrialEnabled;
        }

        #endregion
    }
}