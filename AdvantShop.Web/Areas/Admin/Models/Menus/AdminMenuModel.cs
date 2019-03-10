using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using AdvantShop.Customers;
using AdvantShop.Saas;

namespace AdvantShop.Web.Admin.Models.Menus
{
    [Serializable]
    public class AdminMenuModel
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public Dictionary<string, object> Route { get; set; }
        
        public RouteValueDictionary RouteDictionary { get { return Route != null ? new RouteValueDictionary(Route) : null; } }

        public string Name { get; set; }
        public string Class { get; set; }

        /// <summary>
        /// Роли для модератора
        /// roles - ["None"] - разрешено всем модераторам
        /// roles - [] или null - только админу
        /// roles - ["DisplayCatalog", "DisplayMainPageBestsellers"] - если есть обе роли, то разрешено
        /// </summary>
        public List<RoleAction> Roles { get; set; }

        public bool Visible { get; private set; }
        
        public bool IsHidden { get; set; }
        public bool Selected { get; set; }
        public bool ActiveInSaas { get; set; }

        public ESaasProperty? SaasFeature { get; set; }

        public List<AdminMenuModel> MenuItems { get; set; }

        public string Icon { get; set; }
        public string StatisticsDataType { get; set; }

        public bool IsEmptyUrl()
        {
            return string.IsNullOrWhiteSpace(Controller) || string.IsNullOrWhiteSpace(Action);
        }
        
        public bool IsAccessibleToUser()
        {
            if (!Visible && MenuItems != null)
            {
                Controller = null;
                Action = null;

                return MenuItems.Any(HasVisible);
            }

            return Visible;
        }

        public void SetVisible(AdminMenuModel menuItem)
        {
            var customer = CustomerContext.CurrentCustomer;

            if (customer.IsAdmin && !IsHidden) //  || customer.IsVirtual
            {
                Visible = true;
            }
            else if (Roles == null || Roles.Count == 0)
            {
                Visible = false;
            }
            else
            {
                Visible = true;

                foreach (var role in Roles)
                {
                    if (role == RoleAction.None)
                        continue;

                    Visible &= RoleActionService.GetCustomerRoleActionsByCustomerId(customer.Id).Any(item => item.Role == role) && !IsHidden;
                }
            }

            if (menuItem.MenuItems != null)
            {
                foreach (var item in menuItem.MenuItems)
                {
                    item.SetVisible(item);
                }
            }
        }

        public bool SetSelected(string action, string controller)
        {
            if (Controller == null || Action == null)
            {
                return false;
            }

            if (Controller.ToLower() == controller && Action.ToLower() == action)
            {
                Selected = true;
                return true;
            }

            if (MenuItems != null)
            {
                foreach (var item in MenuItems)
                {
                    if (item.SetSelected(action, controller))
                    {
                        Selected = true;
                        return true;
                    }
                }
            }

            return false;
        }

        public void SetActiveInSaas(AdminMenuModel menuItem)
        {
            ActiveInSaas = true;
            if (SaasFeature.HasValue)
            {
                ActiveInSaas &= SaasDataService.IsEnabledFeature(SaasFeature.Value);
            }

            if (menuItem.MenuItems != null)
            {
                foreach (var item in MenuItems)
                {
                    item.SetActiveInSaas(item);                
                }
            }
        }

        private bool HasVisible(AdminMenuModel item)
        {
            if (item.MenuItems == null)
                return item.Visible;

            foreach (var subItem in item.MenuItems)
            {
                if (subItem.Visible)
                    return true;
                if (subItem.MenuItems != null && subItem.MenuItems.Count > 0 && HasVisible(subItem))
                    return true;
            }
            return false;
        }
    }
}