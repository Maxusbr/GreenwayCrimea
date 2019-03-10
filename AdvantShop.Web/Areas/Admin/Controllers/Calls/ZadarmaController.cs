using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.IPTelephony.Zadarma;
using AdvantShop.Saas;
using Call = AdvantShop.Core.Services.IPTelephony.Call;

namespace AdvantShop.Web.Admin.Controllers.Calls
{
    public class ZadarmaController : BaseCallController
    {
        private static readonly Object SyncObject = new Object();

        //private static void WriteLog(HttpRequestBase req)
        //{
        //    var sb = new StringBuilder();
        //    sb.AppendFormat("{0}: {1}\r\n", DateTime.Now.ToString("HH:mm:ss:ffffff"), req.RawUrl);
        //    sb.Append("POST PARAMS:\r\n");
        //    string[] keys = req.Form.AllKeys;
        //    for (int i = 0; i < keys.Length; i++)
        //    {
        //        sb.AppendFormat("{0}: {1}\r\n", keys[i], req.Form[keys[i]]);
        //    }
        //    Statistic.CommonStatistic.WriteLog(sb.ToString());
        //}

        public ActionResult PushNotification([ModelBinder(typeof(ZadarmaModelBinder))] ZadarmaNotificationDto model)
        {
            lock (SyncObject)
            {
                var request = ControllerContext.RequestContext.HttpContext.Request;
                if (request["zd_echo"].IsNotEmpty()) // валидация ссылки
                    return Content(request["zd_echo"]);

                // for debug
                //WriteLog(request);

                if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveTelephony)
                    return Content("no telephony");

                if (model == null || SettingsTelephony.CurrentIPTelephonyOperatorType != EOperatorType.Zadarma)
                    return Content("error");

                var sign = request.Headers["Signature"];
                //Statistic.CommonStatistic.WriteLog("Signature: " + sign);
                if (sign.IsNullOrEmpty() || sign != model.Signature)
                    return Content("unauthorized");

                var call = CallService.GetCall(model.PbxCallId, EOperatorType.Zadarma) ?? new Call
                {
                    CallId = model.PbxCallId,
                    SrcNum = model.SrcNum,
                    DstNum = model.DstNum,
                    Extension = model.Internal,
                    CallDate = model.CallStart,
                    OperatorType = EOperatorType.Zadarma
                };
                call.IsComplete = false;

                switch (model.Event)
                {
                    case EZadarmaEvent.NOTIFY_START:
                        var startModel = (ZadarmaStartDto)model;
                        call.Type = ECallType.In;
                        call.Phone = model.SrcNum;
                        break;
                    case EZadarmaEvent.NOTIFY_INTERNAL:
                        var internalModel = (ZadarmaInternalDto)model;
                        call.Type = ECallType.In;
                        call.Phone = model.SrcNum;
                        SendIncomingCallNotify(call);
                        break;
                    case EZadarmaEvent.NOTIFY_END:
                        var endModel = (ZadarmaEndDto)model;
                        call.Extension = endModel.Internal;
                        call.IsComplete = true;
                        if (endModel.CallStatus == ECallHangupStatus.Answer)
                        {
                            call.Duration = endModel.Duration;
                            call.CallAnswerDate = call.CallDate.AddSeconds(endModel.Duration);
                            call.RecordLink = endModel.IsRecorded ? endModel.CallIdWithRec : null;
                        }
                        else
                        {
                            call.Type = ECallType.Missed;
                            call.HangupStatus = endModel.CallStatus;
                            ProcessMissedCall(call);
                        }
                        SendCallEndedNotify(call);
                        break;
                    case EZadarmaEvent.NOTIFY_OUT_START:
                        var outStartModel = (ZadarmaOutStartDto)model;
                        call.Type = ECallType.Out;
                        call.Phone = model.DstNum;
                        if (call.SrcNum.IsNullOrEmpty())
                            call.SrcNum = model.Internal;
                        break;
                    case EZadarmaEvent.NOTIFY_OUT_END:
                        var outEndModel = (ZadarmaOutEndDto)model;
                        call.Extension = outEndModel.Internal;
                        call.IsComplete = true;
                        if (outEndModel.CallStatus == ECallHangupStatus.Answer)
                        {
                            call.Duration = outEndModel.Duration;
                            call.CallAnswerDate = call.CallDate.AddSeconds(outEndModel.Duration);
                            call.RecordLink = outEndModel.IsRecorded ? outEndModel.CallIdWithRec : null;
                        }
                        SendCallEndedNotify(call);
                        break;
                }

                if (call.Id == 0)
                    call.Id = CallService.AddCall(call);
                else
                    CallService.UpdateCall(call);

                CallService.ProcessCall(call);

                return new EmptyResult();
            }
        }
    }
}