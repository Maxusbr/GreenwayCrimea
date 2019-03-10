using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;

using AdvantShop.Shipping.Boxberry;
using AdvantShop.Core.Services.Shipping.Boxberry;
using System;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class BoxberryShippingModel : ShippingMethodAdminModel, IValidatableObject
    {
        public string Token
        {
            get { return Params.ElementOrDefault(BoxberryTemplate.Token); }
            set { Params.TryAddValue(BoxberryTemplate.Token, value.DefaultOrEmpty()); }
        }
        public string IntegrationToken
        {
            get { return Params.ElementOrDefault(BoxberryTemplate.IntegrationToken); }
            set { Params.TryAddValue(BoxberryTemplate.IntegrationToken, value.DefaultOrEmpty()); }
        }        

        public string ReceptionPointCode
        {
            get { return Params.ElementOrDefault(BoxberryTemplate.ReceptionPointCode); }
            set { Params.TryAddValue(BoxberryTemplate.ReceptionPointCode, value.DefaultOrEmpty()); }
        }

        public string DefaultWeight
        {
            get { return Params.ElementOrDefault(BoxberryTemplate.DefaultWeight) ?? "1"; }
            set { Params.TryAddValue(BoxberryTemplate.DefaultWeight, value.TryParseFloat().ToString()); }
        }
        public string DefaultLength
        {
            get { return Params.ElementOrDefault(BoxberryTemplate.DefaultLength) ?? "100"; }
            set { Params.TryAddValue(BoxberryTemplate.DefaultLength, value.TryParseFloat().ToString()); }
        }
        public string DefaultWidth
        {
            get { return Params.ElementOrDefault(BoxberryTemplate.DefaultWidth) ?? "100"; }
            set { Params.TryAddValue(BoxberryTemplate.DefaultWidth, value.TryParseFloat().ToString()); }
        }
        public string DefaultHeight
        {
            get { return Params.ElementOrDefault(BoxberryTemplate.DefaultHeight) ?? "100"; }
            set { Params.TryAddValue(BoxberryTemplate.DefaultHeight, value.TryParseFloat().ToString()); }
        }

   public bool CalculateCourier
        {
            get { return  Params.ElementOrDefault(BoxberryTemplate.CalculateCourier).TryParseBool(); }
            set { Params.TryAddValue(BoxberryTemplate.CalculateCourier, value.ToString()); }
        }
        
        public List<BoxberryParcelPoint> ListReceptionPoints
        {
            get
            {
                return new BoxberryApiService(
                    this.Token, 
                    this.ReceptionPointCode, 
                    float.Parse(this.DefaultWeight), 
                    float.Parse(this.DefaultHeight), 
                    float.Parse(this.DefaultWidth), 
                    float.Parse(this.DefaultLength)).GetListPointsForParcels();
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
