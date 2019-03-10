using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class AvangardControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtShopId, txtShopPassword, txtAvSign })
                           ? new Dictionary<string, string>
                               {
                                   {AvangardTemplate.ShopId, txtShopId.Text},
                                   {AvangardTemplate.ShopPassword, txtShopPassword.Text},
                                   {AvangardTemplate.AvSign, txtAvSign.Text},
                               }
                           : null;
            }
            set
            {
                txtShopId.Text = value.ElementOrDefault(AvangardTemplate.ShopId);
                txtShopPassword.Text = value.ElementOrDefault(AvangardTemplate.ShopPassword);
                txtAvSign.Text = value.ElementOrDefault(AvangardTemplate.AvSign);
            }
        }
    }
}