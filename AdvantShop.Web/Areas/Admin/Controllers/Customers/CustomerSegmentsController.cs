using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.CustomerSegments;
using AdvantShop.Customers;
using AdvantShop.Repository;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.CustomerSegments;
using AdvantShop.Web.Admin.Models.CustomerSegments;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Customers
{
    [Auth(RoleAction.Customers)]
    public partial class CustomerSegmentsController : BaseAdminController
    {
        #region Segments List

        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.CustomerSegments.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.CustomerSegmentsCtrl);

            return View();
        }


        public ActionResult GetList(CustomerSegmentsFilterModel model)
        {
            return Json(new GetCustomerSegments(model).Execute());
        }

        #region Commands

        private void Command(CustomerSegmentsFilterModel command, Action<int, CustomerSegmentsFilterModel> func)
        {
            if (command.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in command.Ids)
                    func(id, command);
            }
            else
            {
                var ids = new GetCustomerSegments(command).GetItemsIds("Id");
                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteSegments(CustomerSegmentsFilterModel command)
        {
            Command(command, (id, c) => CustomerSegmentService.Delete(id));
            return JsonOk();
        }


        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteSegment(int id)
        {
            CustomerSegmentService.Delete(id);
            return JsonOk();
        }

        #endregion

        #region Add | Edit segment

        public ActionResult Add()
        {
            var model = new CustomerSegmentModel() {SegmentFilter = new CustomerSegmentFilter()};

            SetMetaInformation(T("Admin.CustomerSegments.Add.Title"));
            SetNgController(NgControllers.NgControllersTypes.CustomerSegmentCtrl);

            return View("AddEdit", model);
        }

        public ActionResult Edit(int id)
        {
            var segment = CustomerSegmentService.Get(id);
            if (segment == null)
                return Error404();

            var model = new CustomerSegmentModel()
            {
                IsEditMode = true,

                Id = segment.Id,
                Name = segment.Name,
                SegmentFilter = segment.SegmentFilter ?? new CustomerSegmentFilter()
            };

            if (model.SegmentFilter.Categories != null)
            {
                foreach (var categoryId in model.SegmentFilter.Categories)
                {
                    var c = CategoryService.GetCategory(categoryId);
                    if (c != null)
                        model.Categories.Add(c.Name);
                }
            }

            model.Countries = model.SegmentFilter.Countries;
            model.Cities = model.SegmentFilter.Cities;
            
            SetMetaInformation(T("Admin.CustomerSegments.Edit.Title"));
            SetNgController(NgControllers.NgControllersTypes.CustomerSegmentCtrl);

            return View("AddEdit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddEdit(CustomerSegmentModel model)
        {
            if (ModelState.IsValid)
            {
                var id = new SaveCustomerSegment(model).Execute();
                if (id != 0)
                {
                    ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                    return RedirectToAction("Edit", new { id = id });
                }
            }

            ShowErrorMessages();

            SetMetaInformation(T("Admin.Customers.Edit.Title"));
            SetNgController(NgControllers.NgControllersTypes.CustomerSegmentCtrl);

            if (model.IsEditMode)
                return RedirectToAction("Edit", new { id = model.Id });

            return RedirectToAction("Add");
        }

        public JsonResult GetCategories()
        {
            var categories = CategoryService.GetCategories().Select(x => x.Name).Distinct();
            return Json(new { categories });
        }

        public JsonResult GetCities()
        {
            var cities = CityService.GetAll().Select(x => x.Name).Distinct();
            return Json(new { cities });
        }

        public JsonResult GetCountries()
        {
            var countries = CountryService.GetAllCountryIdAndName().Select(x => x.Name).Distinct();
            return Json(new { countries });
        }

        #endregion

        public ActionResult GetCustomersBySegment(CustomersBySegmentFilterModel model)
        {
            var result = new GetCustomersBySegment(model).Execute();

            if (model.OutputDataType == FilterOutputDataType.Csv)
            {
                var fileName = "export_grid_customersbysegment_" + model.Id + ".csv";
                var fullFilePath = new ExportCustomersBySegment(result, fileName).Execute();
                return File(fullFilePath, "application/octet-stream", fileName);
            }

            return Json(result);
        }
    }
}
