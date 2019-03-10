//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Repository;
using AdvantShop.SEO;
using Resources;
using Image = System.Drawing.Image;

namespace Admin
{
    public partial class m_Brand : AdvantShopAdminPage
    {
        protected int BrandId
        {
            get { return Request["id"].TryParseInt(); }
        }

        private Brand _brand;

        private void MsgErr(bool clean)
        {
            if (clean)
            {
                lblError.Visible = false;
                lblError.Text = "";
            }
            else
            {
                lblError.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text = @"<br/>" + messageText;
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (!Valid()) return;

            if (BrandId != 0)
            {
                SaveBrand();
            }
            else
            {
                CreateBrand();
            }
            //
            // Close window
            //
            if (lblError.Visible == false)
            {
                CommonHelper.RegCloseScript(this, string.Empty);
            }
        }

        protected void btnDeleteLogo_Click(object sender, EventArgs e)
        {
            if (BrandId != 0)
            {
                BrandService.DeleteBrandLogo(BrandId);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_m_News_Header));

            if (IsPostBack) return;
            ddlCountry.Items.Clear();
            ddlCountry.Items.Add(new ListItem(Resource.Client_Brands_AllCoutries, "0"));
            var list = CountryService.GetAllCountryIdAndName().OrderBy(x => x.Name).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                ddlCountry.Items.Add(new ListItem(list[i].Name, list[i].CountryId.ToString(CultureInfo.InvariantCulture)));
            }

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (BrandId != 0 && (_brand = BrandService.GetBrandById(BrandId)) != null)
            {
                btnOK.Text = Resource.Admin_m_News_Save;
                LoadBrandById(BrandId);
            }
            else
            {
                btnOK.Text = Resource.Admin_m_News_Add;
                chkEnabled.Checked = true;
                imgLogo.Visible = false;
                btnDeleteImage.Visible = false;
                txtName.Text = string.Empty;
                txtBrandSiteUrl.Text = string.Empty;
                txtSortOrder.Text = @"0";
                txtTitle.Text = @"#STORE_NAME# - #BRAND_NAME#";
                txtMetaKeywords.Text = @"#STORE_NAME# - #BRAND_NAME#";
                txtMetaDescription.Text = @"#STORE_NAME# - #BRAND_NAME#";
                FCKDescription.Text = "";
                FCKBriefDescription.Text = "";
            }
        }

        protected bool Valid()
        {

            txtURL.Text = txtURL.Text.Replace("\'", "");
            if (string.IsNullOrEmpty(txtURL.Text))
            {
                MsgErr(Resource.Admin_m_News_NoID);
                return false;
            }
            if (!UrlService.IsAvailableUrl(BrandId, ParamType.Brand, txtURL.Text))
            {
                MsgErr(Resource.Admin_SynonymExist);
                return false;
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MsgErr(Resource.Admin_m_News_NoTitle);
                return false;
            }

            int sort = 0;
            if (!int.TryParse(txtSortOrder.Text, out sort))
            {
                MsgErr(Resource.Admin_m_Brand_WrongSorting);
                return false;
            }
            if (fuBrandLogo.HasFile && !FileHelpers.CheckFileExtension(fuBrandLogo.FileName, EAdvantShopFileTypes.Image))
            {
                MsgErr(Resource.Admin_ErrorMessage_WrongImageExtension);
                return false;
            }
            MsgErr(true); // Clean
            return true;
        }

        protected void SaveBrand()
        {
            try
            {
                var meta = new MetaInfo();
                if (!string.IsNullOrEmpty(txtTitle.Text) || !string.IsNullOrEmpty(txtH1.Text) ||
                    !string.IsNullOrEmpty(txtMetaKeywords.Text) ||
                    !string.IsNullOrEmpty(txtMetaDescription.Text))
                {
                    meta.ObjId = BrandId;
                    meta.Type = MetaType.Brand;
                    meta.Title = txtTitle.Text;
                    meta.H1 = txtH1.Text;
                    meta.MetaKeywords = txtMetaKeywords.Text;
                    meta.MetaDescription = txtMetaDescription.Text;
                }
                var brand = new Brand
                {
                    BrandId = BrandId,
                    Name = txtName.Text.Trim(),
                    Description = FCKDescription.Text,
                    BriefDescription = FCKBriefDescription.Text,
                    Enabled = chkEnabled.Checked,
                    UrlPath = txtURL.Text.Trim(),
                    SortOrder = txtSortOrder.Text.TryParseInt(),
                    CountryId = SQLDataHelper.GetInt(ddlCountry.SelectedValue),
                    BrandSiteUrl = txtBrandSiteUrl.Text,
                    Meta = meta,
                };
                MetaInfoService.DeleteMetaInfo(BrandId, MetaType.Brand);
                BrandService.UpdateBrand(brand);

                if (fuBrandLogo.HasFile)
                {
                    PhotoService.DeletePhotos(BrandId, PhotoType.Brand);

                    var tempName = PhotoService.AddPhoto(new Photo(0, BrandId, PhotoType.Brand) { OriginName = fuBrandLogo.FileName });
                    if (!string.IsNullOrWhiteSpace(tempName))
                    {
                        using (var image = Image.FromStream(fuBrandLogo.FileContent))
                        {
                            FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.BrandLogo, tempName),
                                SettingsPictureSize.BrandLogoWidth, SettingsPictureSize.BrandLogoHeight, image);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(hfGoogleLinks.Value))
                {
                    PhotoService.DeletePhotos(BrandId, PhotoType.Brand);

                    FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));

                    var photoName = hfGoogleLinks.Value.Md5() + Path.GetExtension(hfGoogleLinks.Value).Split('?').FirstOrDefault();
                    var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);

