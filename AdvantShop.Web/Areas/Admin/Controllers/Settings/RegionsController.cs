using System;
using System.Web.Mvc;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Repository;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings.System;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class RegionsController : BaseAdminController
    {
        public JsonResult GetRegionsAutocomplete(string q)
        {
            var result = RegionService.GetRegionsByName(q);
            return Json(result);
        }

        #region Add/Edit/Get/Delete

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddRegion(Region model)
        {
            if (string.IsNullOrEmpty(model.Name))
                return Json(new { result = false });

            try
            {
                var region = new Region()
                {
                    Name = model.Name,
                    CountryId = model.CountryId,
                    RegionCode = model.RegionCode,
                    SortOrder = model.SortOrder
                };

                RegionService.InsertRegion(region);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        public JsonResult EditRegion(Region model)
        {
            if (string.IsNullOrEmpty(model.Name))
                return Json(new { result = false });

            try
            {
                var region = new Region()
                {
                    Name = model.Name,
                    CountryId = model.CountryId,
                    RegionCode = model.RegionCode,
                    SortOrder = model.SortOrder,
                    RegionId = model.RegionId
                };

                RegionService.UpdateRegion(region);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        public JsonResult GetRegionItem(int RegionID)
        {
            var region = RegionService.GetRegion(RegionID);
            return Json(region);
        }


        public JsonResult GetRegions(AdminRegionFilterModel model)
        {
            if(model.id != null)
            {
                model.CountryId = (int)model.id;
            }
            var hendler = new GetRegion(model);
            var result = hendler.Execute();

            return Json(result);
        }

        public JsonResult DeleteRegion(AdminRegionFilterModel model)
        {
            Command(model, (id, c) =>
            {
                RegionService.DeleteRegion(id);
                return true;
            });

            return Json(true);
        }

        #endregion    
        
        #region Inplace

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InplaceRegion(AdminRegionFilterModel model)
        { 
            var region = RegionService.GetRegion(model.RegionId);

            region.Name = model.Name;
            region.CountryId = model.CountryId;
            region.RegionCode = model.RegionCode;
            region.SortOrder = model.SortOrder;

            RegionService.UpdateRegion(region);

            return Json(new { result = true });
        }

        #endregion

        #region Command

        private void Command(AdminRegionFilterModel model, Func<int, AdminRegionFilterModel, bool> func)
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
                model.CountryId = model.id != null ? (int)model.id : 0;
                var handler = new GetRegion(model);
                var RegioniD = handler.GetItemsIds();

                foreach (int id in RegioniD)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }
        
        #endregion

    }
}
