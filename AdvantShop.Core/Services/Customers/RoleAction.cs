using System;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Customers
{
    public enum Role
    {
        [Localize("Core.Customers.Role.User")]
        User = 0,
        [Localize("Core.Customers.Role.Moderator")]
        Moderator = 50,
        [Localize("Core.Customers.Role.Administrator")]
        Administrator = 100,
        [Localize("Core.Customers.Role.Guest")]
        Guest = 150
    }
    
    public enum RoleAction
    {
        [Localize("Core.Customers.RoleActionCategory.None")]
        None,

        [Localize("Core.Customers.RoleActionCategory.Catalog")]
        Catalog,

        [Localize("Core.Customers.RoleActionCategory.Orders")]
        Orders,

        [Localize("Core.Customers.RoleActionCategory.Customers")]
        Customers,
        
        [Localize("Core.Customers.RoleActionCategory.Cms")]
        Cms,

        [Localize("Core.Customers.RoleActionCategory.Modules")]
        Modules,

        [Localize("Core.Customers.RoleActionCategory.Design")]
        Design,

        [Localize("Core.Customers.RoleActionCategory.Marketing")]
        Marketing,
        
        [Localize("Core.Customers.RoleActionCategory.Settings")]
        Settings,

        [Localize("Core.Customers.RoleActionCategory.Crm")]
        Crm,

        [Localize("Core.Customers.RoleActionCategory.BonusSystem")]
        BonusSystem,

        [Localize("Core.Customers.RoleActionCategory.Tasks")]
        Tasks,

        //[Localize("Core.Customers.RoleActionCategory.Landing")]
        //Landing
    }
    
    public class CustomerRoleAction
    {
        public CustomerRoleAction()
        {
            CustomerId = Guid.Empty;
        }

        public Guid CustomerId { get; set; }        
        public RoleAction Role { get; set; }
    }

    /// <summary>
    /// Какие заказы может видеть менеджер 
    /// </summary>
    public enum ManagersOrderConstraint
    {
        /// <summary>
        /// Все заказы
        /// </summary>
        [Localize("Core.Customers.ManagersOrderConstraint.All")]
        All = 0,

        /// <summary>
        /// Только назначенные заказы
        /// </summary>
        [Localize("Core.Customers.ManagersOrderConstraint.Assigned")]
        Assigned = 1,

        /// <summary>
        /// Назначенные и свободные заказы
        /// </summary>
        [Localize("Core.Customers.ManagersOrderConstraint.AssignedAndFree")]
        AssignedAndFree = 2
    }

    /// <summary>
    /// Какие лиды может видеть менеджер
    /// </summary>
    public enum ManagersLeadConstraint
    {
        [Localize("Core.Customers.ManagersLeadConstraint.All")]
        All = 0,

        [Localize("Core.Customers.ManagersLeadConstraint.Assigned")]
        Assigned = 1,

        [Localize("Core.Customers.ManagersLeadConstraint.AssignedAndFree")]
        AssignedAndFree = 2
    }

	public enum CustomerGroupType
	{
		[Localize("Core.Customers.CustomerGroupType.Buyer")]
		Buyer = 0,

		[Localize("Core.Customers.CustomerGroupType.Partner")]
		Partner = 1
	}
}