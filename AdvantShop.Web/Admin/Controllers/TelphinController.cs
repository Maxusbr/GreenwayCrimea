using System;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.IPTelephony.Telphin;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using Call = AdvantShop.Core.Services.IPTelephony.Call;

namespace AdvantShop.Admin.Controllers
{
    public class TelphinOldController : CallController
    {
        private static readonly Object SyncObject = new Object();

        public ActionResult PushNotification([ModelBinder(typeof(TelphinModelBinder))] TelphinNotificationDto model)
        {
            lock (SyncObject)
            {
                if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveTelephony)
                    return Content("error");

                if (model == null || SettingsTelephony.CurrentIPTelephonyOperatorType != EOperatorType.Telphin)
                    return Content("error");

                //AdvantShop.Statistic.CommonStatistic.WriteLog(DateTime.Now.ToString("HH:mm:ss:ffffff") + ": " + ControllerContext.RequestContext.HttpContext.Request.RawUrl);

                var call = CallService.GetCall(model.CallID, EOperatorType.Telphin) ?? new Call
                {
                    CallId = model.CallID,
                    SrcNum = PreparePhone(model.CallerIDNum.Trim('+')),
                    DstNum = model.CallFlow == ETelphinCallFlow.In && model.CalledDID.IsNotEmpty() ? model.CalledDID : PreparePhone(model.CalledNumber.Trim('+')),
                    Extension = PreparePhone(model.CallFlow == ETelphinCallFlow.In ? model.CalledExtension : model.CallerExtension),
                    CallDate = model.EventDateTime,
                    Phone = PreparePhone(model.EventType == ETelphinEvent.DialIn ? model.CallerIDNum.Trim('+') : model.CalledNumber.Trim('+')),
                    OperatorType = EOperatorType.Telphin
                };

                switch (model.EventType)
                {
                    case ETelphinEvent.DialIn:
                        var dialInModel = (TelphinDialInDto)model;
                        call.Type = ECallType.In;
                        var customers = CustomerService.GetCustomersByPhone(call.Phone);
                        call.ManagerId = customers.Select(x => x.ManagerId).Where(x => x.HasValue).FirstOrDefault();
                        if (!customers.Any())
                        {
                            customers.Add(new Customer(CustomerGroupService.DefaultCustomerGroup)
                            {
                                FirstName = dialInModel.CallerIDName ?? string.Empty,
                                StandardPhone = StringHelper.ConvertToStandardPhone(dialInModel.CallerIDNum)
                            });
                        }

                        new Thread(() => Notify(customers, call.CallId)).Start();
                        break;

                    case ETelphinEvent.DialOut:
                        var dialOutModel = (TelphinDialOutDto)model;
                        call.Type = ECallType.Out;
                        break;

                    case ETelphinEvent.HangUp:
                        var hangUpModel = (TelphinHangUpDto)model;
                        call.Duration = (int)hangUpModel.Duration;
                        call.RecordLink = hangUpModel.RecID;
                        if (hangUpModel.CallStatus == ETelphinCallStatus.Answer && call.Type == ECallType.Missed)
                        {
                            call.Type = model.CallFlow == ETelphinCallFlow.In ? ECallType.In : ECallType.Out;
                        }
                        if (!call.CallAnswerDate.HasValue && call.Type != ECallType.Out && hangUpModel.CallStatus != ETelphinCallStatus.Answer)
                        {
                            call.Type = ECallType.Missed;
                            call.HangupStatus = hangUpModel.CallStatus.ToString().TryParseEnum<ECallHangupStatus>();
                        }
                        break;

                    case ETelphinEvent.Answer:
                        var answerModel = (TelphinAnswerDto)model;
                        call.Extension = PreparePhone(model.CallFlow == ETelphinCallFlow.In ? model.CalledExtension : model.CallerExtension); // добавочный ответившего на звонок
                        call.CallAnswerDate = answerModel.EventDateTime;
                        if (call.Type == ECallType.Missed)
                            call.Type = model.CallFlow == ETelphinCallFlow.In ? ECallType.In : ECallType.Out;
                        break;
                }

                if (call.Id == 0)
                    call.Id = CallService.AddCall(call);
                else
                    CallService.UpdateCall(call);

                CallService.ProcessCall(call);

                return Content("ok");
            }
        }

        private string PreparePhone(string phone)
        {
            if (!phone.Contains("*"))
                return phone;
            var result = phone.Split("*").LastOrDefault() ?? string.Empty;
            return result.Contains("@") ? result.Split("@").FirstOrDefault() ?? string.Empty : result;
        }
    }
}
