namespace AdvantShop.Web.Admin.Models
{
    public class SelectItemModel
    {
        public SelectItemModel() { }

        public SelectItemModel(string label, string value)
        {
            this.label = label;
            this.value = value;
        }

        public string label { get; set; }
        public string value { get; set; }
    }
}
