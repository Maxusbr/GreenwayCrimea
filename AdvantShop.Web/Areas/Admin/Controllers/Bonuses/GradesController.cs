using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Grades;
using AdvantShop.Web.Admin.Models;
using AdvantShop.Web.Admin.Models.Grades;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Bonuses
{
    [Auth(RoleAction.BonusSystem)]
    [SaasFeature(Saas.ESaasProperty.BonusSystem)]
    public class GradesController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Grades.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.GradesCtrl);
            return View();
        }

        public JsonResult GetAllGrades()
        {
            var r = GradeService.GetAll().Select(x => new { Value = x.Id, Text = x.Name }).ToList();
            return JsonOk(r);
        }

        public JsonResult GetGradesSelectItems()
        {
            var items = GradeService.GetAll().Select(x => new SelectItemModel(x.Name, x.Id.ToString())).ToList();
            return Json(items);
        }

        public JsonResult GetGrades(GradeFilterModel model)
        {
            var handler = new GetGradeHandler(model);
            var result = handler.Execute();

            return Json(result);
        }

        #region Commands

        private void Command(GradeFilterModel model, Func<int, GradeFilterModel, bool> func)
        {
            if (model.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in model.Ids)
                {
                    func(id, model);
                }
            }
            else
            {
                var handler = new GetGradeHandler(model);
                var ids = handler.GetItemsIds("Id");

                foreach (int id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteGrade(GradeFilterModel model)
        {
            try
            {
                Command(model, (id, c) =>
                {
                    new DeleteGradeHandler(id).Execute();
                    return true;
                });
                return JsonOk(true);
            }
            catch (BlException e)
            {
                ModelState.AddModelError(e.Property, e.Message);
                return JsonError();
            }
        }

        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            return ProcessJsonResult(new DeleteGradeHandler(id));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult InplaceGrade(GradeModel model)
        {
            var grade = GradeService.Get(model.Id);
            if (grade == null || string.IsNullOrWhiteSpace(model.Name))
                return Json(new { result = false });

            grade.Name = model.Name.Trim();
            grade.BonusPercent = model.BonusPercent;
            grade.PurchaseBarrier = model.PurchaseBarrier;
            grade.SortOrder = model.SortOrder;

            GradeService.Update(grade);

            return Json(new { result = true });
        }

        //public ActionResult Add()
        //{
        //    var model = new GradeModel { IsEditMode = false };
        //    SetMetaInformation(T("Admin.Grade.Index.Title"));
        //    SetNgController(NgControllers.NgControllersTypes.GradesCtrl);
        //    return View("AddEdit", model);
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult Add(GradeModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var handler = new AddUpdateGrade(model);
        //        var result = handler.Execute();

        //        if (result != 0)
        //        {
        //            ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
        //            return RedirectToAction("Edit", new { id = result });
        //        }
        //    }

        //    ShowErrorMessages();

        //    SetMetaInformation(T("Admin.Grades.Index.Title"));
        //    SetNgController(NgControllers.NgControllersTypes.GradesCtrl);

        //    return View("AddEdit", model);
        //}

        //public ActionResult Edit(int id)
        //{
        //    var grade = GradeService.Get(id);
        //    if (grade == null)
        //        return RedirectToAction("Index");

        //    var model = new GradeModel
        //    {
        //        IsEditMode = true,

        //        Id = grade.Id,
        //        Name = grade.Name,
        //        SortOrder = grade.SortOrder,
        //        BonusPercent = grade.BonusPercent,
        //        PurchaseBarrier = grade.PurchaseBarrier
        //    };

        //    SetMetaInformation(T("Admin.Grades.Index.Title") + " - " + grade.Name);
        //    SetNgController(NgControllers.NgControllersTypes.GradesCtrl);

        //    return View("AddEdit", model);
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult Edit(GradeModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var handler = new AddUpdateGrade(model);
        //        var result = handler.Execute();

        //        if (result != 0)
        //        {
        //            ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
        //            return RedirectToAction("Edit", new { id = model.Id });
        //        }
        //    }

        //    ShowErrorMessages();

        //    SetMetaInformation(T("Admin.Grades.Index.Title") + " - " + model.Name);
        //    SetNgController(NgControllers.NgControllersTypes.GradesCtrl);

        //    return View("Index", model);
        //}

        public JsonResult Get(int id)
        {
            var dbModel = GradeService.Get(id);
            if (dbModel == null)
                return JsonError();

            return JsonOk((GradeModel)dbModel);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Add(GradeModel model)
        {
            return ProcessJsonResult(new AddUpdateGrade(model, false));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Edit(GradeModel model)
        {
            return ProcessJsonResult(new AddUpdateGrade(model, true));
        }

        public JsonResult DefaultGrade()
        {
            var gradeid = BonusSystem.DefaultGrade;
            return JsonOk(gradeid);
        }
    }
}
