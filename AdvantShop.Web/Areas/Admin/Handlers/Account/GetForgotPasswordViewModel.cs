using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.ViewModels.Account;

namespace AdvantShop.Web.Admin.Handlers.Account
{
    public class GetForgotPasswordViewModel
    {
        private readonly string _email;
        private readonly string _hash;

        public GetForgotPasswordViewModel(string email, string hash)
        {
            _email = email;
            _hash = hash;
        }

        public ForgotPasswordViewModel Execute()
        {
            var model = new ForgotPasswordViewModel
            {
                View = EForgotPasswordView.ForgotPassword,
                Email = _email,
                Hash = _hash,
            };

            if (_email.IsNotEmpty() && _hash.IsNotEmpty())
            {
                var customer = CustomerService.GetCustomerByEmail(_email);
                model.View = customer != null && (customer.IsAdmin || customer.IsModerator)
                    && ValidationHelper.DeleteSigns(SecurityHelper.GetPasswordHash(customer.Password)).ToLower() == _hash.ToLower()
                    ? EForgotPasswordView.PasswordRecovery
                    : EForgotPasswordView.RecoveryError;
            }

            return model;
        }
    }
}
