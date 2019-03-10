using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Web.Admin.Models.Modules
{
    public enum ModulesPreFilterType
    {
        [Localize("Admin.Models.Modules.ModulesPreFilterType.None")]
        None,
        [Localize("Admin.Models.Orders.ModulesPreFilterType.New")]
        Bestsellers,
        [Localize("Admin.Models.Orders.ModulesPreFilterType.Bestsellers")]
        New,
        [Localize("Admin.Models.Orders.ModulesPreFilterType.Free")]
        Free,
        [Localize("Admin.Models.Orders.ModulesPreFilterType.Paid")]
        Paid
    }
}
