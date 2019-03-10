using System;
using System.Web.Mvc;

namespace AdvantShop.Models
{
    public class FractionalNumberModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(Single) &&
                bindingContext.ModelType != typeof(Decimal) &&
                bindingContext.ModelType != typeof(Double))
            {
                return base.BindModel(controllerContext, bindingContext);
            }

            var type = bindingContext.ModelType;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == null) return null;

            if (bindingContext.ModelType == typeof(Single))
            {
                float temp;
                if (float.TryParse(valueProviderResult.AttemptedValue.Replace(".", ","), out temp))
                {
                    return temp;
                }
            }
            if (bindingContext.ModelType == typeof(Decimal))
            {
                decimal temp;
                if (decimal.TryParse(valueProviderResult.AttemptedValue.Replace(".", ","), out temp))
                {
                    return temp;
                }
            }
            if (bindingContext.ModelType == typeof(Double))
            {
                double temp;
                if (double.TryParse(valueProviderResult.AttemptedValue.Replace(".", ","), out temp))
                {
                    return temp;
                }
            }
            return null;
        }
    }
}