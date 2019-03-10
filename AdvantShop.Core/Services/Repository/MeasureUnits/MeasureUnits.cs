//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

namespace AdvantShop.Repository
{
    public class MeasureUnits
    {
        public enum WeightUnit
        {
            Kilogramm = 0,
            Pound
        }

        public static Dictionary<WeightUnit, float> WeightRates = new Dictionary<WeightUnit, float>
                                                                   {
                                                                       {WeightUnit.Kilogramm, 1},
                                                                       {WeightUnit.Pound, 0.45359237F}
                                                                   };

        public static float ConvertWeight(float value, WeightUnit from, WeightUnit to)
        {
            return value * WeightRates[from] / WeightRates[to];
        }
    }
}