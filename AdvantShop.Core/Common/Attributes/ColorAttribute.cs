using System;

namespace AdvantShop.Core.Common.Attributes
{
    public class ColorAttribute : Attribute, IAttribute<string>
    {
        private readonly string _color;

        public ColorAttribute(string color)
        {
            _color = color;
        }

        public string Value
        {
            get
            {
                return _color ?? string.Empty;
                    //string.IsNullOrEmpty(displayName) ? string.Format("[[{0}]]", _resourceKey) : displayName;
            }
        }
    }
}