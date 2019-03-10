<%@ WebHandler Language="C#" Class="AdvantShop.Admin.HttpHandlers.Lead.CreateTask" %>

using System;
using System.Web;
using Admin;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Saas;
using Newtonsoft.Json;
using Resources;

namespace AdvantShop.Admin.HttpHandlers.Lead
{
    public class CreateTask : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            if (!Authorize(context)
                || (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCrm))
                return;

            var lead = LeadService.GetLead(context.Request["leadid"].TryParseInt());

            if (lead == null ||
                String.IsNullOrEmpty(context.Request["taskname"]))
            {
                WriteErrorResponse(context, "Error");
                return;
            }

            if (!string.IsNullOrEmpty(HttpUtility.HtmlEncode(context.Request["email"]))
                && !Helpers.ValidationHelper.IsValidEmail(HttpUtility.HtmlEncode(context.Request["email"])))
            {
                WriteErrorResponse(context, "Error");
                return;
            }

            var appointedManager = ManagerService.GetManager(CustomerContext.CurrentCustomer.Id);
            if (appointedManager == null)
            {
                WriteErrorResponse(context, Resource.Admin_Leads_Error_NotManager);
                return;
            }

            var managerId = context.Request["manager"].TryParseInt();

            var customer = CustomerService.GetCustomer(context.Request["customerid"].TryParseGuid());
            if (customer == null)
            {
                customer = new Customer()
                {
                    FirstName = HttpUtility.HtmlEncode(context.Request["name"]),
                    Phone = HttpUtility.HtmlEncode(context.Request["phone"]),
                    EMail = HttpUtility.HtmlEncode(context.Request["email"]),
                    ManagerId = managerId
                };

                customer.Id = CustomerService.InsertNewCustomer(customer);
            }

            if (customer.Id == Guid.Empty)
            {
                WriteErrorResponse(context, Resource.Admin_Leads_Error_UserEmailExist);
                return;
            }

            var taskId = 0;

            try
            {
                var task = new ManagerTask()
                {
                    Name = HttpUtility.HtmlEncode(context.Request["taskname"]),
                    Description = HttpUtility.HtmlEncode(context.Request["taskname"]),
                    AssignedManagerId = managerId,
                    AppointedManagerId = appointedManager.ManagerId,
                    DueDate = DateTime.Now.AddMonths(1),
                    LeadId = lead.Id,
                    CustomerId = customer.Id
                };

                taskId = task.TaskId = ManagerTaskService.AddManagerTask(task);

                // update lead
                //if (lead.LeadStatus == LeadStatus.New)
                //    lead.LeadStatus = LeadStatus.Processing;

                if (!lead.ManagerId.HasValue)
                    lead.ManagerId = managerId;

                LeadService.UpdateLead(lead);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            context.Response.Write(JsonConvert.SerializeObject(new { result = "success", taskid = taskId }));
        }

        private void WriteErrorResponse(HttpContext context, string error)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { result = "error", error = error }));
        }
    }
}