//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Payment
{
    /// <summary>
    /// Summary description for Cash
    /// </summary>
    [PaymentKey("Cash")]
    public class Cash : PaymentMethod, IPaymentCurrencyHide
    {
        public override ProcessType ProcessType
        {
            get { return ProcessType.None; }
        }

        public override Dictionary<string,string> Parameters
        {
            get {return new Dictionary<string, string>();}
        }
    }
}