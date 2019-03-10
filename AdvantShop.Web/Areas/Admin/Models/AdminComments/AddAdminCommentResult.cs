namespace AdvantShop.Web.Admin.Models.AdminComments
{
    public class AddAdminCommentResult
    {
        public bool Result { get; set; }
        public string Error { get; set; }
        public AdminCommentModel Comment { get; set; }
    }
}
