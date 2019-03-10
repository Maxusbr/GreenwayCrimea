using System;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Vk;
using Newtonsoft.Json;

namespace AdvantShop.Configuration
{
    public class SettingsVk
    {
        public static string ApplicationId
        {
            get { return SettingProvider.Items["SettingsVk.ApplicationId"]; }
            set { SettingProvider.Items["SettingsVk.ApplicationId"] = value; }
        }

        /// <summary>
        /// Токен с правами только на юзера (потому, что сначала нужно получить группы, а уже потом авторизоваться от имени группы)
        /// </summary>
        public static string TokenUser
        {
            get { return SettingProvider.Items["SettingsVk.TokenUser"]; }
            set { SettingProvider.Items["SettingsVk.TokenUser"] = value; }
        }

        /// <summary>
        /// Токен с правами на группу
        /// </summary>
        public static string TokenGroup
        {
            get { return SettingProvider.Items["SettingsVk.TokenGroup"]; }
            set { SettingProvider.Items["SettingsVk.TokenGroup"] = value; }
        }

        public static int TokenUserErrorCount
        {
            get { return Convert.ToInt32(SettingProvider.Items["SettingsVk.TokenGroupUserCount"]); }
            set { SettingProvider.Items["SettingsVk.TokenGroupUserCount"] = value.ToString(); }
        }

        public static int TokenGroupErrorCount
        {
            get { return Convert.ToInt32(SettingProvider.Items["SettingsVk.TokenGroupErrorCount"]); }
            set { SettingProvider.Items["SettingsVk.TokenGroupErrorCount"] = value.ToString(); }
        }

        public static long UserId
        {
            get { return SettingProvider.Items["SettingsVk.UserId"].TryParseLong(); }
            set { SettingProvider.Items["SettingsVk.UserId"] = value.ToString(); }
        }

        public static VkGroup Group
        {
            get
            {
                var str = SettingProvider.Items["SettingsVk.Group"];
                return !string.IsNullOrWhiteSpace(str) ? JsonConvert.DeserializeObject<VkGroup>(str) : null;
            }
            set
            {
                SettingProvider.Items["SettingsVk.Group"] = value != null ? JsonConvert.SerializeObject(value) : null;
            }
        }

        public static long? LastMessageId
        {
            get { return SettingProvider.Items["SettingsVk.LastMessageId"].TryParseLong(true); }
            set { SettingProvider.Items["SettingsVk.LastMessageId"] = value != null ? value.ToString() : null; }
        }

        public static long? LastSendedMessageId
        {
            get { return SettingProvider.Items["SettingsVk.LastSendedMessageId"].TryParseLong(true); }
            set { SettingProvider.Items["SettingsVk.LastSendedMessageId"] = value != null ? value.ToString() : null; }
        }
    }
}