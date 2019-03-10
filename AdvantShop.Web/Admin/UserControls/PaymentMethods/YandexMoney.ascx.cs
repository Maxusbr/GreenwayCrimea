using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class YandexMoneyControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] {txtShopID, txtScID}, null, new[] {txtShopID})
                    ? new Dictionary<string, string>
                    {
                        {YandexMoneyTemplate.ShopId, txtShopID.Text},
                        {YandexMoneyTemplate.ScId, txtScID.Text},
                        {
                            YandexMoneyTemplate.YaPaymentType,
                            ddlPaymentType.SelectedValue ?? ddlPaymentType.Items[0].Value
                        }
                    }
                    : null;
            }
            set
            {
                txtShopID.Text = value.ElementOrDefault(YandexMoneyTemplate.ShopId);
                txtScID.Text = value.ElementOrDefault(YandexMoneyTemplate.ScId);

                if (ddlPaymentType.Items.FindByValue(value.ElementOrDefault(YandexMoneyTemplate.YaPaymentType)) != null)
                {
                    ddlPaymentType.SelectedValue = value.ElementOrDefault(YandexMoneyTemplate.YaPaymentType);
                }
            }
        }
    }
}