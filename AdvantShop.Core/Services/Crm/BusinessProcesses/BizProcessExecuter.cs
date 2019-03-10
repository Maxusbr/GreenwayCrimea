using System;
using AdvantShop.CMS;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.Webhook;
using AdvantShop.Core.Services.Webhook.Models;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Saas;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public class BizProcessExecuter : WebhookExecuter
    {
        public static void OrderAdded(Order order)
        {
            //if (!SaasDataService.IsEnabledFeature(ESaasProperty.HaveCrm))
            //    return;
            var data = (WebhookOrderModel)order;
            data.CurrentCustomerId = CustomerContext.CurrentCustomer != null ? CustomerContext.CurrentCustomer.Id : Guid.Empty;
            MakeSystemRequest("advantshopevents/orderAdded", data, !order.IsFromAdminArea);

            MakeRequestAsync<BizProcessWebhookUrlList, BizProcessWebhookUrl>(x => x.EventType == EBizProcessEventType.OrderCreated, data);
        }

        public static void OrderStatusChanged(Order order)
        {
            //if (!SaasDataService.IsEnabledFeature(ESaasProperty.HaveCrm))
            //    return;
            var data = (WebhookOrderModel)order;
            data.CurrentCustomerId = CustomerContext.CurrentCustomer != null ? CustomerContext.CurrentCustomer.Id : Guid.Empty;
            MakeSystemRequest("advantshopevents/orderStatusChanged", data, !order.IsFromAdminArea);

            MakeRequestAsync<BizProcessWebhookUrlList, BizProcessWebhookUrl>(x => x.EventType == EBizProcessEventType.OrderStatusChanged, data);
        }

        public static void LeadAdded(Lead lead)
        {
            //if (!SaasDataService.IsEnabledFeature(ESaasProperty.HaveCrm))
            //    return;
            var data = (WebhookLeadModel)lead;
            data.CurrentCustomerId = CustomerContext.CurrentCustomer != null ? CustomerContext.CurrentCustomer.Id : Guid.Empty;
            MakeSystemRequest("advantshopevents/leadAdded", data, !lead.IsFromAdminArea);

            MakeRequestAsync<BizProcessWebhookUrlList, BizProcessWebhookUrl>(x => x.EventType == EBizProcessEventType.LeadCreated, data);
        }

        public static void LeadStatusChanged(Lead lead)
        {
            //if (!SaasDataService.IsEnabledFeature(ESaasProperty.HaveCrm))
            //    return;
            var data = (WebhookLeadModel)lead;
            MakeSystemRequest("advantshopevents/leadStatusChanged", data, !lead.IsFromAdminArea);

            MakeRequestAsync<BizProcessWebhookUrlList, BizProcessWebhookUrl>(x => x.EventType == EBizProcessEventType.LeadStatusChanged, data);
        }

        public static void CallMissed(Call call)
        {
            //if (!SaasDataService.IsEnabledFeature(ESaasProperty.HaveCrm))
            //    return;
            var data = (WebhookCallModel)call;
            data.CurrentCustomerId = CustomerContext.CurrentCustomer != null ? CustomerContext.CurrentCustomer.Id : Guid.Empty;
            MakeSystemRequest("advantshopevents/callMissed", data);

            MakeRequestAsync<BizProcessWebhookUrlList, BizProcessWebhookUrl>(x => x.EventType == EBizProcessEventType.CallMissed, data);
        }

        public static void ReviewAdded(Review review)
        {
            //if (!SaasDataService.IsEnabledFeature(ESaasProperty.HaveCrm))
            //    return;
            var data = (WebhookReviewModel)review;
            data.CurrentCustomerId = CustomerContext.CurrentCustomer != null ? CustomerContext.CurrentCustomer.Id : Guid.Empty;
            MakeSystemRequest("advantshopevents/reviewAdded", data);

            MakeRequestAsync<BizProcessWebhookUrlList, BizProcessWebhookUrl>(x => x.EventType == EBizProcessEventType.ReviewAdded, data);
        }

        public static void MessageReply(Customer customer)
        {
            //if (!SaasDataService.IsEnabledFeature(ESaasProperty.HaveCrm))
            //    return;
            var data = (WebhookCustomerModel)customer;
            data.CurrentCustomerId = CustomerContext.CurrentCustomer != null ? CustomerContext.CurrentCustomer.Id : Guid.Empty;
            MakeSystemRequest("advantshopevents/messageReply", data);

            MakeRequestAsync<BizProcessWebhookUrlList, BizProcessWebhookUrl>(x => x.EventType == EBizProcessEventType.MessageReply, data);
        }

        public static void ProcessDeferredTasks()
        {
            //if (!SaasDataService.IsEnabledFeature(ESaasProperty.HaveCrm))
            //    return;
            MakeSystemRequest("advantshopevents/processDeferredTasks");
        }
    }
}