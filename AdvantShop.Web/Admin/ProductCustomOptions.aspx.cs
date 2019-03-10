//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;

namespace Admin
{
    public partial class ProductCustomOptions : AdvantShopAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            productCustomOption.ProductId = Request["productid"].TryParseInt();
        }
    }
}