using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class ChronoPayControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] {  txtProductId,
                    txtProductName,
                    txtSharedSecret },null,null)
                           ? new Dictionary<string, string>
                               {
                                   {ChronoPayTemplate.ProductId, txtProductId.Text},
                                   {ChronoPayTemplate.ProductName, txtProductName.Text},
                                   {ChronoPayTemplate.SharedSecret, txtSharedSecret.Text}
                               }
                           : null;
            }
            set
            {
                txtProductId.Text = value.ElementOrDefault(ChronoPayTemplate.ProductId);
                txtProductName.Text = value.ElementOrDefault(ChronoPayTemplate.ProductName);
                txtSharedSecret.Text = value.ElementOrDefault(ChronoPayTemplate.SharedSecret);
            }
        }
   
    }
}