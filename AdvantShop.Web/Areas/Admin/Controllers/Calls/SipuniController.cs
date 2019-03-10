using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.IPTelephony.Sipuni;
using AdvantShop.Saas;
using Call = AdvantShop.Core.Services.IPTelephony.Call;

namespace AdvantShop.Web.Admin.Controllers.Calls
{
    public class SipuniController : BaseCallController
    {
        private static readonly Object SyncObject = new Object();

        public JsonResult PushNotification([ModelBinder(typeof(SipuniModelBinder))] SipuniNotificationDto model)
        {
            lock (SyncObject)
            {
                //AdvantShop.Statistic.CommonStatistic.WriteLog(DateTime.Now.ToString("HH:mm:ss:ffffff") + ": " + ControllerContext.RequestContext.HttpContext.Request.RawUrl);

                if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveTelephony)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

                if (model == null || SettingsTelephony.CurrentIPTelephonyOperatorType != EOperatorType.Sipuni)
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);

                var call = CallService.GetCall(model.CallId, EOperatorType.Sipuni)
                           ?? new Call
                           {
                               CallId = model.CallId,
                               SrcNum = model.SrcNum.ToString(),
                               DstNum = model.DstNum.Split('_').FirstOrDefault(),
                               CallDate = model.Timestamp,
                               OperatorType = EOperatorType.Sipuni
                           };
                call.IsComplete = false;

                switch (model.Event)
                {
                    case ESipuniEvent.Call:
                        var callModel = (SipuniCallDto)model;
                        call.Type = callModel.SrcType == ESipuniAddressType.Outer ? ECallType.In : ECallType.Out;
                        call.Phone = callModel.SrcType == ESipuniAddressType.Outer ? callModel.SrcNum.ToString() : callModel.DstNum;

                        if (callModel.DstType == ESipuniAddressType.Inner && callModel.DstNum.IsNotEmpty() && callModel.DstNum.Length > 3)
                            call.Extension = callModel.DstNum.Substring(callModel.DstNum.Length - 3, 3);
                        else if (callModel.SrcType == ESipuniAddressType.Inner && callModel.SrcNum.ToString().IsNotEmpty() && callModel.SrcNum.ToString().Length > 3)
                            call.Extension = callModel.SrcNum.ToString().Substring(callModel.SrcNum.ToString().Length - 3, 3);

                        if ((!callModel.IsInnerCall || SettingsTelephony.SipuniConsiderInnerCalls) && callModel.SrcType == ESipuniAddressType.Outer) 
                            SendIncomingCallNotify(call);
                        break;
                    case ESipuniEvent.Answer:
                        break;
                    case ESipuniEvent.HangUp:
                    case ESipuniEvent.SecondaryHangUp:
                        var hangupModel = (SipuniHangupDto)model;
                        call.CallDate = hangupModel.CallStartTimestamp;
                        call.IsComplete = true;
                        if (hangupModel.CallAnswerTimestamp.HasValue)
                        {
                            call.CallAnswerDate = hangupModel.CallAnswerTimestamp.Value;
                            call.Duration = (int)(hangupModel.Timestamp - hangupModel.CallAnswerTimestamp.Value).TotalSeconds;
                        }
                        call.RecordLink = hangupModel.CallRecordLink;
                        if (call.Type != ECallType.Out && !hangupModel.CallAnswerTimestamp.HasValue)
                        {
                            call.Type = ECallType.Missed;
                            call.HangupStatus = hangupModel.Status.ToString().TryParseEnum<ECallHangupStatus>();
                            ProcessMissedCall(call);
                        }
                        SendCallEndedNotify(call);
                        break;
                }

                if (call.Id == 0)
                    call.Id = CallService.AddCall(call);
                else
                    CallService.UpdateCall(call);

                CallService.ProcessCall(call);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}