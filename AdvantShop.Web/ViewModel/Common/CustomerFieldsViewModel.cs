using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using Newtonsoft.Json;

namespace AdvantShop.ViewModel.Common
{
    public class CustomerFieldsViewModel
    {
        public CustomerFieldsViewModel() { }

        public CustomerFieldsViewModel(List<CustomerFieldWithValue> customerFields, string modelName = "", string ngModelName = "", string ngChangeFunc = "", 
            string cssParamName = "col-xs-4", string cssParamValue = "col-xs-8", bool checkFields = false)
        {
            CustomerFields = customerFields;
            ModelName = modelName;
            NgModelName = ngModelName;
            NgChangeFunc = ngChangeFunc;
            CssParamName = cssParamName;
            CssParamValue = cssParamValue;

            if (checkFields)
            {
                var modelFieldValues = CustomerFields != null ? CustomerFields.ToDictionary(x => x.Id, x => x.Value) : new Dictionary<int, string>();
                CustomerFields = CustomerFieldService.GetCustomerFieldsWithValue(Guid.Empty).Where(x => x.ShowInClient).ToList();

                foreach (var field in CustomerFields)
                {
                    if (modelFieldValues.ContainsKey(field.Id))
                        field.Value = modelFieldValues[field.Id];
                }
            }
        }

        public List<CustomerFieldWithValue> CustomerFields { get; set; }

        public string ModelName { get; set; }

        public string NgModelName { get; set; }

        public string NgChangeFunc { get; set; }

        public string CssParamName { get; set; }

        public string CssParamValue { get; set; }


        public string CustomerFieldsSerialized { get { return CustomerFields != null ? JsonConvert.SerializeObject(CustomerFields) : "[]"; } }

        public string GetName()
        {
            return GetName(null, string.Empty);
        }

        public string GetName(int? index, string field)
        {
            var prefix = NgModelName.IsNotEmpty() ? string.Format("{0}.", NgModelName) : string.Empty;
            var postfix = field.IsNotEmpty() ? string.Format(".{0}", field) : string.Empty;
            var indexPart = index.HasValue ? string.Format("[{0}]", index.Value) : string.Empty;
            return string.Format("{0}CustomerFields{1}{2}", prefix, indexPart, postfix);
        }
    }
}