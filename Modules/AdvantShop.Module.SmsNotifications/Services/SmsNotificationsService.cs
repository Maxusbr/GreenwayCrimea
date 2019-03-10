using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Services.Loging;
using AdvantShop.Core.Services.Loging.Smses;
using AdvantShop.Core.Services.Repository;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Module.SmsNotifications.Domain;
using AdvantShop.Module.SmsNotifications.Status;
using AdvantShop.Orders;
using AdvantShop.Payment;

namespace AdvantShop.Module.SmsNotifications.Services
{
    public enum ESMSRecipientType
    {
        Subscriber,
        Customer,
        OrderCustomer,
        All
    }

    public class PhoneComparer : IEqualityComparer<KeyValuePair<Guid, long>>
    {
        public bool Equals(KeyValuePair<Guid, long> x, KeyValuePair<Guid, long> y)
        {
            return x.Value == y.Value;
        }

        public int GetHashCode(KeyValuePair<Guid, long> obj)
        {
            return obj.Value.GetHashCode();
        }
    }
    
    public class SmsNotificationsService
    {
        #region Fields

        private static ISmsLoger Loger = LogingManager.GetSmsLoger();

        private static readonly Regex PhoneValid = new Regex("^([0-9]{10,15})$", RegexOptions.Compiled | RegexOptions.Singleline);

        #endregion

        #region Install / Uninstall module

