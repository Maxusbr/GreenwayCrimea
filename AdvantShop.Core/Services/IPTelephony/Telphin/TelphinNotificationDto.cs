using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Core.Services.IPTelephony.Telphin
{
    public enum ETelphinCallFlow
    {
        None = 0,
        In = 1,
        Out = 2
    }

    public enum ETelphinCallStatus
    {
        None = 0,
        /// <summary>
        /// вызов был отвечен, также при EventType 'answer'
        /// </summary>
        Answer = 1,
        /// <summary>
        /// вызов получил сигнал "занято"
        /// </summary>
        Busy = 2,
        /// <summary>
        /// звонок не отвечен (истек таймер ожидания на сервере)
        /// </summary>
        Noanswer = 3,
        /// <summary>
        /// звонящий отменил вызов до истечения таймера ожидания на сервере
        /// </summary>
        Cancel = 4,
        /// <summary>
        /// произошла ошибка во время вызова
        /// </summary>
        Congestion = 5,
        /// <summary>
        /// у вызываемого абонента отсутствует регистрация
        /// </summary>
        Chanunavail = 6,
        /// <summary>
        /// При EventType 'dial-in' и 'dial-out'
        /// </summary>
        Calling = 7
    }

    public enum ETelphinEvent
    {
        None = 0,
        [Localize("Core.IPTelephony.Telphin.ETelphinEvent.DialIn")]
        DialIn = 1,
        [Localize("Core.IPTelephony.Telphin.ETelphinEvent.DialOut")]
        DialOut = 2,
        [Localize("Core.IPTelephony.Telphin.ETelphinEvent.HangUp")]
        HangUp = 3,
        [Localize("Core.IPTelephony.Telphin.ETelphinEvent.Answer")]
        Answer = 4
    }

    public class TelphinModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            var eventType = request["EventType"];
            var @event = TelphinEvent.EventsDict.ContainsValue(eventType)
                    ? TelphinEvent.EventsDict.First(x => x.Value == eventType).Key
                    : ETelphinEvent.None;
            if (@event == ETelphinEvent.None)
                return null;
            return TelphinNotificationDto.Factory(@event, request);
        }
    }

    public abstract class TelphinNotificationDto : CallNotificationDto
    {
        /// <summary>
        /// Тип события. Может принимать значния dial-in, dial-out, hangup или answer в зависимости от типа события.
        /// </summary>
        public virtual ETelphinEvent EventType
        {
            get { return ETelphinEvent.None; }
        }

        /// <summary>
        /// Уникальный идентификатор вызова. Не меняется при переадресациях. Можно использовать для идентификации принадлежности различных событий одному вызову.
        /// </summary>
        public string CallID { get; set; }
        
        /// <summary>
        /// Номер вызывающего абонента
        /// </summary>
        public string CallerIDNum { get; set; }
        
        /// <summary>
        /// Имя вызывающего абонента (если есть).
        /// </summary>
        public string CallerIDName { get; set; }
        
        // Публичный номер вызываемого абонента (если есть)
        public string CalledDID { get; set; }
        
        /// <summary>
        /// Имя вызываемого добавочного (в виде xxx*yyy@domain)
        /// </summary>
        public string CalledExtension { get; set; }
        
        // Идентификатор добавочного CalledExtension
        //public int? CalledExtensionID { get; set; }
        
        /// <summary>
        /// Статус вызова
        /// </summary>
        public ETelphinCallStatus CallStatus { get; set; }
        
        /// <summary>
        /// Направление вызова
        /// </summary>
        public ETelphinCallFlow CallFlow { get; set; }

        /// <summary>
        /// добавочный, с которого произведен вызов (в виде xxx*yyy@domain)
        /// </summary>
        public string CallerExtension { get; set; }

        // Идентификатор добавочного CallerExtension
        //public int? CallerExtensionID { get; set; }

        /// <summary>
        /// вызываемый номер
        /// </summary>
        public string CalledNumber { get; set; }

        // Уникальный идентификатор вызова для управления им
        //public string CallAPIID { get; set; }

        /// <summary>
        /// время генерации события: микросекунды c  1 января 1970 года.
        /// </summary>
        public long EventTime { get; set; }

        public DateTime EventDateTime { get { return (EventTime/1000000).ToDateTimeFromUnixTime(); } }

        protected TelphinNotificationDto()
        {
        }

        protected TelphinNotificationDto(HttpRequestBase request)
        {
            CallID = request["CallID"];
            CallerIDNum = request["CallerIDNum"] ?? string.Empty;
            CallerIDName = request["CallerIDName"];
            CalledDID = request["CalledDID"];
            CalledExtension = request["CalledExtension"] ?? string.Empty;
            //CalledExtensionID = request["CalledExtensionID"].TryParseInt(true);
            CallStatus = request["CallStatus"].TryParseEnum<ETelphinCallStatus>();
            CallFlow = request["CallFlow"].TryParseEnum<ETelphinCallFlow>();
            CallerExtension = request["CallerExtension"] ?? string.Empty;
            //CallerExtensionID = request["CallerExtensionID"].TryParseInt(true);
            CalledNumber = request["CalledNumber"] ?? string.Empty;
            //CallAPIID = request["CallAPIID"];
            EventTime = request["EventTime"].TryParseLong();
        }

        public static TelphinNotificationDto Factory(ETelphinEvent type, HttpRequestBase request)
        {
            switch (type)
            {
                case ETelphinEvent.DialIn:
                    return new TelphinDialInDto(request);
                case ETelphinEvent.DialOut:
                    return new TelphinDialOutDto(request);
                case ETelphinEvent.HangUp:
                    return new TelphinHangUpDto(request);
                case ETelphinEvent.Answer:
                    return new TelphinAnswerDto(request);
                default:
                    return null;
            }
        }
    }

    public class TelphinDialInDto : TelphinNotificationDto
    {
        public override ETelphinEvent EventType
        {
            get { return ETelphinEvent.DialIn; }
        }

        public TelphinDialInDto(HttpRequestBase request)
            : base(request)
        {
        }

        public TelphinDialInDto()
        {
        }
    }

    public class TelphinDialOutDto : TelphinNotificationDto
    {
        public override ETelphinEvent EventType
        {
            get { return ETelphinEvent.DialOut; }
        }

        public TelphinDialOutDto(HttpRequestBase request)
            : base(request)
        {
        }

        public TelphinDialOutDto()
        {
        }
    }

    public class TelphinHangUpDto : TelphinNotificationDto
    {
        public override ETelphinEvent EventType
        {
            get { return ETelphinEvent.HangUp; }
        }

        /// <summary>
        /// Если на добавочном включена запись разговоров, то тут содержится ее идентификатор
        /// </summary>
        public string RecID { get; set; }

        /// <summary>
        /// время разговора в микросекундах
        /// </summary>
        public long Duration { get; set; }

        public TelphinHangUpDto(HttpRequestBase request)
            : base(request)
        {
            RecID = request["RecID"] ?? string.Empty;
            Duration = request["Duration"].TryParseLong() / 1000000;
        }

        public TelphinHangUpDto()
        {
        }
    }

    public class TelphinAnswerDto : TelphinNotificationDto
    {
        public override ETelphinEvent EventType
        {
            get { return ETelphinEvent.Answer; }
        }

        public TelphinAnswerDto(HttpRequestBase request)
            : base(request)
        {
        }

        public TelphinAnswerDto()
        {
        }
    }
}
