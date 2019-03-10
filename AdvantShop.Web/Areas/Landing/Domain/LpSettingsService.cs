using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.Caching;
using AdvantShop.Core.SQL;

namespace AdvantShop.App.Landing.Domain
{
    public class LpSettingsService
    {
        private const double SettingCacheTime = 20;
        private const string SettingCacheName = "LandingPage_Setting_";


        public void AddOrUpdate(LpSettings setting)
        {
            SQLDataAccess.ExecuteNonQuery(
                "If ((Select Count(*) From [Module].[LandingPageSettings] Where LandingPageId = @landingPageId and Name = @name) > 0) " +
                " Update [Module].[LandingPageSettings] Set [Value] = @Value Where LandingPageId = @landingPageId and Name = @name " +
                "Else " +
                " Insert Into [Module].[LandingPageSettings] ([LandingPageId],[Name],[Value]) Values (@LandingPageId,@Name,@Value) ",
                CommandType.Text,
                new SqlParameter("@LandingPageId", setting.LandingPageId),
                new SqlParameter("@Name", setting.Name),
                new SqlParameter("@Value", setting.Value ?? string.Empty)
                );

            CacheManager.RemoveByPattern(SettingCacheName);

        }

        public string Get(int landingPageId, string name)
        {
            return
                CacheManager.Get(SettingCacheName + landingPageId + name, SettingCacheTime,
                    () =>
                    {
                        var setting =
                            SQLDataAccess.Query<LpSettings>(
                                "Select * From [Module].[LandingPageSettings] Where LandingPageId = @landingPageId and Name = @name",
                                new {landingPageId, name}).FirstOrDefault();

                        return setting != null ? setting.Value : string.Empty;
                    });
        }

    }
}
