using System.Collections.Generic;
using AdvantShop.Core.Controls;

namespace Admin.UserControls.PaymentMethods
{
    public partial class CyberPlatControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return base.Parameters;
            }
            set
            {
                base.Parameters = value;
            }
        }
    
    }
}