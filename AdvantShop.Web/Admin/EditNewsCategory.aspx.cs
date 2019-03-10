//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.News;
using AdvantShop.SEO;
using Resources;
using System.Text.RegularExpressions;

namespace Admin
{
    public partial class EditNewsCategory : AdvantShopAdminPage
    {
        private static NewsCategory _newsCategory;

        private void MsgErr(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = message.IsNotEmpty();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MsgErr(string.Empty);
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Properties_ListPropreties));
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            var newsCategoryId = 0;
            if (!int.TryParse(Request["newscategoryid"], out newsCategoryId))
            {
                return;
            }

            _newsCategory = NewsService.GetNewsCategoryById(newsCategoryId);

            txtNewsCategoryName.Text = _newsCategory.Name;
            txtNewsCategiryUrlPath.Text = _newsCategory.UrlPath;
            txtNewsCategorySortOrder.Text = _newsCategory.SortOrder.ToString();

            editMetaFields.MetaInfo = _newsCategory.Meta;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValidData())
            {
                MsgErr(Resource.Admn_NewsCategory_WrongData);
                return;
            }

            var metaInfo = editMetaFields.MetaInfo;
            metaInfo.Type = MetaType.NewsCategory;

            if (string.IsNullOrEmpty(Request["newscategoryid"]))
            {
                NewsService.InsertNewsCategory(
                    new NewsCategory
                    {
                        Name = txtNewsCategoryName.Text,
                        UrlPath = txtNewsCategiryUrlPath.Text,
                        SortOrder = txtNewsCategorySortOrder.Text.TryParseInt(),
                        Meta = metaInfo
                    });
            }
            else
            {
                metaInfo.ObjId = _newsCategory.NewsCategoryId;
                _newsCategory.Name = txtNewsCategoryName.Text;
                _newsCategory.UrlPath = txtNewsCategiryUrlPath.Text;
                _newsCategory.SortOrder = txtNewsCategorySortOrder.Text.TryParseInt();
                _newsCategory.Meta = metaInfo;

                NewsService.UpdateNewsCategory(_newsCategory);
            }

            Response.Redirect("newscategory.aspx");
        }

        protected bool IsValidData()
        {
            bool valid = true;

            valid &= txtNewsCategorySortOrder.Text.IsNotEmpty();
            valid &= txtNewsCategiryUrlPath.Text.IsNotEmpty();
            valid &= txtNewsCategoryName.Text.IsNotEmpty();

            var r = new Regex("^[a-zA-Z0-9_-]*$");
            valid &= r.IsMatch(txtNewsCategiryUrlPath.Text) && UrlService.IsAvailableUrl(_newsCategory!= null ? _newsCategory.NewsCategoryId :0, ParamType.NewsCategory, txtNewsCategiryUrlPath.Text);



            return valid;
        }
    }
}