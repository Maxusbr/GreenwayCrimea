namespace AdvantShop.Web.Admin.Models.Grastin
{
    public class OrderActionsModel
    {
        public int OrderId { get; set; }
        public bool ShowSendOrderForGrasting { get; set; }
        public bool ShowSendOrderForRussianPost { get; set; }
        public bool ShowSendOrderForBoxberry { get; set; }
        public bool ShowSendOrderForHermes { get; set; }
        public bool ShowSendRequestForIntake { get; set; }
        public bool ShowSendRequestForAct { get; set; }
        public bool ShowSendRequestForMark { get; set; }
    }
}
