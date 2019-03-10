using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Localization;
using AdvantShop.Mails;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Customers;
using AdvantShop.Web.Admin.Models.Customers;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Customers
{
    [Auth(RoleAction.Customers)]
    public partial class CustomersController : BaseAdminController
    {
        #region Customers List

        public ActionResult Index(CustomersFilterModel filter)
        {
            if (!string.IsNullOrEmpty(filter.Search))
            {
                var customer = CustomerService.GetCustomerByEmail(filter.Search);
                if (customer != null)
                    return RedirectToAction("Edit", new { id = customer.Id });

                var customerByCode = ClientCodeService.GetCustomerByCode(filter.Search, Guid.Empty);
                if (customerByCode != null && customerByCode.Id != Guid.Empty)
                {
                    return RedirectToAction("Edit", new { id = customerByCode.Id, code = filter.Search });
                }
            }

            var model = new GetIndexModel(filter).Execute();

            SetMetaInformation(T("Admin.Customers.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.CustomersCtrl);

            return View(model);
        }


        public ActionResult GetCustomers(CustomersFilterModel model)
        {
            var handler = new GetCustomersHandler(model);
            var result = handler.Execute();

            if (model.OutputDataType == FilterOutputDataType.Csv)
            {
                var fileName = "export_grid_customers.csv";
                var fullFilePath = new ExportCustomersHandler(result, fileName).Execute();
                return File(fullFilePath, "application/octet-stream", fileName);
            }

            return Json(result);

        }

        #region Commands

        private bool Command(CustomersFilterModel command, Func<Guid, CustomersFilterModel, bool> func)
        {
            bool result = true;

            if (command.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in command.Ids)
                {
                    if (func(id, command) == false)
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                var handler = new GetCustomersHandler(command);
                var ids = handler.GetItemsIds("[Customer].[CustomerID]");

                foreach (var id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                    {
                        if (func(id, command) == false)
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteCustomers(CustomersFilterModel command)
        {
            bool result = Command(command, (id, c) => DeleteCustomerById(id));
            return result ? JsonOk(result) :  JsonError("Произошла ошибка при удалении пользователей");
        }


        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteCustomer(Guid customerid)
        {
            var result = DeleteCustomerById(customerid);
            return Json(result);
        }

        private bool DeleteCustomerById(Guid customerid)
        {
            if (!CustomerService.CanDelete(customerid))
            {
                // add message this customer can be deleted
                return false;
            }

            try
            {
                CustomerService.DeleteCustomer(customerid);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("DeleteCustomerById", ex);
                return false;
            }

            return true;
        }

        #endregion

        #region Add | Edit customer

        public ActionResult Add(AddCustomerModel addCustomerModel)
        {
            var model = new GetCustomer(addCustomerModel).Execute();

            SetMetaInformation(T("Admin.Customers.Add.Title"));
            SetNgController(NgControllers.NgControllersTypes.CustomerCtrl);

            return View("AddEdit", model);
        }

        public ActionResult Edit(Guid id, string code)
        {
            var model = new GetCustomer(id, code).Execute();

            if (model == null)
                return Error404();

            SetMetaInformation(T("Admin.Customers.Edit.Title"));
            SetNgController(NgControllers.NgControllersTypes.CustomerCtrl);

            return View("AddEdit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddEdit(CustomersModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new AddUpdateCustomer(model).Execute();

                if (result)
                {
                    ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                    return RedirectToAction("Edit", new { id = model.CustomerId });
                }
            }

            ShowErrorMessages();

            SetMetaInformation(T("Admin.Customers.Edit.Title"));
            SetNgController(NgControllers.NgControllersTypes.CustomerCtrl);

            if (model.IsEditMode)
                return RedirectToAction("Edit", new { id = model.CustomerId });

            return RedirectToAction("Add");
        }

        [HttpGet]
        public JsonResult GetOrders(Guid customerId)
        {
            var model = OrderService.GetCustomerOrderHistory(customerId).Select(x => new
            {
                OrderId = x.OrderID,
                x.OrderNumber,
                x.Status,
                x.Payed,
                x.ArchivedPaymentName,
                x.ShippingMethodName,
                Sum = PriceFormatService.FormatPrice(x.Sum),
                OrderDate = Culture.ConvertDateWithoutSeconds(x.OrderDate),
                x.ManagerId,
                x.ManagerName
            });
            return Json(new { DataItems = model });
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangePassword(Guid customerId, string pass, string pass2)
        {
            if (string.IsNullOrWhiteSpace(pass) || pass != pass2 || pass.Length < 6)
                return Json(new { result = false, error = "Пароль должен быть не меньше 6 символов. Пароли должны совпадать" });

            var customer = CustomerService.GetCustomer(customerId);
            if (customer == null)
                return Json(new { result = false, error = "Пользователь не найден" });

            CustomerService.ChangePassword(customerId, pass, false);

            return Json(new { result = true });
        }


        #endregion
        
        [HttpGet]
        public JsonResult GetCustomerWithContact(Guid customerId, string code)
        {
            var customer = CustomerService.GetCustomer(customerId);
            if (customer == null && code.IsNotEmpty())
            {
                customer = ClientCodeService.GetCustomerByCode(code, customerId);
                customer.Code = code;
            }
            if (customer == null)
            {
                return Json(null);
            }

            return Json(new
            {
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.Patronymic,
                Email = customer.EMail,
                customer.Phone,
                customer.StandardPhone,
                customer.SubscribedForNews,
                customer.BonusCardNumber,
                customer.CustomerGroup,
                customer.Code,
                customer.RegistredUser,
                customer.Contacts
            });
        }

        [HttpGet]
        public JsonResult GetCustomersAutocomplete(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Json(null);

            var customers = CustomerService.GetCustomersForAutocomplete(q).Select(x => new
            {
                label = x.LastName + " " + x.FirstName + ", " + x.EMail + " " + x.Phone,
                value = x.Id,
                CustomerId = x.Id,
                x.FirstName,
                x.LastName,
                x.Patronymic,
                Email = x.EMail,
                x.Phone
            });

            return Json(customers);
        }

        public JsonResult GetLetterToCustomer(GetLetterToCustomerModel model)
        {
            if (!string.IsNullOrEmpty(model.CustomerId) && string.IsNullOrEmpty(model.FirstName) &&
                string.IsNullOrEmpty(model.LastName) && string.IsNullOrEmpty(model.Patronymic))
            {
                var customer = CustomerService.GetCustomer(model.CustomerId.TryParseGuid());
                if (customer != null)
                {
                    model.FirstName = customer.FirstName;
                    model.LastName = customer.LastName;
                    model.Patronymic = customer.Patronymic;
                }
            }

            var mailTemplate = new SendToCustomerTemplate(model.FirstName ?? "", model.LastName ?? "", model.Patronymic ?? "", "", "");
            mailTemplate.BuildMail();

            return Json(new { subject = mailTemplate.Subject, text = mailTemplate.Body });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SendLetterToCustomer(string customerId, string email, string subject, string text)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                return Json(new { result = false, errors = "Укажите валидный email" });

            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(text))
                return Json(new { result = false, errors = "Введите заголовок и текст письма" });

            var id = customerId.TryParseGuid();
            if (id == Guid.Empty)
            {
                var customer = CustomerService.GetCustomerByEmail(email);
                if (customer != null)
                    id = customer.Id;
            }

            SendMail.SendMailNow(id, email, subject, text, true);

            return Json(new { result = true });
        }

        [HttpGet]
        public JsonResult GetCustomerFields()
        {
            return Json(CustomerFieldService.GetCustomerFields());
        }

        [HttpGet]
        public JsonResult GetCustomerFieldValues(int id)
        {
            return Json(CustomerFieldService.GetCustomerFieldValues(id).Select(x => new { label = x.Value, value = x.Value }));
        }
    }
}
