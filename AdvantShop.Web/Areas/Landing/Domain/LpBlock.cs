
namespace AdvantShop.App.Landing.Domain
{
    public class LpBlock
    {
        public int Id { get; set; }
        public int LandingId { get; set; }
        public string Name { get; set; }
        public string ContentHtml { get; set; }
        public string Type { get; set; }
        public string Settings { get; set; }
        public int SortOrder { get; set; }
        public bool Enabled { get; set; }

        public dynamic MappedSettings;
    }

    public class LpSubBlock
    {
        public int Id { get; set; }
        public int LandingBlockId { get; set; }
        public string Name { get; set; }
        public string ContentHtml { get; set; }
        public string Type { get; set; }
        public string Settings { get; set; }
        public int SortOrder { get; set; }

        public dynamic MappedSettings;
    }
}