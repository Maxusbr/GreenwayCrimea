//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
using System;
using AdvantShop.DownloadableContent.Interfaces;
using Newtonsoft.Json;

namespace AdvantShop.DownloadableContent
{
    [Serializable]
    public class DownloadableContentObject : IDownloadableContent
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "StringId")]
        public string StringId { get; set; }

        [JsonProperty(PropertyName = "Version")]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "Active")]
        public bool Active { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Price")]
        public float Price { get; set; }

        [JsonProperty(PropertyName = "Currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "DetailsLink")]
        public string DetailsLink { get; set; }

        [JsonProperty(PropertyName = "BriefDescription")]
        public string BriefDescription { get; set; }

        [JsonProperty(PropertyName = "SortOrder")]
        public int SortOrder { get; set; }

        [JsonProperty(PropertyName = "Icon")]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "Developer")]
        public string Developer { get; set; }

        [JsonProperty(PropertyName = "DeveloperSupport")]
        public string DeveloperSupport { get; set; }

        [JsonProperty(PropertyName = "DeveloperWebSite")]
        public string DeveloperWebSite { get; set; }

        public string DcType { get; set; }

        public string CurrentVersion { get; set; }
        public bool IsLocalVersion { get; set; }
        public bool IsInstall { get; set; }

        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }

        public bool NeedUpdate { get; set; }

        
    }
}