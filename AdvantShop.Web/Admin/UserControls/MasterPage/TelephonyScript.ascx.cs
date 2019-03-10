using System;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Saas;

namespace AdvantShop.Admin.UserControls.MasterPage
{
    public partial class TelephonyScript : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Visible = IPTelephonyOperator.Current.Type != EOperatorType.None && (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveTelephony));
        }
    }
}