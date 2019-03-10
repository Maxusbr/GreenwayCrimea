using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Saas;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Users;
using AdvantShop.Web.Admin.Models.Users;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Core;
using AdvantShop.Web.Admin.ViewModels.Settings;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public class UsersController : BaseAdminController
    {
        public JsonResult GetUsers(UsersFilterModel model)
        {
            var handler = new GetUsersHandler(model);
            var result = handler.Execute();

            return Json(result);
        }

        #region Inplace Users

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InplaceUser(AdminUserModel model)
        {
            if (model.CustomerId == CustomerContext.CustomerId)
                return Json(new { result = false });

            var dbModel = CustomerService.GetCustomer(model.CustomerId);
            dbModel.Enabled = model.Enabled;
            CustomerService.UpdateCustomer(dbModel);

            return Json(new { result = true });
        }

        #endregion


        #region Commands

        private void Command(UsersFilterModel command, Func<Guid, UsersFilterModel, bool> func)
        {
            if (command.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in command.Ids)
                {
                    func(id, command);
                }
            }
            else
            {
                var handler = new GetUsersHandler(command);
                var ids = handler.GetItemsIds("Customer.CustomerID");

                foreach (var id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteUsers(UsersFilterModel command)
        {
            Command(command, (id, c) => DeleteCustomerById(id));
            return Json(true);
        }

        #endregion


        #region CRUD User

        public JsonResult GetUser(Guid customerId)
        {
            var dbModel = CustomerService.GetCustomer(customerId);
            if (dbModel == null)
                return JsonError(T("Admin.Users.Validate.NotFound"));

            return JsonOk(new GetUserModel(dbModel).Execute());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddUser(AdminUserModel model)
        {
            return ProcessJsonResult(new AddEditUserHandler(model, false));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateUser(AdminUserModel model)
        {
            return ProcessJsonResult(new AddEditUserHandler(model, true));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteUser(Guid customerid)
        {
            List<string> messages;

            return DeleteCustomerById(customerid, out messages)
                ? JsonOk()
                : JsonError(messages.Any() ? messages.AggregateString("<br/>") : string.Empty);
        }

        private bool DeleteCustomerById(Guid customerid)
        {
            List<string> messages;
            return DeleteCustomerById(customerid, out messages);
        }

        private bool DeleteCustomerById(Guid customerid, out List<string> messages)
        {
            if (!CustomerService.CanDelete(customerid, out messages))
                return false;

            try
            {
                CustomerService.DeleteCustomer(customerid);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("DeleteCustomerById", ex);
                messages.Add(T("Admin.Users.ErrorOnDeleteUser"));
                return false;
            }

            return true;
        }

        #endregion

        public JsonResult ChangeManagersPageState(bool state)
        {
            SettingsCheckout.ShowManagersPage = state;
            return JsonOk();
        }

        public JsonResult ChangeEnableManagersModuleState(bool state)
        {
            SettingsCheckout.EnableManagersModule = state;
            return JsonOk();
        }

        public JsonResult SendChangePasswordMail(Guid customerId)
        {
            return ProcessJsonResult(new SendChangePasswordEmailHandler(customerId));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangePassword(Guid customerId, string password, string passwordConfirm)
        {
            return ProcessJsonResult(new ChangePasswordHandler(customerId, password, passwordConfirm));
        }

        /// <summary>
        /// данные для формы редактирования/добавления сотрудника
        /// </summary>
        /// <param name="customerId">id редактируемого сотрудника</param>
        /// <returns></returns>
        public JsonResult GetUserFormData(Guid? customerId)
        {
            return ProcessJsonResult(new GetUserFormDataHandler(customerId));
        }

        public JsonResult GetSaasDataInformation()
        {
            var resultData = new UsersViewModel();

            try
            {
                var saasData = SaasDataService.CurrentSaasData;

                resultData.ManagersCount = ManagerService.GetManagersCount();
                resultData.ManagersLimitation = SaasDataService.IsSaasEnabled;
                resultData.ManagersLimit = resultData.ManagersLimitation ? saasData.EmployeesCount : 0;

                resultData.EmployeesCount = EmployeeService.GetEmployeeCount();
                resultData.EmployeesLimit = SaasDataService.IsSaasEnabled ? saasData.EmployeesCount : int.MaxValue;
                resultData.EnableEmployees = !SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && resultData.EmployeesCount < resultData.EmployeesLimit);

                resultData.ShowManagersPage = SettingsCheckout.ShowManagersPage;
                resultData.EnableManagersModule = SettingsCheckout.EnableManagersModule;
            }
            catch (BlException e)
            {
                ModelState.AddModelError(e.Property, e.Message);
                return JsonError();
            }
            return JsonOk(resultData);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Invite(AdminUserModel[] users)
        {
            if (!TrialService.IsTrialEnabled)
                return JsonError();

            foreach (var user in users)
            {
                if (CustomerService.CheckCustomerExist(user.Email))
                    continue;

                var customer = new Customer(CustomerGroupService.DefaultCustomerGroup)
                {
                    EMail = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CustomerRole = Role.Moderator,
                    Password = StringHelper.GeneratePassword(8)
                };

                CustomerService.InsertNewCustomer(customer);

                foreach (RoleAction roleAction in Enum.GetValues(typeof(RoleAction)))
                {
                    if (roleAction == RoleAction.Settings || roleAction == RoleAction.None)
                        continue;
                    RoleActionService.UpdateOrInsertCustomerRoleAction(customer.Id, roleAction.ToString(), true);
                }

                customer.Password = SecurityHelper.GetPasswordHash(customer.Password);
                var mailTemplate = new UserRegisteredMailTemplate(customer.EMail, customer.FirstName, customer.LastName, Localization.Culture.ConvertDate(DateTime.Now),
                    ValidationHelper.DeleteSigns(SecurityHelper.GetPasswordHash(customer.Password)));
                mailTemplate.BuildMail();

                TrialService.SendMessage(customer.EMail, mailTemplate);
            }
            return JsonOk();
        }
    }
}
