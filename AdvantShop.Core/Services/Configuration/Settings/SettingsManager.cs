using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;

namespace AdvantShop.Configuration
{
    public class SettingsManager
    {
        public static ManagersOrderConstraint ManagersOrderConstraint
        {
            get { return (ManagersOrderConstraint)SettingProvider.Items["ManagersOrderConstraint"].TryParseInt(); }
            set { SettingProvider.Items["ManagersOrderConstraint"] = ((int)value).ToString(); }
        }

        public static ManagersLeadConstraint ManagersLeadConstraint
        {
            get { return (ManagersLeadConstraint)SettingProvider.Items["ManagersLeadConstraint"].TryParseInt(); }
            set { SettingProvider.Items["ManagersLeadConstraint"] = ((int)value).ToString(); }
        }
    }
}