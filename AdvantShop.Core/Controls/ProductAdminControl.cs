using System.Web.UI;

namespace AdvantShop.Core.Controls
{
    public class ProductAdminControl
    {
        public string Name { get; private set; }
        public ProductAdminTabContent Control { get; private set; }

        public ProductAdminControl(string name, string filePath, TemplateControl page)
        {
            Name = name;
            Control = (ProductAdminTabContent)page.LoadControl(filePath);
        }
    }

    public class ProductAdminTabContent : UserControl
    {
        public virtual void Save(int productId) { }
    }
}
