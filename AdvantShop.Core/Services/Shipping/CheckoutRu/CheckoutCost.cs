using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Globalization;

namespace AdvantShop.Shipping.CheckoutRu
{
    [Serializable()]
    public class CheckoutCost
    {
        [JsonProperty(PropertyName = "postamat")]
        private CheckoutDeliveryCost Postamats { get; set; }

        [JsonProperty(PropertyName = "pvz")]
        private CheckoutDeliveryCost Pvz { get; set; }

        [JsonProperty(PropertyName = "express")]
        private CheckoutDeliveryCost Express { get; set; }

        [JsonProperty(PropertyName = "mail")]
        private CheckoutDeliveryCost Mail { get; set; }


        private List<CheckoutDeliveryModel> GetDeliveriesWithPoints(CheckoutDeliveryCost checkoutDeliveryCost, DeliveryType deliveryType, int deliveryCompanyId = 0)
        {
            var deliveriesList = new List<CheckoutDeliveryModel>();

            if (checkoutDeliveryCost != null && checkoutDeliveryCost.deliveries != null)
            {
                for (int i = 0; i < checkoutDeliveryCost.deliveries.Count; i++)
                {
                    var deliveryId = 0;
                    if (!int.TryParse(checkoutDeliveryCost.deliveries[i], out deliveryId))
                    {
                        continue;
                    }

                    var minDeliveryTerm = 0;
                    if (!int.TryParse(checkoutDeliveryCost.minTerms[i], out minDeliveryTerm))
                    {
                        continue;
                    }

                    var maxDeliveryTerm = 0;
                    if (!int.TryParse(checkoutDeliveryCost.maxTerms[i], out maxDeliveryTerm))
                    {
                        continue;
                    }

                    var cost = 0f;
                    if (!float.TryParse(checkoutDeliveryCost.costs[i], NumberStyles.Float, CultureInfo.InvariantCulture, out cost))
                    {
                        continue;
                    }

                    //если нужна выборка по компании, но данная доставка к ней не относится
                    if (deliveryCompanyId != 0 && deliveryId != deliveryCompanyId)
                    {
                        continue;
                    }

                    deliveriesList.Add(new CheckoutDeliveryModel
                    {
                        DeliveryId = deliveryId,
                        Cost = cost,
                        Code = checkoutDeliveryCost.codes[i].Trim().Length > 50 ? checkoutDeliveryCost.codes[i].Trim().Substring(0,49) : checkoutDeliveryCost.codes[i].Trim(),
                        MaxDeliveryTerm = maxDeliveryTerm,
                        MinDeliveryTerm = minDeliveryTerm,
                        Address = checkoutDeliveryCost.addresses[i],
                        AdditionalInfo = checkoutDeliveryCost.additionalInfo[i],
                        Npp = checkoutDeliveryCost.npp[i],
                        CheckoutDeliveryType = deliveryType
                    });
                }
            }

            return deliveriesList;
        }

        private List<CheckoutDeliveryModel> GetDeliveriesWithoutPoints(CheckoutDeliveryCost checkoutDeliveryCost, DeliveryType deliveryType, int deliveryCompanyId = 0)
        {
            if (checkoutDeliveryCost != null)
            {
                var isValid = true;

                var deliveryId = 0;
                isValid &= int.TryParse(checkoutDeliveryCost.deliveryId, out deliveryId);

                var minDeliveryTerm = 0;
                isValid &= int.TryParse(checkoutDeliveryCost.minDeliveryTerm, out minDeliveryTerm);

                var maxDeliveryTerm = 0;
                isValid &= int.TryParse(checkoutDeliveryCost.maxDeliveryTerm, out maxDeliveryTerm);

                var cost = 0f;
                isValid &= float.TryParse(checkoutDeliveryCost.cost, NumberStyles.Float, CultureInfo.InvariantCulture, out cost);

                //если нужна выборка по компании, но данная доставка к ней не относится
                isValid &= deliveryCompanyId == 0 || (deliveryCompanyId != 0 && deliveryId == deliveryCompanyId);

                if (isValid)
                {
                    return new List<CheckoutDeliveryModel>
                {
                    new CheckoutDeliveryModel
                    {
                        DeliveryId = deliveryId,
                        Cost = cost,
                        MaxDeliveryTerm = maxDeliveryTerm,
                        MinDeliveryTerm = minDeliveryTerm,
                        CheckoutDeliveryType = deliveryType
                    }
                };
                }
            }

            return new List<CheckoutDeliveryModel>();
        }

        public List<CheckoutDeliveryModel> GetDeliveries(DeliveryType deliveryType)
        {
            switch (deliveryType)
            {
                case DeliveryType.Postamat:
                    return GetDeliveriesWithPoints(Postamats, DeliveryType.Postamat);
                case DeliveryType.Pvz:
                    return GetDeliveriesWithPoints(Pvz, DeliveryType.Pvz);
                case DeliveryType.Express:
                    return GetDeliveriesWithoutPoints(Express, DeliveryType.Express);
                case DeliveryType.Mail:
                    return GetDeliveriesWithoutPoints(Mail, DeliveryType.Mail);
            }

            return new List<CheckoutDeliveryModel>();
        }

        public List<CheckoutDeliveryModel> GetDeliveries(DeliveryType deliveryType, int deliveryId)
        {
            switch (deliveryType)
            {
                case DeliveryType.Postamat:
                    return GetDeliveriesWithPoints(Postamats, DeliveryType.Postamat, deliveryId);
                case DeliveryType.Pvz:
                    return GetDeliveriesWithPoints(Pvz, DeliveryType.Pvz, deliveryId);
                case DeliveryType.Express:
                    return GetDeliveriesWithoutPoints(Express, DeliveryType.Express, deliveryId);
                case DeliveryType.Mail:
                    return GetDeliveriesWithoutPoints(Mail, DeliveryType.Mail, deliveryId);
            }

            return new List<CheckoutDeliveryModel>();
        }
    }
}