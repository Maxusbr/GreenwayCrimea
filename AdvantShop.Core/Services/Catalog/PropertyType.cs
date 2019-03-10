
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Catalog
{
    /// <summary>
    /// Property type of view on catalog pages
    /// </summary>
    public enum PropertyType
    {
        [Localize("Core.Catalog.PropertyType.Checkbox")]
        Checkbox = 1,

        [Localize("Core.Catalog.PropertyType.Selectbox")]
        Selectbox = 2,

        [Localize("Core.Catalog.PropertyType.Range")]
        Range = 3
    }
}