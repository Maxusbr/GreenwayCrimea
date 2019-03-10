using System;

namespace AdvantShop.Core.Common.Attributes
{
    public class ShippingKeyAttribute : Attribute, IAttribute<string>
    {
        private readonly string _name;
        public ShippingKeyAttribute(string name)
        {
            _name = name;
        }

        public string Value
        {
            get { return _name; }
        }
    }

    public class PaymentKeyAttribute : Attribute, IAttribute<string>
    {
        private readonly string _name;
        public PaymentKeyAttribute(string name)
        {
            _name = name;
        }

        public string Value
        {
            get { return _name; }
        }
    }

    public class StringNameAttribute : Attribute, IAttribute<string>
    {
        private readonly string _name;
        public StringNameAttribute(string name)
        {
            _name = name;
        }

        public string Value
        {
            get { return _name; }
        }
    }

    public class ExportFeedKeyAttribute : Attribute, IAttribute<string>
    {
        private readonly string _name;
        public ExportFeedKeyAttribute(string name)
        {
            _name = name;
        }

        public string Value
        {
            get { return _name; }
        }
    }
}