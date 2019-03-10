
using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Scheduler;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace AdvantShop.ExportImport
{
    [Serializable()]
    public class ExportFeedSettings
    {
        [JsonProperty(PropertyName = "FileName")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "FileExtention")]
        public string FileExtention { get; set; }

        [JsonProperty(PropertyName = "PriceMargin")]
        public float PriceMargin { get; set; }

        //[JsonProperty(PropertyName = "ExportNotActiveProducts")]
        //public bool ExportNotActiveProducts { get; set; }

        //[JsonProperty(PropertyName = "ExportNotAmountProducts")]
        //public bool ExportNotAmountProducts { get; set; }

        [JsonProperty(PropertyName = "AdditionalUrlTags")]
        public string AdditionalUrlTags { get; set; }

        [JsonProperty(PropertyName = "Active")]
        public bool Active { get; set; }

        [JsonProperty(PropertyName = "IntervalType")]
        public TimeIntervalType IntervalType { get; set; }

        [JsonProperty(PropertyName = "Interval")]
        public int Interval { get; set; }

        [JsonProperty(PropertyName = "JobStartTime")]
        public DateTime JobStartTime { get; set; }

        [JsonProperty(PropertyName = "AdvancedSettings")]
        public string AdvancedSettings { get; set; }

        [JsonProperty(PropertyName = "ExportAllProducts")]
        public bool ExportAllProducts { get; set; }


        [JsonIgnore]
        private string _fileName;

        [JsonIgnore]
        public string FileFullName
        {
            get
            {
                if (string.IsNullOrEmpty(_fileName))
                {
                    _fileName = FileName + "." + FileExtention;
                    _fileName = _fileName.Replace("#DATE#", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    _fileName = _fileName.Replace("#SALT#", BitConverter.ToString(new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(SettingsLic.LicKey ?? string.Empty)), 0, 10));
                    return _fileName;
                }
                else
                {
                    return _fileName;
                }
            }
        }

        [JsonIgnore]
        public string FileFullPath { get { return SettingsGeneral.AbsolutePath + FileFullName; } }

        [JsonIgnore]
        public string Value { get; set; }

    }
}
