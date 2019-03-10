using System;
using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Customers;

namespace AdvantShop.Core.Services.Security
{
    public class AuthorizeRoleAttribute : Attribute, IAttribute<List<RoleAction>>
    {
        private List<RoleAction> _keys;

        public AuthorizeRoleAttribute()
        {
            _keys = new List<RoleAction>();
        }

        public AuthorizeRoleAttribute(RoleAction key)
        {
            _keys = new List<RoleAction> {key};
        }

        public AuthorizeRoleAttribute(RoleAction key1, RoleAction key2)
        {
            _keys = new List<RoleAction> { key1, key2 };
        }

        public AuthorizeRoleAttribute(RoleAction key1, RoleAction key2, RoleAction key3)
        {
            _keys = new List<RoleAction> { key1, key2, key3 };
        }

        public AuthorizeRoleAttribute(List<RoleAction> key)
        {
            _keys = key ?? new List<RoleAction>();
        }

        public List<RoleAction> Value { get { return _keys; } }
    }
}
