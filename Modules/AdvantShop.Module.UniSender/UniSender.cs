//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Customers;
using AdvantShop.Module.UniSender.Domain;
using AdvantShop.Core.Services.Mails;

namespace AdvantShop.Module.UniSender
{
    public class UniSender : ISendMails
    {
        public static string ModuleID
        {
            get { return "UniSender"; }
        }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "UniSender";

                    case "en":
                        return "UniSender";

                    default:
                        return "UniSender";
                }
            }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleID; }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new UniSenderSettingsControl() }; }
        }

        public bool HasSettings
        {
            get { return true; }
        }

        public bool CheckAlive()
        {
            return ModulesRepository.IsInstallModule(ModuleID);
        }

        public bool InstallModule()
        {
            ModuleSettingsProvider.SetSettingValue("UniSenderId", string.Empty, ModuleID);
            ModuleSettingsProvider.SetSettingValue("UniSenderFromName", string.Empty, ModuleID);
            ModuleSettingsProvider.SetSettingValue("UniSenderFromEmail", string.Empty, ModuleID);
            ModuleSettingsProvider.SetSettingValue("UniSenderRegUsersList", string.Empty, ModuleID);
            return true;
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("UniSenderId", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("UniSenderFromName", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("UniSenderFromEmail", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("UniSenderRegUsersList", ModuleID);
            ModuleSettingsProvider.RemoveSqlSetting("UniSenderOrderCustomersList", ModuleID);
            return true;
        }

        public bool UpdateModule()
        {
            ModuleSettingsProvider.SetSettingValue("UniSenderOrderCustomersList", string.Empty, ModuleID);
            return true;
        }

        private class UniSenderSettingsControl : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Настройки";

                        case "en":
                            return "Settings";

                        default:
                            return "Settings";
                    }
                }
            }

            public string File
            {
                get { return "UniSenderSettings.ascx"; }
            }

            #endregion
        }

        public bool SendMails(string subject, string message, EMailRecipientType recipientType)
        {
            bool result = false;

            if (recipientType.HasFlag(EMailRecipientType.Subscriber) && !string.IsNullOrEmpty(UniSenderSettings.RegUsersList))
                result |= UniSenderService.SendMail(UniSenderSettings.RegUsersList, subject, message);

            if (recipientType.HasFlag(EMailRecipientType.OrderCustomer) && !string.IsNullOrEmpty(UniSenderSettings.OrderCustomersList))
                result |= UniSenderService.SendMail(UniSenderSettings.OrderCustomersList, subject, message);

            return result;
        }

        public void SubscribeEmail(ISubscriber subscriber)
        {
            if (subscriber.CustomerType == EMailRecipientType.Subscriber || subscriber.CustomerType == EMailRecipientType.None)
            {
                if (!string.IsNullOrEmpty(UniSenderSettings.RegUsersList))
                    UniSenderService.SubscribeListMembers(UniSenderSettings.RegUsersList, new List<ISubscriber> { subscriber });
            }
            else if (subscriber.CustomerType == EMailRecipientType.OrderCustomer)
            {
                if (!string.IsNullOrEmpty(UniSenderSettings.OrderCustomersList))
                    UniSenderService.SubscribeListMembers(UniSenderSettings.OrderCustomersList, new List<ISubscriber> { subscriber });
            }
        }

        public void UnsubscribeEmail(string email)
        {
            if (!string.IsNullOrEmpty(UniSenderSettings.RegUsersList))
                UniSenderService.UnsubscribeListMembers(UniSenderSettings.RegUsersList,
                    new List<UniSenderListMember> { new UniSenderListMember { Email = email } });
        }
    }
}