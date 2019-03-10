using System.Collections.Generic;
using AdvantShop.Core.Controls;

namespace Admin.UserControls.PaymentMethods
{
    public partial class Cash : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get { return new Dictionary<string, string>(); }
            set { }
        }
    }
}