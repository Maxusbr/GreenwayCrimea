using System;
using System.Web.Mvc;
using AdvantShop.SEO;
using AdvantShop.Web.Admin.Handlers.Settings.System;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    public partial class ErrorLog404Controller : BaseAdminController
    {
        #region Get/Delete
        
        public JsonResult GetPage404(AdminErrorLog404FilterModel model)
        {
            var hendler = new GetErrorLog404(model);
            var result = hendler.Execute();

            return Json(result);
        }

        public JsonResult DeleteItems(AdminErrorLog404FilterModel model)
        {
            Command(model, (id, c) =>
            {
                Error404Service.DeleteError404(id);
                return true;
            });

            return Json(true);
        }
        #endregion
        
        #region Command

        private void Command(AdminErrorLog404FilterModel model, Func<int, AdminErrorLog404FilterModel, bool> func)
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
                var handler = new GetErrorLog404(model);
                var Id404page = handler.GetItemsIds();

                foreach (int id in Id404page)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

        #endregion
    }
}
