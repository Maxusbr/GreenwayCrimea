//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Saas;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;

namespace Admin
{
    public partial class NotInTariff : AdvantShopAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(SettingsMain.ShopName);
            if (!SaasDataService.IsSaasEnabled)
            {
                return;
            }

            lblCurrentTariff.Text = SaasDataService.CurrentSaasData.Name;

            var nextSaasPlanData = SaasDataService.GetNextSaasDataFromService();
            if (nextSaasPlanData != null)
            {
                lblNextTariff.Text = nextSaasPlanData.Name;

                liProductsCount.Visible = nextSaasPlanData.ProductsCount != 0;
                lblProductsCount.Text = nextSaasPlanData.ProductsCount.ToString();

                liManagers.Visible = nextSaasPlanData.EmployeesCount != 0;
                lblManagers.Text = nextSaasPlanData.EmployeesCount.ToString();
                
                                
                liTelephony.Visible = nextSaasPlanData.HaveTelephony;
                liCallback.Visible = nextSaasPlanData.HaveTelephony;
                li1C.Visible = nextSaasPlanData.Have1C;
                liCrm.Visible = nextSaasPlanData.HaveCrm;
                liUserTracking.Visible = nextSaasPlanData.HaveCustomerLog;
                liTags.Visible = nextSaasPlanData.HaveTags;
                liMobileAdmin.Visible = nextSaasPlanData.HaveMobileAdmin;
                liAllowCustom.Visible = nextSaasPlanData.HaveCustom;
                liVipSupport.Visible = nextSaasPlanData.HaveVIPsupport;
                liBonus.Visible = nextSaasPlanData.BonusSystem;
            }
            else
            {
                pnlNextSaasPlan.Visible = false;
            }

        }
    }
}