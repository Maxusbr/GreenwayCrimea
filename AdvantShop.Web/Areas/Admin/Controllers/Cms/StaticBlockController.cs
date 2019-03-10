using System;
using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.StaticBlock;
using AdvantShop.Web.Admin.Models.StaticBlock;
using AdvantShop.Web.Admin.ViewModels.StaticBlock;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Cms
{
    [Auth(RoleAction.Cms)]
    public partial class StaticBlockController : BaseAdminController
    {
        #region List

        public ActionResult Index()
        {
            var model = new StaticBlockViewModel();
            SetMetaInformation(T("Admin.StaticBlock.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.StaticBlockCtrl);

            return View(model);
        }

        public JsonResult GetItems(StaticBlockFilterModel model)
        {
            return Json(new GetStaticBlock(model).Execute());
        }

        public JsonResult Inplace(StaticBlockAddEditModel model)
        {
            var sb = StaticBlockService.GetPagePart(model.StaticBlockId);
            sb.Enabled = model.Enabled;
            StaticBlockService.UpdatePagePart(sb);
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteItem(int id)
        {
            StaticBlockService.DeleteBlock(id);
            return Json(true);
        }

        #region Commands

        private void Command(StaticBlockFilterModel command, Func<int, StaticBlockFilterModel, bool> func)
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
                var handler = new GetStaticBlock(command);
                var ids = handler.GetItemsIds();

                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Active(StaticBlockFilterModel model)
        {
            Command(model, (id, c) =>
            {
                StaticBlockService.SetStaticBlockActivity(id, true);
                return true;
            });

            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Deactive(StaticBlockFilterModel model)
        {
            Command(model, (id, c) =>
            {
                StaticBlockService.SetStaticBlockActivity(id, false);
                return true;
            });

            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Delete(StaticBlockFilterModel command)
        {
            Command(command, (id, c) =>
            {
                StaticBlockService.DeleteBlock(id);
                return true;
            });
            return Json(true);
        }

        #endregion

        #endregion

        #region Add/Get/Edit

        public JsonResult Add(StaticBlockAddEditModel model)
        {
            if (ModelState.IsValid)
            {
                var staticBlock = new StaticBlock()
                {
                    Key = model.Key.DefaultOrEmpty(),
                    InnerName = model.InnerName.DefaultOrEmpty(),
                    Content = model.Content ?? string.Empty,
                    Enabled = model.Enabled
                };

                StaticBlockService.AddStaticBlock(staticBlock);
                return JsonOk();
            }
            return JsonError();
        }

        public JsonResult Edit(StaticBlockAddEditModel model)
        {
            if (ModelState.IsValid)
            {
                var sb = StaticBlockService.GetPagePart(model.StaticBlockId);
                if (sb == null)
                    return JsonError("Статического блока не существует");

                sb.Key = model.Key.DefaultOrEmpty();
                sb.InnerName = model.InnerName.DefaultOrEmpty();
                sb.Content = model.Content ?? string.Empty;
                sb.Enabled = model.Enabled;

                StaticBlockService.UpdatePagePart(sb);

                return JsonOk();
            }
            return JsonError();
        }

        public JsonResult Get(int id)
        {
            return Json(StaticBlockService.GetPagePart(id));
        }

        #endregion
    }
}
