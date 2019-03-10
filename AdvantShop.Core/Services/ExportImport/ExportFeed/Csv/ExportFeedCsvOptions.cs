
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.ExportImport
{
    [Serializable()]
    public class ExportFeedCsvOptions : ExportFeedSettings
    {
        [JsonProperty(PropertyName = "CsvEnconing")]
        public string CsvEnconing { get; set; }

        [JsonProperty(PropertyName = "CsvSeparator")]
        public string CsvSeparator { get; set; }

        [JsonProperty(PropertyName = "CsvColumSeparator")]
        public string CsvColumSeparator { get; set; }

        [JsonProperty(PropertyName = "CsvPropertySeparator")]
        public string CsvPropertySeparator { get; set; }

        [JsonProperty(PropertyName = "CsvExportNoInCategory")]
        public bool CsvExportNoInCategory { get; set; }

        [JsonProperty(PropertyName = "CsvCategorySort")]
        public bool CsvCategorySort { get; set; }

        [JsonProperty(PropertyName = "AllOffersToMultiOfferColumn")]
        public bool AllOffersToMultiOfferColumn { get; set; }

        [JsonProperty(PropertyName = "FieldMapping")]
        public List<ProductFields> FieldMapping { get; set; }
        
        [JsonProperty(PropertyName = "ModuleFieldMapping")]
        public List<CSVField> ModuleFieldMapping { get; set; }
    }
}
