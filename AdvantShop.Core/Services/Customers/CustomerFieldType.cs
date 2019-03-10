using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Services.Crm.BusinessProcesses;

namespace AdvantShop.Customers
{
    public enum CustomerFieldType
    {
        [Localize("Core.Customers.CustomerFieldType.Select"), FieldType(EFieldType.Select)]
        Select = 0,
        [Localize("Core.Customers.CustomerFieldType.Text"), FieldType(EFieldType.Text)]
        Text = 1,
        [Localize("Core.Customers.CustomerFieldType.Number"), FieldType(EFieldType.Number)]
        Number = 2,
        [Localize("Core.Customers.CustomerFieldType.TextArea"), FieldType(EFieldType.Text)]
        TextArea = 3,
        [Localize("Core.Customers.CustomerFieldType.Date"), FieldType(EFieldType.Date)]
        Date = 4
    }
}
