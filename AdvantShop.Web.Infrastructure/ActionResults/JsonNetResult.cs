using System;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace AdvantShop.Web.Infrastructure.ActionResults
{
    public class JsonNetResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            
            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            
            var result = JsonConvert.SerializeObject(Data); //, Formatting.Indented
            response.Write(result);
        }
    }
}