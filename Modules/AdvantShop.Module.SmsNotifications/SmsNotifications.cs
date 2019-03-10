using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Customers;
using AdvantShop.Module.SmsNotifications.Services;
using AdvantShop.Orders;
using AdvantShop.Module.SmsNotifications.Status;

namespace AdvantShop.Module.SmsNotifications
{
    public class SmsNotifications : IModuleSms, ISendOrderNotifications, IAdminBundles
    {
        public static readonly string ModuleId = "SmsNotifications";

        #region Implementation of IModule

        public string ModuleStringId
        {
            get { return ModuleId; }
        }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "СМС информирование";
                    case "en":
                        return "SMS Inform";
                    default:
                        return "SMS Inform";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new SmsNotificationsSetting(), new SmsNotificationsSending() }; }
        }

        public bool HasSettings
        {
            get { return true; }
        }

        public bool CheckAlive()
        {
            return ModulesRepository.IsInstallModule(ModuleStringId);
        }

        public bool InstallModule()
        {
            return SmsNotificationsService.InstallModule();
        }

        public bool UninstallModule()
        {
            return SmsNotificationsService.UninstallModule();
        }

        public bool UpdateModule()
        {
            return SmsNotificationsService.UpdateModule();
        }

        private class SmsNotificationsSending : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Смс рассылка сообщений";

                        case "en":
                            return "Bulk SMS sending";

                        default:
                            return "Bulk SMS sending";
                    }
                }
            }

            public string File
            {
                get { return "SmsSending.ascx"; }
            }

            #endregion
        }

        private class SmsNotificationsSetting : IModuleControl
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
                get { return "SmsNotifications.ascx"; }
            }

            #endregion
        }

        #endregion

        #region Implementation of IModuleSms

        public void SendSms(Guid customerid, long phoneNumber, string text)
        {
            SmsNotificationsService.SendNow(customerid, phoneNumber, text);
        }

        public string RenderSendSmsButton(Guid customerId, long phoneNumber)
        {
            return string.Format(
                "<a href=\"module.aspx?module={1}&currentcontrolindex=1&phone={3}&customerId={4}\" style=\"display: inline-block;\">" +
                "<img src=\"{0}/Modules/{1}/images/sms.png\" alt=\"{2}\" title=\"{2}\" style=\"vertical-align: middle;\" />" +
                "</a>",
                SettingsMain.SiteUrl.Trim('/'), ModuleStringId, "Отправить СМС", phoneNumber, customerId);
        }

        public IHtmlString GetSendSmsButton(Guid customerId, long phoneNumber)
        {
            return new HtmlString(
                "<ui-modal-trigger data-controller=\"'ModalSendSmsCtrl'\" controller-as=\"ctrl\" " +
                        "data-resolve=\"{ params: { customerId: '" + customerId + "', phone: order.phone} }\" " +
                        "on-close=\"$ctrl.closeModal()\" " +
                        "template-url=\"../modules/smsNotifications/scripts/admin/modals/modalSendSms/modalSendSms.html\"> " +
                    "<a href=\"\" class=\"edit link-decoration-none m-l-xs\">Отправить СМС</a> " +
                "</ui-modal-trigger>"
            );
        }

        public bool HaveSmsTemplate(int orderStatusId)
        {
            var status = SmsNotificationsStatus.GetStatus(orderStatusId);

            return status != null && status.Enabled && !string.IsNullOrWhiteSpace(status.Content);

        }

        #endregion




        #region Implementation of ISendOrderNotifications

        public void SendOnOrderAdded(IOrder order)
        {
            SmsNotificationsService.DoOrderAdded(OrderService.GetOrder(order.OrderID));
        }

        public void SendOnOrderChangeStatus(IOrder order)
        {
            SmsNotificationsService.DoOrderChangeStatus(OrderService.GetOrder(order.OrderID));
        }

        public void SendOnOrderUpdated(IOrder order)
        {

        }

        public void SendOnOrderDeleted(int orderId)
        {

        }

        public void SendOnPayOrder(int orderId, bool payed)
        {

        }

        #endregion

        #region IAdminBundles

        public List<string> AdminCssBottom()
        {
            return new List<string>();
        }

        public List<string> AdminJsBottom()
        {
            return new List<string>() { "~/modules/SmsNotifications/scripts/admin/modals/modalSendSms/modalSendSms.js" };
        }

        #endregion
    }
}