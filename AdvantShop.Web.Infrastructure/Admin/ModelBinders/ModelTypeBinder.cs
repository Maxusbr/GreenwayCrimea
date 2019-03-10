using System;
using System.Web.Mvc;
using AdvantShop.Diagnostics;

namespace AdvantShop.Web.Infrastructure.Admin.ModelBinders
{
    public class ModelTypeBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var typeValue = bindingContext.ValueProvider.GetValue("ModelType");

            if (typeValue == null)
                return base.CreateModel(controllerContext, bindingContext, modelType);

            try
            {
                var type = Type.GetType((string)typeValue.ConvertTo(typeof(string)), true);
                var model = Activator.CreateInstance(type);
                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, type);

                return model;
            }
            catch (Exception ex)
            {
                Debug.Log.Error("ModelTypeBinder, " + ex.Message, ex);
            }
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}