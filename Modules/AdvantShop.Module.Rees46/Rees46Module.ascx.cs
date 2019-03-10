using System;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Module.Rees46.Domain;
using AdvantShop.Helpers;
using AdvantShop.Diagnostics;
using System.IO;

namespace AdvantShop.Module.Rees46
{
    public partial class Admin_Rees46Module : System.Web.UI.UserControl
    {
        private void MsgErr(string msg)
        {
            lblMessage.Visible = true;
            lblMessage.Text = msg;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            SetVisible();
        }
        
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtShopKey.Text = Rees46Settings.ShopKey;
            }

            locRelatedProduct.Text = SettingsCatalog.RelatedProductName;
            locAlternativeProduct.Text = SettingsCatalog.AlternativeProductName;
            txtUrlPushSw.Text = Rees46Settings.PathFilePushSW;

            FileReadWrite("read");

            lblRecommenderNotice.Text =
                string.Format(
                    "Чтобы активировать '{0}' и '{1}' зайдите в <a href=\"{2}\" target=\"blank\">Настройки -> Общие -> Карточка товара -> Источник продуктов для перекрестного маркетинга</a> и выберите \"из модуля\"",
                    SettingsCatalog.RelatedProductName,
                    SettingsCatalog.AlternativeProductName,
                    UrlService.GetUrl("admin/CommonSettings.aspx#tabid=details"));

            var list =
                Enum.GetNames(typeof (Recomender))
                    .Select(recom => new ListItem((string) GetLocalResourceObject(recom), recom))
                    .ToList();

            ddlRelatedProduct.DataSource = list;
            ddlRelatedProduct.DataBind();

            if (ddlRelatedProduct.Items.FindByValue(Rees46Settings.RelatedProduct) != null)
                ddlRelatedProduct.SelectedValue = Rees46Settings.RelatedProduct;

            ddlAlternativeProduct.DataSource = list;
            ddlAlternativeProduct.DataBind();

            if (ddlAlternativeProduct.Items.FindByValue(Rees46Settings.AlternativeProduct) != null)
                ddlAlternativeProduct.SelectedValue = Rees46Settings.AlternativeProduct;

            ddlCatalogTopBlock.DataSource = list;
            ddlCatalogTopBlock.DataBind();

            if (ddlCatalogTopBlock.Items.FindByValue(Rees46Settings.CatalogTopBlock) != null)
                ddlCatalogTopBlock.SelectedValue = Rees46Settings.CatalogTopBlock;

            ddlCatalogBottomBlock.DataSource = list;
            ddlCatalogBottomBlock.DataBind();

            if (ddlCatalogBottomBlock.Items.FindByValue(Rees46Settings.CatalogBottomBlock) != null)
                ddlCatalogBottomBlock.SelectedValue = Rees46Settings.CatalogBottomBlock;

            ddlMainPage.DataSource = list.Where(x => x.Value != "also_bought" && x.Value != "similar").ToList();
            ddlMainPage.DataBind();

            if (ddlMainPage.Items.FindByValue(Rees46Settings.MainPageBlock) != null)
                ddlMainPage.SelectedValue = Rees46Settings.MainPageBlock;

            chkDisplayInShoppingCart.Checked = Rees46Settings.DisplayInShoppingCart;

            txtLimit.Text = Rees46Settings.Limit.ToString();

            dblRegisteredShop.Items.Clear();
            dblRegisteredShop.Items.Add(new ListItem() { Text = "Да", Value = "Да" });
            dblRegisteredShop.Items.Add(new ListItem() { Text = "Нет", Value = "Нет" });
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Rees46Settings.ShopKey = txtShopKey.Text.Trim();
            Rees46Settings.RelatedProduct = ddlRelatedProduct.SelectedValue;
            Rees46Settings.AlternativeProduct = ddlAlternativeProduct.SelectedValue;
            Rees46Settings.Limit = txtLimit.Text.TryParseInt(4);

            Rees46Settings.MainPageBlock = ddlMainPage.SelectedValue;
            Rees46Settings.CatalogTopBlock = ddlCatalogTopBlock.SelectedValue;
            Rees46Settings.CatalogBottomBlock = ddlCatalogBottomBlock.SelectedValue;
            Rees46Settings.DisplayInShoppingCart = chkDisplayInShoppingCart.Checked;
            Rees46Settings.PathFilePushSW = SettingsMain.SiteUrl.Trim('/') + "/modules/rees46/js/push_sw.js";

            FileReadWrite("write");
        }

        protected void btnMoveReg_Click(object sender, EventArgs e)
        {
            Rees46Settings.RegisteredShop = false;
            SetVisible();
            updPanel.Update();
        }

        protected void btnReg_Click(object sender, EventArgs e)
        {
            if (dblRegisteredShop.SelectedValue == "Да")
            {
                Rees46Settings.RegisteredShop = true;
            }
            else
            {
                lblErr.Text = "Процесс регистрации займет около минуты, пожалуйста ожидайте.";
                lblErr.Visible = true;
                if (!Helpers.ValidationHelper.IsValidEmail(txtEmail.Text))
                {
                    lblErr.Text = "Неправильный email";
                    lblErr.Visible = true;
                    updPanel.Update();
                    return;
                }

                if (txtPhone.Text.IsNullOrEmpty() && txtPhone.Text.Length == 11)
                {
                    lblErr.Text = "Неверно указан номер телефона";
                    lblErr.Visible = true;
                    updPanel.Update();
                    return;
                }

                if (txtFirstName.Text.IsNullOrEmpty() || txtlastName.Text.IsNullOrEmpty())
                {
                    lblErr.Text = "Неверно указано Имя/Фамилия";
                    lblErr.Visible = true;
                    updPanel.Update();
                    return;
                }
                var data = new RegCustomers();
                data.Email = txtEmail.Text.Trim();
                data.Phone = txtPhone.Text.Trim();
                data.First_Name = txtFirstName.Text.Trim();
                data.Last_Name = txtlastName.Text.Trim();
                var result = Rees46Service.Registration(data);
                Rees46Settings.RegisteredShop = result.status;
                lblErr.Text = result.status ? "Вы успешно зарегистрировались в системе Rees46!" : result.message;
                lblErr.Visible = !result.status;
                settingsRees46.Visible = result.status;
                autorizeRees46.Visible = !result.status;
            }
            SetVisible();
            updPanel.Update();
        }

        private void FileReadWrite(string type)
        {
            var filename = SettingsGeneral.AbsolutePath.Trim('\\') + "\\manifest.json";
            try
            {
                if (!File.Exists(filename) && type == "read")
                {
                    FileHelpers.CreateFile(filename);
                    var text = "{\n\t\"name\": \"REES46\",\n\t\"gcm_sender_id\": \"605730184710\"\n}";
                    using (var wr = new StreamWriter(filename))
                    {
                        wr.Write(text);
                    }
                }
                else if (type == "write")
                {
                    using (var wr = new StreamWriter(filename))
                    {
                        wr.Write(txtManifest.Text);
                    }
                }
                else
                {
                    using (var read = new StreamReader(filename))
                    {
                        txtManifest.Text = read.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        private void SetVisible()
        {
            settingsRees46.Visible = Rees46Settings.RegisteredShop;
            autorizeRees46.Visible = !Rees46Settings.RegisteredShop;
        }
    }
}