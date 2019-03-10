//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Taxes;

namespace AdvantShop.Orders
{
    [Serializable]
    public class OrderItem : IOrderItem
    {
        public OrderItem()
        {
            AccrueBonuses = true;
        }

        public int OrderItemID { get; set; }

        public int OrderID { get; set; }

        public int? ProductID { get; set; }

        public int? CertificateID { get; set; }

        [Compare("Core.Orders.OrderItem.ArtNo")]
        public string ArtNo { get; set; }

        [Compare("Core.Orders.OrderItem.Name")]
        public string Name { get; set; }

        [Compare("Core.Orders.OrderItem.Price")]
        public float Price { get; set; }

        [Compare("Core.Orders.OrderItem.Amount")]
        public float Amount { get; set; }

        public float DecrementedAmount { get; set; }

        [Compare("Core.Orders.OrderItem.Color")]
        public string Color { get; set; }

        [Compare("Core.Orders.OrderItem.Size")]
        public string Size { get; set; }

        [Compare("Core.Orders.OrderItem.IsCouponApplied")]
        public bool IsCouponApplied { get; set; }

        public float SupplyPrice { get; set; }

        [Compare("Core.Orders.OrderItem.Weight")]
        public float Weight { get; set; }

        public int? PhotoID { get; set; }

        public bool IgnoreOrderDiscount { get; set; }
        public bool AccrueBonuses { get; set; }

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

                _photo = PhotoID != null
                            ? PhotoService.GetPhoto<ProductPhoto>(PhotoID.Value, PhotoType.Product)
                            : null;

                if (_photo == null)
                    _photo = ProductID != null
                            ? PhotoService.GetPhotoByObjId<ProductPhoto>(ProductID.Value, PhotoType.Product)
                            : null;

                if (_photo == null)
                    _photo = new ProductPhoto(0, PhotoType.Product, "");

                return _photo;
            }
        }

        public int? TaxId { get; set; }
        public string TaxName { get; set; }
        public TaxType? TaxType { get; set; }
        public float? TaxRate { get; set; }
        public bool? TaxShowInPrice { get; set; }

        public List<EvaluatedCustomOptions> SelectedOptions { get; set; }

        public static explicit operator OrderItem(ShoppingCartItem item)
        {
            var orderItem = new OrderItem
            {
                ProductID = item.Offer.ProductId,
                Name = item.Offer.Product.Name,
                ArtNo = item.Offer.ArtNo,
                Price = item.PriceWithDiscount,
                Amount = item.Amount,
                SupplyPrice = item.Offer.SupplyPrice,
                SelectedOptions = CustomOptionsService.DeserializeFromXml(item.AttributesXml, item.Offer.Product.Currency.Rate),
                Weight = item.Offer.Product.Weight,
                IsCouponApplied = item.IsCouponApplied,
                Color = item.Offer.ColorID != null ? item.Offer.Color.ColorName : null,
                Size = item.Offer.SizeID != null ? item.Offer.Size.SizeName : null,
                PhotoID = item.Offer.Photo != null ? item.Offer.Photo.PhotoId : (int?)null,
                IgnoreOrderDiscount = item.ModuleKey.IsNotEmpty() && item.Discount.HasValue,
                AccrueBonuses = item.Offer.Product.AccrueBonuses,
                Width = item.Offer.Product.Width,
                Length = item.Offer.Product.Length,
                Height = item.Offer.Product.Height
            };
            var tax = item.Offer.Product.TaxId != null ? TaxService.GetTax(item.Offer.Product.TaxId.Value) : null;
            if (tax != null && tax.Enabled)
            {
                orderItem.TaxId = tax.TaxId;
                orderItem.TaxName = tax.Name;
                orderItem.TaxType = tax.TaxType;
                orderItem.TaxRate = tax.Rate;
                orderItem.TaxShowInPrice = tax.ShowInPrice;
            }

            return orderItem;
        }

        public static explicit operator OrderItem(LeadItem item)
        {
            var p = item.ProductId != null ? ProductService.GetProduct(item.ProductId.Value) : null;

            return new OrderItem
            {
                ProductID = item.ProductId,
                Name = item.Name,
                ArtNo = item.ArtNo,
                Price = item.Price,
                Amount = item.Amount,
                Weight = item.Weight,
                Color = item.Color,
                Size = item.Size,
                PhotoID = item.PhotoId,
                OrderItemID = item.LeadItemId,
                AccrueBonuses = p != null ? p.AccrueBonuses : true,
                Width = item.Width,
                Length = item.Length,
                Height = item.Height
            };
        }

        public static bool operator ==(OrderItem first, OrderItem second)
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }

            if (((object)first == null) || ((object)second == null))
            {
                return false;
            }

            return first.ProductID == second.ProductID && first.ArtNo == second.ArtNo && 
                   first.Color == second.Color && first.Size == second.Color && 
                   first.SelectedOptions.SequenceEqual(second.SelectedOptions);
        }

        public static bool operator !=(OrderItem first, OrderItem second)
        {
            return !(first == second);
        }

        public bool Equals(OrderItem other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.ArtNo == ArtNo &&
                other.Name == Name &&
                other.ProductID == ProductID &&
                other.Amount == Amount &&
                other.Price == Price &&
                other.SupplyPrice == SupplyPrice &&
                other.Color == Color &&
                other.Size == Size &&
                Equals(other.SelectedOptions, SelectedOptions) &&
                other.OrderItemID == OrderItemID)
            {
                return true;
            }

            //WARNING !!!!!! Equals() is same shit as == operator !!!!!!!!!!!
            return other == this;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj.GetType() == typeof(OrderItem) && Equals((OrderItem)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ProductID ?? 1 * 397 ^ Amount.GetHashCode() * 397 ^ (SelectedOptions != null ? SelectedOptions.AggregateHash() : 1);
            }
        }

        public string _unit { get; set; }
        public string Unit
        {
            get
            {
                if (_unit != null)
                    return _unit;
                var product = ProductID != null ? ProductService.GetProduct(ProductID.Value) : null;
                _unit = product != null ? product.Unit : string.Empty;
                return _unit;
            }
        }
    }
}