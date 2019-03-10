//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
using System.Collections.Generic;
using System.Linq;

namespace AdvantShop.Repository
{
    public class Measure
    {
        public float[] XYZ { get; set; }
        public float Amount { get; set; }
    }

    public class MeasureHelper
    {
        public static float[] GetDimensions(List<Measure> dimensions)
        {
            var result = new float[3];
            foreach (var item in dimensions)
            {
                item.XYZ = item.XYZ.OrderByDescending(d => d).ToArray();
            }

            foreach (var dim in dimensions)
            {
                if (dim.XYZ[0] >= result[0])
                    result[0] = dim.XYZ[0];

                if (dim.XYZ[1] >= result[1])
                    result[1] = dim.XYZ[1];

                result[2] += dim.XYZ[2] * dim.Amount;
            }
            return result;
        }
    }
}