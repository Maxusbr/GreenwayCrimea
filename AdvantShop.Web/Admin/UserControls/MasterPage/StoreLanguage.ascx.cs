//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Core.Services.Localization;

namespace Admin.UserControls.MasterPage
{
    public partial class StoreLanguage : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rprLanguages.DataSource = LanguageService.GetList();
            rprLanguages.DataBind();
        }
    }
}