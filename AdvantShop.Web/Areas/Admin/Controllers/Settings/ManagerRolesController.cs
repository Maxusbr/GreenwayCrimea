using System;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.ManagerRoles;
using AdvantShop.Web.Admin.Models.ManagerRoles;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public class ManagerRolesController : BaseAdminController
    {
        public JsonResult GetManagerRoles(ManagerRolesFilterModel model)
        {
            var handler = new GetManagerRolesHandler(model);
            var result = handler.Execute();

            return Json(result);
        }

        #region Inplace ManagerRoles

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InplaceManagerRole(AdminManagerRoleModel model)
        {
            var dbModel = ManagerRoleService.GetManagerRole(model.Id);
            dbModel.SortOrder = model.SortOrder;
            ManagerRoleService.UpdateManagerRole(dbModel);

            return Json(new { result = true });
        }

        #endregion

        #region Commands

        private void Command(ManagerRolesFilterModel command, Func<int, ManagerRolesFilterModel, bool> func)
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
                var handler = new GetManagerRolesHandler(command);
                var ids = handler.GetItemsIds("Id");

                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteManagerRoles(ManagerRolesFilterModel command)
        {
            Command(command, (id, c) =>
            {
                ManagerRoleService.DeleteManagerRole(id);
                return true;
            });
            return Json(true);
        }

        #endregion
        
        #region CRUD ManagerRole

        public JsonResult GetManagerRole(int id)
        {
            var dbModel = ManagerRoleService.GetManagerRole(id);
            if (dbModel == null)
                return Json(new { result = false });

            var handler = new GetManagerRoleModel(dbModel);
            var result = handler.Execute();

            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddManagerRole(AdminManagerRoleModel model)
        {
            if (model.Name.IsNullOrEmpty())
                return Json(new { result = false });

            var dbModel = new ManagerRole
            {
                Name = model.Name.DefaultOrEmpty().Trim(),
                SortOrder = model.SortOrder,
            };
            ManagerRoleService.AddManagerRole(dbModel);

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateManagerRole(AdminManagerRoleModel model)
        {
            if (model.Name.IsNullOrEmpty())
                return Json(new { result = false });

            var dbModel = ManagerRoleService.GetManagerRole(model.Id);
            if (dbModel == null)
                return Json(new { result = false });

            dbModel.Name = model.Name.DefaultOrEmpty();
            dbModel.SortOrder = model.SortOrder;

            ManagerRoleService.UpdateManagerRole(dbModel);

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteManagerRole(int id)
        {
            ManagerRoleService.DeleteManagerRole(id);
            return Json(new { result = true });
        }

        #endregion
    }
}
