using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Saas;
using AdvantShop.Web.Admin.Handlers.Common;
using AdvantShop.Web.Admin.Models.Users;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.Users
{
    public class AddEditUserHandler : AbstractCommandHandler<object>
    {
        private readonly AdminUserModel _model;
        private readonly bool _editMode;
        private readonly Customer _currentCustomer;
        private readonly bool _roleActionsForbidden;

        private Customer _customer;

        public AddEditUserHandler(AdminUserModel model, bool editMode)
        {
            _model = model;
            _editMode = editMode;
            _roleActionsForbidden = SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.RoleActions;
            _currentCustomer = CustomerContext.CurrentCustomer;
        }

        protected override void Load()
        {
            _customer = _editMode 
                ? CustomerService.GetCustomer(_model.CustomerId) 
                : CustomerService.GetCustomerByEmail(_model.Email);
        }

        protected override void Validate()
        {
            if (_editMode && _customer == null)
                throw new BlException(T("Admin.Users.Validate.NotFound"));

            if (_model.CustomerRole == Role.Moderator && _roleActionsForbidden)
                throw new BlException(T("Admin.Users.Validate.AccessRightsNotInTariff"));
            if (_model.CustomerRole == Role.Administrator && _currentCustomer.IsModerator)
                throw new BlException(T("Admin.Users.Validate.NoAccessToCreateAdmin"));
            if (_model.CustomerRole != Role.Administrator && _model.CustomerRole != Role.Moderator)
                throw new BlException(T("Admin.Users.Validate.SetAccessRights"));

            if (_model.Email.IsNullOrEmpty() || _model.FirstName.IsNullOrEmpty() || _model.LastName.IsNullOrEmpty())
                throw new BlException(T("Admin.Users.Validate.EnterData"));

            if (!ValidationHelper.IsValidEmail(_model.Email) && !string.Equals(_model.Email, "admin"))
                throw new BlException(T("Admin.Users.Validate.WrongEmail"));

            if ((_editMode && _customer.EMail != _model.Email && CustomerService.ExistsEmail(_model.Email)) ||
                (!_editMode && _customer != null && (_customer.IsAdmin || _customer.IsModerator)))
                throw new BlException(T("Admin.Users.Validate.EmailIsBusy"));
        }

        protected override object Handle()
        {
            var addingNew = _customer == null;
            if (addingNew)
            {
                _customer = new Customer()
                {
                    Id = _model.CustomerId,
                    Password = StringHelper.GeneratePassword(8)
                };
            }
            else
            {
                ManagerRoleService.DeleteMap(_customer.Id);
                if (_customer.CustomerRole == Role.Moderator && _model.CustomerRole != Role.Moderator)
                    RoleActionService.DeleteCustomerRoleActions(_customer.Id);
            }

            _customer.CustomerGroupId = CustomerGroupService.DefaultCustomerGroup;
            _customer.CustomerRole = _model.CustomerRole;
            _customer.EMail = _model.Email;
            _customer.FirstName = _model.FirstName.DefaultOrEmpty().Trim();
            _customer.LastName = _model.LastName.DefaultOrEmpty().Trim();
            _customer.Phone = _model.Phone.DefaultOrEmpty().Trim();
            _customer.StandardPhone = StringHelper.ConvertToStandardPhone(_model.Phone);
            _customer.Enabled = _model.Enabled;
            _customer.HeadCustomerId = _model.HeadCustomerId;
            _customer.BirthDay = _model.BirthDay;
            _customer.City = _model.City;

            if (addingNew)
                CustomerService.InsertNewCustomer(_customer);
            else
                CustomerService.UpdateCustomer(_customer);

            if (_customer.CustomerRole == Role.Moderator && _currentCustomer.IsAdmin)
            {
                RoleActionService.DeleteCustomerRoleActions(_customer.Id);
                foreach (var customerRoleAction in _model.RoleActionKeys)
                {
                    if (customerRoleAction.Enabled)
                    {
                        RoleActionService.UpdateOrInsertCustomerRoleAction(_customer.Id, customerRoleAction.Key.ToString(), true);
                    }
                }
            }

            var manager = new Manager
            {
                CustomerId = _customer.Id,
                DepartmentId = _model.DepartmentId,
                Position = _model.Position,
                Active = true
            };
            ManagerService.AddOrUpdateManager(manager);

            if (_model.PhotoEncoded.IsNotEmpty())
                new UploadAvatarCropped(_customer, _model.Avatar, _model.PhotoEncoded).Execute();

            foreach (var roleId in _model.ManagerRolesIds)
            {
                ManagerRoleService.AddMap(_customer.Id, roleId);
            }

            if (addingNew)
            {
                _customer.Password = SecurityHelper.GetPasswordHash(_customer.Password);

                var mailTemplate = new UserRegisteredMailTemplate(_customer.EMail, _customer.FirstName, _customer.LastName, Localization.Culture.ConvertDate(DateTime.Now),
                    ValidationHelper.DeleteSigns(SecurityHelper.GetPasswordHash(_customer.Password)));
                mailTemplate.BuildMail();
                SendMail.SendMailNow(_customer.Id, _customer.EMail, mailTemplate.Subject, mailTemplate.Body, true);
            }
            return new
            {
                customer = _customer,
                reloadPage = _editMode && _customer.Id == _currentCustomer.Id
            };
        }
    }
}
