using System;
using System.Linq;
using System.Collections.Generic;

using AdvantShop.Customers;
using AdvantShop.Web.Admin.Models.Users;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Web.Admin.Handlers.Users
{
    public class GetUserRoleActions
    {
        private readonly Guid? _customerId;

        public GetUserRoleActions(Guid? customerId)
        {
            _customerId = customerId;
        }

        public List<UserRoleActionModel> Execute()
        {
            var model = new List<UserRoleActionModel>();

            var customerRoleActions = _customerId.HasValue ? RoleActionService.GetRoleActionsByCustomerId(_customerId.Value) : new List<RoleAction>();
            foreach (RoleAction roleAction in Enum.GetValues(typeof(RoleAction)))
            {
                if (roleAction == RoleAction.None)
                {
                    continue;
                }
                model.Add(new UserRoleActionModel
                {
                    Key = roleAction.ToString(),
                    Name = roleAction.Localize(),
                    Enabled = !_customerId.HasValue || customerRoleActions.Any(item => item == roleAction)
                });
            }

            return model;
        }
    }
}
