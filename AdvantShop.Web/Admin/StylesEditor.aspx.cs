//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Diagnostics;
using Resources;

namespace Admin
{
    public partial class StylesEditor : AdvantShopAdminPage
    {
        private string _path;
        protected string CssContent;

        protected void Page_Load(object sender, EventArgs e)
        {
            _path = Server.MapPath("~/userfiles/extra.css");
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_MasterPageAdmin_StylesEditor));
            MsgErr(true);
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(_path))
                {
                    using (File.Create(_path))
                    {
                        //nothing here, just  create file
                    }
                }

                using (TextReader reader = new StreamReader(_path))
                {
                    CssContent = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        private void MsgErr(bool boolClean)
        {
            if (boolClean)
            {
                lblInfo.Visible = false;
                lblInfo.Text = string.Empty;
            }
            else
            {
                lblInfo.Visible = false;
            }
        }

        private void MsgErr(string strMessageText, bool isSucces)
        {
            const string strSuccesFormat = "<div class=\"label-box-admin good\" style=\"margin-top:20px; margin-bottom:20px; width: 735px;\">{0} // at {1}</div>";
            const string strFailFormat = "<div class=\"label-box-admin error\" style=\"margin-top:20px; margin-bottom:20px; width: 735px;\">{0} // at {1}</div>";

            lblInfo.Visible = true;

            if (isSucces)
            {
                lblInfo.Text = string.Format(strSuccesFormat, strMessageText, DateTime.Now.ToString());
            }
            else
            {
                lblInfo.Text = string.Format(strFailFormat, strMessageText, DateTime.Now.ToString());
            }

        }
    }
}