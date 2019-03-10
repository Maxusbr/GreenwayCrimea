using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
namespace AdvantShop.Handlers.ProductDetails
{
    public class GetProductCustomOptionsHandler
    {
        #region Fields

        private readonly int _productId;
        private readonly string _selectedOptions;

        #endregion

        public GetProductCustomOptionsHandler(int productId, string selectedOptions)
        {
            _productId = productId;
            _selectedOptions = selectedOptions;
        }

        public string Get()
        {
            var kvSelectedOptions = new List<KeyValuePair<int, string>>();
            var customOptions = CustomOptionsService.GetCustomOptionsByProductId(_productId);
            var selectedOptions = new List<OptionItem>();

            if (_selectedOptions.IsNotEmpty())
            {
                foreach (var selectedOption in _selectedOptions.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    //var option = selectedOption.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                    //if (option.Length == 2)
                    if (selectedOption.Contains('_'))
                    {
                        var optionKey = selectedOption.Substring(0, selectedOption.IndexOf('_'));
                        var optionValue = selectedOption.Substring(selectedOption.IndexOf('_') + 1);
                        //kvSelectedOptions.Add(new KeyValuePair<int, string>(option[0].TryParseInt(), HttpUtility.HtmlEncode(option[1])));
                        kvSelectedOptions.Add(new KeyValuePair<int, string>(optionKey.TryParseInt(), HttpUtility.HtmlEncode(optionValue)));
                    }
                }

                if (kvSelectedOptions.Count == 0)
                    return string.Empty;

                var index = 0;
                foreach (var customOption in customOptions)
                {
                    var kvOption = kvSelectedOptions.Find(x => x.Key == index);

                    if (customOption.InputType == CustomOptionInputType.DropDownList || customOption.InputType == CustomOptionInputType.RadioButton)
                    {
                        selectedOptions.Add(!kvOption.Equals(default(KeyValuePair<int, string>))
                                                ? customOption.Options.WithId(kvOption.Value.TryParseInt())
                                                : null);
                    }

                    if (customOption.InputType == CustomOptionInputType.CheckBox)
                    {
                        selectedOptions.Add((!kvOption.Equals(default(KeyValuePair<int, string>)) || customOption.IsRequired) && kvOption.Value == "1"
                                                ? customOption.Options[0]
                                                : null);
                    }

                    if (customOption.InputType == CustomOptionInputType.TextBoxMultiLine || customOption.InputType == CustomOptionInputType.TextBoxSingleLine)
                    {
                        customOption.Options[0].Title = kvOption.Value;
                        selectedOptions.Add(!kvOption.Equals(default(KeyValuePair<int, string>)) || customOption.IsRequired
                                                ? customOption.Options[0]
                                                : null);
                    }
                    index++;
                }
            }
            else
            {
                foreach (var item in customOptions.Where(x => x.IsRequired))
                {
                    var firstOption = item.Options.FirstOrDefault();
                    if (firstOption != null)
                    {
                        selectedOptions.Add(firstOption);
                    }
                }
            }

            var attributesXml = CustomOptionsService.SerializeToXml(customOptions, selectedOptions) ?? string.Empty;

            return attributesXml;
        }
    }
}