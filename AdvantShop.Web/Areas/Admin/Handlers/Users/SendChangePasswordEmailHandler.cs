using System;
using AdvantShop.Core;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.Users
{
    public class SendChangePasswordEmailHandler : AbstractCommandHandler
    {
        private readonly Guid _customerId;
        private Customer _customer;

        public SendChangePasswordEmailHandler(Guid customerId)
        {
            _customerId = customerId;
        }

        protected override void Load()
        {
            _customer = CustomerService.GetCustomer(_customerId);
        }

        protected override void Validate()
        {
            if (_customer == null)
                throw new BlException(T("Admin.Users.Validate.NotFound"));
        }

        protected override void Handle()
        {
            var mailTpl = new UserPasswordRepairMailTemplate(_customer.EMail,
                ValidationHelper.DeleteSigns(SecurityHelper.GetPasswordHash(_customer.Password)).ToLower());
            mailTpl.BuildMail();

            SendMail.SendMailNow(_customer.Id, _customer.EMail, mailTpl.Subject, mailTpl.Body, true);
        }
    }
}
