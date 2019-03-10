//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Xml;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;

namespace AdvantShop.Core
{
    public class ModeConfigService
    {
        private const string ModeConfigCacheKey = "ModeConfig.";

        public enum Modes
        {
            DemoMode,
            TrialMode,
            SaasMode
        }

        public static bool IsModeEnabled(Modes mode)
        {
            var cacheKey = ModeConfigCacheKey + mode;

            bool value;

            if (!CacheManager.TryGetValue(cacheKey, out value))
            {
                var doc = new XmlDocument();
                doc.Load(SettingsGeneral.AbsolutePath + "Web.ModeSettings.config");

                var root = doc.ChildNodes.OfType<XmlNode>().First(p => p.Name.Equals("modesettings"));
                if (root != null)
                {
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        if (node.Name == mode.ToString() && node.Attributes != null)
                        {
                            value = Boolean.Parse(node.Attributes["value"].Value);
                            CacheManager.Insert(cacheKey, value, 60);

                            return value;
                        }
                    }
                }
                throw new NotImplementedException("this mode is not configured in ~/Web.ModeSettings.config");
            }
            return value;
        }
    }
}