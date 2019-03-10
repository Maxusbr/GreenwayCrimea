using System;
using System.Web.UI;
using AdvantShop.Core.Modules;
using AdvantShop.Module.Disqus.Domain;

namespace AdvantShop.Module.Disqus
{
    public partial class Admin_DisqusModule : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtShortname.Text = DisqusSettings.ShortName;
            }
        }

        protected void Save()
        {
            DisqusSettings.ShortName = txtShortname.Text;

            lblMessage.Text = (String)GetLocalResourceObject("Disqus_Saved");
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}