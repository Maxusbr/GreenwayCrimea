namespace AdvantShop.Core.Services.FullSearch
{
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public class SearchField : System.Attribute
    {
        public string[] CombinedSearchFields;
        public SearchField(params string[] values)
        {

            this.CombinedSearchFields = values;
        }
    }
}
