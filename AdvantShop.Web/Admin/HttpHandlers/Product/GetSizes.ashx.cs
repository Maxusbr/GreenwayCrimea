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
    /// Summary description for GetSizes
    /// </summary>
    public class GetSizes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var sizes = SQLDataAccess.Query<Size>("Select SizeID, SizeName, SortOrder From Catalog.Size order by  SortOrder, SizeName");
            var dto = sizes.Select(x => new { Id = x.SizeId, Name = x.SizeName }).ToList();
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