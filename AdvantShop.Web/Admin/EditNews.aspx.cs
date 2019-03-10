//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.News;
using AdvantShop.SEO;
using Resources;
using Image = System.Drawing.Image;

namespace Admin
{
    public partial class EditNews : AdvantShopAdminPage
    {
        private NewsItem _newsItem;

        protected int NewsId
        {
            get { return Request["newsid"].TryParseInt(); }
        }

        protected bool AddingNew
        {
            get { return NewsId == 0; }
        }

        private void MsgErr(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = message.IsNotEmpty();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MsgErr(string.Empty);

            btnSave.Text = AddingNew ? Resource.Admin_m_News_Add : Resource.Admin_m_News_Save;
            var title = AddingNew ? Resource.Admin_EditNews_AddingNews : Resource.Admin_EditNews_EditingNews;
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, title));
            lblSubHead.Text = title;

            lblImageInfo.Text = string.Format("* {0} {1}x{2}px", Resource.Admin_m_News_ResultImageSize,
                SettingsPictureSize.NewsImageWidth, SettingsPictureSize.NewsImageHeight);
            
            _newsItem = NewsService.GetNewsById(NewsId);

            newsProducts.NewsId = NewsId;

            if (!IsPostBack)
            {
                ddlNewsCategory.DataBind();

                if (!AddingNew)
                {
                    if (_newsItem == null)
                    {
                        Response.Redirect("NewsAdmin.aspx");
                        return;
                    }
                }
                else
                {
                    var newsCategories = NewsService.GetNewsCategories().ToList();
                    _newsItem = new NewsItem
                    {
                        Title = Resource.Admin_m_News_Header,
                        AddingDate = DateTime.Now,
                        UrlPath = @"news-" + NewsService.GetLastId(),
                        NewsCategoryId = newsCategories.Any() ? newsCategories.First().NewsCategoryId : 0
                    };
                    pnlImage.Visible = false;
                }
                LoadNews(_newsItem);
            }
        }

        public void Page_PreRender(object sender, EventArgs e)
        {
        }

        private void LoadNews(NewsItem news)
        {
            lblHead.Text = news.Title;
            txtDate.Text = news.AddingDate.ToShortDateString();
            txtTime.Text = news.AddingDate.ToString("HH:mm");
            txtUrlPath.Text = news.UrlPath;

            rightNavigation.NewsId = NewsId;
            rightNavigation.NewsCategoryId = news.NewsCategoryId;

            if (news.NewsId == 0)
                return;

            txtTitle.Text = news.Title;
            txtNewsText.Text = news.TextToPublication;
            txtAnnotation.Text = news.TextAnnotation;
            chkOnMainPage.Checked = news.ShowOnMainPage;

            ddlNewsCategory.SelectedValue = news.NewsCategoryId.ToString(CultureInfo.InvariantCulture);

            if (news.Picture != null && news.Picture.PhotoName.IsNotEmpty())
            {
                pnlImage.Visible = true;
                imgNewsPicture.ImageUrl = FoldersHelper.GetPath(FolderType.News, news.Picture.PhotoName, true);
                imgNewsPicture.ToolTip = news.Picture.PhotoName;
            }
            else
            {
                pnlImage.Visible = false;
            }

            editMetaFields.MetaInfo =
                MetaInfoService.GetMetaInfo(news.NewsId, MetaType.News) ??
                new MetaInfo(0, 0, MetaType.News, string.Empty, string.Empty, string.Empty, string.Empty);

            aToClient.HRef = "../" + UrlService.GetLinkDB(ParamType.News, NewsId);
            aToClient.Visible = true;
        }

        private NewsItem GetNewsItemFromForm()
        {
            _newsItem = AddingNew
                ? new NewsItem()
                : NewsService.GetNewsById(NewsId);

            _newsItem.AddingDate = SQLDataHelper.GetDateTime(txtDate.Text + " " + txtTime.Text);
            _newsItem.Title = txtTitle.Text;
            _newsItem.TextToPublication = txtNewsText.Text;
            _newsItem.TextAnnotation = txtAnnotation.Text;
            _newsItem.NewsCategoryId = ddlNewsCategory.SelectedValue.TryParseInt();
            _newsItem.ShowOnMainPage = chkOnMainPage.Checked;

            txtUrlPath.Text = StringHelper.TransformUrl(txtUrlPath.Text);
            _newsItem.UrlPath = txtUrlPath.Text;

            var metaInfo = editMetaFields.MetaInfo;
            metaInfo.Type = MetaType.News;
            metaInfo.ObjId = _newsItem.NewsId;
            _newsItem.Meta = metaInfo;

            return _newsItem;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValidData())
                return;

            if (AddingNew)
            {
                var id = CreateNews();
                Response.Redirect("EditNews.aspx?NewsId=" + id);
            }
            else
            {
                UpdateNews();
                //Response.Redirect("NewsAdmin.aspx");
                Response.Redirect("EditNews.aspx?NewsId=" + NewsId);
            }
        }

        private int CreateNews()
        {
            var id = NewsService.InsertNews(GetNewsItemFromForm());
            if (fuNewsPicture.HasFile)
            {
                var photoName = PhotoService.AddPhoto(new Photo(0, id, PhotoType.News) { OriginName = fuNewsPicture.FileName });
                if (photoName.IsNotEmpty())
                {
                    using (var image = Image.FromStream(fuNewsPicture.FileContent))
                        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.News, photoName),
                            SettingsPictureSize.NewsImageWidth, SettingsPictureSize.NewsImageHeight, image);
                }
            }
            return id;
        }


        private void UpdateNews()
        {
            NewsService.UpdateNews(GetNewsItemFromForm());

            if (fuNewsPicture.HasFile)
            {
                PhotoService.DeletePhotos(NewsId, PhotoType.News);
                
                var photoName = PhotoService.AddPhoto(new Photo(0, NewsId, PhotoType.News) { OriginName = fuNewsPicture.FileName });
                if (!string.IsNullOrWhiteSpace(photoName))
                {
                    using (var image = Image.FromStream(fuNewsPicture.FileContent))
                        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.News, photoName),
                            SettingsPictureSize.NewsImageWidth, SettingsPictureSize.NewsImageHeight, image);
                }
            }
        }

        protected bool IsValidData()
        {
            var errors = new List<string>();
            var validTabs = new List<bool> {true, true};

            if (ddlNewsCategory.SelectedValue.TryParseInt() < 1)
            {
                errors.Add(Resource.Admin_m_News_NoCategory);
                validTabs[0] = false;
            }
            
            DateTime temp;
            if (!DateTime.TryParse(txtDate.Text, out  temp))
            {
                errors.Add(Resource.Admin_m_News_WrongDateFormat);
                validTabs[0] = false;
            }

            if (txtUrlPath.Text.IsNullOrEmpty())
            {
                errors.Add(Resource.Admin_m_News_NoID);
                validTabs[0] = false;
            }

            if (!UrlService.IsAvailableUrl(NewsId, ParamType.News, txtUrlPath.Text))
            {
                errors.Add(Resource.Admin_SynonymExist);
                validTabs[0] = false;
            }

            if (txtTitle.Text.IsNullOrEmpty())
            {
                errors.Add(Resource.Admin_m_News_NoTitle);
                validTabs[0] = false;
            }

            if (fuNewsPicture.HasFile && !FileHelpers.CheckFileExtension(fuNewsPicture.FileName, EAdvantShopFileTypes.Image))
            {
                errors.Add(Resource.Admin_ErrorMessage_WrongImageExtension);
                validTabs[0] = false;
            }

            if (txtNewsText.Text.IsNullOrEmpty())
            {
                errors.Add(Resource.Admin_m_News_NoMessageText);
                validTabs[1] = false;
            }

            if (txtAnnotation.Text.IsNullOrEmpty())
            {
                errors.Add(Resource.Admin_m_News_NoAnnotation);
                validTabs[1] = false;
            }

            imgExclMain.Visible = !validTabs[0];
            imgExclText.Visible = !validTabs[1];

            if (errors.Any())
                MsgErr(errors.AggregateString("<br/>"));

            return validTabs.All(v => v);
        }

        protected void btnDeleteImage_Click(object sender, EventArgs e)
        {
            if (NewsId != 0)
            {
                NewsService.DeleteNewsImage(NewsId);
                pnlImage.Visible = false;
            }
        }

        protected void sds_Init(object sender, EventArgs e)
        {
            ((SqlDataSource)sender).ConnectionString = AdvantShop.Connection.GetConnectionString();
        }
    }
}