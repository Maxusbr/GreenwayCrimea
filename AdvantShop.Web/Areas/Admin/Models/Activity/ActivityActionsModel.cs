using System;
using System.Collections.Generic;

namespace AdvantShop.Web.Admin.Models.Activity
{
    public class ActivityActionsModel
    {
        public List<ActivityActionsItemModel> DataItems { get; set; }
    }

    public class ActivityActionsItemModel
    {
        public string EventType { get; set; }
        public string CreateTime { get; set; }
        public string CreateTimeTitle { get; set; }
        public string Time { get; set; }
        public string Link { get; set; }
    }
}
