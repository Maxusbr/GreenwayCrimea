using System;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.Services.Vk;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Shared
{
    [Auth(RoleAction.Settings, RoleAction.Customers)]
    public partial class VkController : BaseAdminController
    {
        #region Settings 

        public JsonResult GetVkSettings()
        {
            var service = new VkApiService();

            var group = SettingsVk.Group;

            return Json(new
            {
                clientId = SettingsVk.ApplicationId,
                groups = !string.IsNullOrEmpty(SettingsVk.TokenUser) ? service.GetUserGroups() : null,
                group = group,
            });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveAuthVkUser(string clientId, string accessToken, string userId)
        {
            SettingsVk.ApplicationId = clientId;
            SettingsVk.TokenUser = accessToken;
            SettingsVk.UserId = Convert.ToInt64(userId);

            return JsonOk();
        }

        public JsonResult GetVkGroups()
        {
            return Json(new VkApiService().GetUserGroups());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveAuthVkGroup(VkGroup group, string accessToken)
        {
            if (group == null ||
                group.Id == 0 ||
                string.IsNullOrWhiteSpace(group.Name) ||
                string.IsNullOrWhiteSpace(accessToken))
            {
                return JsonError();
            }

            SettingsVk.Group = group;
            SettingsVk.TokenGroup = accessToken;
            SettingsVk.TokenGroupErrorCount = 0;

            TaskManager.TaskManagerInstance().Init();

            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteGroup()
        {
            SettingsVk.Group = null;
            SettingsVk.TokenGroup = null;
            SettingsVk.TokenUser = null;
            SettingsVk.TokenUserErrorCount = 0;
            SettingsVk.TokenGroupErrorCount = 0;
            SettingsVk.LastMessageId = null;
            SettingsVk.LastSendedMessageId = null;

            return JsonOk();
        }

        #endregion
        
        [HttpGet]
        public JsonResult GetCustomerMessages(Guid customerId)
        {
            var service = new VkApiService();
            if (!service.IsVkActive())
                return Json(null);

            return Json(VkService.GetCustomerMessages(customerId));
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SendVkMessage(long userId, string message)
        {
            var vkService = new VkApiService();
            var result = vkService.SendMessageByGroup(userId, message.Replace("\r\n", "<br>"));

            return result != 0 ? JsonOk() : JsonError();
        }
    }
}
