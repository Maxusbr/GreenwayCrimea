//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Helpers
{
    public static class QueryHelper
    {
        public static string RemoveQueryParam(string query, string name)
        {
            query = query.Trim('?');

            if (query.IsNullOrEmpty())
                return string.Empty;

            var list = new List<string>();
            list.AddRange(query.Split('&'));
            list.RemoveAll(item => item.ToLower().StartsWith(name.ToLower() + "="));

            return list.Count > 0 ? "?" + string.Join("&", list) : string.Empty;
        }

        public static string ChangeQueryParam(string query, string[] names, string[] values)
        {
            if (names.Length != values.Length)
            {
                return query;
            }
            for (int i = 0; i < names.Length; i++)
            {
                query = ChangeQueryParam(query, names[i], values[i]);
            }
            return query;
        }

        public static string ChangeQueryParam(string query, string name, string value)
        {
            query = query.Trim('?');

            if (query.IsNullOrEmpty())
                return value.IsNullOrEmpty() ? string.Empty : string.Format("?{0}={1}", name, value);

            var list = new List<string>();
            list.AddRange(query.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries));

            int paramInd;
            if ((paramInd = list.FindIndex(item => item.ToLower().StartsWith(name.ToLower() + "="))) != -1)
            {
                if (value.IsNullOrEmpty())
                    list.RemoveAt(paramInd);
                else
                    list[paramInd] = string.Format("{0}={1}", name, value);
            }
            else if (value.IsNotEmpty())
                list.Add(string.Format("{0}={1}", name, value));

            return list.Count > 0 ? "?" + string.Join("&", list) : string.Empty;
        }

        public static string CreateQueryString(IEnumerable<KeyValuePair<string, string>> pars)
        {
            return pars.Aggregate(new StringBuilder(), (sb, par) => sb.AppendFormat("{0}={1}&", par.Key, par.Value),
                                  sb => sb.ToString().TrimEnd('&'));
        }
    }
}