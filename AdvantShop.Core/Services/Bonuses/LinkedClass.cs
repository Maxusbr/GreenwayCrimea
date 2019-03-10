using System;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Services.Bonuses
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class LinkedClass : Attribute, IAttribute<Type>
    {
        private readonly Type _name;

        public LinkedClass(Type type)
        {
            _name = type;
        }

        public Type Value
        {
            get { return _name; }
        }
    }
}
