using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Subscription;
using AdvantShop.Web.Admin.Models.Subscription;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Customers
{
    public partial class SubscriptionController : BaseAdminController
    {
        [Auth(RoleAction.Customers)]
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Subscription.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.SubscriptionCtrl);

            return View();
        }

        #region Get/Delete
        
        public JsonResult getSubscribes(SubscriptionFilterModel model)
        {
            var handler = new GetSubscription(model);
            var result = handler.Execute();
            
            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteSubscription(SubscriptionFilterModel model)
        {
            Command(model, (id, c) =>
            {
                SubscriptionService.DeleteSubscription(id);
                return true;
            });

            return Json(true);
        }

        #endregion

        #region Inplace

        public JsonResult InplaceSubscription(SubscriptionFilterModel model)
        {
            int id = 0;
            Int32.TryParse(model.Id.ToString(), out id);
            if (id == 0)
            {
                return Json(new { result = false });
            }
            var subscribe = SubscriptionService.GetSubscription(id);

            subscribe.Subscribe = model.Enabled != null ? (bool)model.Enabled : false;

            SubscriptionService.UpdateSubscription(subscribe);

            return Json(new { result = true });
        }

        #endregion

        #region Export/Import

        public ActionResult Export(bool openInBrowser = false)
        {
            var filename = "subscribers" + DateTime.Now.ToShortDateString() + ".csv";
            var filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp) + filename;
            using (var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
            {

                streamWriter.WriteLine("{0};{1};{2};{3};{4};",
                    LocalizationService.GetResource("Admin.Subscribe.Export.Email"),
                    LocalizationService.GetResource("Admin.Subscribe.Export.Status"),
                    LocalizationService.GetResource("Admin.Subscribe.Export.Date"),
                    LocalizationService.GetResource("Admin.Subscribe.Export.UnsubscribeDate"),
                    LocalizationService.GetResource("Admin.Subscribe.Export.Header"));

                foreach (var subscriber in SubscriptionService.GetSubscriptions())
                {
                    streamWriter.WriteLine("{0};{1};{2};{3};{4};",
                        subscriber.Email,
                        subscriber.Subscribe ? "1" : "0",
                        subscriber.SubscribeDate != DateTime.MinValue ? subscriber.SubscribeDate.ToString() : string.Empty,
                        subscriber.UnsubscribeDate != DateTime.MinValue ? subscriber.UnsubscribeDate.ToString() : string.Empty,
                        subscriber.UnsubscribeReason);
                }

            }

            return openInBrowser ? File(filePath, "text/plain") : File(filePath, "application/octet-stream", filename);
        }

        public JsonResult Import(HttpPostedFileBase file)
        {
            var filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            FileHelpers.CreateDirectory(filePath);
            var outputFilePath = filePath + "importSubscription" + DateTime.Now.ToShortDateString() + ".csv";

            var result = new ImportSubscriptionHandlers(file, outputFilePath).Execute();
            return Json(result);
        }

        #endregion

        #region Commands

        private void Command(SubscriptionFilterModel command, Func<int, SubscriptionFilterModel, bool> func)
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
                var handler = new GetSubscription(command);
                var ids = handler.GetItemsIds();

                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }
        #endregion
    }
}
