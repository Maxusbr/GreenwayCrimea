using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class YandexKassaControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(
                           new[] { txtShopID, txtScID, txtPassword }, null, new[] {txtShopID})
                           ? new Dictionary<string, string>
                               {
                                   {YandexKassaTemplate.ShopID, txtShopID.Text},
                                   {YandexKassaTemplate.ScID, txtScID.Text},
                                   {YandexKassaTemplate.YaPaymentType, ddlPaymentType.SelectedValue ?? ddlPaymentType.Items[0].Value},
                                   {YandexKassaTemplate.DemoMode, cbDemoMode.Checked.ToString()},
                                   {YandexKassaTemplate.Password, txtPassword.Text},

                                   {YandexKassaTemplate.SendReceiptData, cbSendReceiptData.Checked.ToString()},
                                   //{YandexKassaTemplate.VatType, ddlVatType.SelectedValue ?? ddlVatType.Items[0].Value},
                               }
                           : null;
            }
            set
            {
                txtShopID.Text = value.ElementOrDefault(YandexKassaTemplate.ShopID);
                txtScID.Text = value.ElementOrDefault(YandexKassaTemplate.ScID);
                cbDemoMode.Checked = value.ElementOrDefault(YandexKassaTemplate.DemoMode).TryParseBool();
                txtPassword.Text = value.ElementOrDefault(YandexKassaTemplate.Password);
                if (ddlPaymentType.Items.FindByValue(value.ElementOrDefault(YandexKassaTemplate.YaPaymentType)) != null)
                {
                    ddlPaymentType.SelectedValue = value.ElementOrDefault(YandexKassaTemplate.YaPaymentType);
                }

                cbSendReceiptData.Checked = value.ElementOrDefault(YandexKassaTemplate.SendReceiptData).TryParseBool();
                //if (ddlVatType.Items.FindByValue(value.ElementOrDefault(YandexKassaTemplate.VatType)) != null)
                //{
                //    ddlVatType.SelectedValue = value.ElementOrDefault(YandexKassaTemplate.VatType);
                //}
            }
        }
    }
}