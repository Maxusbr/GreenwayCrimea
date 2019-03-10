//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Shipping
{
    public interface IShippingOption
    {
        OptionValidationResult Validate();
        string ForMailTemplate();
    }
    
    public interface IShipping
    {
        IEnumerable<BaseShippingOption> GetOptions();
    }

    public static class BaseShippingExtensions
    {
        public static string KeyAttribute(this BaseShipping obj)
        {
            return AttributeHelper.GetAttributeValue<ShippingKeyAttribute, string>(obj);
        }
    }
}