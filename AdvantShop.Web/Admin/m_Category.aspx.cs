//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.SEO;
using AdvantShop.Trial;
using Resources;
using Image = System.Drawing.Image;
using AdvantShop.Core.SQL;
using AdvantShop.Saas;

namespace Admin
{
    public partial class m_Category : AdvantShopAdminPage
    {
        #region Fields

        private int _categoryId = -1;
        private int _parentCategoryId;
        private Category _currentCategory;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_m_Category_AddCategory));

            if (!string.IsNullOrEmpty(Request["categoryid"]) && Request["categoryid"] != "Add")
            {
                _categoryId = Request["categoryid"].TryParseInt();
            }

            _parentCategoryId = Request["parentcategoryid"].TryParseInt();

            lblImageInfo.Text = string.Format("({0}x{1}px):", SettingsPictureSize.BigCategoryImageWidth, SettingsPictureSize.BigCategoryImageHeight);
            lblMiniPhotoInfo.Text = string.Format("({0}x{1}px):", SettingsPictureSize.SmallCategoryImageWidth, SettingsPictureSize.SmallCategoryImageHeight);
            lblIconPhotoInfo.Text = string.Format("({0}x{1}px):", SettingsPictureSize.IconCategoryImageWidth, SettingsPictureSize.IconCategoryImageHeight);

            pnlSearchPicture.Visible = pnlSearchMiniPicture.Visible = pnlSearchIcon.Visible =
                AdvantShop.Core.Modules.AttachedModules.GetModules<IPhotoSearcher>().Any();

            if (!IsPostBack)
            {
                foreach (ESortOrder enumItem in Enum.GetValues(typeof(ESortOrder)))
                {
                    ddlSorting.Items.Add(new ListItem
                    {
                        Text = enumItem.Localize(),
                        Value = ((int)enumItem).ToString(),
                    });
                }
            }

            liTags.Visible = !SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveTags;
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (_categoryId != -1 && (_currentCategory = CategoryService.GetCategory(_categoryId)) != null)
            {
                LoadCategory(_categoryId);

                _parentCategoryId = _currentCategory.ParentCategoryId;

                btnAdd.Text = Resource.Admin_m_Category_Save;
                lblSubHead.Text = Resource.Admin_m_Category_EditCategory;
            }
            else
            {
                txtName.Text = string.Empty;
                txtName.Focus();
                fckDescription.Text = string.Empty;
                txtSortIndex.Text = @"0";

                txtTitle.Text = string.Empty;
                txtMetaKeywords.Text = string.Empty;
                txtMetaDescription.Text = string.Empty;

                btnAdd.Text = Resource.Admin_m_Category_Add;
                lblSubHead.Text = Resource.Admin_m_Category_AddCategory;

                pnlPropertyGroups.Visible = false;
                pnlRecomendations.Visible = false;
            }

            if (!IsPostBack)
            {
                LoadTree();
            }

            grid.DataSource = PropertyGroupService.GetListByCategory(_categoryId);
            grid.DataBind();

            var categories = CategoryService.GetCategories();
            var list = new List<ListItem>() { new ListItem(Resource.Admin_Catalog_AutoRecommendations_NotSelected, "-1") };
            categories = categories.OrderBy(x => x.SortOrder).Take(1000).ToList();
            LoadAllCategories(categories, list, 0, "");

            ddlRelatedCategory.DataSource = list;
            ddlRelatedCategory.DataBind();

            ddlAlternativeCategory.DataSource = list;
            ddlAlternativeCategory.DataBind();

            lvRelatedCategories.DataSource = CategoryService.GetRelatedCategories(_categoryId, RelatedType.Related);
            lvRelatedCategories.DataBind();

            lvAlternativeCategories.DataSource = CategoryService.GetRelatedCategories(_categoryId, RelatedType.Alternative);
            lvAlternativeCategories.DataBind();

            if (!IsPostBack)
            {
                var properties = PropertyService.GetAllProperties();

                ddlAlternativeProperties.DataSource = properties;
                ddlAlternativeProperties.DataBind();

                ddlRelatedProperties.DataSource = properties;
                ddlRelatedProperties.DataBind();
            }

            lvAltProperties.DataSource = CategoryService.GetRelatedProperties(_categoryId, RelatedType.Alternative);
            lvAltProperties.DataBind();

            lvAltPropertyValues.DataSource = CategoryService.GetRelatedPropertyValues(_categoryId, RelatedType.Alternative);
            lvAltPropertyValues.DataBind();

            lvRelatedProperties.DataSource = CategoryService.GetRelatedProperties(_categoryId, RelatedType.Related);
            lvRelatedProperties.DataBind();

            lvRelatedPropertyValues.DataSource = CategoryService.GetRelatedPropertyValues(_categoryId, RelatedType.Related);
            lvRelatedPropertyValues.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            MsgErr(true);

            if (_categoryId != -1)
            {
                SaveCategory();
            }
            else
            {
                CreateCategory();
            }
        }

        #region Load/Save category

        private void LoadCategory(int catId)
        {
            try
            {
                txtName.Text = _currentCategory.Name;
                if (_currentCategory.Picture != null && !string.IsNullOrEmpty(_currentCategory.Picture.PhotoName) &&
                    File.Exists(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Big, _currentCategory.Picture.PhotoName)))
                {
                    imgCategoryPicture.ImageUrl = FoldersHelper.GetImageCategoryPath(CategoryImageType.Big, _currentCategory.Picture.PhotoName, true);
                    imgCategoryPicture.ToolTip = _currentCategory.Picture.PhotoName;

                    btnDeleteImage.Visible = true;
                    PictureFileUpload.Visible = false;
                }
                else
                {
                    imgCategoryPicture.ImageUrl = "../images/nophoto_small.jpg";
                    btnDeleteImage.Visible = false;
                    PictureFileUpload.Visible = true;
                }

                if (_currentCategory.MiniPicture != null && !string.IsNullOrEmpty(_currentCategory.MiniPicture.PhotoName) &&
                    File.Exists(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, _currentCategory.MiniPicture.PhotoName)))
                {
                    imgMiniPicture.ImageUrl = FoldersHelper.GetImageCategoryPath(CategoryImageType.Small, _currentCategory.MiniPicture.PhotoName, true);
                    imgMiniPicture.ToolTip = _currentCategory.MiniPicture.PhotoName;

                    btnDeleteMiniImage.Visible = true;
                    MiniPictureFileUpload.Visible = false;
                }
                else
                {
                    imgMiniPicture.ImageUrl = "../images/nophoto_small.jpg";
                    btnDeleteMiniImage.Visible = false;
                    MiniPictureFileUpload.Visible = true;
                }

                if (_currentCategory.Icon != null && !string.IsNullOrEmpty(_currentCategory.Icon.PhotoName) &&
                    File.Exists(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Icon, _currentCategory.Icon.PhotoName)))
                {
                    imgIcon.ImageUrl = FoldersHelper.GetImageCategoryPath(CategoryImageType.Icon, _currentCategory.Icon.PhotoName, true);
                    imgIcon.ToolTip = _currentCategory.Icon.PhotoName;

                    btnDeleteIcon.Visible = true;
                    IconFileUpload.Visible = false;
                }
                else
                {
                    imgIcon.ImageUrl = "../images/nophoto_small.jpg";
                    btnDeleteIcon.Visible = false;
                    IconFileUpload.Visible = true;
                }

                SubCategoryDisplayStyle.SelectedValue = ((int)_currentCategory.DisplayStyle).ToString();

                rblDisplayBrands.SelectedValue = _currentCategory.DisplayBrandsInMenu.ToString();
                rblDisplaySubCategories.SelectedValue = _currentCategory.DisplaySubCategoriesInMenu.ToString();
                rblEnableCategory.SelectedValue = _currentCategory.Enabled.ToString();
                rblHiddenCategory.SelectedValue = _currentCategory.Hidden.ToString();

                fckDescription.Text = _currentCategory.Description;
                fckBriefDescription.Text = _currentCategory.BriefDescription;
                txtSortIndex.Text = _currentCategory.SortOrder.ToString();

                txtSynonym.Text = _currentCategory.UrlPath;

                ddlSorting.SelectedValue = ((int)_currentCategory.Sorting).ToString();

                var meta = MetaInfoService.GetMetaInfo(catId, MetaType.Category);
                if (meta == null)
                {
                    _currentCategory.Meta = new MetaInfo(0, 0, MetaType.Category, string.Empty, string.Empty, string.Empty, string.Empty);
                    chbDefaultMeta.Checked = true;
                }
                else
                {
                    chbDefaultMeta.Checked = false;
                    _currentCategory.Meta = meta;
                    txtTitle.Text = _currentCategory.Meta.Title;
                    txtMetaKeywords.Text = _currentCategory.Meta.MetaKeywords;
                    txtMetaDescription.Text = _currentCategory.Meta.MetaDescription;
                    txtH1.Text = _currentCategory.Meta.H1;
                }

                lbTag.Items.Clear();
                foreach (var tag in _currentCategory.Tags)
                {
                    var item = new ListItem(tag.Name, tag.Name);
                    item.Selected = true;
                    lbTag.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                //MsgErr(ex.Message);
                Debug.Log.Error(ex);
            }
        }

        private void SaveCategory()
        {
            MsgErr(true);

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MsgErr(Resource.Admin_m_Category_NoName);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSynonym.Text))
            {
                MsgErr(Resource.Admin_m_Category_NoSynonym);
                return;
            }

            string synonym = txtSynonym.Text.Trim();

            if (String.IsNullOrEmpty(synonym))
            {
                MsgErr(Resource.Admin_m_Category_NoSynonym);
                return;
            }

            var oldSynonym = UrlService.GetObjUrlFromDb(ParamType.Category, _categoryId);

            if (oldSynonym != synonym)
            {
                var pattern = !SettingsMain.EnableCyrillicUrl ? "^[a-zA-Z0-9_-]*$" : "^[a-zA-Zа-яА-Я0-9_-]*$";
                var reg = new Regex(pattern);
                if (!reg.IsMatch(synonym))
                {
                    MsgErr(Resource.Admin_m_Category_SynonymInfo);
                    return;
                }

                if (!UrlService.IsAvailableUrl(_categoryId, ParamType.Category, synonym))
                {
                    MsgErr(Resource.Admin_SynonymExist);
                    return;
                }
            }

            var category = new Category
            {
                CategoryId = _categoryId,
                Name = txtName.Text.Trim(),
                UrlPath = synonym,
                ParentCategoryId = tree.SelectedValue.TryParseInt(),
                Description =
                    fckDescription.Text == "<br />" || fckDescription.Text == "&nbsp;" || fckDescription.Text == "\r\n"
                        ? string.Empty
                        : fckDescription.Text,
                BriefDescription =
                    fckBriefDescription.Text == "<br />" || fckBriefDescription.Text == "&nbsp;" ||
                    fckBriefDescription.Text == "\r\n"
                        ? string.Empty
                        : fckBriefDescription.Text,
                Enabled = bool.Parse(rblEnableCategory.SelectedValue),
                Hidden = bool.Parse(rblHiddenCategory.SelectedValue),
                DisplayStyle = (ECategoryDisplayStyle)SubCategoryDisplayStyle.SelectedValue.TryParseInt(),
                DisplayChildProducts = false,
                DisplayBrandsInMenu = bool.Parse(rblDisplayBrands.SelectedValue),
                DisplaySubCategoriesInMenu = bool.Parse(rblDisplaySubCategories.SelectedValue),
                SortOrder = txtSortIndex.Text.TryParseInt(),
                Sorting = (ESortOrder)ddlSorting.SelectedValue.TryParseInt()
            };
            
            var tagsTitle = Request.Params[lbTag.UniqueID]; //lbTag.Items.AsQueryable().Where<ListItem>(x => x.Selected).ToList();
            if (tagsTitle.IsNotEmpty())
            {
                var titles = tagsTitle.Split(',');
                category.Tags = titles.Select(x => new Tag
                {
                    Name = x,
                    UrlPath = StringHelper.TransformUrl(StringHelper.Translit(x)),
                    Enabled = true,
                    VisibilityForUsers = true
                }).ToList();
            }
            else
            {
                category.Tags = new List<Tag>();
            }


            FileHelpers.UpdateDirectories();
            if (PictureFileUpload.HasFile)
            {
                if (!FileHelpers.CheckFileExtension(PictureFileUpload.FileName, EAdvantShopFileTypes.Image))
                {
                    MsgErr(Resource.Admin_ErrorMessage_WrongImageExtension);
                    return;
                }

                PhotoService.DeletePhotos(_categoryId, PhotoType.CategoryBig);

                var tempName = PhotoService.AddPhoto(new Photo(0, _categoryId, PhotoType.CategoryBig) { OriginName = PictureFileUpload.FileName });
                if (!string.IsNullOrWhiteSpace(tempName))
                {
                    using (Image image = Image.FromStream(PictureFileUpload.FileContent))
                        FileHelpers.SaveResizePhotoFile(
                            FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Big, tempName),
                            SettingsPictureSize.BigCategoryImageWidth, SettingsPictureSize.BigCategoryImageHeight, image);
                }
            }
            else if (!string.IsNullOrEmpty(hfGooglePicture.Value))
            {
                PhotoService.DeletePhotos(_categoryId, PhotoType.CategoryBig);
                FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));

                var photoName = hfGooglePicture.Value.Md5() + Path.GetExtension(hfGooglePicture.Value).Split('?').FirstOrDefault();
                var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);

                if (FileHelpers.DownloadRemoteImageFile(hfGooglePicture.Value, photoFullName))
                {
                    var tempName = PhotoService.AddPhoto(new Photo(0, _categoryId, PhotoType.CategoryBig) { OriginName = photoName });
                    if (!string.IsNullOrWhiteSpace(tempName))
                    {
                        using (var image = Image.FromFile(photoFullName))
                        {
                            FileHelpers.SaveResizePhotoFile(
                                FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Big, tempName),
                                SettingsPictureSize.BigCategoryImageWidth, SettingsPictureSize.BigCategoryImageHeight,
                                image);
                        }
                    }
                }
                hfGooglePicture.Value = string.Empty;
                FileHelpers.DeleteFile(photoFullName);
            }

            if (MiniPictureFileUpload.HasFile)
            {
                if (!FileHelpers.CheckFileExtension(MiniPictureFileUpload.FileName, EAdvantShopFileTypes.Image))
                {
                    MsgErr(Resource.Admin_ErrorMessage_WrongImageExtension);
                    return;
                }

                PhotoService.DeletePhotos(_categoryId, PhotoType.CategorySmall);

                var tempName = PhotoService.AddPhoto(new Photo(0, _categoryId, PhotoType.CategorySmall) { OriginName = MiniPictureFileUpload.FileName });
                if (!string.IsNullOrWhiteSpace(tempName))
                {
                    using (Image image = Image.FromStream(MiniPictureFileUpload.FileContent))
                        FileHelpers.SaveResizePhotoFile(
                            FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, tempName),
                            SettingsPictureSize.SmallCategoryImageWidth, SettingsPictureSize.SmallCategoryImageHeight,
                            image);
                }
            }
            else if (!string.IsNullOrEmpty(hfGoogleMiniPicture.Value))
            {
                PhotoService.DeletePhotos(_categoryId, PhotoType.CategorySmall);
                FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));

                var photoName = hfGoogleMiniPicture.Value.Md5() + Path.GetExtension(hfGoogleMiniPicture.Value).Split('?').FirstOrDefault();
                var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);

                if (FileHelpers.DownloadRemoteImageFile(hfGoogleMiniPicture.Value, photoFullName))
                {
                    var tempName = PhotoService.AddPhoto(new Photo(0, _categoryId, PhotoType.CategorySmall) { OriginName = photoName });
                    if (!string.IsNullOrWhiteSpace(tempName))
                    {
                        using (var image = Image.FromFile(photoFullName))
                        {
                            FileHelpers.SaveResizePhotoFile(
                                FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, tempName),
                                SettingsPictureSize.SmallCategoryImageWidth,
                                SettingsPictureSize.SmallCategoryImageHeight, image);
                        }
                    }
                }
                hfGoogleMiniPicture.Value = string.Empty;
                FileHelpers.DeleteFile(photoFullName);
            }

            if (IconFileUpload.HasFile)
            {
                if (!FileHelpers.CheckFileExtension(IconFileUpload.FileName, EAdvantShopFileTypes.Image))
                {
                    MsgErr(Resource.Admin_ErrorMessage_WrongImageExtension);
                    return;
                }

                PhotoService.DeletePhotos(_categoryId, PhotoType.CategoryIcon);

                var tempName = PhotoService.AddPhoto(new Photo(0, _categoryId, PhotoType.CategoryIcon) { OriginName = IconFileUpload.FileName });
                if (!string.IsNullOrWhiteSpace(tempName))
                {
                    using (Image image = Image.FromStream(IconFileUpload.FileContent))
                        FileHelpers.SaveResizePhotoFile(
                            FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Icon, tempName),
                            SettingsPictureSize.IconCategoryImageWidth, SettingsPictureSize.IconCategoryImageHeight,
                            image);
                }
            }
            else if (!string.IsNullOrEmpty(hfGoogleIcon.Value))
            {
                PhotoService.DeletePhotos(_categoryId, PhotoType.CategoryIcon);
                FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));

                var photoName = hfGoogleIcon.Value.Md5() + Path.GetExtension(hfGoogleIcon.Value).Split('?').FirstOrDefault();
                var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);

                if (FileHelpers.DownloadRemoteImageFile(hfGoogleIcon.Value, photoFullName))
                {
                    var tempName = PhotoService.AddPhoto(new Photo(0, _categoryId, PhotoType.CategoryIcon) { OriginName = photoName });
                    if (!string.IsNullOrWhiteSpace(tempName))
                    {
                        using (var image = Image.FromFile(photoFullName))
                        {
                            FileHelpers.SaveResizePhotoFile(
                                FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Icon, tempName),
                                SettingsPictureSize.IconCategoryImageWidth, SettingsPictureSize.IconCategoryImageHeight,
                                image);
                        }
                    }
                }
                hfGoogleMiniPicture.Value = string.Empty;
                FileHelpers.DeleteFile(photoFullName);
            }


            category.Meta = new MetaInfo(0, category.CategoryId, MetaType.Category, txtTitle.Text, txtMetaKeywords.Text, txtMetaDescription.Text, txtH1.Text);

            var isParentCategoryChanged = CategoryService.GetCategory(_categoryId).ParentCategoryId != category.ParentCategoryId;

            if (!CategoryService.UpdateCategory(category, true))
            {
                MsgErr("Failed to save category");
            }

            if (isParentCategoryChanged || _categoryId == 0)
            {
                CategoryService.RecalculateProductsCountManual();
                CategoryService.ClearCategoryCache();                
            }
        }

        private void CreateCategory()
        {
            MsgErr(true);

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MsgErr(Resource.Admin_m_Category_NoName);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtSynonym.Text))
            {
                MsgErr(Resource.Admin_m_Category_NoSynonym);
                return;
            }

            var pattern = !SettingsMain.EnableCyrillicUrl ? "^[a-zA-Z0-9_-]*$" : "^[a-zA-Zа-яА-Я0-9_-]*$";
            var reg = new Regex(pattern);

            if (!reg.IsMatch(txtSynonym.Text))
            {
                MsgErr(Resource.Admin_m_Category_SynonymInfo);
                return;
            }

            if (!UrlService.IsAvailableUrl(ParamType.Category, txtSynonym.Text))
            {
                MsgErr(Resource.Admin_SynonymExist);
                return;
            }

            if ((PictureFileUpload.HasFile && !FileHelpers.CheckFileExtension(PictureFileUpload.FileName, EAdvantShopFileTypes.Image)) ||
                (MiniPictureFileUpload.HasFile && !FileHelpers.CheckFileExtension(MiniPictureFileUpload.FileName, EAdvantShopFileTypes.Image)) ||
                (IconFileUpload.HasFile && !FileHelpers.CheckFileExtension(IconFileUpload.FileName, EAdvantShopFileTypes.Image)))
            {
                MsgErr(Resource.Admin_ErrorMessage_WrongImageExtension);
                return;
            }

            var category = new Category
            {
                Name = txtName.Text.Trim(),
                UrlPath = txtSynonym.Text,
                ParentCategoryId = tree.SelectedValue.TryParseInt(),
                Description = fckDescription.Text,
                BriefDescription = fckBriefDescription.Text,
                SortOrder = txtSortIndex.Text.TryParseInt(),
                Enabled = bool.Parse(rblEnableCategory.SelectedValue),
                Hidden = bool.Parse(rblHiddenCategory.SelectedValue),
                DisplayChildProducts = false,
                DisplayStyle = (ECategoryDisplayStyle)int.Parse(SubCategoryDisplayStyle.SelectedValue),
                Meta =
                    new MetaInfo(0, 0, MetaType.Category, txtTitle.Text, txtMetaKeywords.Text, txtMetaDescription.Text, txtH1.Text),
                Sorting = (ESortOrder)ddlSorting.SelectedValue.TryParseInt()
            };

            var tagsTitle = Request.Params[lbTag.UniqueID]; //lbTag.Items.AsQueryable().Where<ListItem>(x => x.Selected).ToList();
            if (tagsTitle.IsNotEmpty())
            {
                var titles = tagsTitle.Split(',');
                category.Tags = titles.Select(x => new Tag
                {
                    Name = x,
                    UrlPath = StringHelper.TransformUrl(StringHelper.Translit(x)),
                    Enabled = true,
                    VisibilityForUsers = true
                }).ToList();
            }

            try
            {
                category.CategoryId = CategoryService.AddCategory(category, true, true);
                if (category.CategoryId == 0)
                    return;

                if (PictureFileUpload.HasFile)
                {
                    var tempName = PhotoService.AddPhoto(new Photo(0, category.CategoryId, PhotoType.CategoryBig) { OriginName = PictureFileUpload.FileName });
                    if (!string.IsNullOrWhiteSpace(tempName))
                    {
                        using (Image image = Image.FromStream(PictureFileUpload.FileContent))
                            FileHelpers.SaveResizePhotoFile(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Big, tempName), SettingsPictureSize.BigCategoryImageWidth, SettingsPictureSize.BigCategoryImageHeight, image);
                    }
                }
                else if (!string.IsNullOrEmpty(hfGooglePicture.Value))
                {
                    FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));

                    var photoName = hfGooglePicture.Value.Md5() + Path.GetExtension(hfGooglePicture.Value).Split('?').FirstOrDefault();
                    var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);

                    if (FileHelpers.DownloadRemoteImageFile(hfGooglePicture.Value, photoFullName))
                    {
                        var tempName = PhotoService.AddPhoto(new Photo(0, category.CategoryId, PhotoType.CategoryBig) { OriginName = photoName });
                        if (!string.IsNullOrWhiteSpace(tempName))
                        {
                            using (var image = Image.FromFile(photoFullName))
                            {
                                FileHelpers.SaveResizePhotoFile(
                                    FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Big, tempName),
                                    SettingsPictureSize.BigCategoryImageWidth, SettingsPictureSize.BigCategoryImageHeight,
                                    image);
                            }
                        }
                    }
                    hfGooglePicture.Value = string.Empty;
                    FileHelpers.DeleteFile(photoFullName);
                }

                if (MiniPictureFileUpload.HasFile)
                {
                    var tempName = PhotoService.AddPhoto(new Photo(0, category.CategoryId, PhotoType.CategorySmall) { OriginName = MiniPictureFileUpload.FileName });
                    if (!string.IsNullOrWhiteSpace(tempName))
                    {
                        using (Image image = Image.FromStream(MiniPictureFileUpload.FileContent))
                            FileHelpers.SaveResizePhotoFile(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, tempName), SettingsPictureSize.SmallCategoryImageWidth, SettingsPictureSize.SmallCategoryImageHeight, image);
                    }
                }
                else if (!string.IsNullOrEmpty(hfGoogleMiniPicture.Value))
                {
                    FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));

                    var photoName = hfGoogleMiniPicture.Value.Md5() + Path.GetExtension(hfGoogleMiniPicture.Value).Split('?').FirstOrDefault();
                    var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);

                    if (FileHelpers.DownloadRemoteImageFile(hfGoogleMiniPicture.Value, photoFullName))
                    {
                        var tempName = PhotoService.AddPhoto(new Photo(0, category.CategoryId, PhotoType.CategorySmall) { OriginName = photoName });
                        if (!string.IsNullOrWhiteSpace(tempName))
                        {
                            using (var image = Image.FromFile(photoFullName))
                            {
                                FileHelpers.SaveResizePhotoFile(
                                    FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, tempName),
                                    SettingsPictureSize.SmallCategoryImageWidth,
                                    SettingsPictureSize.SmallCategoryImageHeight, image);
                            }
                        }
                    }
                    hfGoogleMiniPicture.Value = string.Empty;
                    FileHelpers.DeleteFile(photoFullName);
                }

                if (IconFileUpload.HasFile)
                {
                    var tempName = PhotoService.AddPhoto(new Photo(0, category.CategoryId, PhotoType.CategoryIcon) { OriginName = IconFileUpload.FileName });
                    if (!string.IsNullOrWhiteSpace(tempName))
                    {
                        using (Image image = Image.FromStream(IconFileUpload.FileContent))
                            FileHelpers.SaveResizePhotoFile(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Icon, tempName), SettingsPictureSize.IconCategoryImageWidth, SettingsPictureSize.IconCategoryImageHeight, image);
                    }
                }
                else if (!string.IsNullOrEmpty(hfGoogleIcon.Value))
                {
                    FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));

                    var photoName = hfGoogleIcon.Value.Md5() + Path.GetExtension(hfGoogleIcon.Value).Split('?').FirstOrDefault();
                    var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);

                    if (FileHelpers.DownloadRemoteImageFile(hfGoogleIcon.Value, photoFullName))
                    {
                        var tempName = PhotoService.AddPhoto(new Photo(0, category.CategoryId, PhotoType.CategoryIcon) { OriginName = photoName });
                        if (!string.IsNullOrWhiteSpace(tempName))
                        {
                            using (var image = Image.FromFile(photoFullName))
                            {
                                FileHelpers.SaveResizePhotoFile(
                                    FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Icon, tempName),
                                    SettingsPictureSize.IconCategoryImageWidth, SettingsPictureSize.IconCategoryImageHeight,
                                    image);
                            }
                        }
                    }
                    hfGoogleMiniPicture.Value = string.Empty;
                    FileHelpers.DeleteFile(photoFullName);
                }

                TrialService.TrackEvent(TrialEvents.AddCategory, "");
            }
            catch (Exception ex)
            {
                MsgErr(ex.Message + "at CreateCategory");
                Debug.Log.Error(ex);
            }

            if (category.CategoryId != 0)
            {
                Response.Redirect("catalog.aspx?categoryid=" + category.CategoryId);
            }
        }

        private void LoadAllCategories(List<Category> categories, List<ListItem> list, int categoryId, string offset)
        {
            foreach (var category in categories.Where(c => c.ParentCategoryId == categoryId).OrderBy(c => c.SortOrder).ToList())
            {
                list.Add(new ListItem(HttpUtility.HtmlDecode(offset + category.Name), category.CategoryId.ToString()));

                if (categories.Any(c => c.ParentCategoryId == category.CategoryId))
                {
                    LoadAllCategories(categories, list, category.CategoryId, offset + "&nbsp;&nbsp;");
                }
            }
        }

        #endregion

        #region Groups

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteGroup")
            {
                PropertyGroupService.DeleteGroupFromCategory(Convert.ToInt32(e.CommandArgument), _categoryId);
            }

            if (e.CommandName == "AddGroup")
            {
                var footer = grid.FooterRow;

                var groupId = ((DropDownList)footer.FindControl("ddlNewGroupName")).SelectedValue.TryParseInt();
                if (groupId == 0)
                {
                    grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ffcccc");
                    return;
                }

                PropertyGroupService.AddGroupToCategory(groupId, _categoryId);
                grid.ShowFooter = false;
            }

            if (e.CommandName == "CancelAdd")
            {
                grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
                grid.ShowFooter = false;
            }
        }

        protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void btnAddGroup_OnClick(object sender, EventArgs e)
        {
            grid.ShowFooter = true;
            grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
            //grid.DataBound += grid_DataBound;
        }

        protected void grid_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                var ddlNewGroups = ((DropDownList)e.Row.FindControl("ddlNewGroupName"));
                if (ddlNewGroups != null)
                {
                    ddlNewGroups.DataSource = PropertyGroupService.GetList();
                    ddlNewGroups.DataBind();
                }
            }
        }

        #endregion

        #region Category methods

        protected void btnDeleteImage_Click(object sender, EventArgs e)
        {
            PhotoService.DeletePhotos(_categoryId, PhotoType.CategoryBig);

            var cacheKey = CacheNames.GetCategoryCacheObjectName(_categoryId);
            CacheManager.Remove(cacheKey);
        }

        protected void btnDeleteMiniImage_Click(object sender, EventArgs e)
        {
            PhotoService.DeletePhotos(_categoryId, PhotoType.CategorySmall);

            var cacheKey = CacheNames.GetCategoryCacheObjectName(_categoryId);
            CacheManager.Remove(cacheKey);
        }

        protected void btnDeleteIcon_Click(object sender, EventArgs e)
        {
            PhotoService.DeletePhotos(_categoryId, PhotoType.CategoryIcon);

            var cacheKey = CacheNames.GetCategoryCacheObjectName(_categoryId);
            CacheManager.Remove(cacheKey);
        }

        public void PopulateNode(object sender, TreeNodeEventArgs e)
        {
            LoadChildCategories(e.Node);
        }

        private void LoadTree()
        {
            var node = new TreeNode { Text = Resource.Admin_m_Category_Root, Value = @"0", Selected = true };
            tree.Nodes.Add(node);

            LoadChildCategories(tree.Nodes[0]);

            IList<Category> parentCategories = CategoryService.GetParentCategories(_parentCategoryId);

            if (parentCategories != null)
            {
                TreeNodeCollection nodes = tree.Nodes[0].ChildNodes;
                for (int i = parentCategories.Count - 1; i >= 0; i--)
                {
                    int ii = i;
                    TreeNode tn =
                        (from TreeNode n in nodes
                         where n.Value == parentCategories[ii].CategoryId.ToString()
                         select n).SingleOrDefault();
                    if (tn != null)
                    {
                        tn.Select();
                        tn.Expand();
                    }
                    else
                    {
                        break;
                    }
                    nodes = tn.ChildNodes;
                }
            }

            if (!string.IsNullOrEmpty(tree.SelectedValue))
            {
                var tt = CategoryService.GetCategory(tree.SelectedValue.TryParseInt());
                if (tt != null)
                    lParent.Text = tt.Name;
            }

            if (string.IsNullOrEmpty(lParent.Text))
                lParent.Text = Resource.Admin_m_Category_No;
        }

        private void LoadChildCategories(TreeNode node)
        {
            foreach (Category c in CategoryService.GetChildCategoriesByCategoryId(node.Value.TryParseInt(), false))
            {
                if (c.CategoryId != _categoryId)
                {
                    var newNode = new TreeNode
                    {
                        Text = string.Format("{0} ({1})", c.Name, c.ProductsCount),
                        Value = c.CategoryId.ToString()

                    };
                    if (c.HasChild)
                    {
                        newNode.Expanded = false;
                        newNode.PopulateOnDemand = true;
                    }
                    else
                    {
                        newNode.Expanded = true;
                        newNode.PopulateOnDemand = false;
                    }
                    node.ChildNodes.Add(newNode);
                }
            }
        }

        protected void lbParentChange_Click(object sender, EventArgs e)
        {
            mpeTree.Show();
        }

        protected void Select_change(object sender, EventArgs e)
        {
            mpeTree.Show();
            lParent.Text = CategoryService.GetCategory(tree.SelectedValue.TryParseInt()).Name;
        }

        protected void btnUpdateParent_Click(object sender, EventArgs e)
        {
            mpeTree.Hide();
            var category = CategoryService.GetCategory(tree.SelectedValue.TryParseInt());
            var levelCurrent = _categoryId >= 0 ? GetLevelCategory(_categoryId) : 1;
            var levelSelect = GetLevelCategory(category.CategoryId);
            if (category.CategoryId != 0 && levelSelect < levelCurrent)
            {
                lParent.Text = category.Name;
            }
            else
            {
                MsgErr("Вы указали недопустимую категорию в качестве родителя");
            }
        }

        private int GetLevelCategory(int id)
        {
            try
            {
                return SQLDataAccess.ExecuteScalar<int>("Select CatLevel From Catalog.Category where CategoryID = @id",
                    CommandType.Text,
                    new SqlParameter("id", id.ToString()));
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return 0;
            }
        }

        private void MsgErr(bool clean)
        {
            if (clean)
            {
                lblError.Visible = false;
                lblError.Text = string.Empty;
            }
            else
            {
                lblError.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text = messageText + @"<br/>";
        }

        #endregion

        #region Related/Alternative categories

        #region Related

        protected void btnAddRelatedCategory_Click(object sender, EventArgs e)
        {
            var relCategoryId = Convert.ToInt32(ddlRelatedCategory.SelectedValue);
            if (relCategoryId <= 0)
                return;

            var categoryIds = CategoryService.GetRelatedCategoryIds(_categoryId, RelatedType.Related);

            if (categoryIds == null || !categoryIds.Contains(relCategoryId))
            {
                CategoryService.AddRelatedCategory(_categoryId, relCategoryId, RelatedType.Related);
            }
        }

        protected void lvRelatedCategories_RowCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRelCat")
            {
                CategoryService.DeleteRelatedCategory(_categoryId, Convert.ToInt32(e.CommandArgument), RelatedType.Related);
            }
        }

        protected void lvRelatedProperties_RowCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRelatedProperty")
            {
                var propertyId = Convert.ToInt32(e.CommandArgument);
                CategoryService.DeleteRelatedProperty(_categoryId, propertyId, RelatedType.Related);
            }

            if (e.CommandName == "DeleteRelatedPropertyValue")
            {
                var propertyValueId = Convert.ToInt32(e.CommandArgument);
                CategoryService.DeleteRelatedPropertyValue(_categoryId, propertyValueId, RelatedType.Related);
            }
        }

        protected void btnAddRelatedProperty_Click(object sender, EventArgs e)
        {
            if (inpRelTypeProperty.Checked)
            {
                var propertyId = ddlRelatedProperties.SelectedValue.TryParseInt();
                var props = CategoryService.GetRelatedPropertyIds(_categoryId, RelatedType.Related);

                if (propertyId > 0 && (props == null || props.All(x => x != propertyId)))
                    CategoryService.AddRelatedProperty(_categoryId, propertyId, RelatedType.Related);
            }
            else
            {
                var propertyValueId = ddlPropertValue.SelectedValue.TryParseInt();
                var props = CategoryService.GetRelatedPropertyValuesIds(_categoryId, RelatedType.Related);

                if (propertyValueId > 0 && (props == null || props.All(x => x != propertyValueId)))
                    CategoryService.AddRelatedPropertyValue(_categoryId, propertyValueId, RelatedType.Related);
            }

        }

        protected void ddlRelatedProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillPropertyValue();
        }

        private void FillPropertyValue()
        {
            ddlPropertValue.Items.Clear();
            if (string.IsNullOrEmpty(ddlRelatedProperties.SelectedValue))
                return;

            foreach (var propVal in PropertyService.GetValuesByPropertyId(SQLDataHelper.GetInt(ddlRelatedProperties.SelectedValue)))
            {
                ddlPropertValue.Items.Add(new ListItem(propVal.Value, propVal.PropertyValueId.ToString(CultureInfo.InvariantCulture)));
            }
            ddlPropertValue.DataBind();
        }

        #endregion

        #region Alternative

        protected void lvAltProperties_RowCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteAltProperty")
            {
                CategoryService.DeleteRelatedProperty(_categoryId, Convert.ToInt32(e.CommandArgument), RelatedType.Alternative);
            }

            if (e.CommandName == "DeleteAltPropertyValue")
            {
                CategoryService.DeleteRelatedPropertyValue(_categoryId, Convert.ToInt32(e.CommandArgument), RelatedType.Alternative);
            }
        }

        protected void btnAddAltProperty_Click(object sender, EventArgs e)
        {
            if (inpAltTypeProperty.Checked)
            {
                var propertyId = ddlAlternativeProperties.SelectedValue.TryParseInt();
                var props = CategoryService.GetRelatedPropertyIds(_categoryId, RelatedType.Alternative);

                if (propertyId > 0 && props != null && props.All(x => x != propertyId))
                    CategoryService.AddRelatedProperty(_categoryId, propertyId, RelatedType.Alternative);
            }
            else
            {
                var propertyValueId = ddlAlternativePropertyValues.SelectedValue.TryParseInt();
                var props = CategoryService.GetRelatedPropertyValuesIds(_categoryId, RelatedType.Alternative);

                if (propertyValueId > 0 && (props == null || props.All(x => x != propertyValueId)))
                    CategoryService.AddRelatedPropertyValue(_categoryId, propertyValueId, RelatedType.Alternative);
            }
        }

        protected void btnAddAlternativeCategory_Click(object sender, EventArgs e)
        {
            var categoryId = Convert.ToInt32(ddlAlternativeCategory.SelectedValue);
            if (categoryId <= 0)
                return;

            var categoryIds = CategoryService.GetRelatedCategoryIds(_categoryId, RelatedType.Alternative);

            if (categoryIds == null || !categoryIds.Contains(categoryId))
            {
                CategoryService.AddRelatedCategory(_categoryId, categoryId, RelatedType.Alternative);
            }
        }

        protected void lvAlternativeCategories_RowCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteAltCat")
            {
                CategoryService.DeleteRelatedCategory(_categoryId, Convert.ToInt32(e.CommandArgument), RelatedType.Alternative);
            }
        }

        protected void ddlAlternativeProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillAltPropertyValue();
        }

        private void FillAltPropertyValue()
        {
            ddlAlternativePropertyValues.Items.Clear();
            if (string.IsNullOrEmpty(ddlAlternativeProperties.SelectedValue))
                return;

            foreach (var propVal in PropertyService.GetValuesByPropertyId(SQLDataHelper.GetInt(ddlAlternativeProperties.SelectedValue)))
            {
                ddlAlternativePropertyValues.Items.Add(new ListItem(propVal.Value, propVal.PropertyValueId.ToString(CultureInfo.InvariantCulture)));
            }
            ddlAlternativePropertyValues.DataBind();
        }

        #endregion

        #endregion
    }
}