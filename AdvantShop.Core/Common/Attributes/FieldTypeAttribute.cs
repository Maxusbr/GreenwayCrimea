using System;
using AdvantShop.Core.Services.Crm.BusinessProcesses;

namespace AdvantShop.Core.Common.Attributes
{
    public class FieldTypeAttribute : Attribute, IAttribute<EFieldType>
    {
        private EFieldType _type;

        public FieldTypeAttribute(EFieldType type)
        {
            _type = type;
        }

        public EFieldType Value
        {
            get { return _type; }
        }
    }
}