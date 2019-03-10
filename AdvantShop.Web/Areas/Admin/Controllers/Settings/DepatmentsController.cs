using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Departments;
using AdvantShop.Web.Admin.Models.Departments;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public class DepartmentsController : BaseAdminController
    {
        public JsonResult GetDepartments(DepartmentsFilterModel model)
        {
            var handler = new GetDepartmentsHandler(model);
            var result = handler.Execute();

            return Json(result);
        }

        #region Inplace Departments

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InplaceDepartment(AdminDepartmentModel model)
        {
            var dbModel = DepartmentService.GetDepartment(model.DepartmentId);
            dbModel.Enabled = model.Enabled;
            dbModel.Sort = model.Sort;
            DepartmentService.UpdateDepartments(dbModel);

            return Json(new { result = true });
        }

        #endregion

        #region Commands

        private void Command(DepartmentsFilterModel command, Func<int, DepartmentsFilterModel, bool> func)
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
                var handler = new GetDepartmentsHandler(command);
                var ids = handler.GetItemsIds("DepartmentId");

                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteDepartments(DepartmentsFilterModel command)
        {
            Command(command, (id, c) =>
            {
                DepartmentService.DeleteDepartments(id);
                return true;
            });
            return Json(true);
        }

        #endregion
        
        #region CRUD Department

        public JsonResult GetDepartment(int departmentId)
        {
            var dbModel = DepartmentService.GetDepartment(departmentId);
            if (dbModel == null)
                return Json(new { result = false });

            var handler = new GetDepartmentModel(dbModel);
            var result = handler.Execute();

            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddDepartment(AdminDepartmentModel model)
        {
            if (model.Name.IsNullOrEmpty())
                return Json(new { result = false });

            var dbModel = new Department
            {
                Name = model.Name.DefaultOrEmpty(),
                Sort = model.Sort,
                Enabled = model.Enabled
            };
            DepartmentService.AddDepartments(dbModel);

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateDepartment(AdminDepartmentModel model)
        {
            if (model.Name.IsNullOrEmpty())
                return Json(new { result = false });

            var dbModel = DepartmentService.GetDepartment(model.DepartmentId);
            if (dbModel == null)
                return Json(new { result = false });

            dbModel.Name = model.Name.DefaultOrEmpty();
            dbModel.Sort = model.Sort;
            dbModel.Enabled = model.Enabled;

            DepartmentService.UpdateDepartments(dbModel);

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteDepartment(int departmentId)
        {
            DepartmentService.DeleteDepartments(departmentId);
            return Json(new { result = true });
        }

        #endregion

        public JsonResult GetDepartmentsSelectOptions()
        {
            var result = DepartmentService.GetDepartmentsList().Select(x => new { label = x.Name, value = x.DepartmentId.ToString() }).ToList();
            result.Insert(0, new { label = LocalizationService.GetResource("Admin.Departments.NotSelected"), value = "-1" });
            return Json(result);
        }
    }
}
