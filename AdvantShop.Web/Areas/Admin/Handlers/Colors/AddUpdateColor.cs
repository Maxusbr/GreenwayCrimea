using System;
using System.Drawing;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Models.Colors;
using Color = AdvantShop.Catalog.Color;

namespace AdvantShop.Web.Admin.Handlers.Colors
{
    public class AddUpdateColor
    {
        private readonly ColorModel _model;
        private readonly HttpPostedFileBase _colorIconFile;
        private readonly bool _isEditMode;

        public AddUpdateColor(ColorModel model, HttpPostedFileBase colorIconFile, bool isEditMode)
        {
            _model = model;
            _colorIconFile = colorIconFile;
            _isEditMode = isEditMode;
        }

        public bool Execute()
        {
            try
            {
                var color = new Color()
                {
                    ColorName = _model.ColorName.DefaultOrEmpty(),
                    ColorCode = _model.ColorCode.DefaultOrEmpty(),
                    SortOrder = _model.SortOrder,
                };

                if (!_isEditMode)
                {
                    color.ColorId = ColorService.AddColor(color);
                }
                else
                {
                    color.ColorId = _model.ColorId;

                    ColorService.UpdateColor(color);
                }

                if (_colorIconFile != null && FileHelpers.CheckFileExtension(_colorIconFile.FileName, EAdvantShopFileTypes.Image))
                {
                    AddColorPhoto(color.ColorId);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return false;
            }

            return true;
        }

        private void AddColorPhoto(int colorId)
        {
            try
            {
                PhotoService.DeletePhotos(colorId, PhotoType.Color);

                var tempName = PhotoService.AddPhoto(new Photo(0, colorId, PhotoType.Color)
                {
                    OriginName = _colorIconFile.FileName
                });

                if (!string.IsNullOrWhiteSpace(tempName))
                {
                    using (Image image = Image.FromStream(_colorIconFile.InputStream))
                    {
                        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetImageColorPathAbsolut(ColorImageType.Catalog, tempName),
                            SettingsPictureSize.ColorIconWidthCatalog, SettingsPictureSize.ColorIconHeightCatalog, image);

                        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetImageColorPathAbsolut(ColorImageType.Details, tempName),
                            SettingsPictureSize.ColorIconWidthDetails, SettingsPictureSize.ColorIconHeightDetails, image);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }
    }
}
