using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Models.CustomerFieldValues;

namespace AdvantShop.Web.Admin.Models.CustomerFields
{
    public class AdminCustomerFieldModel
    {
        public AdminCustomerFieldModel()
        {
            FieldValues = new List<AdminCustomerFieldValueModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public CustomerFieldType FieldType { get; set; }
        public int SortOrder { get; set; }
        public bool Required { get; set; }
        public bool ShowInClient { get; set; }
        public bool Enabled { get; set; }

        public List<AdminCustomerFieldValueModel> FieldValues { get; set; }

        public string FieldTypeFormatted { get { return FieldType.Localize(); } }
        public bool HasValues { get { return FieldType == CustomerFieldType.Select; } }
    }
}
