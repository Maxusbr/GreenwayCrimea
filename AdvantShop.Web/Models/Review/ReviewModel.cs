using System.Collections.Generic;
using System.Web;

namespace AdvantShop.Models.Review
{
    public partial class ReviewModel : BaseModel
    {
        public int EntityId { get; set; }
        public int EntityType { get; set; }
        public int ParentId { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Agreement { get; set; }
        public List<HttpPostedFileBase> File { get; set; }
    }
}