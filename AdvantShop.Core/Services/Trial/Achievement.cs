//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------


namespace AdvantShop.Trial
{
    public class Achievement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Instructions { get; set; }
        public string Description { get; set; }
        public string Recommendations { get; set; }
        public int SortOrder { get; set; }
        public bool Complete { get; set; }
        public int Points { get; set; }
    }
}