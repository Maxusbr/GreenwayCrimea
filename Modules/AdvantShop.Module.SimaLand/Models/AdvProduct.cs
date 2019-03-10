using AdvantShop.Catalog;
using AdvantShop.Module.SimaLand.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Models
{
    public class AdvProduct : Product
    {
        public AdvProduct()
        {

        }

        public AdvProduct(Product product)
        {
            ProductId = product.ProductId;
            ArtNo = product.ArtNo;
            Name = product.Name;
            Photo = product.Photo;
            PhotoDesc = product.PhotoDesc;
            Ratio = product.Ratio;
            Discount = product.Discount;
            Weight = product.Weight;
            Length = product.Length;
            Width = product.Width;
            Height = product.Height;
            BriefDescription = product.BriefDescription;
            Description = product.Description;
            Enabled = product.Enabled;
            Recomended = product.Recomended;
            New = product.New;
            BestSeller = product.BestSeller;
            OnSale = product.OnSale;
            AllowPreOrder = product.AllowPreOrder;
            CategoryEnabled = product.CategoryEnabled;
            Unit = product.Unit;
            ShippingPrice = product.ShippingPrice;
            MinAmount = product.MinAmount;
            MaxAmount = product.MaxAmount;
            Multiplicity = product.Multiplicity;
            SalesNote = product.SalesNote;
            GoogleProductCategory = product.GoogleProductCategory;
            YandexMarketCategory = product.YandexMarketCategory;
            YandexTypePrefix = product.YandexTypePrefix;
            YandexModel = product.YandexModel;
            Gtin = product.Gtin;
            Adult = product.Adult;
            ManufacturerWarranty = product.ManufacturerWarranty;
            ModifiedBy = product.ModifiedBy;
            ActiveView360 = product.ActiveView360;
            BarCode = product.BarCode;
            BrandId = product.BrandId;
            UrlPath = product.UrlPath;
            HasMultiOffer = product.HasMultiOffer;
            CurrencyID = product.CurrencyID;
            Offers = product.Offers;
            Meta = product.Meta;
            Tags = product.Tags;
        }

        public void CheckAttributes(SimalandProduct slProduct)
        {
            this.Name = PSLModuleSettings.NotUpdateName && (Name != null) ? Name : slProduct.name ?? string.Empty;
            this.Description = PSLModuleSettings.NotUpdateDescription && (Description != null) ? Description : slProduct.description;
            this.UrlPath = PSLModuleSettings.NotUpdateUrl && (UrlPath != null) ? UrlPath : slProduct.slug;
        }
    }
}
