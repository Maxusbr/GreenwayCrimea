using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using AdvantShop.Module.StoreReviews.Domain;

namespace AdvantShop.Module.StoreReviews
{

    public partial class Modules_StoreReviews_EditReview : System.Web.UI.Page
    {
        private int _reviewId;
        private StoreReview _review;

        private void MsgErr(string message)
        {
            lError.Visible = !string.IsNullOrEmpty(message);
            lError.Text = message;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MsgErr(string.Empty);

            if (string.IsNullOrEmpty(Request["id"]) || !int.TryParse(Request["id"], out _reviewId)
                || (_review = StoreReviewRepository.GetStoreReview(_reviewId)) == null)
            {
                return;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void LoadData()
        {
            if (_review == null)
            {
                return;
            }

            txtDateAdded.Text = _review.DateAdded.ToString("yyyy.MM.dd HH:mm");
            txtReviewerName.Text = HttpUtility.HtmlDecode(_review.ReviewerName);
            txtEmail.Text = _review.ReviewerEmail;
            rblRating.SelectedValue = _review.Rate.ToString();
            txtReview.Text = HttpUtility.HtmlDecode(_review.Review);
            ckbModerated.Checked = _review.Moderated;

            var imagePath = HostingEnvironment.MapPath(StoreReviews.ImagePath  + _review.ReviewerImage);
            if (File.Exists(imagePath))
            {
                pnlReviewerImage.Visible = true;
                fuReviewerImage.Visible = false;

                imgReviewerImage.ImageUrl = StoreReviews.ImagePath + _review.ReviewerImage;
            }
        }

        protected void btnSaveClick(object sender, EventArgs e)
        {
            if (_review == null)
            {
                return;
            }

            DateTime date = _review.DateAdded;
            try
            {
                date = DateTime.ParseExact(txtDateAdded.Text, "yyyy.MM.dd HH:mm", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                MsgErr((string)GetLocalResourceObject("StoreReviews_WrongDateFormat"));
                return;
            }
            
            var imagePath = HostingEnvironment.MapPath(StoreReviews.ImagePath);
            if (!Directory.Exists(imagePath))
                Directory.CreateDirectory(imagePath);
            
            if (fuReviewerImage.HasFile)
            {
                if (!StoreReviewRepository.CheckImageExtension(fuReviewerImage.FileName))
                {
                    MsgErr((string)GetLocalResourceObject("StoreReviews_WrongImageFormat"));
                    return;
                }

                var imageName = _review.Id + Path.GetExtension(fuReviewerImage.FileName);
                StoreReviewRepository.SaveAndResizeImage(System.Drawing.Image.FromStream(fuReviewerImage.FileContent), imagePath + imageName);
                if (File.Exists(imagePath + imageName))
                {
                    _review.ReviewerImage = imageName;
                }
            }

            _review.DateAdded = date;
            _review.Moderated = ckbModerated.Checked;
            _review.Rate = string.IsNullOrEmpty(rblRating.SelectedValue) ? 0 : Convert.ToInt32(rblRating.SelectedValue);
            _review.Review = HttpUtility.HtmlEncode(txtReview.Text);
            _review.ReviewerEmail = txtEmail.Text;
            _review.ReviewerName = HttpUtility.HtmlEncode(txtReviewerName.Text);

            StoreReviewRepository.UpdateStoreReview(_review);

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

        protected void btnDeleteReviewerImage_Click(object sender, EventArgs e)
        {
            if (_review != null)
            {
                StoreReviewRepository.DeleteReviewerImage(_review.Id);

                pnlReviewerImage.Visible = false;
                fuReviewerImage.Visible = true;

                imgReviewerImage.ImageUrl = string.Empty;
            }
        }
    }
}