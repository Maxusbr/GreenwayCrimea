using System;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Core.Services.IPTelephony.Sipuni
{
    public enum ESipuniAddressType
    {
        None = 0,
        Outer = 1,
        Inner = 2
    }

    public enum ESipuniStatus
    {
        None = 0,
        Answer = 1,
        Busy = 2,
        Noanswer = 3,
        Cancel = 4,
        Congestion = 5,
        Chanunavail = 6
    }

    public enum ESipuniEvent
    {
        None = 0,
        Call = 1,
        HangUp = 2,
        Answer = 3,
        SecondaryHangUp = 4
    }

    public class SipuniModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            var @event = request["event"].TryParseEnum<ESipuniEvent>();
            if (@event == ESipuniEvent.None) return null;
            return SipuniNotificationDto.Factory(@event, request);
        }
    }
    
    public abstract class SipuniNotificationDto : CallNotificationDto
    {
        public virtual ESipuniEvent Event
        {
            get { return ESipuniEvent.None; }
        }
        public string CallId { get; set; }
        public long SrcNum { get; set; }
        public ESipuniAddressType SrcType { get; set; }
        public string DstNum { get; set; }
        public ESipuniAddressType DstType { get; set; }
        public DateTime Timestamp { get; set; }
        public string Channel { get; set; }

        protected SipuniNotificationDto()
        {
        }

        protected SipuniNotificationDto(HttpRequestBase request)
        {
            CallId = request["call_id"];
            SrcNum = request["src_num"].Trim(new []{'+'}).TryParseLong();
            SrcType = request["src_type"].TryParseEnum<ESipuniAddressType>();
            DstNum = request["dst_num"] ?? string.Empty;
            DstType = request["dst_type"].TryParseEnum<ESipuniAddressType>();
            Timestamp = request["timestamp"].TryParseLong().ToDateTimeFromUnixTime();
            Channel = request["channel"];
        }

        public static SipuniNotificationDto Factory(ESipuniEvent type, HttpRequestBase request)
        {
            switch (type)
            {
                case ESipuniEvent.Call:
                    return new SipuniCallDto(request);
                case ESipuniEvent.HangUp:
                    return new SipuniHangupDto(request);
                case ESipuniEvent.Answer:
                    return new SipuniAnswerDto(request);
                case ESipuniEvent.SecondaryHangUp:
                    return new SipuniSecondaryhangupDto(request);
                default:
                    return null;
            }
        }
    }

    public class SipuniCallDto : SipuniNotificationDto
    {
        public bool IsInnerCall { get; set; }

        public override ESipuniEvent Event
        {
            get { return ESipuniEvent.Call; }
        }

        public SipuniCallDto(HttpRequestBase request)
            : base(request)
        {
            IsInnerCall = request["is_inner_call"].IsNotEmpty() && request["is_inner_call"] == "1"; // TryParseBool не конвертит строку "1" в true
        }

        public SipuniCallDto()
        {
        }
    }

    public class SipuniHangupDto : SipuniNotificationDto
    {
        public ESipuniStatus Status { get; set; }
        public DateTime CallStartTimestamp { get; set; }
        public DateTime? CallAnswerTimestamp { get; set; }
        public string CallRecordLink { get; set; }
        public override ESipuniEvent Event
        {
            get { return ESipuniEvent.HangUp; }
        }

        public SipuniHangupDto(HttpRequestBase request)
            : base(request)
        {
            Status = request["status"].TryParseEnum<ESipuniStatus>();
            CallStartTimestamp = request["call_start_timestamp"].TryParseLong().ToDateTimeFromUnixTime();
            var callAnswerTimestamp = request["call_answer_timestamp"].TryParseLong();
            CallAnswerTimestamp = callAnswerTimestamp > 0
                ? (DateTime?)callAnswerTimestamp.ToDateTimeFromUnixTime()
                : null;
            CallRecordLink = HttpUtility.UrlDecode(request["call_record_link"]);
        }

        public SipuniHangupDto()
        {
        }
    }

    public class SipuniAnswerDto : SipuniNotificationDto
    {
        public override ESipuniEvent Event
        {
            get { return ESipuniEvent.Answer; }
        }
        public SipuniAnswerDto(HttpRequestBase request)
            : base(request)
        {
        }

        public SipuniAnswerDto()
        {
        }
    }

    public class SipuniSecondaryhangupDto : SipuniHangupDto
    {
        public override ESipuniEvent Event
        {
            get { return ESipuniEvent.SecondaryHangUp; }
        }
        public SipuniSecondaryhangupDto(HttpRequestBase request)
            : base(request)
        {
        }

        public SipuniSecondaryhangupDto()
        {
        }
    }
}
