//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    public partial class m_ProductVideos : AdvantShopAdminPage
    {
        protected int VideoId
        {
            get
            {
                int id = 0;
                int.TryParse(Request["id"], out id);
                return id;
            }
        }

        protected int ProductId
        {
            get
            {
                int id = 0;
                int.TryParse(Request["productid"], out id);
                return id;
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
            lblError.Text = @"<br/>" + messageText;
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (VideoId != 0)
            {
                SaveVideo();
            }
            else
            {
                CreateVideo();
            }

            if (lblError.Visible == false)
            {
                CommonHelper.RegCloseScript(this, string.Empty);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_m_ProductVideos_Header));

            if (!IsPostBack)
                if (VideoId != 0)
                {
                    btnOK.Text = Resource.Admin_m_ProductVideos_Save;
                    LoadVideoById(VideoId);
                }
        }

        private void SaveVideo()
        {
            //
            // Validation
            //
            MsgErr(true);

            if (VideoId == 0)
            {
                MsgErr(Resource.Admin_m_News_WrongPageParameters);
                return;
            }

            //if (String.IsNullOrEmpty(txtName.Text.Trim()))
            //{
            //    MsgErr(Resource.Admin_m_ProductVideos_NoName);
            //    return;
            //}

            if (String.IsNullOrEmpty(txtPlayerCode.Text.Trim()))
            {
                MsgErr(Resource.Admin_m_ProductVideos_NoPlayerCode);
                return;
            }

            int sortOrder = 0;
            if (!String.IsNullOrEmpty(txtSortOrder.Text))
            {
                int.TryParse(txtSortOrder.Text, out sortOrder);
            }

            try
            {
                ProductVideoService.UpdateProductVideo(new ProductVideo
                    {
                        ProductVideoId = VideoId,
                        Name = txtName.Text.Trim(),
                        PlayerCode = txtPlayerCode.Text.Trim(),
                        Description = txtDescription.Text.Trim(),
                        VideoSortOrder = sortOrder
                    });
            }
            catch (Exception ex)
            {
                MsgErr(ex.Message + " SaveProductVideos");
                Debug.Log.Error(ex);
            }
        }

        private void CreateVideo()
        {
            //
            // Validation
            //
            MsgErr(true);

            //if (String.IsNullOrEmpty(txtName.Text.Trim()))
            //{
            //    MsgErr(Resource.Admin_m_ProductVideos_NoName);
            //    return;
            //}


            string playercode = txtPlayerCode.Text.Trim();
            if (string.IsNullOrEmpty(playercode))
            {
                string error;
                playercode = ProductVideoService.GetPlayerCodeFromLink(txtVideoLink.Text.Trim(), out error);
                if (!string.IsNullOrEmpty(error))
                {
                    MsgErr(error);
                    return;
                }
            }

            int sortOrder = 0;
            if (!String.IsNullOrEmpty(txtSortOrder.Text))
            {
                int.TryParse(txtSortOrder.Text, out sortOrder);
            }

            try
            {
                ProductVideoService.AddProductVideo(new ProductVideo
                    {
                        ProductId = ProductId,
                        Name = txtName.Text.Trim(),
                        PlayerCode = playercode,
                        Description = txtDescription.Text.Trim(),
                        VideoSortOrder = sortOrder
                    });
            }
            catch (Exception ex)
            {
                MsgErr(ex.Message + " CreateVideo");
                Debug.Log.Error(ex);
            }
        }

        private void LoadVideoById(int videoId)
        {
            ProductVideo pv = ProductVideoService.GetProductVideo(videoId);

            if (pv == null)
            {
                MsgErr("Video with this ID not exist");
                return;
            }

            preview.InnerHtml = pv.PlayerCode;
            preview.Visible = true;
            txtName.Text = pv.Name;
            txtPlayerCode.Text = pv.PlayerCode;
            txtDescription.Text = pv.Description;
            txtSortOrder.Text = pv.VideoSortOrder.ToString(CultureInfo.InvariantCulture);
        }
    }
}