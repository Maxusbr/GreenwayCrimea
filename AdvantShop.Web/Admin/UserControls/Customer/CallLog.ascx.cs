using System;
using System.Collections.Generic;
using AdvantShop.Core.Services.IPTelephony;

namespace AdvantShop.Admin.UserControls.Customer
{
    public partial class CallLog : System.Web.UI.UserControl
    {
        public List<Call> Calls;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}