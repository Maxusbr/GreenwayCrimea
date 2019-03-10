using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Configuration
{
    public class SettingsTasks
    {
        public static int DefaultTaskGroup
        {
            get { return SettingProvider.Items["Tasks_DefaultTaskGroup"].TryParseInt(); }
            set { SettingProvider.Items["Tasks_DefaultTaskGroup"] = value.ToString(); }
        }
    }
}