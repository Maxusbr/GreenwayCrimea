using System;
using System.IO;
using System.Text;
using System.Web.UI;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Helpers;
using AdvantShop.Module.BuyInTime.Domain;
using AdvantShop.FilePath;

namespace AdvantShop.Module.BuyInTime
{
    public partial class BuyInTimeModuleAddEdit : Page
    {
        private const string ModuleName = "BuyInTime";
        protected int Id;
        private BuyInTimeProductModel _action;


        protected void Page_Load(object sender, EventArgs e)
        {
            Id = Request["Id"].TryParseInt();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            if (Id != 0)
            {
                LoadAction();
            }
            else
            {
                DateTime now = DateTime.Now;
                txtDateStart.Text = now.ToString();
                txtDateExpired.Text = now.Date.AddDays(5).ToString();

                ckeActionText.Text = ModuleSettingsProvider.GetSettingValue<string>("BuyInTimeDefaultActionText", ModuleName);
                ckeMobileActionText.Text = ModuleSettingsProvider.GetSettingValue<string>("BuyInTimeDefaultActionText", ModuleName);
                chkShowInMobile.Checked = true;
                txtSortOrder.Text = "0";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Id != 0)
                SaveAction();
            else
                CreateAction();

            if (!lblMessage.Visible)
            {
                var jScript = new StringBuilder();
                jScript.Append("<script type=\'text/javascript\' language=\'javascript\'> ");
                jScript.Append("window.opener.location.reload(true); ");
                jScript.Append("self.close();");
                jScript.Append("</script>");
                Type csType = this.GetType();
                ClientScriptManager clScriptMng = this.ClientScript;
                clScriptMng.RegisterClientScriptBlock(csType, "Close_window", jScript.ToString());
            }
        }

        //protected void ddlShowMode_OnChanged(object sender, EventArgs e)
        //{
        //    if (ddlShowMode.SelectedValue == "1")
        //    {
        //        ckeActionText.Text = ModuleSettingsProvider.GetSettingValue<string>("BuyInTimeDefaultActionText", ModuleName);
        //    }
        //    else if (ddlShowMode.SelectedValue == "2")
        //    {
        //        ckeActionText.Text = ModuleSettingsProvider.GetSettingValue<string>("BuyInTimeDefaultActionText", ModuleName);
        //    }
        //}

        protected void DeletePicture_Click(object sender, EventArgs e)
        {
            _action = BuyInTimeService.Get(Id);

            string filepath = BuyInTimeService.PicturePath + _action.Picture;
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }

            BuyInTimeService.UpdatePicture(_action.Id, null);

            liPictureName.Text = null;
            pnlPicture.Visible = false;
            //fuPucture.Visible = true;
            fuPuctureDiv.Attributes["style"] = "display:block";
        }

        #region Private methods

        private void LoadAction()
        {
            _action = BuyInTimeService.Get(Id);

            if (_action != null)
            {
                var product = ProductService.GetProduct(_action.ProductId);

                txtProductArtNo.Text = product != null ? product.ArtNo : string.Empty;
                txtDiscount.Text = _action.DiscountInTime.ToString();
                txtDateStart.Text = _action.DateStart.ToString();
                txtDateExpired.Text = _action.DateExpired.ToString();
                ddlShowMode.SelectedValue = _action.ShowMode.ToString();
                ckeActionText.Text = _action.ActionText;
                chkIsRepeat.Checked = _action.IsRepeat;
                txtDaysRepeat.Text = _action.DaysRepeat.ToString();
                txtSortOrder.Text = _action.SortOrder.ToString();

                chkShowInMobile.Checked = _action.ShowInMobile;
                ckeMobileActionText.Text = _action.MobileActionText;

                if (_action.Picture.IsNotEmpty())
                {
                    pnlPicture.Visible = true;
                    //fuPucture.Visible = false;
                    fuPuctureDiv.Attributes["style"] = "display:none";

                    liPictureName.Text = _action.Picture;
                    liPicture.Text = string.Format("<img src='../../pictures/modules/{0}/{1}?rnd={2}'/>", ModuleName.ToLower(), _action.Picture, (new Random()).Next(1000));
                }
                else
                {
                    pnlPicture.Visible = false;
                    //fuPucture.Visible = true;
                    fuPuctureDiv.Attributes["style"] = "display:block";
                }
            }
        }

