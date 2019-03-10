using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Web.Infrastructure.ActionResults
{
    public class CommandResult
    {
        [JsonProperty(PropertyName = "result")]
        public bool Result { get; set; }

        [JsonProperty(PropertyName = "errors", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<string> Errors { get; set; }

        [JsonProperty(PropertyName = "error", DefaultValueHandling = DefaultValueHandling.Ignore)]
        ///[Obsolete("Do not use in future, use errors")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "obj", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object Obj { get; set; }
    }
}
