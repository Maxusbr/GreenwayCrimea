namespace AdvantShop.Models.User
{
    public class ForgotPasswordModel
    {
        public string View { get; set; }

        public string Email { get; set; }

        public string RecoveryCode { get; set; }
    }
}