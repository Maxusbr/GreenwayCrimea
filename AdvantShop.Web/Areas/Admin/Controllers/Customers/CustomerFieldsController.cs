using System;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.CustomerFields;
using AdvantShop.Web.Admin.Models.CustomerFields;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Customers
{
    [Auth(RoleAction.Settings)]
    public class CustomerFieldsController : BaseAdminController
    {
        public JsonResult GetPaging(CustomerFieldsFilterModel model)
        {
            var handler = new GetCustomerFieldsHandler(model);
            var result = handler.Execute();

            return Json(result);
        }

        #region Inplace CustomerFields

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult Inplace(AdminCustomerFieldModel model)
        {
            var dbModel = CustomerFieldService.GetCustomerField(model.Id);
            if (dbModel == null)
                return Json(new { result = false });
            dbModel.SortOrder = model.SortOrder;
            dbModel.Required = model.Required;
            dbModel.Enabled = model.Enabled;
            dbModel.ShowInClient = model.ShowInClient;
            CustomerFieldService.UpdateCustomerField(dbModel);

            return Json(new { result = true });
        }

        #endregion

        #region Commands

        private void Command(CustomerFieldsFilterModel command, Func<int, CustomerFieldsFilterModel, bool> func)
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
                var handler = new GetCustomerFieldsHandler(command);
                var ids = handler.GetItemsIds("Id");

                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteItems(CustomerFieldsFilterModel command)
        {
            Command(command, (id, c) =>
            {
                CustomerFieldService.DeleteCustomerField(id);
                return true;
            });
            return Json(true);
        }

        #endregion
        
        #region CRUD CustomerField

        public JsonResult Get(int id)
        {
            var dbModel = CustomerFieldService.GetCustomerField(id);
            if (dbModel == null)
                return Json(new { result = false });

            var handler = new GetCustomerFieldModel(dbModel);
            var result = handler.Execute();

            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Add(AdminCustomerFieldModel model)
        {
            if (model.Name.IsNullOrEmpty())
                return Json(new { result = false });

            var customerField = new AddEditCustomerFieldHandler(model, false).Execute();

            return Json(new { result = customerField != null });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Update(AdminCustomerFieldModel model)
        {
            if (model.Name.IsNullOrEmpty())
                return Json(new { result = false });

            var customerField = new AddEditCustomerFieldHandler(model, true).Execute();

            return Json(new { result = customerField != null });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            CustomerFieldService.DeleteCustomerField(id);
            return Json(new { result = true });
        }

        #endregion

        public JsonResult GetCustomerFieldTypes()
        {
            return Json(CustomerFieldService.GetCustomerFieldTypesSelectOptions());
        }

        public JsonResult GetCustomerFieldFormData()
        {
            return Json(new
            {
                fieldTypes = CustomerFieldService.GetCustomerFieldTypesSelectOptions()
            });
        }
    }
}
