//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Catalog
{
    [Serializable]
    public class EvaluatedCustomOptions
    {
        public int CustomOptionId { get; set; }
        public int OptionId { get; set; }
        public string CustomOptionTitle { get; set; }
        public string OptionTitle { get; set; }
        public float OptionPriceBc { get; set; }
        public string FormatPrice
        {
            get
            {
                if (OptionPriceBc == 0)
                    return string.Empty;

                return OptionPriceType == OptionPriceType.Fixed
                    ? (OptionPriceBc > 0 ? "+ " : string.Empty) + OptionPriceBc.FormatPrice()
                    : OptionPriceBc + " %";
            }
        }

        public OptionPriceType OptionPriceType { get; set; }

        public static bool operator ==(EvaluatedCustomOptions first, EvaluatedCustomOptions second)
        {
            object firstObj = first;
            object secondObj = second;

            if ((firstObj != null) && (secondObj != null))
            {
                return first.CustomOptionId == second.CustomOptionId && first.OptionId == second.OptionId;
            }
            if ((firstObj == null) && (secondObj == null))
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(EvaluatedCustomOptions first, EvaluatedCustomOptions second)
        {
            return !(first == second);
        }

        public bool Equals(EvaluatedCustomOptions other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.CustomOptionId == CustomOptionId && other.OptionId == OptionId && Equals(other.CustomOptionTitle, CustomOptionTitle) && Equals(other.OptionTitle, OptionTitle) && other.OptionPriceBc == OptionPriceBc && Equals(other.OptionPriceType, OptionPriceType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(EvaluatedCustomOptions)) return false;
            return Equals((EvaluatedCustomOptions)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = CustomOptionId;
                result = (result * 397) ^ OptionId;
                //result = (result*397) ^ (CustomOptionTitle != null ? CustomOptionTitle.GetHashCode() : 0);
                //result = (result*397) ^ (OptionTitle != null ? OptionTitle.GetHashCode() : 0);
                //result = (result*397) ^ OptionPriceBc.GetHashCode();
                //result = (result*397) ^ OptionPriceType.GetHashCode();
                return result;
            }
        }
    }
}