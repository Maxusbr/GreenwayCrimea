using System;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.CustomerFieldValues;
using AdvantShop.Web.Admin.Models.CustomerFieldValues;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Customers
{
    [Auth(RoleAction.Settings)]
    public class CustomerFieldValuesController : BaseAdminController
    {
        public JsonResult GetPaging(CustomerFieldValuesFilterModel model)
        {
            var handler = new GetCustomerFieldValuesHandler(model);
            var result = handler.Execute();

            return Json(result);
        }

        #region Inplace CustomerFieldValues

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult Inplace(AdminCustomerFieldValueModel model)
        {
            var dbModel = CustomerFieldService.GetCustomerFieldValue(model.Id);
            if (dbModel == null)
                return Json(new { result = false });
            dbModel.SortOrder = model.SortOrder;
            CustomerFieldService.UpdateCustomerFieldValue(dbModel);

            return Json(new { result = true });
        }

        #endregion

        #region Commands

        private void Command(CustomerFieldValuesFilterModel command, Func<int, CustomerFieldValuesFilterModel, bool> func)
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
                var handler = new GetCustomerFieldValuesHandler(command);
                var ids = handler.GetItemsIds("Id");

                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteItems(CustomerFieldValuesFilterModel command)
        {
            Command(command, (id, c) =>
            {
                CustomerFieldService.DeleteCustomerFieldValue(id);
                return true;
            });
            return Json(true);
        }

        #endregion

        #region CRUD CustomerFieldValue

        public JsonResult Get(int id)
        {
            var dbModel = CustomerFieldService.GetCustomerFieldValue(id);
            if (dbModel == null)
                return Json(new { result = false });

            var handler = new GetCustomerFieldValueModel(dbModel);
            var result = handler.Execute();

            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Add(AdminCustomerFieldValueModel model)
        {
            if (model.Value.IsNullOrEmpty() || model.CustomerFieldId == 0)
                return Json(new { result = false });

            var dbModel = new CustomerFieldValue
            {
                CustomerFieldId = model.CustomerFieldId,
                Value = model.Value.DefaultOrEmpty().Trim(),
                SortOrder = model.SortOrder,
            };
            CustomerFieldService.AddCustomerFieldValue(dbModel);

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Update(AdminCustomerFieldValueModel model)
        {
            if (model.Value.IsNullOrEmpty())
                return Json(new { result = false });

            var dbModel = CustomerFieldService.GetCustomerFieldValue(model.Id);
            if (dbModel == null)
                return Json(new { result = false });

            dbModel.Value = model.Value.DefaultOrEmpty().Trim();
            dbModel.SortOrder = model.SortOrder;

            CustomerFieldService.UpdateCustomerFieldValue(dbModel);

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            CustomerFieldService.DeleteCustomerFieldValue(id);
            return Json(new { result = true });
        }

        #endregion
    }
}
