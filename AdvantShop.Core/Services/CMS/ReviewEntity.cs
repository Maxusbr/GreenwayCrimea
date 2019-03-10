//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.CMS
{
    public class ReviewEntity
    {
        public int ReviewEntityId { get; set; }
        public EntityType Type { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string PhotoDescription { get; set; }
    }
}