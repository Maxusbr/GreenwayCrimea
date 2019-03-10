<%@ WebHandler Language="C#" Class="AutocompleteTags" %>

using System.Linq;
using System.Web;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.UrlRewriter;
using Newtonsoft.Json;

public class AutocompleteTags : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        var name = context.Request["data[q]"];
        //var objId = context.Request["objId"];
        //var 
        var temp = TagService.Gets(name);
        if (!temp.Any())
        {
            if (UrlService.IsAvailableUrl(ParamType.Tag, name))
            {
                temp.Add(new Tag() { Id = 0, Name = name });
            }
        }
        context.Response.ContentType = "application/json";
        var resonse = new
        {
            q = name,
            results = temp.Select(x => new { id = x.Name, text = x.Name }).ToList()
        };
        context.Response.Write(JsonConvert.SerializeObject(resonse));
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}