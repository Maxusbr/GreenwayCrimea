using System.Collections.Generic;
using AdvantShop.Catalog;

namespace AdvantShop.App.Landing.Domain
{
    public enum LpType
    {
        None,
        OneProduct,
        FewProducts,
        Universal
    }

    public enum LpGoal
    {
        No,
        Contact,
        Order,
    }

    /// <summary>
    /// Предустановленные настройки
    /// </summary>
    public class LpConfiguration
    {
        public LpType Type { get; set; }
        public LpGoal Goal { get; set; }
        public List<Product> Products { get; set; }
    }
}
