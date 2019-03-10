using System;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Services.IPTelephony.Zadarma
{
    public enum EZadarmaEvent
    {
        None = 0,
        /// <summary>
        /// начало входящего звонка в АТС
        /// </summary>
        NOTIFY_START = 1,
        /// <summary>
        /// начало входящего звонка на внутренний номер АТС
        /// </summary>
        NOTIFY_INTERNAL = 2,
        /// <summary>
        /// конец входящего звонка на внутренний номер АТС
        /// </summary>
        NOTIFY_END = 3,
        /// <summary>
        /// начало исходящего звонка с АТС
        /// </summary>
        NOTIFY_OUT_START = 4,
        /// <summary>
        /// конец исходящего звонка с АТС
        /// </summary>
        NOTIFY_OUT_END = 5
    }

    public class ZadarmaModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            var @event = request["event"].TryParseEnum<EZadarmaEvent>();
            if (@event == EZadarmaEvent.None)
                return null;
            return ZadarmaNotificationDto.Factory(@event, request);
        }
    }

    public abstract class ZadarmaNotificationDto : CallNotificationDto
    {
        protected ZadarmaNotificationDto() { }

        public virtual EZadarmaEvent Event { get { return EZadarmaEvent.None; } }

        /// <summary>
        /// id звонка
        /// </summary>
        public string PbxCallId { get; set; }

        /// <summary>
        /// время начала звонка
        /// </summary>
        public DateTime CallStart { get; set; }

        /// <summary>
        /// номер звонящего, не указан для NOTIFY_OUT_START
        /// </summary>
        public string SrcNum { get; set; }

        /// <summary>
        /// номер, на который позвонили
        /// </summary>
        public string DstNum { get; set; }

        /// <summary>
        /// (опциональный) внутренний номер, не указан для NOTIFY_START
        /// </summary>
        public string Internal { get; set; }

        public string Signature { get; set; }

        protected ZadarmaNotificationDto(HttpRequestBase request)
        {
            PbxCallId = request["pbx_call_id"];
            CallStart = request["call_start"].TryParseDateTime();
            SrcNum = request["caller_id"].DefaultOrEmpty().Trim('+');
            Internal = request["internal"];
        }

        public static ZadarmaNotificationDto Factory(EZadarmaEvent type, HttpRequestBase request)
        {
            switch (type)
            {
                case EZadarmaEvent.NOTIFY_START:
                    return new ZadarmaStartDto(request);
                case EZadarmaEvent.NOTIFY_INTERNAL:
                    return new ZadarmaInternalDto(request);
                case EZadarmaEvent.NOTIFY_END:
                    return new ZadarmaEndDto(request);
                case EZadarmaEvent.NOTIFY_OUT_START:
                    return new ZadarmaOutStartDto(request);
                case EZadarmaEvent.NOTIFY_OUT_END:
                    return new ZadarmaOutEndDto(request);
                default:
                    return null;
            }
        }
    }

    public abstract class ZadarmaInDto : ZadarmaNotificationDto
    {
        protected ZadarmaInDto() { }

        public override EZadarmaEvent Event { get { return EZadarmaEvent.None; } }

        protected ZadarmaInDto(HttpRequestBase request) : base(request)
        {
            DstNum = request["called_did"].DefaultOrEmpty().Trim('+');
            Signature = StringHelper.EncodeTo64(SecurityHelper.EncodeWithHmacSha1(
                StringHelper.AggregateStrings("", request["caller_id"], request["called_did"], request["call_start"]), SettingsTelephony.ZadarmaSecret));
        }
    }

    public class ZadarmaStartDto : ZadarmaInDto
    {
        public ZadarmaStartDto() { }

        public override EZadarmaEvent Event { get { return EZadarmaEvent.NOTIFY_START; } }

        public ZadarmaStartDto(HttpRequestBase request) : base(request)
        {
            
        }
    }

    public class ZadarmaInternalDto : ZadarmaInDto
    {
        public ZadarmaInternalDto() { }

        public override EZadarmaEvent Event { get { return EZadarmaEvent.NOTIFY_INTERNAL; } }

        public ZadarmaInternalDto(HttpRequestBase request) : base(request)
        {

        }
    }

    public class ZadarmaEndDto : ZadarmaInDto
    {
        public ZadarmaEndDto() { }

        public override EZadarmaEvent Event { get { return EZadarmaEvent.NOTIFY_END; } }

        /// <summary>
        /// длительность в секундах
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// состояние звонка
        /// </summary>
        public string Disposition { get; set; }

        ///// <summary>
        ///// код статуса звонка Q.931
        ///// </summary>
        //public string StatusCode { get; set; }

        /// <summary>
        /// 1 - есть запись звонка, 0 - нет записи
        /// </summary>
        public bool IsRecorded { get; set; }

        /// <summary>
        /// id звонка с записью
        /// </summary>
        public string CallIdWithRec { get; set; }

        public ECallHangupStatus CallStatus { get; set; }

        public ZadarmaEndDto(HttpRequestBase request) : base(request)
        {
            Duration = request["duration"].TryParseInt();
            Disposition = request["disposition"];
            //StatusCode = request["status_code"];
            IsRecorded = request["is_recorded"] == "1";
            CallIdWithRec = request["call_id_with_rec"];

            switch (Disposition)
            {
                case "answered":
                    CallStatus = ECallHangupStatus.Answer;
                    break;
                case "busy":
                    CallStatus = ECallHangupStatus.Busy;
                    break;
                case "cancel":
                    CallStatus = ECallHangupStatus.Cancel;
                    break;
                case "no answer":
                    CallStatus = ECallHangupStatus.Noanswer;
                    break;
                case "failed":
                    CallStatus = ECallHangupStatus.Chanunavail;
                    break;
                case "no money":
                case "unallocated number":
                case "no day limit":
                case "line limit":
                case "no money, no limit":
                    break;
            }
        }
    }

    public class ZadarmaOutStartDto : ZadarmaNotificationDto
    {
        public ZadarmaOutStartDto() { }

        public override EZadarmaEvent Event { get { return EZadarmaEvent.NOTIFY_OUT_START; } }

        public ZadarmaOutStartDto(HttpRequestBase request) : base(request)
        {
            DstNum = request["destination"].DefaultOrEmpty().Trim('+');
            Signature = StringHelper.EncodeTo64(SecurityHelper.EncodeWithHmacSha1(
                StringHelper.AggregateStrings("", request["internal"], request["destination"], request["call_start"]), SettingsTelephony.ZadarmaSecret));
        }
    }

    public class ZadarmaOutEndDto : ZadarmaEndDto
    {
        public ZadarmaOutEndDto() { }

        public override EZadarmaEvent Event { get { return EZadarmaEvent.NOTIFY_OUT_END; } }

        public ZadarmaOutEndDto(HttpRequestBase request) : base(request)
        {
            DstNum = request["destination"].DefaultOrEmpty().Trim('+');
            Signature = StringHelper.EncodeTo64(SecurityHelper.EncodeWithHmacSha1(
                StringHelper.AggregateStrings("", request["internal"], request["destination"], request["call_start"]), SettingsTelephony.ZadarmaSecret));
        }
    }
}
