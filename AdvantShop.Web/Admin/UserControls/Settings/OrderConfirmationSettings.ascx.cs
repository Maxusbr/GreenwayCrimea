using System;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Saas;
using System.Web.UI.WebControls;
using AdvantShop.Shipping;

namespace Admin.UserControls.Settings
{
    public partial class OrderConfirmationSettings : System.Web.UI.UserControl
    {
        public string ErrMessage = Resources.Resource.Admin_CommonSettings_InvalidOrderConfirmation;
        protected bool IsAdmin = CustomerContext.CurrentCustomer.IsAdmin;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlOutOfStockAction.Items.Add(new ListItem("Создавать заявку подзаказ", eOutOfStockAction.Preorder.ToString()));
                ddlOutOfStockAction.Items.Add(new ListItem("Создавать заказ", eOutOfStockAction.Order.ToString()));
                ddlOutOfStockAction.Items.Add(new ListItem("Создавать лид", eOutOfStockAction.Lead.ToString(), !SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveCrm));
                ddlOutOfStockAction.Items.Add(new ListItem("Разрешить добавлять в корзину", eOutOfStockAction.Cart.ToString()));

                LoadData();
            }
                
        }

        private void LoadData()
        {
            var crmEnable = (!SaasDataService.IsSaasEnabled ||
                             (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveCrm));

            cbAmountLimitation.Checked = SettingsCheckout.AmountLimitation;
            ddlOutOfStockAction.SelectedValue = SettingsCheckout.OutOfStockAction.ToString();

            cbDenyToByPreorderedProductsWithZerroAmount.Checked = SettingsCheckout.DenyToByPreorderedProductsWithZerroAmount;

            chkIsShowUserAgreementText.Checked = SettingsCheckout.IsShowUserAgreementText;
            txtUserAgreementText.Text = SettingsCheckout.UserAgreementText;
            cbProceedToPayment.Checked = SettingsCheckout.ProceedToPayment;
            var group = CustomerGroupService.GetCustomerGroup(CustomerGroupService.DefaultCustomerGroup);
            if (group != null)
                txtMinimalPriceForDefaultGroup.Text = group.MinimumOrderPrice.ToString("#0.00");

            txtMaximalPricecertificate.Text = SettingsCheckout.MaximalPriceCertificate.ToString("#0.00");
            txtMinimalPriceCertificate.Text = SettingsCheckout.MinimalPriceCertificate.ToString("#0.00");

            ckbEnableGiftCertificateService.Checked = SettingsCheckout.EnableGiftCertificateService;
            ckbDisplayPromoTextbox.Checked = SettingsCheckout.DisplayPromoTextbox;

            ckbBuyInOneClick.Checked = SettingsCheckout.BuyInOneClick;
            ckbBuyInOneClickDisableInCheckout.Checked = SettingsCheckout.BuyInOneClickDisableInCheckout;
            rbBuyInOneClickActionCreateOrder.Checked = SettingsCheckout.BuyInOneClickCreateOrder || !crmEnable;
            rbBuyInOneClickActionCreateLead.Checked = !SettingsCheckout.BuyInOneClickCreateOrder && crmEnable;
            lActionCreateLead.Visible = crmEnable;

            txtOneClickLinkText.Text = SettingsCheckout.BuyInOneClickLinkText;
            txtOneClickFirstText.Text = SettingsCheckout.BuyInOneClickFirstText;
            txtOneClickButtonText.Text = SettingsCheckout.BuyInOneClickButtonText;


            ddlOneClickDefaultShipping.Items.Add(new ListItem("----", "0"));
            foreach (var shipping in ShippingMethodService.GetAllShippingMethods(true)) {
                ddlOneClickDefaultShipping.Items.Add(new ListItem(shipping.Name, shipping.ShippingMethodId.ToString()));
            }

            if (ddlOneClickDefaultShipping.Items.FindByValue(SettingsCheckout.BuyInOneClickDefaultShippingMethod.ToString()) != null)
            {
                ddlOneClickDefaultShipping.SelectedValue = SettingsCheckout.BuyInOneClickDefaultShippingMethod.ToString();
            }

            ddlOneClickDefaultPayment.Items.Add(new ListItem("----", "0"));
            foreach (var payment in PaymentService.GetAllPaymentMethods(true))
            {
                ddlOneClickDefaultPayment.Items.Add(new ListItem(payment.Name, payment.PaymentMethodId.ToString()));
            }

            if (ddlOneClickDefaultPayment.Items.FindByValue(SettingsCheckout.BuyInOneClickDefaultPaymentMethod.ToString()) != null)
            {
                ddlOneClickDefaultPayment.SelectedValue = SettingsCheckout.BuyInOneClickDefaultPaymentMethod.ToString();
            }

            chkMultiplyGiftsCount.Checked = SettingsCheckout.MultiplyGiftsCount;

            chkShowStatusInfo.Checked = SettingsCheckout.PrintOrder_ShowStatusInfo;
            chkShowMap.Checked = SettingsCheckout.PrintOrder_ShowMap;

            //rbGoogleMap.Checked = SettingsCheckout.PrintOrder_MapType == "googlemap";
            rbYandexMap.Checked = SettingsCheckout.PrintOrder_MapType == "yandexmap";
            rbYandexMap.Enabled = false;

            txtOrderId.Text = (OrderService.GetLastDbOrderId() + 1).ToString();

            txtOrderNumberFormat.Text = SettingsCheckout.OrderNumberFormat;

            ckbManagerConfirmed.Checked = SettingsCheckout.ManagerConfirmed;
        }

        public bool SaveData()
        {
            bool isCorrect = true;

            SettingsCheckout.IsShowUserAgreementText = chkIsShowUserAgreementText.Checked;
            SettingsCheckout.UserAgreementText = txtUserAgreementText.Text;
            SettingsCheckout.BuyInOneClick = ckbBuyInOneClick.Checked;
            SettingsCheckout.BuyInOneClickDisableInCheckout = ckbBuyInOneClickDisableInCheckout.Checked;
            SettingsCheckout.BuyInOneClickFirstText = txtOneClickFirstText.Text;
            SettingsCheckout.BuyInOneClickButtonText = txtOneClickButtonText.Text;
            SettingsCheckout.BuyInOneClickCreateOrder = rbBuyInOneClickActionCreateOrder.Checked;
            SettingsCheckout.BuyInOneClickLinkText = txtOneClickLinkText.Text;

            SettingsCheckout.BuyInOneClickDefaultShippingMethod = ddlOneClickDefaultShipping.SelectedValue.TryParseInt();
            SettingsCheckout.BuyInOneClickDefaultPaymentMethod = ddlOneClickDefaultPayment.SelectedValue.TryParseInt();

            SettingsCheckout.AmountLimitation = cbAmountLimitation.Checked;
            SettingsCheckout.OutOfStockAction = ddlOutOfStockAction.SelectedValue.TryParseEnum<eOutOfStockAction>();
            SettingsCheckout.DenyToByPreorderedProductsWithZerroAmount = cbDenyToByPreorderedProductsWithZerroAmount.Checked;

            SettingsCheckout.ProceedToPayment = cbProceedToPayment.Checked;

            SettingsCheckout.MultiplyGiftsCount = chkMultiplyGiftsCount.Checked;

            SettingsCheckout.PrintOrder_ShowStatusInfo = chkShowStatusInfo.Checked;
            SettingsCheckout.PrintOrder_ShowMap = chkShowMap.Checked;
            SettingsCheckout.PrintOrder_MapType = rbYandexMap.Checked ? "googlemap" : "yandexmap";

            if (SettingsCheckout.EnableGiftCertificateService != ckbEnableGiftCertificateService.Checked)
            {
                var listMethod = PaymentService.GetAllPaymentMethods(true);
                var method = listMethod.FirstOrDefault(x => x is PaymentGiftCertificate);
                if (method == null && ckbEnableGiftCertificateService.Checked)
                {
                    PaymentService.AddPaymentMethod(new PaymentGiftCertificate
                    {
                        Enabled = true,
                        Name = Resources.Resource.Client_GiftCertificate,
                        Description = Resources.Resource.Payment_GiftCertificateDescription,
                        SortOrder = 0
                    });
                }
                else if (method != null && !ckbEnableGiftCertificateService.Checked)
                {
                    PaymentService.DeletePaymentMethod(method.PaymentMethodId);
                    SettingsDesign.GiftSertificateVisibility = false;
                }
            }

            SettingsCheckout.EnableGiftCertificateService = ckbEnableGiftCertificateService.Checked;
            SettingsCheckout.DisplayPromoTextbox = ckbDisplayPromoTextbox.Checked;

            float price = 0;
            if (float.TryParse(txtMaximalPricecertificate.Text, out price) && price >= 0)
            {
                SettingsCheckout.MaximalPriceCertificate = price;
            }
            else
            {
                ErrMessage += Resources.Resource.Admin_CommonSettings_CertificateMaxPriceError;
                isCorrect = false;
            }

            if (float.TryParse(txtMinimalPriceCertificate.Text, out price) && price >= 0)
            {
                SettingsCheckout.MinimalPriceCertificate = price;
            }
            else
            {
                ErrMessage += Resources.Resource.Admin_CommonSettings_CertificateMinPriceError;
                isCorrect = false;
            }


            if (float.TryParse(txtMinimalPriceForDefaultGroup.Text, out price) && price >= 0)
            {
                SettingsCheckout.MinimalOrderPriceForDefaultGroup = price;
            }
            else
            {
                ErrMessage += Resources.Resource.Admin_CommonSettings_OrderMinPriceError;
                isCorrect = false;
            }

            if (string.IsNullOrWhiteSpace(txtOrderNumberFormat.Text))
            {
                ErrMessage += Resources.Resource.Admin_CommonSettings_OrderNumberFormatError;
                isCorrect = false;
            }
            else
            {
                SettingsCheckout.OrderNumberFormat =
                    txtOrderNumberFormat.Text.Trim().Replace("\t", "").Replace("\r", "").Replace("\n", "");
            }
            var oldManagerConfirmed = SettingsCheckout.ManagerConfirmed;
            SettingsCheckout.ManagerConfirmed = ckbManagerConfirmed.Checked;
            if (SettingsCheckout.ManagerConfirmed && !oldManagerConfirmed)
            {
                OrderService.ManagerConfirmOrders(rbManagerConfirmedTrue.Checked);
            }

            LoadData();

            return isCorrect;
        }

        protected void btnChangeOrderNumber_Click(object sender, EventArgs e)
        {
            var newOrderId = txtOrderId.Text.TryParseInt() - 1;
            var lastOrderId = OrderService.GetLastDbOrderId();

            if (newOrderId < 0 || (lastOrderId != 0 && newOrderId < lastOrderId))
            {
                lblOrderSaveResult.Text = Resources.Resource.Admin_CommonSettings_ChangeOrderFail;
                lblOrderSaveResult.CssClass = "error-msg-text";
                return;
            }

            try
            {
                OrderService.ResetOrderID(newOrderId);

                lblOrderSaveResult.Text = Resources.Resource.Admin_CommonSettings_ChangeOrderSuccess;
                lblOrderSaveResult.CssClass = "success-msg-text";
            }
            catch (Exception ex)
            {
                lblOrderSaveResult.Text = Resources.Resource.Admin_CommonSettings_ChangeOrderFail;
                lblOrderSaveResult.CssClass = "error-msg-text";
                Debug.Log.Error(ex);
            }
        }


    }
}