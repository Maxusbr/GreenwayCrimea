using System;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Orders;

namespace AdvantShop.Core.Services.IPTelephony.CallBack
{
    public class CallBack
    {
        private readonly bool _enabled;

        public CallBack()
        {
            _enabled = SettingsTelephony.CallBackEnabled;
        }

        public virtual bool Enabled
        {
            get { return _enabled; }
        }

        public bool IsWorkTime()
        {
            WorkTime workTime;
            if (!(workTime = WorkSchedule.Schedule.Get(DateTime.Now.DayOfWeek)).Enabled)
                return false;

            var hour = DateTime.Now.Hour;
            return workTime.From.Hours >= workTime.To.Hours
                ? hour >= workTime.From.Hours || hour < workTime.To.Hours
                : hour >= workTime.From.Hours && hour < workTime.To.Hours;
        }

        public CallBackAnswer MakeRequest(string phone, bool check)
        {
            if (!_enabled)
                return new CallBackAnswer(false, string.Empty);

            return IsWorkTime()
                ? check
                    ? new CallBackAnswer(false, SettingsTelephony.CallBackWorkTimeTextFormatted)
                    : CreateCallBack(phone)
                : CreateLead(phone);
        }

        public virtual CallBackAnswer CreateCallBack(string phone)
        {
            return null;
        }

        private CallBackAnswer CreateLead(string phone)
        {
            var orderSource = OrderSourceService.GetOrderSource(OrderType.Callback);

            var lead = new Lead
            {
                Phone = phone,
                Description = LocalizationService.GetResource("Core.IPTelephony.CallBack.LeadAdminComment"),
                OrderSourceId = orderSource.Id,
            };

            var customer = CustomerService.GetCustomersByPhone(phone).FirstOrDefault()
                           ?? new Customer(CustomerGroupService.DefaultCustomerGroup)
                           {
                               Phone = phone,
                               StandardPhone = StringHelper.ConvertToStandardPhone(lead.Phone),
                               CustomerRole = Role.User
                           };

            lead.CustomerId = customer.Id;
            lead.Customer = customer;

            LeadService.AddLead(lead, true);

            return new CallBackAnswer(true, SettingsTelephony.CallBackNotWorkTimeText);
        }
    }
}