                    if (FileHelpers.DownloadRemoteImageFile(hfGoogleLinks.Value, photoFullName))
                    {
                        var tempName = PhotoService.AddPhoto(new Photo(0, BrandId, PhotoType.Brand) { OriginName = photoName });
                        if (!string.IsNullOrWhiteSpace(tempName))
                        {
                            using (var image = Image.FromFile(photoFullName))
                            {
                                FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.BrandLogo, tempName), 
                                    SettingsPictureSize.BrandLogoWidth, SettingsPictureSize.BrandLogoHeight, image);
                            }
                        }
                    }

                    hfGoogleLinks.Value = string.Empty;
                    FileHelpers.DeleteFile(photoFullName);
                }
            }
            catch (Exception ex)
            {
                MsgErr(ex.Message + " SaveBrand main");
                Debug.Log.Error(ex);
            }
        }

        protected void CreateBrand()
        {
            try
            {
                var brand = new Brand
                {
                    Name = txtName.Text,
                    Description = FCKDescription.Text,
                    BriefDescription = FCKBriefDescription.Text,
                    Enabled = chkEnabled.Checked,
                    UrlPath = txtURL.Text,
                    SortOrder = txtSortOrder.Text.TryParseInt(),
                    CountryId = SQLDataHelper.GetInt(ddlCountry.SelectedValue),
                    BrandSiteUrl = txtBrandSiteUrl.Text,
                    Meta = new MetaInfo
                    {
                        Type = MetaType.Brand,
                        MetaDescription = txtMetaDescription.Text,
                        Title = txtTitle.Text,
                        MetaKeywords = txtMetaKeywords.Text,
                        H1 = txtH1.Text
                    }
                };

                var brandId = BrandService.AddBrand(brand);
                if (fuBrandLogo.HasFile)
                {
                    var tempName = PhotoService.AddPhoto(new Photo(0, brandId, PhotoType.Brand) { OriginName = fuBrandLogo.FileName });
                    if (!string.IsNullOrWhiteSpace(tempName))
                    {
                        using (Image image = Image.FromStream(fuBrandLogo.FileContent))
                        {
                            FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.BrandLogo, tempName), 
                                SettingsPictureSize.BrandLogoWidth, SettingsPictureSize.BrandLogoHeight, image);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(hfGoogleLinks.Value))
                {
                    FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));

                    var photoName = hfGoogleLinks.Value.Md5() + Path.GetExtension(hfGoogleLinks.Value).Split('?').FirstOrDefault();
                    var photoFullName = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoName);

                    if (FileHelpers.DownloadRemoteImageFile(hfGoogleLinks.Value, photoFullName))
                    {
                        var tempName = PhotoService.AddPhoto(new Photo(0, brandId, PhotoType.Brand) { OriginName = photoName });
                        if (!string.IsNullOrWhiteSpace(tempName))
                        {
                            using (var image = Image.FromFile(photoFullName))
                            {
                                FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.BrandLogo, tempName),
                                    SettingsPictureSize.BrandLogoWidth, SettingsPictureSize.BrandLogoHeight, image);
                            }
                        }
                    }
                    hfGoogleLinks.Value = string.Empty;
                    FileHelpers.DeleteFile(photoFullName);
                }

                if (lblError.Visible == false)
                {
                    txtName.Text = string.Empty;
                    FCKDescription.Text = string.Empty;
                    FCKBriefDescription.Text = string.Empty;
                    chkEnabled.Checked = true;
                }

                // close
            }
            catch (Exception ex)
            {
                MsgErr(ex.Message + " CreateBrand main");
                Debug.Log.Error(ex);
            }
        }

        protected void LoadBrandById(int brandId)
        {
            txtURL.Text = _brand.UrlPath;
            txtName.Text = _brand.Name;

            if (_brand.BrandLogo != null && _brand.BrandLogo.PhotoName.IsNotEmpty() && File.Exists(FoldersHelper.GetPathAbsolut(FolderType.BrandLogo, _brand.BrandLogo.PhotoName)))
            {
                btnDeleteImage.Visible = true;
                fuBrandLogo.Visible = false;
                imgLogo.ImageUrl = FoldersHelper.GetPath(FolderType.BrandLogo, _brand.BrandLogo.PhotoName, true);
                imgLogo.ToolTip = _brand.BrandLogo.PhotoName;
            }
            else
            {
                btnDeleteImage.Visible = false;
                fuBrandLogo.Visible = true;
                imgLogo.ImageUrl = "../images/nophoto_small.jpg";
            }
            var meta = MetaInfoService.GetMetaInfo(BrandId, MetaType.Brand);
            txtSortOrder.Text = _brand.SortOrder.ToString(CultureInfo.InvariantCulture);
            FCKDescription.Text = _brand.Description;
            FCKBriefDescription.Text = _brand.BriefDescription;
            chkEnabled.Checked = _brand.Enabled;
            if (meta != null && (!string.IsNullOrEmpty(meta.Title) || !string.IsNullOrEmpty(meta.H1) ||
                !string.IsNullOrEmpty(meta.MetaKeywords) || !string.IsNullOrEmpty(meta.MetaDescription)))
            {
                chbDefaultMeta.Checked = false;
                txtTitle.Text = _brand.Meta.Title;
                txtH1.Text = _brand.Meta.H1;
                txtMetaKeywords.Text = _brand.Meta.MetaKeywords;
                txtMetaDescription.Text = _brand.Meta.MetaDescription;
            }
            txtBrandSiteUrl.Text = _brand.BrandSiteUrl;
            ddlCountry.SelectedValue = _brand.CountryId.ToString(CultureInfo.InvariantCulture);
        }
    }
}