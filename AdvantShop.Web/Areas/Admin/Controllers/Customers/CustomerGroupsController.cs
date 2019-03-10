using System;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.CustomerGroups;
using AdvantShop.Web.Admin.Models.CustomerGroups;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Customers
{
    [Auth(RoleAction.Customers)]
    public partial class CustomerGroupsController : BaseAdminController
    {
        public ActionResult Index(CustomerGroupsFilterModel filter)
        {            
            SetMetaInformation(T("Admin.CustomerGroups.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.CustomerGroupsCtrl);
            return View();
        }

        public JsonResult GetCustomerGroups(CustomerGroupsFilterModel model)
        {
            return Json(new GetCustomerGroups(model).Execute());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Inplace(AdminCustomerGroupModel model)
        {
            var group = CustomerGroupService.GetCustomerGroup(model.CustomerGroupId);
            if (group != null)
            {
                var discount = model.GroupDiscount >= 0 && model.GroupDiscount <= 100 ? model.GroupDiscount : 0;

                var isDiscountChanged = group.GroupDiscount != discount;

                group.GroupName = model.GroupName.DefaultOrEmpty();
                group.GroupDiscount = discount;
                group.MinimumOrderPrice = model.MinimumOrderPrice;
                
                CustomerGroupService.UpdateCustomerGroup(group);

                if (isDiscountChanged)
                    CategoryService.ClearCategoryCache();
            }
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddCustomerGroup(AdminCustomerGroupModel model)
        {
            if (string.IsNullOrWhiteSpace(model.GroupName))
                return JsonError();

            CustomerGroupService.AddCustomerGroup(new CustomerGroup()
            {
                GroupName = model.GroupName.DefaultOrEmpty(),
                GroupDiscount = model.GroupDiscount >= 0 && model.GroupDiscount <= 100 ? model.GroupDiscount : 0,
                MinimumOrderPrice = model.MinimumOrderPrice
            });
            return JsonOk();
        }

        #region Commands

        private void Command(CustomerGroupsFilterModel command, Func<int, CustomerGroupsFilterModel, bool> func)
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
                var ids = new GetCustomerGroups(command).GetItemsIds("CustomerGroupId");

                foreach (var id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteCustomerGroups(CustomerGroupsFilterModel command)
        {
            Command(command, (id, c) =>
            {
                CustomerGroupService.DeleteCustomerGroup(id);
                return true;
            });
            return Json(true);
        }


        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteCustomerGroup(int customerGroupId)
        {
            CustomerGroupService.DeleteCustomerGroup(customerGroupId);
            return Json(true);
        }
    }
}
