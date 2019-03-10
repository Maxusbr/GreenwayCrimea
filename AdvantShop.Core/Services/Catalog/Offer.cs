//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Xml.Serialization;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Catalog
{
    [Serializable]
    public class Offer
    {
        public int OfferId { get; set; }

        public int ProductId { get; set; }

        public float Amount { get; set; }

        /// <summary>
        /// Price from db
        /// </summary>
        public float BasePrice { get; set; }

        private float? _roundedPrice;

        public float RoundedPrice
        {
            get
            {
                return _roundedPrice ??
                       (float) (_roundedPrice = PriceService.RoundPrice(BasePrice, null, Product.Currency.Rate));
            }
        }

        public float SupplyPrice { get; set; }

        public int? ColorID { get; set; }

        public int? SizeID { get; set; }

        public bool Main { get; set; }

        public string ArtNo { get; set; }

        [NonSerialized] private Color _color;

        [XmlIgnore]
        public Color Color
        {
            get { return _color ?? (_color = ColorService.GetColor(ColorID)); }
        }

        [NonSerialized] 
        private Size _size;

        [XmlIgnore]
        public Size Size
        {
            get { return _size ?? (_size = SizeService.GetSize(SizeID)); }
        }

        [NonSerialized] private Product _product;

        [XmlIgnore]
        public Product Product
        {
            get { return _product ?? (_product = ProductService.GetProduct(ProductId)); }
        }

        [NonSerialized] 
        private ProductPhoto _photo;

        [XmlIgnore]
        public ProductPhoto Photo
        {
            get { return _photo ?? (_photo = PhotoService.GetMainProductPhoto(ProductId, ColorID)); }
        }

        [XmlIgnore]
        public bool CanOrderByRequest
        {
            get { return Product != null && Product.AllowPreOrder && (Amount <= 0 || RoundedPrice == 0); }
        }
    }
}
