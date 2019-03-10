using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Services.Security;
using AdvantShop.Customers;
using AdvantShop.Security;


namespace AdvantShop.Core.HttpHandlers
{
    public abstract class AdminHandler : IHttpHandler, IRequiresSessionState
    {
        protected AdminHandler()
        {
            Localization.Culture.InitializeCulture();
        }

        public bool Authorize(HttpContext context)
        {
            var customer = CustomerContext.CurrentCustomer;

            if (customer.IsAdmin || customer.IsVirtual || Trial.TrialService.IsTrialEnabled)
            {
                return true;
            }

            if (customer.CustomerRole == Role.Moderator)
            {
                if (RoleAccess.Check(customer, context.Request.Url.ToString().ToLower()))
                    return true;

                var attrValue = AttributeHelper.GetAttributeValue<AuthorizeRoleAttribute, List<RoleAction>>(this);
                if (attrValue != null)
                {
                    foreach (var actionKey in attrValue)
                    {
                        if (actionKey != RoleAction.None && customer.HasRoleAction(actionKey))
                            return true;
                    }
                }
            }

            context.Response.Clear();
            context.Response.StatusCode = 403;
            context.Response.Status = "403 Forbidden";
            return false;
        }

        public virtual void ProcessRequest(HttpContext context)
        {
            throw new System.NotImplementedException();
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}