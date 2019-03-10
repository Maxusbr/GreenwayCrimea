using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Crm.BusinessProcesses;

namespace AdvantShop.Web.Admin.Models.Settings
{
    public class TasksSettingsModel
    {
        public TasksSettingsModel()
        {
            TaskGroups = TaskGroupService.GetAllTaskGroups()
                .Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == SettingsTasks.DefaultTaskGroup}).ToList();
            EventTypes = new List<EBizProcessEventType>
            {
                EBizProcessEventType.OrderCreated,
                EBizProcessEventType.OrderStatusChanged,
                EBizProcessEventType.LeadCreated,
                EBizProcessEventType.LeadStatusChanged,
                EBizProcessEventType.CallMissed,
                EBizProcessEventType.ReviewAdded,
                EBizProcessEventType.MessageReply,
            };
        }

        public int DefaultTaskGroupId { get; set; }

        public bool WebNotificationInNewTab { get; set; }

        public List<SelectListItem> TaskGroups { get; set; }

        public List<EBizProcessEventType> EventTypes { get; set; }
    }
}
