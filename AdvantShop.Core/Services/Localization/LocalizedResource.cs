using System.Collections.Generic;

namespace AdvantShop.Core.Services.Localization
{
    public class LocalizedSetPair
    {
        public string Culture { get; set; }
        public Dictionary<string, string> Resources { get; set; }
    }

    public class LocalizedResource
    {
        public int Id { get; set; }

        public int LanguageId { get; set; }

        public string ResourceKey { get; set; }

        public string ResourceValue { get; set; }
    }
}
