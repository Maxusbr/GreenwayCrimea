using System;
using System.Web.UI;
using AdvantShop.Module.GoogleImagesSearch.Domain;

namespace AdvantShop.Module.GoogleImagesSearch
{
    public partial class Admin_GoogleImagesSearchModule : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtApiKey.Text = GoogleImagesSearchSettings.ApiKey;
                txtCSEngineId.Text = GoogleImagesSearchSettings.CSEngineId;
            }
        }

        protected void Save()
        {
            GoogleImagesSearchSettings.ApiKey = txtApiKey.Text;
            GoogleImagesSearchSettings.CSEngineId = txtCSEngineId.Text;

            lblMessage.Text = (string)GetLocalResourceObject("Saved");
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}