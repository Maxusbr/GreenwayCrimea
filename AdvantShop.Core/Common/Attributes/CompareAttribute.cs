using System;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Core.Common.Attributes
{
    public class CompareAttribute : Attribute, IAttribute<string>
    {
        private string _key;

        public CompareAttribute(string key)
        {
            _key = key;
        }

        public string Value
        {
            get { return LocalizationService.GetResource(_key); }
        }
    }
}