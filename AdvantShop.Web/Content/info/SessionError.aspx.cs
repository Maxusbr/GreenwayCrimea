//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Core;
using AdvantShop.Helpers;

namespace ClientPages
{
    public partial class SessionError : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            AdvantShop.Localization.Culture.InitializeCulture(AdvantshopConfigService.GetLocalization());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CommonHelper.DisableBrowserCache();

            int errorCode = 0;
            if (!string.IsNullOrEmpty(Request["errorcode"]))
            {
                try
                {
                    errorCode = int.Parse(Request["errorcode"]);
                }
                catch (Exception ex)
                {
                    AdvantShop.Diagnostics.Debug.Log.Error(ex);
                }
            }

            string errorMessage = string.Empty;
            switch (errorCode)
            {
                case 1:
                    errorMessage = Resources.Resource.Client_Info_DataBaseNotAvailable;
                    break;
                case 2:
                    errorMessage = Resources.Resource.Client_Info_IncorrectDataBaseVersion;
                    break;

                case 3:
                    errorMessage = Resources.Resource.Client_Info_IncorrectDataBase;
                    break;

                case 4:
                    errorMessage = Resources.Resource.Client_Info_IncorrectDataBase;
                    break;

            }

            dvErr.InnerHtml = errorCode == 0 ? string.Empty : errorMessage;
        }
    }
}