using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class InterkassaControl2 : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtShopId }, null, null)
                           ? new Dictionary<string, string>
                               {
                                   {Interkassa2Template.ShopId, txtShopId.Text},
                                   {Interkassa2Template.IsCheckSign, chkCheckSign.Checked.ToString()},
                                   {Interkassa2Template.SecretKey, txtSecretKey.Text}
                               }
                           : null;
            }
            set
            {
                txtShopId.Text = value.ElementOrDefault(Interkassa2Template.ShopId);
                chkCheckSign.Checked = value.ElementOrDefault(Interkassa2Template.IsCheckSign).TryParseBool();
                txtSecretKey.Text = value.ElementOrDefault(Interkassa2Template.SecretKey);
            }
        }

    }
}