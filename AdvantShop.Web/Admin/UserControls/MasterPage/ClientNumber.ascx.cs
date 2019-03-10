using System;
using AdvantShop.Saas;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Admin.UserControls.MasterPage
{
    public partial class ClientNumber : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SaasDataService.IsSaasEnabled)
            {
                if (SaasDataService.CurrentSaasData.ClientNumber.IsNotEmpty())
                {
                    lClientNumber.Text = SaasDataService.CurrentSaasData.ClientNumber + " | ";
                }
                else
                {
                    lClientNumber.Visible = false;
                }
            }
            else
            {
                if (SettingsLic.ClientCode.IsNotEmpty())
                {
                    lClientNumber.Text = SettingsLic.ClientCode + " | ";
                }
                else
                {
                    lClientNumber.Visible = false;
                }
            }

        }
    }
}