        private void SaveAction()
        {
            if (!IsValidData())
                return;

            var product = ProductService.GetProduct(txtProductArtNo.Text);
            if (product == null)
            {
                var offer = OfferService.GetOffer(txtProductArtNo.Text);
                if (offer != null)
                    product = ProductService.GetProduct(offer.ProductId);
            }

            try
            {
                _action = BuyInTimeService.Get(Id);

                _action.ProductId = product.ProductId;
                _action.DiscountInTime = txtDiscount.Text.TryParseFloat();
                _action.DateStart = txtDateStart.Text.TryParseDateTime();
                _action.DateExpired = txtDateExpired.Text.TryParseDateTime();
                _action.ShowMode = ddlShowMode.SelectedValue.TryParseInt();
                _action.ActionText = ckeActionText.Text;
                _action.IsRepeat = chkIsRepeat.Checked;
                _action.DaysRepeat = txtDaysRepeat.Text.TryParseInt(1);
                _action.SortOrder = txtSortOrder.Text.TryParseInt(0);
                _action.ShowInMobile = chkShowInMobile.Checked;
                _action.MobileActionText = ckeMobileActionText.Text;
                //_action.Picture = string.IsNullOrEmpty(liPicture.Text) ? product.Photo : liPictureName.Text;

                BuyInTimeService.Update(_action);

                CategoryService.ClearCategoryCache();

                txtProductArtNo.Text = txtDiscount.Text = txtDateExpired.Text = txtDateStart.Text = string.Empty;

                UploadPicture(_action, liPictureName.Text);
            }
            catch (Exception ex)
            {
                MsgErr("cant add " + ex);
            }

            txtProductArtNo.Text = string.Empty;
        }

        private void CreateAction()
        {
            if (!IsValidData())
                return;

            var product = ProductService.GetProduct(txtProductArtNo.Text);
            if (product == null)
            {
                var offer = OfferService.GetOffer(txtProductArtNo.Text);
                if (offer != null)
                    product = ProductService.GetProduct(offer.ProductId);
            }

            try
            {
                var action = new BuyInTimeProductModel
                {
                    ProductId = product.ProductId,
                    DiscountInTime = txtDiscount.Text.TryParseFloat(),
                    DateStart = txtDateStart.Text.TryParseDateTime(),
                    DateExpired = txtDateExpired.Text.TryParseDateTime(),
                    ShowMode = ddlShowMode.SelectedValue.TryParseInt(),
                    ActionText = ckeActionText.Text,
                    IsRepeat = chkIsRepeat.Checked,
                    DaysRepeat = txtDaysRepeat.Text.TryParseInt(1),
                    SortOrder = txtSortOrder.Text.TryParseInt(0),
                    ShowInMobile = chkShowInMobile.Checked,
                    MobileActionText = ckeMobileActionText.Text,
                };

                BuyInTimeService.Add(action);

                CategoryService.ClearCategoryCache();

                txtProductArtNo.Text = txtDiscount.Text = txtDateExpired.Text = txtDateStart.Text = string.Empty;

                UploadPicture(action, product.Photo);
            }
            catch (Exception ex)
            {
                MsgErr("cant add: " + ex);
            }

            txtProductArtNo.Text = string.Empty;
        }

        private bool IsValidData()
        {
            bool valid = true;

            lblMessage.Visible = false;

            if (txtProductArtNo.Text.IsNullOrEmpty() || txtDiscount.Text.IsNullOrEmpty() ||
                txtDateExpired.Text.IsNullOrEmpty() || txtDateStart.Text.IsNullOrEmpty())
            {
                MsgErr("error");
                valid = false;
            }

            int productId = ProductService.GetProductId(txtProductArtNo.Text);
            if (productId == 0)
            {
                var offer = OfferService.GetOffer(txtProductArtNo.Text);
                if (offer != null)
                    productId = offer.ProductId;
            }

            if (productId == 0)
            {
                MsgErr("artno not exist");
                valid = false;
            }

            return valid;
        }

        private void UploadPicture(BuyInTimeProductModel action, string photoName)
        {
            if (!Directory.Exists(BuyInTimeService.PicturePath))
                Directory.CreateDirectory(BuyInTimeService.PicturePath);

            if (fuPucture.HasFile)
            {
                if (!FileHelpers.CheckFileExtension(fuPucture.FileName, EAdvantShopFileTypes.Image))
                {
                    MsgErr("Неправильный формат");
                    return;
                }

                var fileName = action.Id + Path.GetExtension(fuPucture.FileName);
                fuPucture.SaveAs(BuyInTimeService.PicturePath + fileName);

                BuyInTimeService.UpdatePicture(action.Id, fileName);
            }
            else if (!string.IsNullOrEmpty(photoName) && action.Picture != photoName)
            {
                var path = FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Middle, photoName);
                if (File.Exists(path))
                {
                    var fileName = action.Id + Path.GetExtension(photoName);

                    File.Delete(BuyInTimeService.PicturePath + fileName);
                    File.Copy(path, BuyInTimeService.PicturePath + fileName);

                    BuyInTimeService.UpdatePicture(action.Id, fileName);
                }
            }
        }

        private void MsgErr(string msg)
        {
            lblMessage.Visible = true;
            lblMessage.Text = msg;
        }

        #endregion
    }
}