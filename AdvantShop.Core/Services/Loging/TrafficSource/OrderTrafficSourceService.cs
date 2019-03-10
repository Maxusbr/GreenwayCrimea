using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Services.Loging.TrafficSource
{
    public class OrderTrafficSource
    {
        public int Id { get; set; }
        public int ObjId { get; set; }
        public TrafficSourceType ObjType { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime CreateOn { get; set; }
        public string Referrer { get; set; }
        public string Url { get; set; }
        public string utm_source { get; set; }
        public string utm_medium { get; set; }
        public string utm_campaign { get; set; }
        public string utm_content { get; set; }
        public string utm_term { get; set; }

        public string GoogleClientId { get; set; }
    }

    public enum TrafficSourceType
    {
        Order = 0,
        Lead = 1,
    }

    public static class OrderTrafficSourceService
    {
        public static void Add(int objId, TrafficSourceType objType, TrafficSource source)
        {
            SQLDataAccess.ExecuteNonQuery(
                "INSERT INTO [Order].[OrderTrafficSource] ([ObjId],[ObjType],[CustomerId],[CreateOn],[Referrer],[Url],[utm_source],[utm_medium],[utm_campaign],[utm_content],[utm_term],[GoogleClientId]) " +
                "VALUES (@ObjId,@ObjType,@CustomerId,@CreateOn,@Referrer,@Url,@utm_source,@utm_medium,@utm_campaign,@utm_content,@utm_term,@GoogleClientId) ",
                CommandType.Text,
                new SqlParameter("@ObjId", objId),
                new SqlParameter("@ObjType", (int)objType),
                new SqlParameter("@CustomerId", source.CustomerId),
                new SqlParameter("@CreateOn", source.CreateOn),
                new SqlParameter("@Referrer", source.Referrer ?? (object) DBNull.Value),
                new SqlParameter("@Url", source.Url ?? (object)DBNull.Value),
                new SqlParameter("@utm_source", source.utm_source ?? (object)DBNull.Value),
                new SqlParameter("@utm_medium", source.utm_medium ?? (object)DBNull.Value),
                new SqlParameter("@utm_campaign", source.utm_campaign ?? (object)DBNull.Value),
                new SqlParameter("@utm_content", source.utm_content ?? (object)DBNull.Value),
                new SqlParameter("@utm_term", source.utm_term ?? (object)DBNull.Value),
                new SqlParameter("@GoogleClientId", GetGoogleClientId() ?? (object)DBNull.Value)
            );
        }

        public static OrderTrafficSource Get(int objId, TrafficSourceType objType)
        {
            return
                SQLDataAccess.Query<OrderTrafficSource>(
                    "Select * From [Order].[OrderTrafficSource] Where ObjId=@objId and ObjType=@objType",
                    new {objId, objType = (int) objType}).FirstOrDefault();
        }

        public static string GetGoogleClientId()
        {
            var ga = CommonHelper.GetCookie("_ga");
            if (ga != null && !string.IsNullOrEmpty(ga.Value))
            {
                if (ga.Value.StartsWith("GA"))
                {
                    var gaArr = ga.Value.Split(new[] { "." }, StringSplitOptions.None);
                    if (gaArr.Length == 4)
                        return gaArr[2] + "." + gaArr[3];
                }
                return ga.Value;
            }
            return null;
        }
    }
}
