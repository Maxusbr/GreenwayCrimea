using System;
using AdvantShop.Core.Controls;
using AdvantShop.Configuration;
using Resources;

namespace Admin
{
    public partial class LandingPage : AdvantShopAdminPage
    {
        public string ErrMessage = Resources.Resource.Admin_CommonSettings_InvalidProfit;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Coupons_Header));

            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            if (SettingsLandingPage.LandingPageCommonStatic == null)
            {
                txtProductLandingPageCommonStatic.Text = "<div class='plp-reasons'><div class='plp-subTitle'>5 причин купить у нас:</div><div class='r-container'>" +
                    "<div class='row center-xs'><div class='col-xs-3 cs-bg-3 reas-flex'>" +
                    "<div class='r-item'><div class='r-img'><img src='modules/productlandingpage/templates/lp/pictures/r1.png' alt='' /></div>" +
                    "<div class='r-info'><div class='r-title'>100% гарантия возврата</div>" +
                    "<div class='r-txt'>Мы вернем деньги, если продукция вам не подойдет или не понравится</div></div></div></div><div class='col-xs-3 cs-bg-3 reas-flex'>" +
                    "<div class='r-item'><div class='r-img'><img src='modules/productlandingpage/templates/lp/pictures/r2.png' alt='' /></div>" +
                    "<div class='r-info'><div class='r-title'>Доставим без предоплаты</div>" +
                    "<div class='r-txt'>Мы доставим товар в любой регион России с оплатой при получении</div></div></div></div>" +
                    "<div class='col-xs-3 cs-bg-3 reas-flex'><div class='r-item'><div class='r-img'><img src='modules/productlandingpage/templates/lp/pictures/r3.png' alt='' /></div>" +
                    "<div class='r-info'><div class='r-title'>Подарок при покупке</div>" +
                    "<div class='r-txt'>При любой покупке вы получаете шикарную косметичку в подарок</div></div></div></div>" +
                    "<div class='col-xs-3 cs-bg-3 reas-flex'><div class='r-item'><div class='r-img'><img src='modules/productlandingpage/templates/lp/pictures/r4.png' alt='' /></div>" +
                    "<div class='r-info'><div class='r-title'>При покупке ноутбука</div><div class='r-txt'>Стильная сумка <br /> в подарок!</div></div></div></div>" +
                    "<div class='col-xs-3 cs-bg-3 reas-flex'><div class='r-item'><div class='r-img'><img src='modules/productlandingpage/templates/lp/pictures/r5.png' alt='' /></div>" +
                    "<div class='r-info'><div class='r-title'>Возврат без проблем</div>" +
                    "<div class='r-txt'>Возвращаете товар курьеру если чтото не нравится. + Возврат в течении 14 дней</div></div></div></div></div></div></div>";
            }
            else
            {
                txtProductLandingPageCommonStatic.Text = SettingsLandingPage.LandingPageCommonStatic;
            }
            chbLandingPageActive.Checked = SettingsLandingPage.ActiveLandingPage;
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            // fix for moving landing from module to engine
            var text = txtProductLandingPageCommonStatic.Text.Replace("modules/productlandingpage/", "landings/");

            SettingsLandingPage.LandingPageCommonStatic = text;
            SettingsLandingPage.ActiveLandingPage = chbLandingPageActive.Checked;
        }
    }
}