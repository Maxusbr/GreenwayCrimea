using System;

namespace AdvantShop.Shipping.RangeWeightAndDistanceOption
{
    [Serializable]
    public class WeightLimit
    {
        public float Amount { get; set; }
        public bool PerUnit { get; set; }
        public float Price { get; set; }
    }
}