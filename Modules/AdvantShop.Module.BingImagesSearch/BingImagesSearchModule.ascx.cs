using AdvantShop.Module.BingImagesSearch.Domain;
using System;
using System.Web.UI;


namespace AdvantShop.Module.BingImagesSearch
{
    public partial class Admin_BingImagesSearchModule : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtApiKey.Text = BingImagesSearchSettings.ApiKey;
            }
        }

        protected void Save()
        {
            BingImagesSearchSettings.ApiKey = txtApiKey.Text;

            lblMessage.Text = (string)GetLocalResourceObject("Saved");
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}