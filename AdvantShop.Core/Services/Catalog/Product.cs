//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.SEO;
using AdvantShop.Repository.Currencies;


namespace AdvantShop.Catalog
{
    public enum RelatedType
    {
        Related = 0,
        Alternative = 1
    }

    public class Product
    {
        public Product()
        {
            Discount = new Discount();
            AccrueBonuses = true;
        }

        public int ProductId { get; set; }
        public string ArtNo { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string PhotoDesc { get; set; } // убрать или сделать lazy
        public double Ratio { get; set; } // убрать или сделать lazy
        public Discount Discount { get; set; }
        public float Weight { get; set; }

        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public string BriefDescription { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public bool Recomended { get; set; }
        public bool New { get; set; }
        public bool BestSeller { get; set; }
        public bool OnSale { get; set; }

        public bool AllowPreOrder { get; set; }
        public bool CategoryEnabled { get; set; }

        public string Unit { get; set; }
        public float? ShippingPrice { get; set; }

        public float? MinAmount { get; set; }
        public float? MaxAmount { get; set; }
        public float Multiplicity { get; set; }

        public string SalesNote { get; set; }
        public string GoogleProductCategory { get; set; }
        public string YandexMarketCategory { get; set; }

        public string YandexTypePrefix { get; set; }
        public string YandexModel { get; set; }

        public string Gtin { get; set; }
        public bool Adult { get; set; }
        public bool ManufacturerWarranty { get; set; }

        public float Cbid { get; set; }
        public float Fee { get; set; }

        public string ModifiedBy { get; set; }

        public bool ActiveView360 { get; set; }

        public string BarCode { get; set; }

        public string YandexSizeUnit { get; set; }
        
        public string YandexName { get; set; }

        /// <summary>
        /// Ќачисл€ть бонусы за покупку этого товара
        /// </summary>
        public bool AccrueBonuses { get; set; }

        public int? TaxId { get; set; }

        public int BrandId { get; set; }

        private Brand _brand;
        public Brand Brand
        {
            get { return _brand ?? (_brand = BrandService.GetBrandById(BrandId)); }
        }


        private string _urlPath;
        public string UrlPath
        {
            get { return _urlPath; }
            set { _urlPath = value.ToLower(); }
        }


        public bool HasMultiOffer { get; set; }

        private bool? _gifts;
        public bool HasGifts()
        {
            if (_gifts.HasValue)
                return _gifts.Value;
            _gifts = ProductService.HasGifts(ProductId);
            return _gifts.Value;
        }

        public int CurrencyID { get; set; }
        private Currency _currency;
        public Currency Currency
        {
            get { return _currency ?? (_currency = CurrencyService.GetCurrency(CurrencyID, true)); }
        }

        /// <summary>
        /// Get from DB collection of Offer and set collection
        /// </summary>
        private List<Offer> _offers;

        public List<Offer> Offers
        {
            get { return _offers ?? (_offers = OfferService.GetProductOffers(ProductId)); }
            set { _offers = value; }
        }

        public Discount CalculableDiscount()
        {
            foreach (var discountModule in AttachedModules.GetModules<IDiscount>().Where(x => x != null))
            {
                var discount = ((IDiscount)Activator.CreateInstance(discountModule)).GetDiscount(ProductId);
                if (discount != 0)
                    return new Discount(discount, 0);
            }

            if (Discount.HasValue)
                return Discount;

            return new Discount(DiscountByTimeService.GetDiscountByTime(), 0);
        }

        public MetaType MetaType
        {
            get { return MetaType.Product; }
        }

        private bool _isMetaLoaded;
        private MetaInfo _meta;

        public MetaInfo Meta
        {
            get
            {
                if (_isMetaLoaded)
                    return _meta;

                _isMetaLoaded = true;

                return _meta ??
                       (_meta =
                           MetaInfoService.GetMetaInfo(ProductId, MetaType) ??
                           MetaInfoService.GetDefaultMetaInfo(MetaType, string.Empty));
            }
            set
            {
                _meta = value;
                _isMetaLoaded = true;
            }
        }

        /// <summary>
        /// return collection of ProductPhoto
        /// </summary>
        private List<ProductPhoto> _productphotos;
        public List<ProductPhoto> ProductPhotos
        {
            get { return _productphotos ?? (_productphotos = PhotoService.GetPhotos<ProductPhoto>(ProductId, PhotoType.Product)); }
        }

        /// <summary>
        /// return collection of ProductPhoto, 360 view
        /// </summary>
        private List<ProductPhoto> _productphotos360;
        public List<ProductPhoto> ProductPhotos360
        {
            get { return _productphotos360 ?? (_productphotos360 = PhotoService.GetPhotos<ProductPhoto>(ProductId, PhotoType.Product360)); }
        }


        private List<ProductVideo> _productVideos;
        public List<ProductVideo> ProductVideos
        {
            get { return _productVideos ?? (_productVideos = ProductVideoService.GetProductVideos(ProductId)); }
        }

        private List<PropertyValue> _productPropertyValues;
        public List<PropertyValue> ProductPropertyValues
        {
            get
            {
                return _productPropertyValues ??
                       (_productPropertyValues = PropertyService.GetPropertyValuesByProductId(ProductId));
            }
        }

        private int _categoryId;
        public int CategoryId
        {
            get { return _categoryId == 0 || _categoryId == CategoryService.DefaultNonCategoryId ? _categoryId = ProductService.GetFirstCategoryIdByProductId(ProductId) : _categoryId; }
        }

        private Category _mainCategory;
        public Category MainCategory
        {
            get { return _mainCategory ?? (_mainCategory = CategoryService.GetCategory(CategoryId)); }
        }

        private List<Category> _productCategories;
        [SoapIgnore]
        [XmlIgnoreAttribute]
        public List<Category> ProductCategories
        {
            get { return _productCategories ?? (_productCategories = ProductService.GetCategoriesByProductId(ProductId)); }
        }

        private IList<Tag> _tag;
        public IList<Tag> Tags
        {
            get { return _tag ?? (_tag = TagService.Gets(ProductId, ETagType.Product, true)); }
            set { _tag = value; }
        }
    }
}