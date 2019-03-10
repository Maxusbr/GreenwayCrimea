using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Payment;
using AdvantShop.Repository;
using AdvantShop.Repository.Currencies;
using Resources;

namespace Admin.UserControls.PaymentMethods
{
    public partial class MasterControl : UserControl
    {
        private bool _valid = false;
        public int PaymentMethodID
        {
            get { return (int)(ViewState["MethodID"] ?? 0); }
            set { ViewState["MethodID"] = value; }
        }
        private ParametersControl _ucSpecific;
        public PaymentMethod Method { get; set; }
        public string PaymentType { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Visible)
                return;

            //Dynamic user control load
            var fileName = string.Format("{0}.ascx", PaymentType);

            try
            {
                _ucSpecific = (ParametersControl) LoadControl(fileName);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
                
            if (_ucSpecific == null) return;

            _ucSpecific.ID = "ucSpecific";
            pnlSpecific.Controls.Add(_ucSpecific);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Visible)
                return;

            var currencies = CurrencyService.GetAllCurrencies();
            var currentCurrency = CurrencyService.CurrentCurrency;

            foreach (var currency in currencies.OrderBy(x => x.CurrencyId != currentCurrency.CurrencyId))
            {
                ddlPayCurrency.Items.Add(new ListItem(currency.Name, currency.CurrencyId.ToString()));
            }

            LoadFormData(true);
        }

        private void LoadFormData(bool loadParameters)
        {
            if (Method == null)
                return;

            PaymentMethodID = Method.PaymentMethodId;
            btnDelete.Attributes["data-confirm"] = string.Format(Resource.Admin_PaymentMethod_DeleteConfirm, Method.Name);
            txtName.Text = Method.Name;
            txtDescription.Text = Method.Description;
            txtSortOrder.Text = Method.SortOrder.ToString();
            chkEnabled.Checked = Method.Enabled;
            if (chkEnabled.Checked)
            {
                chkEnabled.Text = Resource.Admin_Checkbox_Enabled;
                chkEnabled.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                chkEnabled.Text = Resource.Admin_Checkbox_Disabled;
                chkEnabled.ForeColor = System.Drawing.Color.Red;
            }

            if (Method.IconFileName != null && File.Exists(FoldersHelper.GetPathAbsolut(FolderType.PaymentLogo, Method.IconFileName.PhotoName)))
            {
                imgIcon.ImageUrl = FoldersHelper.GetPath(FolderType.PaymentLogo, Method.IconFileName.PhotoName, false);
                imgIcon.Visible = true;
            }
            else
            {
                imgIcon.Visible = false;
            }

            if (_ucSpecific != null && loadParameters)
                _ucSpecific.Parameters = Method.Parameters;

            ddlExtrachargeType.SelectedValue = ((int)Method.ExtrachargeType).ToString();
            txtExtracharge.Text = Method.Extracharge.ToString("F2");


            litReturnUrl.Text = Method.SuccessUrl;
            litFailUrl.Text = Method.FailUrl;
            litCancelUrl.Text = Method.CancelUrl;
            litNotificationUrl.Text = Method.NotificationUrl;
            trReturnUrl.Visible = Method.ShowUrls.HasFlag(UrlStatus.ReturnUrl);
            trFailUrl.Visible = Method.ShowUrls.HasFlag(UrlStatus.FailUrl);
            trCancelUrl.Visible = Method.ShowUrls.HasFlag(UrlStatus.CancelUrl);
            trNotificationUrl.Visible = Method.ShowUrls.HasFlag(UrlStatus.NotificationUrl);

            if (ddlPayCurrency.Items.FindByValue(Method.CurrencyId.ToString()) != null)
                ddlPayCurrency.SelectedValue = Method.CurrencyId.ToString();

            paymentCurrency.Visible = !(Method is IPaymentCurrencyHide);

            CacheManager.RemoveByPattern(CacheNames.PaymentOptions);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Visible) return;

            if (!ValidateFormData()) return;

            if (_ucSpecific != null)
            {
                _ucSpecific.SaveOtherData();
            }

            var parameters = _ucSpecific == null ? null : _ucSpecific.Parameters;
            if (parameters == null) 
                return;

            var method = PaymentMethod.Create(PaymentType);
            if (method == null)
                return;

            method.PaymentMethodId = PaymentMethodID;
            method.Name = txtName.Text;
            method.Description = txtDescription.Text;
            method.SortOrder = txtSortOrder.Text.TryParseInt();
            method.Enabled = chkEnabled.Checked && (_ucSpecific == null || _ucSpecific.Parameters != null);

            method.Extracharge = txtExtracharge.Text.TryParseFloat();
            method.ExtrachargeType = (ExtrachargeType)ddlExtrachargeType.SelectedValue.TryParseInt();

            method.CurrencyId = ddlPayCurrency.SelectedValue.TryParseInt();

            method.Parameters = parameters;

