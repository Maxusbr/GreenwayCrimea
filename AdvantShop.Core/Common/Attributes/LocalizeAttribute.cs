using System;
//using Resources;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Core.Common.Attributes
{
    public class ResourceKeyAttribute : Attribute, IAttribute<string>
    {
        private readonly string _resourceKey;

        public ResourceKeyAttribute(string resourceKey)
        {
            _resourceKey = resourceKey;
        }

        public string Value
        {
            get
            {
                var displayName = LocalizationService.GetResource(_resourceKey); //Resource.ResourceManager.GetString(_resourceKey);
                return string.IsNullOrEmpty(displayName) ? string.Format("[[{0}]]", _resourceKey) : displayName;
            }
        }
    }
}