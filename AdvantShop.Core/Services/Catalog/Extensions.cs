//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvantShop.Catalog
{

    public static class Extensions
    {
        public static EvaluatedCustomOptions WithCustomOptionId(this IList<EvaluatedCustomOptions> list, int id)
        {
            if (list != null)
            {
                return list.FirstOrDefault(ev => ev.CustomOptionId == id);
            }
            return null;
        }
    }
}


