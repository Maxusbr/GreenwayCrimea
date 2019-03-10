using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Properties;
using AdvantShop.Web.Admin.Models.Properties;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Catalog
{
    [Auth(RoleAction.Catalog)]
    public partial class PropertiesController : BaseAdminController
    {
        public ActionResult Index(int? groupId)
        {
            var model = new GetPropertiesModel(groupId).Execute();

            SetMetaInformation(T("Admin.Properties.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.PropertiesCtrl);

            return View(model);
        }

        public JsonResult GetProperties(PropertiesFilterModel model)
        {
            var handler = new GetPropertiesHandler(model);
            var result = handler.Execute();

            return Json(result);
        }

        #region Commands

        private void Command(PropertiesFilterModel model, Action<int, PropertiesFilterModel> func)
        {
            if (model.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in model.Ids)
                    func(id, model);
            }
            else
            {
                var ids = new GetPropertiesHandler(model).GetItemsIds("Property.PropertyId");
                foreach (int id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteProperties(PropertiesFilterModel model)
        {
            Command(model, (id, c) => PropertyService.DeleteProperty(id));
            return JsonOk();
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UseInFilter(PropertiesFilterModel model)
        {
            Command(model, (id, c) => PropertyService.ShowInFilter(id, true));
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult NotUseInFilter(PropertiesFilterModel model)
        {
            Command(model, (id, c) => PropertyService.ShowInFilter(id, false));
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UseInDetails(PropertiesFilterModel model)
        {
            Command(model, (id, c) => PropertyService.ShowInDetails(id, true));
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult NotUseInDetails(PropertiesFilterModel model)
        {
            Command(model, (id, c) => PropertyService.ShowInDetails(id, false));
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UseInBrief(PropertiesFilterModel model)
        {
            Command(model, (id, c) => PropertyService.ShowInBrief(id, true));
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult NotUseInBrief(PropertiesFilterModel model)
        {
            Command(model, (id, c) => PropertyService.ShowInBrief(id, false));
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangePropertyGroup(int? newid, PropertiesFilterModel model)
        {
            if (newid != null && newid == 0)
                newid = null;

            Command(model, (id, c) => PropertyService.UpdateGroup(id, newid));
            return JsonOk();
        }


        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteProperty(int propertyId)
        {
            PropertyService.DeleteProperty(propertyId);
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult InplaceProperty(PropertyModel model)
        {
            var property = PropertyService.GetPropertyById(model.PropertyId);
            if (property == null)
                return JsonError();
            
            property.UseInFilter = model.UseInFilter;
            property.UseInDetails = model.UseInDetails;
            property.UseInBrief = model.UseInBrief;
            property.SortOrder = model.SortOrder;

            PropertyService.UpdateProperty(property);

            return JsonOk();
        }


        #region Add | Update Property

        [HttpGet]
        public JsonResult GetProperty(int propertyId)
        {
            var property = PropertyService.GetPropertyById(propertyId);
            return Json(property);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddProperty(Property model)
        {
            if (string.IsNullOrEmpty(model.Name))
                return Json(new { result = false });

            try
            {
                var property = new Property()
                {
                    Name = model.Name.DefaultOrEmpty(),
                    NameDisplayed =
                        string.IsNullOrEmpty(model.NameDisplayed)
                            ? model.Name.DefaultOrEmpty()
                            : model.NameDisplayed.DefaultOrEmpty(),
                    Description = model.Description.DefaultOrEmpty(),
                    Unit = model.Unit.DefaultOrEmpty(),
                    Type = model.Type,
                    GroupId = model.GroupId != 0 ? model.GroupId : default(int?),
                    UseInFilter = model.UseInFilter,
                    UseInDetails = model.UseInDetails,
                    UseInBrief = model.UseInBrief,
                    Expanded = model.Expanded,
                    SortOrder = model.SortOrder
                };

                PropertyService.AddProperty(property);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateProperty(Property model)
        {
            if (string.IsNullOrEmpty(model.Name))
                return Json(new { result = false });

            try
            {
                var property = PropertyService.GetPropertyById(model.PropertyId);

                property.Name = model.Name.DefaultOrEmpty();
                property.NameDisplayed =
                    string.IsNullOrEmpty(model.NameDisplayed)
                        ? model.Name.DefaultOrEmpty()
                        : model.NameDisplayed.DefaultOrEmpty();
                property.Description = model.Description.DefaultOrEmpty();
                property.Unit = model.Unit.DefaultOrEmpty();
                property.Type = model.Type;
                property.GroupId = model.GroupId != 0 ? model.GroupId : default(int?);
                property.UseInFilter = model.UseInFilter;
                property.UseInDetails = model.UseInDetails;
                property.UseInBrief = model.UseInBrief;
                property.Expanded = model.Expanded;
                property.SortOrder = model.SortOrder;

                PropertyService.UpdateProperty(property);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        [HttpGet]
        public JsonResult GetPropertyData()
        {
            var types = (from PropertyType item in Enum.GetValues(typeof (PropertyType))
                select new
                {
                    Text = item.Localize(), 
                    Value = (int)item
                }).ToList();

            var groups = PropertyGroupService.GetList().Select(x => new
            {
                Text = x.Name,
                Value = x.PropertyGroupId
            }).ToList();

            groups.Insert(0, new {Text = T("Admin.Properties.NotSelectedGroup"), Value = 0});

            return Json(new {types, groups});
        }


        #endregion

        #region Get | Add | Update Property group

        [HttpGet]
        public JsonResult GetGroups()
        {
            var groups = new List<PropertyGroup>()
            {
                new PropertyGroup() { Name = LocalizationService.GetResource("Admin.Properties.AllProperties") },
                new PropertyGroup() { Name = LocalizationService.GetResource("Admin.Properties.UngroupedProperties"), PropertyGroupId = -1 }
            };

            groups.AddRange(PropertyGroupService.GetList());

            return Json(groups);
        }

        public JsonResult GetPropertyGroups(PropertyGroupsFilterModel model)
        {
            var handler = new GetPropertyGroupsHandler(model);
            var result = handler.Execute();

            return Json(result);
        }


        [HttpGet]
        public JsonResult GetGroup(int groupId)
        {
            var group = PropertyGroupService.Get(groupId);
            return Json(group);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddGroup(string name, string nameDisplayed, int sortOrder)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Json(new {result = false});

            var group = new PropertyGroup()
            {
                Name = name,
                NameDisplayed = string.IsNullOrEmpty(nameDisplayed) ? name : nameDisplayed,
                SortOrder = sortOrder
            };

            PropertyGroupService.Add(group);

            return Json(new { result = true, groupId = group.PropertyGroupId, name = group.Name });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateGroup(int propertyGroupId, string name, string nameDisplayed, int sortOrder)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Json(new { result = false });

            var group = new PropertyGroup()
            {
                PropertyGroupId = propertyGroupId,
                Name = name,
                NameDisplayed = string.IsNullOrEmpty(nameDisplayed) ? name : nameDisplayed,
                SortOrder = sortOrder
            };

            PropertyGroupService.Update(group);

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteGroup(int propertyGroupId)
        {
            PropertyGroupService.Delete(propertyGroupId);
            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeGroupSorting(int groupId, int? prevGroupId, int? nextGroupId)
        {
            var handler = new ChangeGroupSorting(groupId, prevGroupId, nextGroupId);
            var result = handler.Execute();

            return Json(new { result = result });
        }
        #endregion
    }
}
