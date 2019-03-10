//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Web.UI;
using AdvantShop.Core.Controls;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IProductAdminControls : IModule
    {
        IList<ProductAdminControl> GetProductAdminControls(TemplateControl page);
    }
}
