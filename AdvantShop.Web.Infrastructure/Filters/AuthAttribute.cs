using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;

namespace AdvantShop.Web.Infrastructure.Filters
{
    public class AuthAttribute : ActionFilterAttribute
    {
        private List<RoleAction> _rolesActionKeys;

        public AuthAttribute()
        {
            _rolesActionKeys = new List<RoleAction>();
        }

        public AuthAttribute(RoleAction key)
        {
            _rolesActionKeys = new List<RoleAction> { key };
        }

        public AuthAttribute(RoleAction key1, RoleAction key2)
        {
            _rolesActionKeys = new List<RoleAction> { key1, key2 };
        }

        public AuthAttribute(RoleAction key1, RoleAction key2, RoleAction key3)
        {
            _rolesActionKeys = new List<RoleAction> { key1, key2, key3 };
        }

        public AuthAttribute(List<RoleAction> key)
        {
            _rolesActionKeys = key ?? new List<RoleAction>();
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var customer = CustomerContext.CurrentCustomer;

            if (customer.IsModerator && !HasRole(customer))
            {
                var controller = filterContext.RouteData.Values["controller"].ToString().ToLower();
                var action = filterContext.RouteData.Values["action"].ToString().ToLower();

                if (!(controller.Equals("error") && action.Equals("autherror")))
                {
                    if (!filterContext.IsChildAction)
                    {                        
                        filterContext.Result = new RedirectResult(UrlService.GetAdminUrl("service/RoleAccessIsDenied?roleActionKey=" + _rolesActionKeys[0].ToString()));
                    }
                    else
                    {
                        filterContext.Result = new EmptyResult();
                    }
                }
            }
        }

        private bool HasRole(Customer customer)
        {
            var customerRolesActions = RoleActionService.GetCustomerRoleActionsByCustomerId(customer.Id);

            foreach (var actionKey in _rolesActionKeys)
            {
                if (actionKey != RoleAction.None && customerRolesActions.Any(item => item.Role == actionKey))
                    return true;
            }

            return false;
        }
    }
}