        public static bool InstallModule()
        {
            try
            {
                string textNewOrder;
                string textChangeStatus;

                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        textNewOrder = "Спасибо за заказ! Наши менеджеры скоро свяжутся с вами. Номер заказа #ORDERNUMBER#.";
                        textChangeStatus = "Ваш заказ #ORDERNUMBER# сменил статус на \"#STATUS#\". Комментарий к статусу: #STATUSCOMMENT#.";
                        break;

                    case "en":
                        textNewOrder = "Thank you for your order! Our managers will soon contact you. Order number #ORDERNUMBER#.";
                        textChangeStatus = "Your order #ORDERNUMBER# changed the status of the \"#STATUS#\". Comments to the status of: #STATUSCOMMENT#";
                        break;

                    default:
                        textNewOrder = "Thank you for your order! Our managers will soon contact you. Order number #ORDERNUMBER#.";
                        textChangeStatus = "Your order #ORDERNUMBER# changed the status of the \"#STATUS#\". Comments to the status of: #STATUSCOMMENT#";
                        break;
                }

                ModuleSettingsProvider.SetSettingValue("SmsService", "WwwSms4BRu", SmsNotifications.ModuleId);
                ModuleSettingsProvider.SetSettingValue("TextSmsNewOrder", textNewOrder, SmsNotifications.ModuleId);
                ModuleSettingsProvider.SetSettingValue("TextSmsChangeStatus", textChangeStatus, SmsNotifications.ModuleId);

                if (!ModulesRepository.IsExistsModuleTable("Customers", "SmsNotifications"))
                {
                    ModulesRepository.ModuleExecuteNonQuery(
@"CREATE TABLE Customers.SmsNotifications
	(
	CustomerID uniqueidentifier NOT NULL,
	Code nvarchar(5) NULL,
	Number nvarchar(10) NULL,
	Subscribed4SMSNews bit NOT NULL,
	Subscribed4SMSOrde bit NOT NULL,
	IsBlack bit NOT NULL
	)  ON [PRIMARY]", CommandType.Text);

                    ModulesRepository.ModuleExecuteNonQuery(
@"CREATE UNIQUE NONCLUSTERED INDEX IX_SmsNotifications ON Customers.SmsNotifications
	    (CustomerID) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]",
                        CommandType.Text);

                    ModulesRepository.ModuleExecuteNonQuery(
@"ALTER TABLE Customers.SmsNotifications ADD CONSTRAINT
	FK_SmsNotifications_Customer FOREIGN KEY
	(CustomerID) REFERENCES Customers.Customer
	(CustomerID) ON UPDATE  CASCADE 
	    ON DELETE  CASCADE ", CommandType.Text);
                }

                if (!ModulesRepository.IsExistsModuleTable("Module", "SmsNotificationsStatus"))
                {
                    ModulesRepository.ModuleExecuteNonQuery(
    @"CREATE TABLE Module.SmsNotificationsStatus
	(
	Status int NOT NULL,
	Content nvarchar(max) NULL,
	Enabled bit not NULL
	)  ON [PRIMARY]", CommandType.Text);

                    ModulesRepository.ModuleExecuteNonQuery(
    @"CREATE UNIQUE NONCLUSTERED INDEX IX_SmsNotificationsStatus ON Module.SmsNotificationsStatus
	    (Status) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]",
                        CommandType.Text);

                    ModulesRepository.ModuleExecuteNonQuery(
@"ALTER TABLE Module.SmsNotificationsStatus ADD CONSTRAINT
	FK_SmsNotifications_Status FOREIGN KEY
	(Status) REFERENCES [Order].OrderStatus
	(OrderStatusID) ON UPDATE  CASCADE 
	    ON DELETE  CASCADE ", CommandType.Text);
                }

                Localize();

            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return false;
            }
            return true;
        }

        public static bool UpdateModule()
        {
            try
            {
                ModuleSettingsProvider.RemoveSqlSetting("SendTrackNumberForStatus", SmsNotifications.ModuleId); 
                ModuleSettingsProvider.RemoveSqlSetting("TextSmsChangeStatus", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("RuSmsOnlineComPassword", SmsNotifications.ModuleId);

                if (!ModulesRepository.IsExistsModuleTable("Module", "SmsNotificationsStatus"))
                {
                    ModulesRepository.ModuleExecuteNonQuery(
    @"CREATE TABLE Module.SmsNotificationsStatus
	(
	Status int NOT NULL,
	Content nvarchar(max) NULL,
	Enabled bit not NULL
	)  ON [PRIMARY]", CommandType.Text);

                    ModulesRepository.ModuleExecuteNonQuery(
    @"CREATE UNIQUE NONCLUSTERED INDEX IX_SmsNotificationsStatus ON Module.SmsNotificationsStatus
	    (Status) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]",
                        CommandType.Text);

                    ModulesRepository.ModuleExecuteNonQuery(
@"ALTER TABLE Module.SmsNotificationsStatus ADD CONSTRAINT
	FK_SmsNotifications_Status FOREIGN KEY
	(Status) REFERENCES [Order].OrderStatus
	(OrderStatusID) ON UPDATE  CASCADE 
	    ON DELETE  CASCADE ", CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return false;
            }
            return true;
        }

        public static bool UninstallModule()
        {
            try
            {
                var webReferencesFolder =
                    HttpContext.Current.Server.MapPath("~/App_WebReferences/sms4b");

                if (Directory.Exists(webReferencesFolder))
                    foreach (var file in Directory.GetFiles(webReferencesFolder))
                        File.Delete(file);

                //ModuleSettingsProvider.RemoveSqlSetting("SmsNotificationsEnabled", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("SendTrackNumberForStatus", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("TextSmsChangeStatus", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("DefaultCountryCode", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("UserDefaultCode", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("CodeCountryPhoneAdmin", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("NumberPhoneAdmin", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("SendSmsNewOrder", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("TextSmsNewOrder", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("SendNewOrderAdmin", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("SendSmsChangeStatus", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("TextSmsChangeStatus", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("SendChangeStatusAdmin", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("SendSmsOrderPhone", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("SmsService", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("WwwSms4BRuLogin", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("WwwSms4BRuPassword", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("WwwSms4BRuSender", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("SmslabRuLogin", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("SmslabRuPassword", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("SmslabRuSender", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("WwwSmsimpleRuLogin", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("WwwSmsimpleRuPassword", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("WwwSmsimpleRuOriginId", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("GsmInformRuLogin", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("GsmInformRuPassword", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("GsmInformRuSender", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("WwwIqsmsRuLogin", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("WwwIqsmsRuPassword", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("WwwIqsmsRuSender", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("LeninsmsRuLogin", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("LeninsmsRuApiKey", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("LeninsmsRuSender", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("WwwSmspilotRuLogin", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("WwwSmspilotRuPassword", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("WwwSmspilotRuSender", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("RuSmsOnlineComLogin", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("RuSmsOnlineComSecretKey", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("RuSmsOnlineComPassword", SmsNotifications.ModuleId); 
                ModuleSettingsProvider.RemoveSqlSetting("RuSmsOnlineComSender", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("WwwEpochtaRuApiKey", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("WwwEpochtaRuSender", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("WwwEpochtaRuPrivatKey", SmsNotifications.ModuleId);

                ModuleSettingsProvider.RemoveSqlSetting("StreamTelecomLogin", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("StreamTelecomPassword", SmsNotifications.ModuleId);
                ModuleSettingsProvider.RemoveSqlSetting("StreamTelecomSender", SmsNotifications.ModuleId);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static void Localize()
        {
            var language = LanguageService.GetLanguage("ru-RU");
            if (language != null)
            {
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.Legend", "СМС уведомления");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.SaveChanges", "Сохранить изменения");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.CodeCountryAndPhone", "Код страны/телефон");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.SubscribeForNews", "Получать акции и новости");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.SubscribeForOrders", "Получать уведомления по заказам");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.Wrong_CodeCountryPhone", "Введите числовое значение в поле кода страны");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.Wrong_NumberPhone", "Номер телефона необходимо заполнить 10ти значным номером");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.SaveData", "Данные успешно сохранены");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.NoSaveData", "Не удалось сохранить данные смс информирования");
            }

            language = LanguageService.GetLanguage("en-US");
            if (language != null)
            {
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.Legend", "Sms notification");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.SaveChanges", "Save changes");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.CodeCountryAndPhone", "Country code/phone");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.SubscribeForNews", "Receive shares and news");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.SubscribeForOrders", "Receive order notifications");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.Wrong_CodeCountryPhone", "Enter an number value in the field of the country code");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.Wrong_NumberPhone", "The phone number should fill in the 10 digit number");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.SaveData", "Data has been successfully saved");
                LocalizationService.AddOrUpdateResource(language.LanguageId, "Module.SmsNotifications.NoSaveData", "Failed to save the data SMS informing");
            }
        }

        #endregion

        #region Send sms

        private static SMSSenderService GetCurrentSMSService()
        {
            ESMSSenderService serviceType;
            Enum.TryParse(ModuleSettingsProvider.GetSettingValue<string>("SmsService", SmsNotifications.ModuleId), true, out serviceType);
            if (serviceType == ESMSSenderService.None)
                return null;

            return Assembly.GetExecutingAssembly()
                .GetExportedTypes()
                .Where(t => t.IsSubclassOf(typeof (SMSSenderService)))
                .Select(type => (SMSSenderService) Activator.CreateInstance(type))
                .FirstOrDefault(instance => instance.Type == serviceType);
        }

        private static bool IsValidPhone(long phone)
        {
            return PhoneValid.IsMatch(phone.ToString());
        }

        public static void SendingNews(string message, ESMSRecipientType recipientType)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;
            
            ThreadPool.QueueUserWorkItem(x =>
            {
                var smsService = GetCurrentSMSService();
                if (smsService == null)
                    return;

                Dictionary<Guid, long> phonesDict;
                switch (recipientType)
                {
                    case ESMSRecipientType.Subscriber:
                        phonesDict = CustomerService.GetSubscribedCustomersPhones();
                        break;
                    case ESMSRecipientType.Customer:
                        phonesDict = CustomerService.GetCustomersPhones();
                        break;
                    case ESMSRecipientType.OrderCustomer:
                        phonesDict = OrderService.GetAllOrdersPhones();
                        break;
                    case ESMSRecipientType.All:
                        phonesDict = CustomerService.GetCustomersPhones();
                        foreach (var kvp in OrderService.GetAllOrdersPhones().Where(kvp => !phonesDict.ContainsKey(kvp.Key)))
                            phonesDict.Add(kvp.Key, kvp.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("recipientType");
                }

                var phones = phonesDict.Distinct(new PhoneComparer()).ToList();

                var status = SmsStatus.Sent;

                try
                {
                    smsService.SendSMS(phones.Select(kvp => kvp.Value).Where(IsValidPhone).ToList(), message);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    status = SmsStatus.Error;
                }

                foreach (var phone in phones)
                {
                    Loger.LogSms(new TextMessage
                    {
                        CreateOn = DateTime.Now,
                        CustomerId = phone.Key,
                        Body = message,
                        Status = IsValidPhone(phone.Value) ? status : SmsStatus.Error,
                        Phone = phone.Value
                    });
                }
            });
        }

        public static void SendNow(Guid customerid, long phoneNumber, string text)
        {
            if (string.IsNullOrEmpty(text) || phoneNumber == 0 || !IsValidPhone(phoneNumber))
                return;
            ThreadPool.QueueUserWorkItem(a => SendNowThread(customerid, phoneNumber, text));
        }

        private static void SendNowThread(Guid customerid, long phone, string text)
        {
            var smsService = GetCurrentSMSService();
            if (smsService == null)
                return;

            var status = SmsStatus.Sent;
            try
            {
                var result = smsService.SendSMS(phone, text);
            }
            catch (WebException ex)
            {
                using (var eResponse = ex.Response)
                {
                    if (eResponse != null)
                    {
                        using (var eStream = eResponse.GetResponseStream())
                            if (eStream != null)
                                using (var reader = new StreamReader(eStream))
                                {
                                    var error = reader.ReadToEnd();
                                    Debug.Log.Error(error);
                                    status = SmsStatus.Error;
                                }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                status = SmsStatus.Error;
            }

            Loger.LogSms(new TextMessage
            {
                CreateOn = DateTime.Now,
                CustomerId = customerid,
                Body = text,
                Status = status,
                Phone = phone
            });
        }

        #endregion


        #region Add order, change status

        public static void DoOrderAdded(Order order)
        {
            var message = ModuleSettingsProvider.GetSettingValue<string>("TextSmsNewOrder", SmsNotifications.ModuleId);
            message = message.Replace("#ORDERNUMBER#", order.Number);
            message = message.Replace("#ORDER_SUM#", PriceFormatService.FormatPrice(order.Sum, order.OrderCurrency));

            if (ModuleSettingsProvider.GetSettingValue<bool>("SendSmsNewOrder", SmsNotifications.ModuleId))
            {
                IOrderCustomer orderCustomer = order.GetOrderCustomer();
                if (orderCustomer != null && orderCustomer.StandardPhone.HasValue)
                {
                    SendNow(orderCustomer.CustomerID, orderCustomer.StandardPhone.Value, message);
                }
            }

            if (ModuleSettingsProvider.GetSettingValue<bool>("SendNewOrderAdmin", SmsNotifications.ModuleId))
            {
                var phones = ModuleSettingsProvider.GetSettingValue<string>("NumberPhoneAdmin", SmsNotifications.ModuleId);

                foreach (var phone in phones.Split(','))
                {
                    SendNow(CustomerContext.CustomerId, Convert.ToInt64(phone.Trim()), message);
                }
            }
        }

        public static void DoOrderChangeStatus(Order order)
        {
            try
            {
                var statOrder = order.GetOrderStatus();
                var statModule = SmsNotificationsStatus.GetStatus(statOrder.StatusID);
                if (statModule == null )
                {
                    return;
                }
                if (!statModule.Enabled)
                {
                    return;
                }
                var message = statModule.Content;
                message = message.Replace("#ORDERNUMBER#", order.Number);
                message = message.Replace("#ORDER_SUM#", PriceFormatService.FormatPrice(order.Sum,order.OrderCurrency));
                IOrderStatus status = order.GetOrderStatus();

                if (status == null || status.IsDefault)
                    return;

                message = message.Replace("#STATUS#", status.StatusName);
                message = message.Replace("#TRACKNUMBER#", order.TrackNumber);
                message = message.Replace("#STATUSCOMMENT#",
                    string.IsNullOrEmpty(order.StatusComment) ? "-" : order.StatusComment);

                if (ModuleSettingsProvider.GetSettingValue<bool>("SendSmsChangeStatus", SmsNotifications.ModuleId))
                {
                    IOrderCustomer orderCustomer = order.GetOrderCustomer();
                    if (orderCustomer != null && orderCustomer.StandardPhone.HasValue)
                    {
                        SendNow(orderCustomer.CustomerID, orderCustomer.StandardPhone.Value, message);
                    }
                }
                if (ModuleSettingsProvider.GetSettingValue<bool>("SendChangeStatusAdmin", SmsNotifications.ModuleId))
                {
                    var phones = ModuleSettingsProvider.GetSettingValue<string>("NumberPhoneAdmin",
                        SmsNotifications.ModuleId);
                    foreach (var phone in phones.Split(','))
                    {
                        long newPhone;
                        if (long.TryParse(phone, out newPhone))
                        {
                            SendNow(CustomerContext.CustomerId, newPhone, message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        #endregion
        
    }
}
