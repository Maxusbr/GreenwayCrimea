<%@ WebHandler Language="C#" Class="AdvantShop.Admin.HttpHandlers.Lead.FindRelatedCustomers" %>

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using Newtonsoft.Json;
using Resources;

namespace AdvantShop.Admin.HttpHandlers.Lead
{
    public class FindRelatedCustomers : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            if (!Authorize(context))
                return;

            var email = context.Request["email"];
            var name = context.Request["name"];
            var phone = context.Request["phone"];

            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(phone))
            {
                WriteErrorResponse(context, Resource.Admin_Leads_FindCustomer_Error_WrongData);
                return;
            }

            var query = "";
            var sqlParams = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(email))
            {
                query += "[Email] like '%' + @email + '%' ";
                sqlParams.Add(new SqlParameter("@email", email));
            }
            
            if (!string.IsNullOrWhiteSpace(name))
            {
                query += (query != "" ? " OR " : "") + "[FirstName] like '%' + @name + '%' OR  [LastName] like '%' + @name + '%'";
                sqlParams.Add(new SqlParameter("@name", name));
            }
            
            if (!string.IsNullOrWhiteSpace(phone))
            {
                query += (query != "" ? " OR " : "") + "[Phone] like @phone + '%' OR StandardPhone like '" +
                         StringHelper.ConvertToStandardPhone(phone) + "%'";
                sqlParams.Add(new SqlParameter("@phone", phone));
            }
            
            var customers =
                SQLDataAccess.ExecuteReadList<Customer>("SELECT Top(10) * FROM [Customers].[Customer] WHERE " + query,
                    CommandType.Text, CustomerService.GetFromSqlDataReader, sqlParams.ToArray());


            context.Response.Write(JsonConvert.SerializeObject(new
            {
                result = "success",
                customers = customers.Select(x => new
                {
                    Id = x.Id,
                    Name = x.FirstName + " " + x.LastName,
                    Email = x.EMail,
                    Phone = x.Phone
                })
            }));
        }

        private void WriteErrorResponse(HttpContext context, string error)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { result = "error", error = error }));
        }
    }
}