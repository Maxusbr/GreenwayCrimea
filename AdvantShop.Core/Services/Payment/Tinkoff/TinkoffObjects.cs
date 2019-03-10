//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using Newtonsoft.Json;

namespace AdvantShop.Core.Services.Payment.Tinkoff
{
    public class TinkoffBaseData
    {
        [JsonProperty(Required = Required.Always)]
        public string TerminalKey { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Token { get; set; }

    }
    public class TinkoffInitData : TinkoffBaseData
    {
        [JsonProperty(Required = Required.Always)]
        public int Amount { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string OrderId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IP { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Language { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CustomerKey { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public char? Recurrent { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object DATA { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Receipt Receipt { get; set; }
    }

    public class TinkoffNotifyData
    {
        public string TerminalKey { get; set; }
        public string Token { get; set; }
        public string OrderId { get; set; }
        public string Success { get; set; }
        public string Status { get; set; }
        public string PaymentId { get; set; }
        public string ErrorCode { get; set; }
        public string Amount { get; set; }
        public string RebillId { get; set; }
        public string CardId { get; set; }
        public string Pan { get; set; }
        public object DATA { get; set; }
        public string ExpDate { get; set; }

    }

}
