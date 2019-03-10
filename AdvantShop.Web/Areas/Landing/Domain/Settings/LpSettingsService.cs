using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.Caching;
using AdvantShop.Core.SQL;

namespace AdvantShop.App.Landing.Domain.Settings
{
    public class LpSettingsService
    {
        private const double SettingCacheTime = 20;
        private const string SettingCacheName = "Landing_Setting_";


        public void AddOrUpdate(int landingId, string name, string value)
        {
            SQLDataAccess.ExecuteNonQuery(
                "If ((Select Count(*) From [CMS].[LandingSettings] Where LandingId = @LandingId and Name = @name) > 0) " +
                " Update [CMS].[LandingSettings] Set [Value] = @Value Where LandingId = @LandingId and Name = @name " +
                "Else " +
                " Insert Into [CMS].[LandingSettings] ([LandingId],[Name],[Value]) Values (@LandingId,@Name,@Value) ",
                CommandType.Text,
                new SqlParameter("@LandingId", landingId),
                new SqlParameter("@Name", name),
                new SqlParameter("@Value", value ?? string.Empty)
                );

            CacheManager.RemoveByPattern(SettingCacheName);
        }

        public string Get(int landingId, string name)
        {
            return
                CacheManager.Get(SettingCacheName + landingId + name, SettingCacheTime,
                    () =>
                    {
                        var setting =
                            SQLDataAccess.Query<LpSettings>(
                                "Select * From [CMS].[LandingSettings] Where LandingId = @landingId and Name = @name",
                                new {landingId, name}).FirstOrDefault();

                        return setting != null ? setting.Value : string.Empty;
                    });
        }

    }
}
