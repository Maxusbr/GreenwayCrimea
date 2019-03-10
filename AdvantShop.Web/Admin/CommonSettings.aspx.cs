//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    public partial class CommonSettings : AdvantShopAdminPage
    {
        private void MsgErr(bool clear)
        {
            lblError.Visible = false;
            lblError.Text = string.Empty;
        }

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            //lblError.Text += @"<br/>" + messageText;
            lblError.Text += string.Format("{0} // {1}", messageText, DateTime.Now.ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CommonHelper.DisableBrowserCache();
            MsgErr(true);
            try
            {
                SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_CommonSettings_Header));

                if (!IsPostBack)
                {
                    if (Request.Cookies.AllKeys.Contains("_CommonSettingsSaved"))
                    {
                        MsgErr(Resource.Admin_CommonSettings_ChangesSaved);
                        var httpCookie = Request.Cookies["saved"];
                        //if (httpCookie != null) __liState.Value = httpCookie.Value;
                        Request.Cookies.Remove("saved");
                        Response.Cookies.Remove("saved");
                    }
                    int tab;
                    if (Request["tab"] != null && int.TryParse(Request["tab"], out tab))
                    {
                        //__liState.Value = tab.ToString(CultureInfo.InvariantCulture);
                    }

                }
            }
            catch (Exception ex)
            {
                MsgErr(ex.Message + " atLoad");
                Debug.Log.Error("atLoad", ex);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //bankset.Visible = AdvantShop.Core.AdvantshopConfigService.GetActivityCommonSetting("banksettings");
            //tabBankSettings.Visible = bankset.Visible;
        }

        private bool SaveSettings()
        {
            EditRobotsTxt.SaveRobots();
            //BankSettings.SaveData();

            bool valid = true;
            if (!NewsSettings.SaveData())
            {
                MsgErr(NewsSettings.ErrMessage);
                valid = false;
            }
            if (!MailSettings.SaveData())
            {
                MsgErr(MailSettings.ErrMessage);
                valid = false;
            }
            if (!GeneralSettings.SaveData())
            {
                MsgErr(GeneralSettings.ErrMessage);
                valid = false;
            }
            if (!SEOSettings.SaveData())
            {
                MsgErr(SEOSettings.ErrMessage);
                valid = false;
            }

            if (!CountersSettings.SaveData())
            {
                MsgErr(CountersSettings.ErrMessage);
                valid = false;
            }

            if (!OrderConfirmationSettings.SaveData())
            {
                MsgErr(OrderConfirmationSettings.ErrMessage);
                valid = false;
            }
            if (!NotifyEmailsSettings.SaveData())
            {
                MsgErr(NotifyEmailsSettings.ErrMessage);
                valid = false;
            }

            if (!OAuthSettings.SaveData())
            {
                MsgErr(OAuthSettings.ErrMessage);
                valid = false;
            }


            if (!CatalogSettings.SaveData())
            {
                MsgErr(CatalogSettings.ErrMessage);
                valid = false;
            }

            if (!DetailsSettings.SaveData())
            {
                MsgErr(DetailsSettings.ErrMessage);
                valid = false;
            }

            //if (!TaskSettings.SaveData())
            //{
            //    MsgErr(TaskSettings.ErrMessage);
            //    valid = false;
            //}

            if (!ProfitSettings.SaveData())
            {
                MsgErr(ProfitSettings.ErrMessage);
                valid = false;
            }

            if (!MobileSettings.SaveData())
            {
                MsgErr(MobileSettings.ErrMessage);
                valid = false;
            }

            SocialSettings.SaveData();
            TelephonySettings.SaveData();
            ApiSettings.SaveData();

            return valid;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveSettings())
            {
                MsgErr(Resource.Admin_CommonSettings_ChangesSaved);
            }
            if (GeneralSettings.FlagRedirect)
            {
                //Response.Cookies.Add(new HttpCookie("saved", __liState.Value));
                Response.Redirect("CommonSettings.aspx");
            }
        }
    }
}