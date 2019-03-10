using System;
using System.Collections.Generic;
using AdvantShop.CMS;
using AdvantShop.Core.Services.Crm;

namespace AdvantShop.Web.Admin.Models.Leads
{
    public class LeadEventGroupModel
    {
        public LeadEventGroupModel()
        {
            Events = new List<LeadEventModel>();
        }

        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<LeadEventModel> Events { get; set; }
    }

    public class LeadEventModel : LeadEvent
    {
        public new string Title { get; set; }
        public string SubMessage { get; set; }
        public string EventType { get { return Type.ToString().ToLower(); } }

        public string EmailId { get; set; }
        public string EmailFolder { get; set; }
        public LeadEventEmailDataModel EmailData { get; set; }
        public AdminComment CallComent { get; set; }
        public LeadEventVkDataModel VkData { get; set; }
    }

    public class LeadEventEmailDataModel
    {
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime CreateOn { get; set; }
        public bool IsError { get; set; }
    }

    public class LeadEventVkDataModel
    {
        public long UserId { get; set; }
        public string Photo100 { get; set; }
        public string Type { get; set; }
    }

    public class LeadEventsModel
    {
        public LeadEventsModel()
        {
            EventGroups = new List<LeadEventGroupModel>();
        }

        public List<LeadEventGroupModel> EventGroups { get; set; }
        public List<SelectItemModel> EventTypes { get; set; }
    }
}
