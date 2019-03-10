﻿using AdvantShop.Saas;
using System.Collections.Generic;

namespace AdvantShop.Web.Admin.ViewModels.Import
{
    public class ImportProductsModel
    {
        public string ColumnSeparator { get; set; }

        public string PropertySeparator { get; set; }

        public string PropertyValueSeparator { get; set; }

        public string Encoding { get; set; }

        public bool HaveHeader { get; set; }

        public bool DisableProducts { get; set; }

        
        public string CsvFileName { get; set; }

        public string ZipPhotoFileName { get; set; }

        
        public Dictionary<string, string> Encodings { get; set; }

        public Dictionary<string, string> ColumnSeparators { get; set; }


        public SaasData CurrentSaasData { get; set; }

        public bool IsStartExport { get; set; }


    }
}
