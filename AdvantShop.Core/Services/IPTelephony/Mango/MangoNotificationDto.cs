using System;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.IPTelephony.Mango
{
    public enum EMangoCallFlow
    {
        None = 0,
        Out = 1,
        In = 2
    }

    public enum EMangoEvent
    {
        None = 0,
        Call = 1,
        Recording = 2
    }

    public enum EMangoCallState
    {
        None = 0,
        Appeared = 1,
        Connected = 2,
        OnHold = 3,
        Disconnected = 4
    }

    public enum EMangoRecordingState
    {
        None = 0,
        Started = 1,
        Continued = 2,
        Completed = 3
    }

    public class MangoModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            EMangoEvent @event;
            var vpr = bindingContext.ValueProvider.GetValue("id");
            if (vpr == null || (@event = vpr.AttemptedValue.TryParseEnum<EMangoEvent>()) == EMangoEvent.None)
                return null;

            HttpRequestBase request = controllerContext.HttpContext.Request;
            return MangoNotificationDto.Factory(@event, request);
        }
    }

    public abstract class MangoNotificationDto : CallNotificationDto
    {
        public virtual EMangoEvent Event
        {
            get { return EMangoEvent.None; }
        }

        public string ApiKey { get; set; }
        public string Sign { get; set; }
        public string Json { get; set; }

        protected MangoNotificationDto()
        {
        }

        protected MangoNotificationDto(HttpRequestBase request)
        {
            ApiKey = request["vpbx_api_key"];
            Sign = request["sign"];
            Json = request["json"];
        }

        public static MangoNotificationDto Factory(EMangoEvent type, HttpRequestBase request)
        {
            switch (type)
            {
                case EMangoEvent.Call:
                    return new MangoCallDto(request);
                case EMangoEvent.Recording:
                    return new MangoRecordingDto(request);
                default:
                    return null;
            }
        }
    }

    #region Call

    public class MangoCallDto : MangoNotificationDto
    {
        public override EMangoEvent Event
        {
            get { return EMangoEvent.Call; }
        }

        public CallModel CallModel { get; private set; }

        public MangoCallDto(HttpRequestBase request)
            : base(request)
        {
            CallModel = JsonConvert.DeserializeObject<CallModel>(Json);
        }

        public MangoCallDto()
        {
        }
    }

    public class CallModel
    {
        [JsonProperty("entry_id")]
        public string EntryId { get; set; }

        [JsonProperty("call_id")]
        public string CallId { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("call_state")]
        public string CallState { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("from")]
        public CallFromModel From { get; set; }

        [JsonProperty("to")]
        public CallFromModel To { get; set; }
        
        [JsonProperty("disconnect_reason")]
        public int DisconnectReason { get; set; }

        [JsonIgnore()]
        public DateTime EventDateTime { get { return Timestamp.ToDateTimeFromUnixTime(); } }

        [JsonIgnore()]
        public EMangoCallState EventState { get { return CallState.TryParseEnum<EMangoCallState>(); } }

        [JsonIgnore()]
        public EMangoCallFlow EventFlow
        {
            get
            {
                // направление вызова можно отследить только по короткому номеру во From или To
                return From.Extension.IsNotEmpty() ? EMangoCallFlow.Out : EMangoCallFlow.In;
            }
        }
    }

    public class CallFromModel
    {
        [JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("taken_from_call_id")]
        public string TakenFromCallId { get; set; }
    }

    public class CallToModel
    {
        [JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("line_number")]
        public string LineNumber { get; set; }

        [JsonProperty("acd_group")]
        public string GroupId { get; set; }
    }

    #endregion

    #region Recording

    public class MangoRecordingDto : MangoNotificationDto
    {
        public override EMangoEvent Event
        {
            get { return EMangoEvent.Recording; }
        }

        public RecordingModel RecordingModel { get; private set; }

        public MangoRecordingDto(HttpRequestBase request)
            : base(request)
        {
            RecordingModel = JsonConvert.DeserializeObject<RecordingModel>(Json);
        }

        public MangoRecordingDto()
        {
        }
    }
    
    public class RecordingModel
    {
        [JsonProperty("recording_id")]
        public string RecordingId { get; set; }

        [JsonProperty("recording_state")]
        public string RecordingState { get; set; }

        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("entry_id")]
        public string EntryId { get; set; }

        [JsonProperty("call_id")]
        public string CallId { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("completion_code")]
        public int CompletionCode { get; set; }

        [JsonProperty("recipient")]
        public string Recipient { get; set; }

        [JsonIgnore()]
        public DateTime EventDateTime { get { return Timestamp.ToDateTimeFromUnixTime(); } }

        [JsonIgnore()]
        public EMangoRecordingState EventState { get { return RecordingState.TryParseEnum<EMangoRecordingState>(); } }
    }

    #endregion
}
