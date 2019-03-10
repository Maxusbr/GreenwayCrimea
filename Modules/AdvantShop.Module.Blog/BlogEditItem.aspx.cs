using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Module.Blog.Domain;

namespace AdvantShop.Module.Blog
{

    public partial class BlogEditItem : System.Web.UI.Page
    {
        private int blogItemId = 0;
        private BlogItem blogItem;

        private const string pictPath = "pictures/modules/blog/";

        protected void Page_Load(object sender, EventArgs e)
        {
            //BlogService.UpdateBlogModule();

            lBase.Text = string.Format("<base href='{0}'/>", UrlService.GetUrl("modules/blog/"));

            txtTitle.Attributes.Add("data-translit-field", "urlpath");

            if (string.IsNullOrEmpty(Request["id"]))
                return;

            if (!Int32.TryParse(Request["id"], out blogItemId))
                return;

            BlogProduct.BlogId = blogItemId;

            if ((blogItem = BlogService.GetBlogItem(blogItemId)) == null)
                return;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected void LoadData()
        {
            var categories = BlogService.GetListBlogCategory();
            ddlBlogCategory.Items.Add(new ListItem((String)GetLocalResourceObject("NotSelected"), "-1"));
            foreach (var category in categories)
            {
                ddlBlogCategory.Items.Add(new ListItem(category.Name, category.ItemCategoryId.ToString()));
            }

            if (ddlBlogCategory.Items.Count > 1)
            {
                ddlBlogCategory.SelectedIndex = 1;
            }


            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtTime.Text = DateTime.Now.ToString("HH:mm");


            if (blogItem == null)
            {
                pnlImage.Visible = false;
                BlogProduct.Visible = false;
                return;
            }

            if (categories.Any(item => item.ItemCategoryId == blogItem.ItemCategoryId))
            {
                ddlBlogCategory.SelectedValue = blogItem.ItemCategoryId.ToString();
            }

            var directory = HttpContext.Current.Server.MapPath("~/" + pictPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var fullPicturePath = directory + blogItem.Picture;
            if (File.Exists(fullPicturePath))
            {
                Label10.Text = blogItem.Picture;
                pnlImage.Visible = true;
                Image1.ImageUrl = "~/" + pictPath + blogItem.Picture;
                Image1.ToolTip = blogItem.Picture;
            }
            else
            {
                Label10.Text = @"No picture";
                pnlImage.Visible = false;
            }

            txtDate.Text = blogItem.AddingDate.ToString("yyyy-MM-dd");
            txtTime.Text = blogItem.AddingDate.ToString("HH:mm");
            //lblAddingDate.Text = blogItem.AddingDate.ToString("yyyy.MM.dd HH:mm");

            txtTitle.Text = blogItem.Title;
            txtUrlPath.Text = blogItem.UrlPath;

            txtTextAnnotation.Text = blogItem.TextAnnotation;
            txtTextToPublication.Text = blogItem.TextToPublication;
            //txtTextToEmail.Text = blogItem.TextToEmail;

            txtMetaTitle.Text = blogItem.MetaTitle;
            txtMetaKeywords.Text = blogItem.MetaKeywords;
            txtMetaDescription.Text = blogItem.MetaDescription;
            ckbEnable.Checked = blogItem.Enabled;

        }

        protected void btnSaveClick(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                lError.Visible = true;
                return;
            }

            if (blogItem == null)
            {
                blogItem = new BlogItem
                {
                    Title = txtTitle.Text,
                    UrlPath = txtUrlPath.Text,

                    TextAnnotation = txtTextAnnotation.Text,
                    TextToPublication = txtTextToPublication.Text,
                    //TextToEmail = txtTextToEmail.Text,

                    AddingDate = Convert.ToDateTime(txtDate.Text + " " + txtTime.Text),

                    MetaTitle = txtMetaTitle.Text,
                    MetaKeywords = txtMetaKeywords.Text,
                    MetaDescription = txtMetaDescription.Text,

                    Enabled = ckbEnable.Checked,

                    ItemCategoryId = ddlBlogCategory.SelectedValue != "-1"
                        ? Convert.ToInt32(ddlBlogCategory.SelectedValue)
                        : (int?)null
                };

                if (PictureFileUpload.HasFile)
                {
                    var fileName = Guid.NewGuid() + ".jpg";

                    var directory = HttpContext.Current.Server.MapPath("~/" + pictPath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    var fullPicturePath = directory + fileName;

                    BlogService.SaveAndResizeImage(System.Drawing.Image.FromStream(PictureFileUpload.FileContent), fullPicturePath);

                    blogItem.Picture = fileName;
                }

                BlogService.AddBlogItem(blogItem);
            }
            else
            {
                blogItem.Title = txtTitle.Text;
                blogItem.UrlPath = txtUrlPath.Text;

                blogItem.TextAnnotation = txtTextAnnotation.Text;
                blogItem.TextToPublication = txtTextToPublication.Text;
                //blogItem.TextToEmail = txtTextToEmail.Text;

                blogItem.AddingDate = Convert.ToDateTime(txtDate.Text + " " + txtTime.Text);

                blogItem.MetaTitle = txtMetaTitle.Text;
                blogItem.MetaKeywords = txtMetaKeywords.Text;
                blogItem.MetaDescription = txtMetaDescription.Text;

                blogItem.Enabled = ckbEnable.Checked;

                blogItem.ItemCategoryId = ddlBlogCategory.SelectedValue != "-1"
                    ? Convert.ToInt32(ddlBlogCategory.SelectedValue)
                    : (int?)null;

                if (PictureFileUpload.HasFile)
                {
                    var fileName = Guid.NewGuid() + PictureFileUpload.FileName.Substring(PictureFileUpload.FileName.LastIndexOf("."));

                    var directory = HttpContext.Current.Server.MapPath("~/" + pictPath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    var fullPicturePath = directory + fileName;

                    BlogService.SaveAndResizeImage(System.Drawing.Image.FromStream(PictureFileUpload.FileContent), fullPicturePath);

                    blogItem.Picture = fileName;
                }
                BlogService.UpdateBlogItem(blogItem);
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

        protected void btnDeleteImage_Click(object sender, EventArgs e)
        {
            if (blogItem != null)
            {
                var fullPicturePath = HttpContext.Current.Server.MapPath("~/" + pictPath + blogItem.Picture);
                if (File.Exists(fullPicturePath))
                {
                    File.Delete(fullPicturePath);
                }
                pnlImage.Visible = false;
            }
            blogItem.Picture = null;
            BlogService.UpdateBlogItem(blogItem);
        }

        protected bool ValidateFields()
        {
            bool valid = true;

            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                txtTitle.BorderColor = System.Drawing.Color.Red;
                txtTitle.BorderWidth = 1;
                valid = false;
            }
            else
            {
                txtTitle.BorderColor = System.Drawing.Color.Gray;
                txtTitle.BorderWidth = 1;
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

            if (string.IsNullOrEmpty(txtTextAnnotation.Text))
            {
                txtTextAnnotation.BorderColor = System.Drawing.Color.Red;
                txtTextAnnotation.BorderWidth = 1;
                valid = false;
            }
            else
            {
                txtTextAnnotation.BorderColor = System.Drawing.Color.Gray;
                txtTextAnnotation.BorderWidth = 1;
            }

            if (string.IsNullOrEmpty(txtTextToPublication.Text))
            {
                txtTextToPublication.BorderColor = System.Drawing.Color.Red;
                txtTextToPublication.BorderWidth = 1;
                valid = false;
            }
            else
            {
                txtTextToPublication.BorderColor = System.Drawing.Color.Gray;
                txtTextToPublication.BorderWidth = 1;
            }


            DateTime date;
            if (!DateTime.TryParseExact(txtDate.Text, "yyyy-MM-dd",
                              CultureInfo.CurrentCulture,
                              DateTimeStyles.None,
                              out date))
            {
                txtDate.BorderColor = System.Drawing.Color.Red;
                txtDate.BorderWidth = 1;
                valid = false;
            }
            else
            {
                txtDate.BorderColor = System.Drawing.Color.Gray;
                txtDate.BorderWidth = 1;
            }

            DateTime time;
            if (!DateTime.TryParseExact(txtTime.Text, "HH:mm",
                              CultureInfo.CurrentCulture,
                              DateTimeStyles.None,
                              out time))
            {
                txtTime.BorderColor = System.Drawing.Color.Red;
                txtTime.BorderWidth = 1;
                valid = false;
            }
            else
            {
                txtTime.BorderColor = System.Drawing.Color.Gray;
                txtTime.BorderWidth = 1;
            }

            return valid;
        }
    }
}