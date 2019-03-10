using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Orders;
using Newtonsoft.Json;

namespace AdvantShop.Shipping.Edost
{
	public class EdostBoxberryOption : EdostOption
    {
        //public string PickpointId { get; set; }
        //public string PickpointAddress { get; set; }
        public EdostBoxberryPoint SelectedPoint { get; set; }
        public List<EdostBoxberryPoint> ShippingPoints { get; set; }

        public EdostBoxberryOption()
        {
        }

        public EdostBoxberryOption(ShippingMethod method, EdostTarif tarif, IEnumerable<EdostOffice> offices)
            : base(method, tarif)
        {
            var temp = offices.Where(x => x.TarifId == tarif.Id);
            ShippingPoints = temp.Select(x => new EdostBoxberryPoint
            {
                Id = x.Id,
                Address = x.Name,
                Description = x.Address,
                Tel = x.Tel,
                Scheldule = x.Scheldule
            }).ToList();
            HideAddressBlock = true;
        }

        public override string Template
        {
            get { return UrlService.GetUrl() + "scripts/_partials/shipping/extendTemplate/EdostSelectOption.html"; }
        }

        public override void Update(BaseShippingOption option)
        {
            var opt = option as EdostBoxberryOption;
            if (opt != null && this.Id == opt.Id)
            {
                this.SelectedPoint = opt.SelectedPoint != null ? this.ShippingPoints.FirstOrDefault(x => x.Id == opt.SelectedPoint.Id) : null;
                this.SelectedPoint = this.SelectedPoint ?? opt.ShippingPoints.FirstOrDefault();
            }
        }

        public override OrderPickPoint GetOrderPickPoint()
        {
            return new OrderPickPoint
            {
                PickPointId = Id,
                PickPointAddress = SelectedPoint.Address,
                AdditionalData = JsonConvert.SerializeObject(SelectedPoint)
            };
        }
    }
}