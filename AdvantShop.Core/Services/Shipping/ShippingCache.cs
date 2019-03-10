using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.Shipping;
using Newtonsoft.Json;
using AdvantShop.Diagnostics;

namespace AdvantShop.Core.Services.Shipping
{
    //public interface ICachebleShipping
    //{
    //    IEnumerable<BaseShippingOption> GetOptionsCache();
    //    int GetHashForCache();
    //}

    public class ShippingCache
    {
        public int ShippingMethodId { get; set; }
        public int ParamHash { get; set; }
        public IEnumerable<BaseShippingOption> Options { get; set; }
        public DateTime Created { get; set; }
    }

    public class ShippingCacheRepositiry
    {
        public static ShippingCache Get(int shippingMethodId, int paramHash)
        {
            var set = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            };
            return SQLDataAccess.ExecuteReadOne<ShippingCache>("Select * from [Order].ShippingCache where ShippingMethodID=@ShippingMethodID and ParamHash=@ParamHash",
                                                                CommandType.Text, reader => new ShippingCache
                                                                {
                                                                    ShippingMethodId = SQLDataHelper.GetInt(reader, "ShippingMethodID"),
                                                                    ParamHash = SQLDataHelper.GetInt(reader, "ParamHash"),
                                                                    Options = JsonConvert.DeserializeObject<List<BaseShippingOption>>(SQLDataHelper.GetString(reader, "Options"), set),
                                                                    Created = SQLDataHelper.GetDateTime(reader, "Created"),
                                                                },
                                                                new SqlParameter("@ShippingMethodID", shippingMethodId),
                                                                new SqlParameter("@ParamHash", paramHash));
        }

        public static bool Exist(int shippingMethodId, int paramHash)
        {
            return
                SQLDataAccess.ExecuteScalar<int>("Select count(*) from [Order].ShippingCache where ShippingMethodID=@ShippingMethodID and ParamHash=@ParamHash",
                                                CommandType.Text,
                                                new SqlParameter("@ShippingMethodID", shippingMethodId),
                                                new SqlParameter("@ParamHash", paramHash)) > 0;
        }

        public static void Add(ShippingCache model)
        {
            var set = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            };
            try
            {
                SQLDataAccess.ExecuteNonQuery("Insert into [Order].ShippingCache (ShippingMethodID,ParamHash,Options,Created) VALUES (@ShippingMethodID,@ParamHash,@Options,@Created)",
                    CommandType.Text,
                    new SqlParameter("@ShippingMethodID", model.ShippingMethodId),
                    new SqlParameter("@ParamHash", model.ParamHash),
                    new SqlParameter("@Options", JsonConvert.SerializeObject(model.Options, set)),
                    new SqlParameter("@Created", DateTime.Now));
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        public static void Update(ShippingCache model)
        {
            var set = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            };
            SQLDataAccess.ExecuteNonQuery("Update [Order].ShippingCache set Options= @Options where ParamHash = @ParamHash and ShippingMethodID=@ShippingMethodID",
                CommandType.Text,
                new SqlParameter("@ShippingMethodID", model.ShippingMethodId),
                new SqlParameter("@ParamHash", model.ParamHash),
                new SqlParameter("@Options", JsonConvert.SerializeObject(model.Options, set)),
                new SqlParameter("@Created", DateTime.Now));
        }

        public static void Delete()
        {
            SQLDataAccess.ExecuteNonQuery("Delete from [Order].ShippingCache where Created < @Created", CommandType.Text, new SqlParameter("@Created", DateTime.Now.AddDays(-7)));
        }

        public static void Delete(int id)
        {
            SQLDataAccess.ExecuteNonQuery("Delete from [Order].ShippingCache where ShippingMethodID = @ShippingMethodID", CommandType.Text, new SqlParameter("@ShippingMethodID", id));
        }
    }
}
