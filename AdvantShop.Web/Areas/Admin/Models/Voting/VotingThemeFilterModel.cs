using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Voting
{
    public class VotingThemeFilterModel : BaseFilterModel
    {
        public int? ID { get; set; }
        public int? PsyID { get; set; }
        public string Name { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsHaveNullVoice { get; set; }
        public bool? IsClose { get; set; }
        public string DateAddedFrom { get; set; }
        public string DateAddedTo { get; set; }
        public string DateModifyFrom { get; set; }
        public string DateModifyTo { get; set; }
        public string CountAnswersFrom { get; set; }
        public string CountAnswersTo { get; set; }
    }
}
