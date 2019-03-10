using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Core.SQL;
using Newtonsoft.Json;

namespace AdvantShop.Admin.HttpHandlers.Product
{
    /// <summary>
    /// Summary description for GetColors
    /// </summary>
    public class GetColors : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var colors = SQLDataAccess.Query<Color>("Select ColorID, ColorName, SortOrder From Catalog.Color order by  SortOrder, ColorName");
            var dto = colors.Select(x => new { Id = x.ColorId, Name = x.ColorName }).ToList();
            var json = JsonConvert.SerializeObject(dto);
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}