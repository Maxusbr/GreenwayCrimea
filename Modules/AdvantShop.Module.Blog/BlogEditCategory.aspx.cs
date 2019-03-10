using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Module.Blog.Domain;

namespace AdvantShop.Module.Blog
{
    public partial class BlogEditCategory : System.Web.UI.Page
    {
        private int _blogCategoryId;
        private BlogCategory _blogCategory;

        protected void Page_Load(object sender, EventArgs e)
        {
            lBase.Text = string.Format("<base href='{0}' />", UrlService.GetUrl("modules/blog/"));

            if (string.IsNullOrEmpty(Request["id"]) || !Int32.TryParse(Request["id"], out _blogCategoryId))
                return;

            if ((_blogCategory = BlogService.GetBlogCategory(_blogCategoryId)) == null)
                return;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        protected void LoadData()
        {
            if (_blogCategory == null)
                return;

            txtName.Text = _blogCategory.Name;
            txtUrlPath.Text = _blogCategory.UrlPath;
            txtSortOrder.Text = _blogCategory.SortOrder.ToString();

            txtMetaTitle.Text = _blogCategory.MetaTitle;
            txtMetaDescription.Text = _blogCategory.MetaDescription;
            txtMetaKeywords.Text = _blogCategory.MetaKeywords;
        }

        protected void btnSaveClick(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                lError.Visible = true;
                return;
            }

            if (_blogCategory == null)
            {
                _blogCategory = new BlogCategory()
                {
                    Name = txtName.Text,
                    UrlPath = txtUrlPath.Text,
                    SortOrder = txtSortOrder.Text.TryParseInt(),

                    MetaTitle = txtMetaTitle.Text,
                    MetaKeywords = txtMetaKeywords.Text,
                    MetaDescription = txtMetaDescription.Text,
                };
                
                BlogService.AddBlogCategory(_blogCategory);
            }
            else
            {
                _blogCategory.Name = txtName.Text.Trim();
                _blogCategory.UrlPath = txtUrlPath.Text.Trim();
                _blogCategory.SortOrder = txtSortOrder.Text.TryParseInt();

                _blogCategory.MetaTitle = txtMetaTitle.Text;
                _blogCategory.MetaKeywords = txtMetaKeywords.Text;
                _blogCategory.MetaDescription = txtMetaDescription.Text;

                BlogService.UpdateBlogCategory(_blogCategory);
            }


            var jScript = new StringBuilder();
            jScript.Append("<script type=\'text/javascript\' language=\'javascript\'> ");
            if (string.IsNullOrEmpty(string.Empty))
                jScript.Append("window.opener.location.reload();");
            else
                jScript.Append("window.opener.location =" + string.Empty);
            jScript.Append("self.close();");
            jScript.Append("</script>");
            Type csType = this.GetType();
            ClientScriptManager clScriptMng = this.ClientScript;
            clScriptMng.RegisterClientScriptBlock(csType, "Close_window", jScript.ToString());
        }


        protected bool ValidateFields()
        {
            bool valid = true;

            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.BorderColor = System.Drawing.Color.Red;
                txtName.BorderWidth = 1;
                valid = false;
            }
            else
            {
                txtName.BorderColor = System.Drawing.Color.Gray;
                txtName.BorderWidth = 1;
            }

            var reg = new Regex("^[a-zA-Z0-9_-]*$");
            if (string.IsNullOrEmpty(txtUrlPath.Text) || !reg.IsMatch(txtUrlPath.Text))
            {
                txtUrlPath.BorderColor = System.Drawing.Color.Red;
                txtUrlPath.BorderWidth = 1;
                valid = false;
            }
            else
            {
                txtUrlPath.BorderColor = System.Drawing.Color.Gray;
                txtUrlPath.BorderWidth = 1;
            }
            if (string.IsNullOrEmpty(txtMetaTitle.Text))
            {
                txtMetaTitle.BorderColor = System.Drawing.Color.Red;
                txtMetaTitle.BorderWidth = 1;
                valid = false;
            }
            else
            {
                txtMetaTitle.BorderColor = System.Drawing.Color.Gray;
                txtMetaTitle.BorderWidth = 1;
            }
            if (string.IsNullOrEmpty(txtMetaDescription.Text))
            {
                txtMetaDescription.BorderColor = System.Drawing.Color.Red;
                txtMetaDescription.BorderWidth = 1;
                valid = false;
            }
            else
            {
                txtMetaDescription.BorderColor = System.Drawing.Color.Gray;
                txtMetaDescription.BorderWidth = 1;
            }
            if (string.IsNullOrEmpty(txtMetaKeywords.Text))
            {
                txtMetaKeywords.BorderColor = System.Drawing.Color.Red;
                txtMetaKeywords.BorderWidth = 1;
                valid = false;
            }
            else
            {
                txtMetaKeywords.BorderColor = System.Drawing.Color.Gray;
                txtMetaKeywords.BorderWidth = 1;
            }

            return valid;
        }
    }
}