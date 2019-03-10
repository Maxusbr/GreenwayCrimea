using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.IPTelephony.Telphin
{
    public class TelphinEvent
    {
        public static Dictionary<ETelphinEvent, string> EventsDict
        {
            get
            {
                return new Dictionary<ETelphinEvent, string>
                {
                    { ETelphinEvent.DialIn, "dial-in" },
                    { ETelphinEvent.DialOut, "dial-out" },
                    { ETelphinEvent.HangUp, "hangup" },
                    { ETelphinEvent.Answer, "answer" }
                };
            }
        }

        public TelphinEvent()
        {
            Method = "GET";
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("event_type")]
        public string EventType { get; set; }

        [JsonProperty("eventTypeName")]
        public string EventTypeFormatted
        {
            get
            {
                return EventsDict.ContainsValue(EventType)
                    ? EventsDict.First(x => x.Value == EventType).Key.Localize()
                    : EventType;
            }
        }
    }
}
