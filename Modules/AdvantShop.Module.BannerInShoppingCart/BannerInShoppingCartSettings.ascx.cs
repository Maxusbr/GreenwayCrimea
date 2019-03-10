using System;
using System.Drawing;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.BannerInShoppingCart
{
    public partial class BannerInShoppingCartSettings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            txtStaticBlock.Text = ModuleSettingsProvider.GetSettingValue<string>("BannerStaticBlock", BannerInShoppingCartModule.ModuleID);
            ModuleSettingsProvider.GetSettingValue<string>("BannerImage", BannerInShoppingCartModule.ModuleID);
        }

        protected void Save()
        {
            //if (!Validate())
            //{
            //    lblMessage.Text = (string)GetLocalResourceObject("WrongData");
            //    lblMessage.ForeColor = Color.Red;
            //    lblMessage.Visible = true;
            //    return;
            //}

            ModuleSettingsProvider.SetSettingValue("BannerStaticBlock", txtStaticBlock.Text, BannerInShoppingCartModule.ModuleID);

            //if (fuBanner.HasFile)
            //{
            //    var BannerPath = HostingEnvironment.MapPath("~/Modules/BannerInShoppingCart/Banner");
            //    if (!Directory.Exists(BannerPath))
            //    {
            //        Directory.CreateDirectory(BannerPath);
            //    }

            //    var fileName = BannerPath + DateTime.Now.ToString() + fuBanner.FileName.Substring(fuBanner.FileName.LastIndexOf("."));

            //    fuBanner.SaveAs("~/Modules/BannerInShoppingCart/Banner/Banner/" + fileName);
                

            //    ModuleSettingsProvider.SetSettingValue("BannerImage", fuBanner.FileName, BannerInShoppingCartModule.ModuleID);
            //}

            //ModuleSettingsProvider.SetSettingValue("BannerStaticBlock", txtStaticBlock.Text, BannerInShoppingCartModule.ModuleID);

            lblMessage.Text = (string)GetLocalResourceObject("ChangesSaved");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected bool Validate()
        {
            bool valid = true;

            return valid;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}