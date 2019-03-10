<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Customers.GetRecordLink" %>

using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.Security;
using AdvantShop.Customers;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Customers
{
    [AuthorizeRole(RoleAction.Crm)]
    public class GetRecordLink : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            var type = context.Request["type"].TryParseEnum<EOperatorType>();
            var callId = context.Request["callId"].TryParseInt();
            if (callId == 0 || type == EOperatorType.None)
                return;

            var @operator = IPTelephonyOperator.GetByType(type);
            context.Response.Write(JsonConvert.SerializeObject(new
            {
                Link = @operator.GetRecordLink(callId)
            }));
        }
    }
}