            PaymentService.UpdatePaymentMethod(method);
            Method = PaymentService.GetPaymentMethod(method.PaymentMethodId);
            LoadFormData(true);
            OnSaved(new SavedEventArgs { Enabled = method.Enabled, Name = method.Name });
        }

        private void MsgErr(Label lbl, string message)
        {
            if (lbl == null) { _valid = false; return; } 
            lbl.Visible = true;
            lbl.Text = message;
            _valid = false;
        }

        protected bool ValidateFormData()
        {
            _valid = true;
            new[] { txtName, txtSortOrder }
                .Where(textBox => string.IsNullOrEmpty(textBox.Text))
                .ForEach(textBox => MsgErr((Label)FindControl("msg" + textBox.ID.Substring(3)), Resource.Admin_Messages_EnterValue));
            if (!txtSortOrder.Text.IsInt())
                MsgErr(msgSortOrder, Resource.Admin_Messages_IsInt);
            // if (_ucSpecific != null && _ucSpecific.Parameters == null)
            //   _valid = false;
            return _valid;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var payment = PaymentService.GetPaymentMethod(PaymentMethodID);
            if (payment != null && payment is PaymentGiftCertificate && SettingsCheckout.EnableGiftCertificateService)
            {
                OnErr(new ErrorEventArgs { Message = Resource.Admin_PaymentMethods_DisableGiftCertificateService });
            }

            PaymentService.DeletePaymentMethod(PaymentMethodID);
            if (PaymentService.GetPaymentMethod(PaymentMethodID) == null)
                Response.Redirect("~/Admin/PaymentMethod.aspx", true);
            else
            {
                OnErr(new ErrorEventArgs { Message = Resources.Resource.Admin_PaymentMethods_OrdersExist });
            }
        }

        public event Action<object, SavedEventArgs> Saved;
        public event Action<object, ErrorEventArgs> Err;

        public void OnErr(ErrorEventArgs args)
        {
            Err(this, args);
        }

        public void OnSaved(SavedEventArgs args)
        {
            Saved(this, args);
        }

        public class SavedEventArgs : EventArgs
        {
            public bool Enabled { get; set; }
            public string Name { get; set; }
        }
        public class ErrorEventArgs : EventArgs
        {
            public string Message { get; set; }
        }

        protected void btnAddCountry_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCountry.Text))
            {
                var country = CountryService.GetCountryByName(txtCountry.Text);
                if (country != null)
                {
                    if (!ShippingPaymentGeoMaping.IsExistPaymentCountry(PaymentMethodID, country.CountryId))
                    {
                        ShippingPaymentGeoMaping.AddPaymentCountry(PaymentMethodID, country.CountryId);
                    }
                    txtCountry.Text = string.Empty;
                }
            }
        }

        protected void repeaterCountry_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DelCountry")
            {
                var id = SQLDataHelper.GetInt(e.CommandArgument);
                ShippingPaymentGeoMaping.DeletePaymentCountry(PaymentMethodID, id);
            }
        }

        protected void btnAddCity_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCity.Text)) return;
            var city = CityService.GetCityByName(txtCity.Text);
            if (city == null) return;
            if (!ShippingPaymentGeoMaping.IsExistPaymentCity(PaymentMethodID, city.CityId))
            {
                ShippingPaymentGeoMaping.AddPaymentCity(PaymentMethodID, city.CityId);
            }
            txtCity.Text = string.Empty;
        }

        protected void repeaterCity_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DelCity")
            {
                var id = SQLDataHelper.GetInt(e.CommandArgument);
                ShippingPaymentGeoMaping.DeletePaymentCity(PaymentMethodID, id);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (!fuIcon.HasFile) return;
            if (!FileHelpers.CheckFileExtension(fuIcon.FileName, EAdvantShopFileTypes.Image))
            {
                OnErr(new ErrorEventArgs { Message = Resource.Admin_ErrorMessage_WrongImageExtension });
                return;
            }
            PhotoService.DeletePhotos(PaymentMethodID, PhotoType.Payment);
            var tempName = PhotoService.AddPhoto(new Photo(0, PaymentMethodID, PhotoType.Payment) { OriginName = fuIcon.FileName });
            if (string.IsNullOrWhiteSpace(tempName)) return;
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(fuIcon.FileContent))
            {
                FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.PaymentLogo, tempName), SettingsPictureSize.PaymentIconWidth, SettingsPictureSize.PaymentIconHeight, image);
            }
            imgIcon.ImageUrl = FoldersHelper.GetPath(FolderType.PaymentLogo, tempName, true);
            imgIcon.Visible = true;
        }

        protected void btnDeleteIcon_Click(object sender, EventArgs e)
        {
            PhotoService.DeletePhotos(PaymentMethodID, PhotoType.Payment);
            imgIcon.Visible = false;
            btnDeleteIcon.Visible = false;
        }
    }
}
