using System.Collections.Generic;
using System.Linq;

namespace AdvantShop.Saas
{
    public class SaasFeaturesExclusion
    {
        protected static Dictionary<string, ESaasProperty> FeaturesPages = new Dictionary<string, ESaasProperty>
        {
            {"managers.aspx", ESaasProperty.HaveCrm},
            {"m_manager.aspx", ESaasProperty.HaveCrm},
            {"departments.aspx", ESaasProperty.HaveCrm},
            {"managerstasks.aspx", ESaasProperty.HaveCrm},
            {"managertask.aspx", ESaasProperty.HaveCrm},
            {"leads.aspx", ESaasProperty.HaveCrm},
            {"editlead.aspx", ESaasProperty.HaveCrm},

            {"leads", ESaasProperty.HaveCrm},
            {"tasks", ESaasProperty.HaveCrm},
            {"taskgroups", ESaasProperty.HaveCrm},
            {"settings/tasks", ESaasProperty.HaveCrm},

            {"calls.aspx", ESaasProperty.HaveTelephony},

            {"tags.aspx", ESaasProperty.HaveTags},
            {"tag.aspx", ESaasProperty.HaveTags},

            {"statistics.aspx", ESaasProperty.DeepAnalytics},
            {"statisticsproductsexportcsv.aspx", ESaasProperty.DeepAnalytics},
            {"statisticscustomersexportcsv.aspx", ESaasProperty.DeepAnalytics},
            //{"statisticsordersexportcsv.aspx", SaasProperty.DeepAnalytics},

            {"landingpage.aspx", ESaasProperty.LandingPage},
            {"landing", ESaasProperty.LandingPage},

            {"checkoutfields.aspx", ESaasProperty.OrderAdditionFields},
            {"bonussystemadmin.aspx", ESaasProperty.BonusSystem}
        };

        public static bool IsAvailblePage(string url)
        {
            if (!SaasDataService.IsSaasEnabled)
            {
                return true;
            }

            url = url.ToLower();

            if (FeaturesPages.Any(item => string.Equals(item.Value, ESaasProperty.HaveCrm) && url.Contains(item.Key)))
            {
                return SaasDataService.CurrentSaasData.HaveCrm;
            }
            if (FeaturesPages.Any(item => string.Equals(item.Value, ESaasProperty.Have1C) && url.Contains(item.Key)))
            {
                return SaasDataService.CurrentSaasData.Have1C;
            }
            if (FeaturesPages.Any(item => string.Equals(item.Value, ESaasProperty.HaveTelephony) && url.Contains(item.Key)))
            {
                return SaasDataService.CurrentSaasData.HaveTelephony;
            }
            if (FeaturesPages.Any(item => string.Equals(item.Value, ESaasProperty.HaveTags) && url.Contains(item.Key)))
            {
                return SaasDataService.CurrentSaasData.HaveTags;
            }
            if (FeaturesPages.Any(item => string.Equals(item.Value, ESaasProperty.HaveCustomerLog) && url.Contains(item.Key)))
            {
                return SaasDataService.CurrentSaasData.HaveCustomerLog;
            }

            if (FeaturesPages.Any(item => string.Equals(item.Value, ESaasProperty.DeepAnalytics) && url.Contains(item.Key)))
            {
                return SaasDataService.CurrentSaasData.DeepAnalytics;
            }

            if (FeaturesPages.Any(item => string.Equals(item.Value, ESaasProperty.LandingPage) && url.Contains(item.Key)))
            {
                return SaasDataService.CurrentSaasData.LandingPage;
            }

            if (FeaturesPages.Any(item => string.Equals(item.Value, ESaasProperty.OrderAdditionFields) && url.Contains(item.Key)))
            {
                return SaasDataService.CurrentSaasData.OrderAdditionFields;
            }

            if (FeaturesPages.Any(item => string.Equals(item.Value, ESaasProperty.BonusSystem) && url.Contains(item.Key)))
            {
                return SaasDataService.CurrentSaasData.BonusSystem;
            }

            return true;
        }
    }
}
