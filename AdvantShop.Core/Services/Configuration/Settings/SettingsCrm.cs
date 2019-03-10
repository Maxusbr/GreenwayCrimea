using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Core.Services.Configuration.Settings
{
    public class SettingsCrm
    {
        public static int FinalDealStatusId
        {
            get { return SettingProvider.Items["FinalDealStatusId"].TryParseInt(); }
            set { SettingProvider.Items["FinalDealStatusId"] = value.ToString(); }
        }

        /// <summary>
        /// —татус зазказа, если заказ создаетс€ из лида
        /// </summary>
        public static int OrderStatusIdFromLead
        {
            get { return SettingProvider.Items["OrderStatusIdFromLead"].TryParseInt(); }
            set { SettingProvider.Items["OrderStatusIdFromLead"] = value.ToString(); }
        }
    }
}
