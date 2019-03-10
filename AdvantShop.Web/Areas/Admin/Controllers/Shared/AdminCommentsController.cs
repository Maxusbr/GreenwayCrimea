using System;
using System.Web.Mvc;
using System.Web.SessionState;
using AdvantShop.CMS;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Handlers.AdminComments;
using AdvantShop.Web.Admin.Models.AdminComments;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers
{
    [Auth(RoleAction.Customers, RoleAction.Orders, RoleAction.Crm)]
    public partial class AdminCommentsController : BaseAdminController
    {
        public JsonResult GetComments(int objId, AdminCommentType type)
        {
            var result = new GetAdminCommentsHandler(objId, type).Execute();

            return Json(result);
        }

        public JsonResult GetComment(int id)
        {
            var result = AdminCommentService.GetAdminComment(id);
            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Add(AdminCommentModel model)
        {
            if (model == null)
                return Json(new { Result = false });
            var result = new AddAdminCommentHandler(model).Execute();
            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Update(AdminCommentModel model)
        {
            AdminComment comment;
            if (model == null || (comment = AdminCommentService.GetAdminComment(model.Id)) == null)
                return Json(new { Result = false });

            comment.Text = model.Text ?? "";
            comment.DateModified = DateTime.Now;
            AdminCommentService.UpdateAdminComment(comment);

            return Json(new { Result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            AdminCommentService.DeleteAdminComment(id);
            return Json(new { Result = true });
        }
    }
}
