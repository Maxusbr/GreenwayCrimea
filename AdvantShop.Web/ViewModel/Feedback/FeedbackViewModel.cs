using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.ViewModel.Feedback
{
    public enum FeedbackType
    {
        [Localize("Feedback.FeedbackType.Question")]
        Question,
        //[Localize("Feedback.FeedbackType.Thanks")]
        //Thanks,
        [Localize("Feedback.FeedbackType.Offer")]
        Offer,
        //[Localize("Feedback.FeedbackType.Abuse")]
        //Abuse
    }

    public class FeedbackViewModel
    {
        public string Message { get; set; }

        public string OrderNumber { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        
        public FeedbackType MessageType { get; set; }

        public string CaptchaCode { get; set; }

        public string CaptchaSource { get; set; }

        public bool Agree { get; set; }

		public string PageName { get; set; }
    }
}