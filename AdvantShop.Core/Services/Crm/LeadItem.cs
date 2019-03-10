using System;
using System.Xml.Serialization;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Orders;

namespace AdvantShop.Core.Services.Crm
{
    public class LeadItem
    {
        public int LeadItemId { get; set; }

        public int LeadId { get; set; }

        public int? ProductId { get; set; }

        public string Name { get; set; }

        public string ArtNo { get; set; }

        public float Price { get; set; }

        public float Amount { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public int? PhotoId { get; set; }


        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        [NonSerialized]
        private ProductPhoto _photo;

        [XmlIgnore]
        public ProductPhoto Photo
        {
            get
            {
                if (_photo != null)
                    return _photo;

                _photo = PhotoId != null
                            ? PhotoService.GetPhoto<ProductPhoto>(PhotoId.Value, PhotoType.Product)
                            : null;

                if (_photo == null)
                    _photo = ProductId != null
                            ? PhotoService.GetPhotoByObjId<ProductPhoto>(ProductId.Value, PhotoType.Product)
                            : null;

                if (_photo == null)
                    _photo = new ProductPhoto(0, PhotoType.Product, "");

                return _photo;
            }
        }
        
        public float Weight { get; set; }

        

        public LeadItem()
        {
            
        }

        public LeadItem(Offer offer, float amount)
        {
            ProductId = offer.ProductId;
            Name = offer.Product.Name;
            ArtNo = offer.ArtNo;
            Amount = amount != 0 ? amount : 1;
            Price = PriceService.GetFinalPrice(offer.RoundedPrice, new Discount());
            Color = offer.Color != null ? offer.Color.ColorName : null;
            Size = offer.Size != null ? offer.Size.SizeName : null;
            PhotoId = offer.Photo != null ? offer.Photo.PhotoId : (int?) null;
            Weight = offer.Product.Weight;
            Width = offer.Product.Width;
            Length = offer.Product.Length;
            Height = offer.Product.Height;
        }

        public static explicit operator LeadItem(OrderItem item)
        {
            return new LeadItem
            {
                LeadItemId = item.OrderItemID,
                ProductId = item.ProductID,
                Name = item.Name,
                ArtNo = item.ArtNo,
                Price = item.Price,
                Amount = item.Amount,
                Weight = item.Weight,
                Color = item.Color,
                Size = item.Size,
                PhotoId = item.PhotoID,
                Width = item.Width,
                Length = item.Length,
                Height = item.Height
            };
        }
    }
}
