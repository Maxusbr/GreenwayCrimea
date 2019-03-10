using System;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.PropertyValues;
using AdvantShop.Web.Admin.Models.PropertyValues;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Catalog
{
    [Auth(RoleAction.Catalog)]
    public partial class PropertyValuesController : BaseAdminController
    {
        public ActionResult Index(int propertyId)
        {
            var model = new GetIndexModel(propertyId).Execute();

            if (model == null)
                return Error404();

            SetMetaInformation(T("Admin.Properties.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.PropertyValuesCtrl);

            return View(model);
        }

        public JsonResult GetPropertyValues(PropertyValuesFilterModel model)
        {
            return Json(new GetPropertyValuesHandler(model).Execute());
        }

        #region Commands

        private void Command(PropertyValuesFilterModel command, Action<int, PropertyValuesFilterModel> func)
        {
            if (command.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in command.Ids)
                    func(id, command);
            }
            else
            {
                var ids = new GetPropertyValuesHandler(command).GetItemsIds("PropertyValueId");
                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeletePropertyValues(PropertyValuesFilterModel command)
        {
            Command(command, (id, c) => PropertyService.DeletePropertyValueById(id));
            return Json(true);
        }

        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeletePropertyValue(int propertyValueId)
        {
            PropertyService.DeletePropertyValueById(propertyValueId);
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult InplacePropertyValue(PropertyValueModel model)
        {
            var propertyValue = PropertyService.GetPropertyValueById(model.PropertyValueId);
            if (propertyValue == null)
                return JsonError();

            propertyValue.Value = model.Value ?? string.Empty;
            propertyValue.SortOrder = model.SortOrder;

            PropertyService.UpdatePropertyValue(propertyValue);

            return JsonOk();
        }


        #region Add | Update PropertyValue

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddPropertyValue(PropertyValue model)
        {
            if (string.IsNullOrEmpty(model.Value))
                return JsonError();

            try
            {
                var propertyValue = new PropertyValue()
                {
                    PropertyId = model.PropertyId,
                    Value = model.Value.DefaultOrEmpty(),
                    SortOrder = model.SortOrder
                };

                PropertyService.AddPropertyValue(propertyValue);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return JsonError();
            }

            return JsonOk();
        }

        #endregion

    }
}
