using System.Web.Mvc;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Orders.Shippings;
using AdvantShop.Web.Admin.Models.Grastin;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Orders
{
    [Auth(RoleAction.Orders)]
    public partial class GrastinController : BaseAdminController
    {
        public ActionResult GetOrderActions(int orderId)
        {
            var model = new GrastinOrderActions(orderId).Execute();

            return PartialView("_OrderActions", model);
        }

        public ActionResult GetFormSendOrderForGrastin(int orderId)
        {
            var model = new GrastinGetFormSendOrderForGrastin(orderId).Execute();

            if (model == null)
                return new EmptyResult();
            return PartialView("_SendOrderForGrastin", model);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SendOrderForGrastin(SendOrderForGrastinModel model)
        {
            if (ModelState.IsValid)
            {
                var handler = new GrastinSendOrderForGrastin(model);

                if (handler.Execute())
                    return JsonOk();
                else if (handler.Errors != null)
                    return JsonError(handler.Errors.ToArray());
            }
            return JsonError();
        }

        public ActionResult GetFormSendOrderForRussianPost(int orderId)
        {
            var model = new GrastinGetFormSendOrderForRussianPost(orderId).Execute();

            if (model == null)
                return new EmptyResult();
            return PartialView("_SendOrderForRussianPost", model);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SendOrderForRussianPost(SendOrderForRussianPostModel model)
        {
            if (ModelState.IsValid)
            {
                var handler = new GrastinSendOrderForRussianPost(model);

                if (handler.Execute())
                    return JsonOk();
                else if (handler.Errors != null)
                    return JsonError(handler.Errors.ToArray());
            }
            return JsonError();
        }

        public ActionResult GetFormSendOrderForBoxberry(int orderId)
        {
            var model = new GrastinGetFormSendOrderForBoxberry(orderId).Execute();

            if (model == null)
                return new EmptyResult();
            return PartialView("_SendOrderForBoxberry", model);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SendOrderForBoxberry(SendOrderForBoxberryModel model)
        {
            if (ModelState.IsValid)
            {
                var handler = new GrastinSendOrderForBoxberry(model);

                if (handler.Execute())
                    return JsonOk();
                else if (handler.Errors != null)
                    return JsonError(handler.Errors.ToArray());
            }
            return JsonError();
        }

        public ActionResult GetFormSendOrderForHermes(int orderId)
        {
            var model = new GrastinGetFormSendOrderForHermes(orderId).Execute();

            if (model == null)
                return new EmptyResult();
            return PartialView("_SendOrderForHermes", model);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SendOrderForHermes(SendOrderForHermesModel model)
        {
            if (ModelState.IsValid)
            {
                var handler = new GrastinSendOrderForHermes(model);

                if (handler.Execute())
                    return JsonOk();
                else if (handler.Errors != null)
                    return JsonError(handler.Errors.ToArray());
            }
            return JsonError();
        }

        public ActionResult GetFormSendRequestForIntake(int orderId)
        {
            var model = new GrastinGetFormSendRequestForIntake(orderId).Execute();

            if (model == null)
                return new EmptyResult();
            return PartialView("_SendRequestForIntake", model);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SendRequestForIntake(SendRequestForIntake model)
        {
            if (ModelState.IsValid)
            {
                var handler = new GrastinSendRequestForIntake(model);

                if (handler.Execute())
                    return JsonOk();
                else if (handler.Errors != null)
                    return JsonError(handler.Errors.ToArray());
            }
            return JsonError();
        }

        public ActionResult GetFormSendRequestForAct(int orderId)
        {
            var model = new GrastinGetFormSendRequestForAct(orderId).Execute();

            if (model == null)
                return new EmptyResult();
            return PartialView("_SendRequestForAct", model);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SendRequestForAct(SendRequestForAct model)
        {
            if (ModelState.IsValid)
            {
                var handler = new GrastinSendRequestForAct(model);

                var result = handler.Execute();
                if (!string.IsNullOrEmpty(result))
                    return JsonOk(new {FileName = System.IO.Path.GetFileName(result)});
                else if (handler.Errors != null)
                    return JsonError(handler.Errors.ToArray());
            }
            return JsonError();
        }

        public ActionResult GetAct(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !System.IO.File.Exists(FilePath.FoldersHelper.GetPathAbsolut(FilePath.FolderType.PriceTemp) + fileName))
                return Content("");

            return File(FilePath.FoldersHelper.GetPathAbsolut(FilePath.FolderType.PriceTemp) + fileName, "application/octet-stream", fileName);
        }
    }
}
