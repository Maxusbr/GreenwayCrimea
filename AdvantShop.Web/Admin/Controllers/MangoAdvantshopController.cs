using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.IPTelephony.Mango;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using Call = AdvantShop.Core.Services.IPTelephony.Call;

namespace AdvantShop.Admin.Controllers
{
    public class MangoAdvantshopOldController : CallController
    {
        private static readonly Object SyncObject = new Object();

        //protected static void WriteLog(HttpRequestBase req)
        //{
        //    lock (SyncObject)
        //    {
        //        var sb = new StringBuilder();
        //        sb.AppendFormat("{0}: {1}\r\n", DateTime.Now.ToString("HH:mm:ss:ffffff"), req.RawUrl);
        //        sb.Append("POST PARAMS:\r\n");
        //        string[] keys = req.Form.AllKeys;
        //        for (int i = 0; i < keys.Length; i++)
        //        {
        //            sb.AppendFormat("{0}: {1}\r\n", keys[i], req.Form[keys[i]]);
        //        }
        //        Statistic.CommonStatistic.WriteLog(sb.ToString());
        //    }
        //}

        public ActionResult Ping()
        {
            return Content("ok");
        }

        public ActionResult Result(string id)
        {
            // for debug
            //WriteLog(ControllerContext.HttpContext.Request);
            return Content("ok");
        }

        public ActionResult Events([ModelBinder(typeof(MangoModelBinder))] MangoNotificationDto model)
        {
            lock (SyncObject)
            {
                // for debug
                //WriteLog(ControllerContext.HttpContext.Request);

                if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveTelephony)
                    return Content("no telephony");

                if (SettingsTelephony.CurrentIPTelephonyOperatorType != EOperatorType.Mango || model == null)
                    return Content("error");

                var sign = (SettingsTelephony.MangoApiKey + model.Json + SettingsTelephony.MangoSecretKey).Sha256();
                if (model.ApiKey != SettingsTelephony.MangoApiKey || model.Sign != sign)
                {
                    return Content("unauthorized");
                }

                switch (model.Event)
                {
                    case EMangoEvent.Call:
                        return ProcessCall(((MangoCallDto)model).CallModel);
                    case EMangoEvent.Recording:
                        return ProcessRecording(((MangoRecordingDto)model).RecordingModel);
                    default:
                        return null;
                }
            }
        }

        private ActionResult ProcessCall(CallModel model)
        {
            if (model == null)
                return Content("error");

            var phone = model.EventFlow == EMangoCallFlow.In ? model.From.Number : model.To.Number;
            var customers = CustomerService.GetCustomersByPhone(phone.ToString());
            if (!customers.Any())
            {
                customers.Add(new Customer(CustomerGroupService.DefaultCustomerGroup)
                {
                    StandardPhone = StringHelper.ConvertToStandardPhone(phone)
                });
            }

            // TakenFromCallId - ID вызова, с которого переведен абонент
            //var callId = model.From.TakenFromCallId.IsNotEmpty() ? model.From.TakenFromCallId : model.CallId;

            // если сохранять по CallId - придется сохранять все вызовы, включая очередь обзвона, 
            // так как изначально неясно, который из звонков будет записываться
            var entryId = model.EntryId;
            var call = CallService.GetCall(entryId, EOperatorType.Mango)
                       ?? new Call
                       {
                           CallId = model.EntryId, //model.CallId,
                           SrcNum = model.From.Number,
                           DstNum = model.To.Number,
                           Extension = model.EventFlow == EMangoCallFlow.In ? model.To.Extension : model.From.Extension,
                           CallDate = model.EventDateTime,
                           OperatorType = EOperatorType.Mango
                       };

            if (customers.Any(customer => customer.ManagerId.HasValue))
                call.ManagerId = customers.First(customer => customer.ManagerId.HasValue).ManagerId;

            switch (model.EventState)
            {
                case EMangoCallState.Appeared:
                    call.Type = model.EventFlow == EMangoCallFlow.In ? ECallType.In : ECallType.Out;
                    if (model.EventFlow == EMangoCallFlow.In)
                    {
                        new Thread(() => Notify(customers, call.CallId)).Start();
                    }
                    break;
                case EMangoCallState.Connected:
                    // в уведомлении с состоянием Connected короткий номер ответившего на звонок сотрудника
                    call.Extension = model.EventFlow == EMangoCallFlow.In ? model.To.Extension : model.From.Extension;
                    call.CallAnswerDate = model.EventDateTime;
                    break;
                case EMangoCallState.OnHold:
                    break;
                case EMangoCallState.Disconnected:
                    if (call.CallAnswerDate.HasValue)
                    {
                        // за время вызова АТС может отсылать несколько событий Disconnected, и в БД вызов может быть со статусом Missed - нужно обновить
                        call.Type = model.EventFlow == EMangoCallFlow.In ? ECallType.In : ECallType.Out;
                        call.Duration = (int)(model.EventDateTime - call.CallAnswerDate.Value).TotalSeconds;
                    }
                    else if (call.Type == ECallType.In)
                    {
                        call.Type = ECallType.Missed;
                    }
                    break;
            }

            if (call.Id == 0)
                call.Id = CallService.AddCall(call);
            else
                CallService.UpdateCall(call);

            CallService.ProcessCall(call);
            return null;
        }

        private ActionResult ProcessRecording(RecordingModel model)
        {
            if (model == null || model.EventState != EMangoRecordingState.Completed)
                return null;

            var call = CallService.GetCall(model.EntryId, EOperatorType.Mango);
            //var call = CallService.GetCall(model.CallId, EOperatorType.Mango);
            if (call != null)
            {
                call.RecordLink = model.RecordingId;
                CallService.UpdateCall(call);
            }

            return null;
        }
    }
}