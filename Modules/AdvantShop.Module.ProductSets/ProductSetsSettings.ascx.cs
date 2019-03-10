using System;
using System.Drawing;
using System.Web.UI;
using AdvantShop.Module.ProductSets.Domain;

namespace AdvantShop.Module.ProductSets
{
    public partial class Admin_ProductSetsSettings : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            txtTitle.Text = ProductSetsSettings.Title;
        }

        protected void Save()
        {
            ProductSetsSettings.Title = txtTitle.Text;

            lblMessage.Text = (string)GetLocalResourceObject("ProductSets_Saved");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}