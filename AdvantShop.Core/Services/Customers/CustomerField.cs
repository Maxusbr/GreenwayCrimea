using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Customers
{
    public class CustomerField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CustomerFieldType FieldType { get; set; }
        public int SortOrder { get; set; }
        public bool Required { get; set; }
        public bool Enabled { get; set; }
        public bool ShowInClient { get; set; }
    }

    public class CustomerFieldWithValue : CustomerField
    {
        private string _value;
        public string Value
        {
            get
            {
                if (FieldType == CustomerFieldType.Date)
                {
                    var dt = _value.TryParseDateTime(true);
                    return dt.HasValue ? dt.Value.ToString("yyyy-MM-dd") : null;
                }
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public List<SelectListItem> Values
        {
            get
            {
                if (FieldType == CustomerFieldType.Select)
                {
                    var list = new List<SelectListItem>() { new SelectListItem() { Text = "---", Value = "" } };

                    var items = CustomerFieldService.GetCustomerFieldValues(Id);
                    if (items != null && items.Count > 0)
                    {
                        list.AddRange(items.Select(x => new SelectListItem()
                        {
                            Text = x.Value,
                            Value = x.Value,
                            Selected = x.Value == Value
                        }));
                    }

                    return list;
                }

                return new List<SelectListItem>();
            }
        }
    }
}
