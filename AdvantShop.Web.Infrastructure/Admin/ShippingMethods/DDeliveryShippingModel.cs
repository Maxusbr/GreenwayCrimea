using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;

using AdvantShop.Shipping.DDelivery;
using AdvantShop.Core.Services.Shipping.DDelivery;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class DDeliveryShippingModel : ShippingMethodAdminModel, IValidatableObject
    {
        public string ApiKey
        {
            get { return Params.ElementOrDefault(DDeliveryTemplate.ApiKey); }
            set { Params.TryAddValue(DDeliveryTemplate.ApiKey, value.DefaultOrEmpty()); }
        }

        public string ShopId
        {
            get { return Params.ElementOrDefault(DDeliveryTemplate.ShopId); }
            set { Params.TryAddValue(DDeliveryTemplate.ShopId, value.DefaultOrEmpty()); }
        }

        public string DefaultWeight
        {
            get { return Params.ElementOrDefault(DDeliveryTemplate.DefaultWeight) ?? "1"; }
            set { Params.TryAddValue(DDeliveryTemplate.DefaultWeight, value.TryParseFloat().ToString()); }
        }
        public string DefaultLength
        {
            get { return Params.ElementOrDefault(DDeliveryTemplate.DefaultLength) ?? "100"; }
            set { Params.TryAddValue(DDeliveryTemplate.DefaultLength, value.TryParseFloat().ToString()); }
        }
        public string DefaultWidth
        {
            get { return Params.ElementOrDefault(DDeliveryTemplate.DefaultWidth) ?? "100"; }
            set { Params.TryAddValue(DDeliveryTemplate.DefaultWidth, value.TryParseFloat().ToString()); }
        }
        public string DefaultHeight
        {
            get { return Params.ElementOrDefault(DDeliveryTemplate.DefaultHeight) ?? "100"; }
            set { Params.TryAddValue(DDeliveryTemplate.DefaultHeight, value.TryParseFloat().ToString()); }
        }
        public string ReceptionCompanyId
        {
            get { return Params.ElementOrDefault(DDeliveryTemplate.ReceptionCompanyId) ?? "-1"; }
            set { Params.TryAddValue(DDeliveryTemplate.ReceptionCompanyId, value.TryParseInt().ToString()); }
        }
        public bool CreateDraftOrder
        {
            //  get { return Params.ElementOrDefault(DDeliveryTemplate.CreateDraftOrder).TryParseBool(); }
            get { return true; }
            set { Params.TryAddValue(DDeliveryTemplate.CreateDraftOrder, value.ToString()); }
        }
        public bool GroupingShippingOptions
        {
            get { return Params.ElementOrDefault(DDeliveryTemplate.GroupingShippingOptions).TryParseBool(); }
            set { Params.TryAddValue(DDeliveryTemplate.GroupingShippingOptions, value.ToString()); }
        }

        public bool UseWidget
        {
            get { return Params.ElementOrDefault(DDeliveryTemplate.UseWidget).TryParseBool(); }
            set { Params.TryAddValue(DDeliveryTemplate.UseWidget, value.ToString()); }
        }

        public List<DDeliveryObjectCompany> ListReceptionCompanies
        {
            get
            {
                return new DDeliveryApiService(
                    this.ApiKey,
                    this.ShopId,
                    this.ReceptionCompanyId,
                    this.CreateDraftOrder,
                    this.UseWidget,
                    float.Parse(this.DefaultWeight),
                    float.Parse(this.DefaultHeight),
                    float.Parse(this.DefaultWidth),
                    float.Parse(this.DefaultLength)).GetDeliveryCompanies()
                    ?? new List<DDeliveryObjectCompany>();
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
