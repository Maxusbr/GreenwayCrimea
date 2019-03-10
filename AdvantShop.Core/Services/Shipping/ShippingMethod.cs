//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Taxes;


namespace AdvantShop.Shipping
{
    //*********************************************
    [Serializable]
    public class ShippingMethod
    {
        public int ShippingMethodId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int SortOrder { get; set; }
        public bool DisplayCustomFields { get; set; }
        public bool DisplayIndex { get; set; }
        public bool ShowInDetails { get; set; }

        public string ZeroPriceMessage { get; set; }

        public TaxType TaxType { get; set; }

        private Photo _picture;
        public Photo IconFileName
        {
            get { return _picture ?? (_picture = PhotoService.GetPhotoByObjId(ShippingMethodId, PhotoType.Shipping)); }
            set { _picture = value; }
        }

        public string ShippingType { get; set; }

        private Dictionary<string, string> _params = new Dictionary<string, string>();
        public Dictionary<string, string> Params
        {
            get { return _params; }
            set { _params = value; }
        }
    }
}