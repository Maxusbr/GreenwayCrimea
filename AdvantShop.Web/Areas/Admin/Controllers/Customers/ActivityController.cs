using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Activity;
using AdvantShop.Web.Admin.Models.Activity;
using BotDetect;

namespace AdvantShop.Web.Admin.Controllers.Customers
{
    [Auth(RoleAction.Customers)]
    public partial class ActivityController : BaseAdminController
    {
        public JsonResult GetEmails(Guid customerId, string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains(".") || customerId == Guid.Empty)
                return Json(null);

            var dataItems = new List<ActivityEmailModel>();

            var emailsSendedBySite = CustomerService.GetEmails(customerId, email);
            if (emailsSendedBySite != null && emailsSendedBySite.Count > 0)
            {
                dataItems.AddRange(emailsSendedBySite.Select(x => new ActivityEmailModel()
                {
                    Subject = x.Subject,
                    EmailAddress = x.EmailAddress,
                    Status = x.Status.Localize(),
                    CreateOn = Localization.Culture.ConvertDateWithoutSeconds(x.CreateOn)
                }));
            }

            var emailsFromMail = CustomerService.GetEmails(email);
            if (emailsFromMail != null && emailsFromMail.Count > 0)
            {
                dataItems.AddRange(emailsFromMail.Select(x => new ActivityEmailModel()
                {
                    Subject = x.Subject + " " + (x.From.ToLower().Contains(SettingsMail.Login.ToLower())
                                    ? "[Исходящее письмо]"
                                    : "[Входящее письмо]"),
                    EmailAddress = x.From,
                    CreateOn = x.DateStr
                }));
            }

            return Json(new { DataItems = dataItems });
        }

        public JsonResult GetCalls(long standardPhone)
        {
            var items =
                standardPhone != 0
                    ? CallService.GetCalls(standardPhone).OrderByDescending(x => x.CallDate)
                    : null;

            var dataItems = items != null
                ? items.Select(x => new
                {
                    CallDate = Localization.Culture.ConvertDateWithoutSeconds(x.CallDate),
                    x.SrcNum,
                    x.DstNum,
                    x.CallAnswerDate,
                    x.Id,
                    OperatorType = x.OperatorType,
                    Type = x.Type.Localize()
                })
                : null;

            return Json(new { DataItems = dataItems });
        }

        public JsonResult GetSmses(Guid customerId, long standardPhone)
        {
            var items =
                standardPhone != 0 && customerId != Guid.Empty
                    ? CustomerService.GetSms(customerId, standardPhone)
                    : null;

            var dataItems = items != null
                ? items.Select(x => new
                {
                    x.Phone,
                    x.Body,
                    Status = x.Status.Localize(),
                    CreateOn = Localization.Culture.ConvertDateWithoutSeconds(x.CreateOn),
                })
                : null;

            return Json(new { DataItems = dataItems });
        }

        public JsonResult GetActions(Guid customerId)
        {
            var model = new GetActions(customerId).Execute();
            return Json(model);
        }
    }
}
