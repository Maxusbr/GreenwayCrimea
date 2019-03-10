namespace AdvantShop.Web.Admin.ViewModels.Account
{
    public enum EForgotPasswordView
    {
        ForgotPassword,
        EmailSent,
        PasswordRecovery,
        RecoveryError,
        PasswordChanged
    }

    public class ForgotPasswordViewModel
    {
        public EForgotPasswordView View { get; set; }
        public string Email { get; set; }
        public string Hash { get; set; }
        public bool FirstVisit { get; set; }
    }
}
