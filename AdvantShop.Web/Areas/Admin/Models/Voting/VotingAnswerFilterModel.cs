using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Voting
{
    public class VotingAnswerFilterModel : BaseFilterModel
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public int? CountVoice { get; set; }
        public int? CountVoiceFrom { get; set; }
        public int? CountVoiceTo { get; set; }
        public int SortOrder { get; set; }
        public int? SortOrderFrom { get; set; }
        public int? SortOrderTo { get; set; }
        public bool? IsVisible { get; set; }
        public string DateAddedFrom { get; set; }
        public string DateAddedTo { get; set; }
        public string DateModifyFrom { get; set; }
        public string DateModifyTo { get; set; }
        public int? ThemeId { get; set; }
    }
}
