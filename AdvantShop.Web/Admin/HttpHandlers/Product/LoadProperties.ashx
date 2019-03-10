<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Product.LoadProperties" %>

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;

namespace Admin.HttpHandlers.Product
{
    public class LoadProperties : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var propertyId = context.Request["propertyId"].TryParseInt();
            var productId = context.Request["productId"].TryParseInt();

            var property = PropertyService.GetPropertyById(propertyId);
            if (property == null)
                return;

            var prodPropertyValues = PropertyService.GetPropertyValuesByProductId(productId);

            var result = RenderPropertyItem(property, prodPropertyValues.Where(p => p.PropertyId == property.PropertyId).ToList());

            ReturnResult(context, result);
        }

        private static void ReturnResult(HttpContext context, string result)
        {
            context.Response.ContentType = "application/text";
            context.Response.Write(result);
            context.Response.End();
        }


        private string RenderPropertyItem(Property property, List<PropertyValue> selectedValues)
        {
            var result = new StringBuilder();

            foreach (var propertyValue in property.ValuesList.Skip(10))
            {
                result.AppendFormat(
                    "<div class='propval-item'> <label for='propval_{0}'> <input type='checkbox' name='propval_{3}' id='propval_{0}' {2} value='{0}' /> {1} </div>",
                    propertyValue.PropertyValueId, propertyValue.Value,
                    (selectedValues.Find(p => p.PropertyValueId == propertyValue.PropertyValueId) != null ? "checked" : ""),
                    property.PropertyId);
            }
            
            return result.ToString();
        }
    }
}