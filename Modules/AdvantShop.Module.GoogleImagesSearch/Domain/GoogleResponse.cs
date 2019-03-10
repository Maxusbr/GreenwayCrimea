using System.Collections.Generic;

namespace AdvantShop.Module.GoogleImagesSearch.Domain
{
    public class GoogleResponse
    {
        public GoogleError error { get; set; }
        public List<GoogleResultItem> items { get; set; }
    }
}
