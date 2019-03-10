using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.Loging.Emails;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Core.Services.Vk;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Models;
using AdvantShop.Web.Admin.Models.Leads;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.Leads
{
    public class GetLeadEvents : AbstractCommandHandler<LeadEventsModel>
    {
        private readonly int _leadId;

        public GetLeadEvents(int leadId)
        {
            _leadId = leadId;
        }

        protected override LeadEventsModel Handle()
        {
            var lead = LeadService.GetLead(_leadId);
            if (lead == null)
                return null;

            var model = new LeadEventsModel();

            var leadSource = OrderSourceService.GetOrderSource(lead.OrderSourceId);

            var modelEvents = new List<LeadEventModel>
            {
                new LeadEventModel()
                {
                    Title = T("Admin.Handlers.Leads.GetLeadEvents.LeadCreated"),
                    CreatedDate = lead.CreatedDate,
                    SubMessage =
                        leadSource != null && leadSource.Type != OrderType.None
                            ? T("Admin.Handlers.Leads.GetLeadEvents.LeadSource") + ": " + leadSource.Name
                            : null,
                    Type = LeadEventType.None
                }
            };


            var standardPhone = lead.Customer != null ? StringHelper.ConvertToStandardPhone(lead.Customer.Phone) : null;
            var customerEmail = lead.Customer != null ? lead.Customer.EMail : lead.Email;
            var customerId = lead.Customer != null ? lead.CustomerId : null;

            var events = LeadEventService.GetEvents(lead.Id);

            var emails =
                !string.IsNullOrWhiteSpace(customerEmail)
                    ? CustomerService.GetEmails(customerEmail)
                    : null;

            var sendedEmails =
                !string.IsNullOrWhiteSpace(customerEmail) && customerId != null
                    ? CustomerService.GetEmails(customerId.Value, customerEmail)
                    : null;

            var calls =
                standardPhone != null && standardPhone != 0
                    ? CallService.GetCalls(standardPhone.Value)
                    : null;

            var smses =
                standardPhone != null && standardPhone != 0 && customerId != null
                    ? CustomerService.GetSms(customerId.Value, standardPhone.Value)
                    : null;

            var vkMessages = 
                customerId != null 
                    ? VkService.GetCustomerMessages(customerId.Value) 
                    : null;
            
            if (events != null)
            {
                foreach (var leadEvent in events)
                {
                    var eventModel = new LeadEventModel()
                    {
                        Title = leadEvent.Title,
                        Message = leadEvent.Message,
                        CreatedDate = leadEvent.CreatedDate,
                        Type = leadEvent.Type,
                        TaskId = leadEvent.TaskId,
                        CreatedBy = leadEvent.CreatedBy
                    };

                    modelEvents.Add(eventModel);
                }
            }

            if (emails != null)
            {
                foreach (var email in emails)
                {
                    modelEvents.Add(new LeadEventModel()
                    {
                        EmailId = email.Id,
                        EmailFolder = email.Folder,

                        Title = email.From.ToLower().Contains(SettingsMail.Login.ToLower()) 
                                    ? T("Admin.Handlers.Leads.GetLeadEvents.OutcomingLetter") 
                                    : T("Admin.Handlers.Leads.GetLeadEvents.IncomingLetter"),
                        Message = email.Subject,
                        CreatedDate = email.Date,
                        Type = LeadEventType.Email,
                    });
                }
            }

            if (sendedEmails != null)
            {
                foreach (var email in sendedEmails)
                {
                    modelEvents.Add(new LeadEventModel()
                    {
                        EmailData = new LeadEventEmailDataModel()
                        {
                            CustomerId = customerId.Value,
                            CustomerEmail = customerEmail,
                            CreateOn = email.CreateOn,
                        },
                        Title = T("Admin.Handlers.Leads.GetLeadEvents.OutcomingLetter") +
                                (email.Status == EmailStatus.Error ? " [Не отправлено из-за ошибки]" : ""),
                        Message = email.Subject,
                        CreatedDate = email.CreateOn,
                        Type = LeadEventType.Email,
                    });
                }
            }

            if (calls != null)
            {
                foreach (var call in calls)
                {
                    modelEvents.Add(new LeadEventModel()
                    {
                        Id = call.Id,
                        Title = call.Type.Localize() + T("Admin.Handlers.Leads.GetLeadEvents.CallTitle"),
                        CreatedDate = call.CallDate,
                        Type = LeadEventType.Call,

                        CallComent = AdminCommentService.GetAdminComments(call.Id, AdminCommentType.Call).FirstOrDefault(),

                        SubMessage = 
                            call.CallAnswerDate != null 
                                ? string.Format("<call-record call-id=\"{0}\" operator-type=\"{1}\"></call-record>", call.Id, (int)call.OperatorType)
                                : null
                    });
                }
            }

            if (smses != null)
            {
                foreach (var sms in smses)
                {
                    modelEvents.Add(new LeadEventModel()
                    {
                        Title = LeadEventType.Sms.Localize(),
                        Message = sms.Body,
                        CreatedDate = sms.CreateOn,
                        Type = LeadEventType.Sms
                    });
                }
            }

            if (vkMessages != null)
            {
                foreach (var message in vkMessages)
                {
                    modelEvents.Add(new LeadEventModel()
                    {
                        Title =
                            string.Format("<a href=\"https://vk.com/{0}\" target=\"_blank\" class=\"lead-vk-title\">{1} {2}</a> [{3}]", 
                                !string.IsNullOrEmpty(message.ScreenName) ? message.ScreenName : "id" + message.UserId,
                                message.LastName, 
                                message.FirstName,
                                message.Type == VkMessageType.Received
                                    ? T("Admin.Handlers.Leads.GetLeadEvents.VkReceivedTitle")
                                    : T("Admin.Handlers.Leads.GetLeadEvents.VkSendedTitle")),
                        Message = message.Body,
                        VkData = new LeadEventVkDataModel()
                        {
                            UserId = message.UserId,
                            Photo100 = message.Photo100,
                            Type = message.Type.ToString()
                        },
                        CreatedDate = message.Date,
                        Type = LeadEventType.Vk
                    });
                }
            }

            foreach (var leadEvent in modelEvents.OrderByDescending(x => x.CreatedDate))
            {
                var group = model.EventGroups.Find(x => x.CreatedDate.Year == leadEvent.CreatedDate.Year && 
                                                        x.CreatedDate.Month == leadEvent.CreatedDate.Month && 
                                                        x.CreatedDate.Day == leadEvent.CreatedDate.Day);
                if (group == null)
                {
                    var date = new DateTime(leadEvent.CreatedDate.Year, leadEvent.CreatedDate.Month, leadEvent.CreatedDate.Day);
                    var newGroup = new LeadEventGroupModel() {Title = GetTitleByDate(leadEvent.CreatedDate), CreatedDate = date};
                    newGroup.Events.Add(leadEvent);
                    model.EventGroups.Add(newGroup);
                }
                else
                {
                    group.Events.Add(leadEvent);
                }
            }

            model.EventTypes = new List<SelectItemModel>();

            foreach (LeadEventType eventType in Enum.GetValues(typeof(LeadEventType)))
            {
                if (eventType == LeadEventType.None)
                    continue;

                model.EventTypes.Add(new SelectItemModel(eventType.Localize(), eventType.ToString()));
            }

            return model;
        }

        private string GetTitleByDate(DateTime date)
        {
            var now = DateTime.Now;

            if (date.Day == now.Day && date.Month == now.Month && date.Year == now.Year)
                return T("Admin.Today");

            if (date.Day == now.Day - 1 && date.Month == now.Month && date.Year == now.Year)
                return T("Admin.Yesteday");

            if (date.Year == now.Year)
                return date.ToString("dd MMMM");

            return date.ToString("D");
        }
    }
}
