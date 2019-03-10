using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class CloudPaymentsControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtPublicId, txtApiSecret })
                    ? new Dictionary<string, string>
                    {
                        {CloudPaymentsTemplate.PublicId, txtPublicId.Text},
                        {CloudPaymentsTemplate.APISecret, txtApiSecret.Text},
                        {CloudPaymentsTemplate.Site, ddlSite.SelectedValue},
                        {CloudPaymentsTemplate.SendReceiptData, cbSendReceiptData.Checked.ToString()},
                        {CloudPaymentsTemplate.TaxationSystem, ddlTaxationSystem.SelectedValue }
                    }
                    : null;
            }
            set
            {
                txtPublicId.Text = value.ElementOrDefault(CloudPaymentsTemplate.PublicId);
                txtApiSecret.Text = value.ElementOrDefault(CloudPaymentsTemplate.APISecret);
                ddlSite.SelectedValue = value.ElementOrDefault(CloudPaymentsTemplate.Site);
                cbSendReceiptData.Checked = value.ElementOrDefault(CloudPaymentsTemplate.SendReceiptData).TryParseBool();
                ddlTaxationSystem.SelectedValue = value.ElementOrDefault(CloudPaymentsTemplate.TaxationSystem).TryParseInt().ToString();
            }
        }
    }
}