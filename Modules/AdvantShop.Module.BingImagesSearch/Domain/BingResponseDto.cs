using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.BingImagesSearch.Domain
{
    public class BingResponseDto
    {
        [JsonProperty("items")]
        public List<BingImageDto> Items { get; set; }

        [JsonProperty("errors")]
        public List<BingError> Errors { get; set; }
    }
}
