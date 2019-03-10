using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Controllers.Shared;

namespace AdvantShop.Web.Admin.Filters
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

                if (!(controller.Equals("account") && action.Equals("login")))
                {
                    if (!filterContext.IsChildAction)
                    {
                        var request = filterContext.RequestContext.HttpContext.Request;
                        if (request.IsAjaxRequest())
                            filterContext.Result = new ServiceController().RoleAccessIsDeniedJson(_rolesActionKeys[0]);
                        else
                            filterContext.Result = (ViewResult)new ServiceController().RoleAccessIsDenied(_rolesActionKeys[0]);                     
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
