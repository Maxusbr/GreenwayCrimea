using System;
using AdvantShop.Core;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.Users
{
    public class ChangePasswordHandler : AbstractCommandHandler
    {
        private readonly Guid _customerId;
        private readonly string _password;
        private readonly string _passwordConfirm;

        public ChangePasswordHandler(Guid customerId, string password, string passwordConfirm)
        {
            _customerId = customerId;
            _password = password;
            _passwordConfirm = passwordConfirm;
        }

        protected override void Validate()
        {
            if (!CustomerService.ExistsCustomer(_customerId))
                throw new BlException(T("Admin.Users.Validate.NotFound"));

            if (_password.IsNullOrEmpty() || _passwordConfirm.IsNullOrEmpty())
                throw new BlException(T("Admin.Users.Validate.EnterData"));
            if (_password.Length < 6)
                throw new BlException(T("Admin.Users.Validate.PasswordLength"));
            if (_password != _passwordConfirm)
                throw new BlException(T("Admin.Users.Validate.PasswordNotMatch"));
        }

        protected override void Handle()
        {
            CustomerService.ChangePassword(_customerId, _password, false);
        }
    }
}
