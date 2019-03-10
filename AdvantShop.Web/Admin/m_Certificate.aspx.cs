//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Text.RegularExpressions;
using AdvantShop.Core.Controls;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using Resources;
using AdvantShop.Configuration;

namespace Admin
{
    public partial class m_Certificate : AdvantShopAdminPage
    {
        protected int CertificateId
        {
            get
            {
                int id = 0;
                int.TryParse(Request["id"], out id);
                return id;
            }
        }

        protected static GiftCertificate Certificate;

        private void MsgErr(bool clean)
        {
            if (clean)
            {
                lblError.Visible = false;
                lblError.Text = string.Empty;
            }
            else
            {
                lblError.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text = @"<br/>" + messageText;
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (CertificateId != 0)
            {
                SaveCertificate();
            }
            else
            {
                CommonHelper.RegCloseScript(this, string.Empty);
            }

            // Close window
            if (lblError.Visible == false)
            {
                CommonHelper.RegCloseScript(this, string.Empty);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            AdvantShop.Security.Secure.VerifyAccessLevel();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_m_News_Header));

            if (IsPostBack)
                return;

            if (CertificateId != 0)
            {
                btnOK.Text = Resource.Admin_m_News_Save;
                LoadCertificateById(CertificateId);
            }
            else
            {
                lblCertificateCode.Text = GiftCertificateService.GenerateCertificateCode();
            }
        }

        private bool DataValidation()
        {
            bool boolIsValidPast = true;

            if (string.IsNullOrEmpty(txtFromName.Text.Trim()) == false)
            {
                txtFromName.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtFromName.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            if (string.IsNullOrEmpty(txtToName.Text.Trim()) == false)
            {
                txtToName.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtToName.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            if (string.IsNullOrEmpty(txtSum.Text.Trim()) == false)
            {
                txtSum.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtSum.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            decimal sum = 0;
            if (Decimal.TryParse(txtSum.Text.Trim(), out sum))
            {
                txtSum.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtSum.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            if (string.IsNullOrEmpty(txtEmail.Text.Trim()) == false)
            {
                txtEmail.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtEmail.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            var r = new Regex("\\w+([-+.\']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", RegexOptions.Multiline);
            if (r.IsMatch(txtEmail.Text))
            {
                txtEmail.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtEmail.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            if (!boolIsValidPast)
            {
                MsgErr(Resource.Admin_m_Certificate_WrongFormat);
            }
            else
            {
                MsgErr(false);
            }

            return boolIsValidPast;
        }

        protected void SaveCertificate()
        {
            if (!DataValidation())
                return;

            try
            {
                Certificate.CertificateId = CertificateId;
                Certificate.CertificateCode = lblCertificateCode.Text;
                Certificate.CertificateMessage = txtMessage.Text;
                Certificate.FromName = txtFromName.Text;
                Certificate.ToName = txtToName.Text;
                Certificate.Used = chkUsed.Checked;
                Certificate.Enable = chkEnable.Checked;
                Certificate.Sum = Convert.ToSingle(txtSum.Text);
                Certificate.CertificateMessage = txtMessage.Text;
                Certificate.ToEmail = txtEmail.Text;

                GiftCertificateService.UpdateCertificateById(Certificate);

                OrderService.PayOrder(Certificate.OrderId, chkPaid.Checked);
            }
            catch (Exception ex)
            {
                MsgErr(ex.Message + " SaveSertificate error");
                Debug.Log.Error(ex);
            }
        }
    
        protected void LoadCertificateById(int certificateId)
        {
            Certificate = GiftCertificateService.GetCertificateById(certificateId);
            if (Certificate == null)
            {
                MsgErr("Certificate with this ID does not exist");
                return;
            }

            lblCertificateCode.Text = Certificate.CertificateCode;
            txtFromName.Text = Certificate.FromName;
            txtToName.Text = Certificate.ToName;
            txtSum.Text = Certificate.Sum.ToString("#0.00");
            chkUsed.Checked = Certificate.Used;
            chkEnable.Checked = Certificate.Enable;
            txtMessage.Text = Certificate.CertificateMessage;
            txtEmail.Text = Certificate.ToEmail;
            chkPaid.Checked = OrderService.IsPaidOrder(Certificate.OrderId);
        }
    }
}