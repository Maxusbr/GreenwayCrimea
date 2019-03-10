//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Core.SQL
{
    [Serializable]
    public abstract class FieldFilter
    {
        protected List<SqlParam> Parameters;
        protected string SearchCondition;

        public string ParamName { get; set; }
        public bool HideInCustomData { get; set; }

        protected FieldFilter()
        {
            Parameters = new List<SqlParam>();
            SearchCondition = string.Empty;
        }

        public virtual string GetSqlCondition(string fieldName)
        {
            return SearchCondition;
        }

        public virtual IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            return Parameters;
        }
    }

    [Serializable]
    public class FieldFilterList : FieldFilter
    {
        public List<string> ListFilter { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (string.IsNullOrEmpty(ParamName)) return SearchCondition;
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            for (var i = 0; i <= ListFilter.Count - 1; i++)
            {
                SearchCondition += (i == 0 ? ParamName + i : "," + ParamName + i);
            }
            return string.Format("{0} in ({1})", fieldName, SearchCondition);
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (string.IsNullOrEmpty(ParamName)) return Parameters;
            if (Parameters.Any()) return Parameters;

            for (var i = 0; i <= ListFilter.Count - 1; i++)
            {
                Parameters.Add(new SqlParam { ParameterName = ParamName + i, Value = ListFilter[i] });
            }
            return Parameters;
        }
    }

    [Serializable]
    public class RangeFieldFilter : FieldFilter
    {
        public float? From { get; set; }
        public float? To { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (string.IsNullOrEmpty(ParamName)) return SearchCondition;
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            if (From.HasValue)
                SearchCondition += string.Format("{0} >= {1}from", fieldName, ParamName);

            if (To.HasValue)
                SearchCondition += string.Format(string.IsNullOrEmpty(SearchCondition) ? "{0} <= {1}to" : " AND {0} <= {1}to", fieldName, ParamName);
            return SearchCondition;
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (string.IsNullOrEmpty(ParamName)) return Parameters;
            if (Parameters.Any()) return Parameters;

            if (From.HasValue)
                Parameters.Add(new SqlParam { ParameterName = ParamName + "from", Value = From.Value });
            if (To.HasValue)
                Parameters.Add(new SqlParam { ParameterName = ParamName + "to", Value = To.Value });
            return Parameters;
        }
    }

    [Serializable]
    public class RangeListFieldFilter : FieldFilter
    {
        public List<RangeFieldFilter> ListFilter { get; set; }
        public override string GetSqlCondition(string fieldName)
        {
            if (string.IsNullOrEmpty(ParamName)) return SearchCondition;
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            for (var i = 0; i <= ListFilter.Count - 1; i++)
            {
                SearchCondition +=
                    string.Format("{0} ({1} >= {2}from{3} and {1} <= {2}to{3}) ", ((SearchCondition != "") ? " OR " : " "), fieldName, ParamName, i);
            }
            return " (" + SearchCondition + ") ";
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (string.IsNullOrEmpty(ParamName)) return Parameters;
            if (Parameters.Any()) return Parameters;

            for (var i = 0; i <= ListFilter.Count - 1; i++)
            {
                Parameters.Add(new SqlParam { ParameterName = ParamName + "from" + i, Value = ListFilter[i].From });
                Parameters.Add(new SqlParam { ParameterName = ParamName + "to" + i, Value = ListFilter[i].To });
            }
            return Parameters;
        }
    }

    [Serializable]
    public class CompareFieldFilter : FieldFilter
    {
        public string Expression { get; set; }
        public override string GetSqlCondition(string fieldName)
        {
            if (string.IsNullOrEmpty(ParamName)) return SearchCondition;
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            SearchCondition = string.Format("{0} LIKE '%'+{1}+'%'", fieldName, ParamName);
            return SearchCondition;
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (string.IsNullOrEmpty(ParamName)) return Parameters;
            if (Parameters.Any()) return Parameters;

            Parameters.Add(new SqlParam { ParameterName = ParamName, Value = Expression.Trim() });
            return Parameters;
        }
    }

    [Serializable]
    public class CompareFieldListFilter : FieldFilter
    {
        public List<CompareFieldFilter> ListFilter { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (string.IsNullOrEmpty(ParamName)) return SearchCondition;
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            for (var i = 0; i <= ListFilter.Count - 1; i++)
            {
                SearchCondition += string.Format(string.IsNullOrWhiteSpace(SearchCondition)
                                                        ? " ({0} LIKE '%'+{1}{2}+'%') "
                                                        : " or ({0} LIKE '%'+{1}{2}+'%') ",
                                                        fieldName, ParamName, i);
            }
            return SearchCondition = " (" + SearchCondition + ") ";
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (string.IsNullOrEmpty(ParamName)) return Parameters;
            if (Parameters.Any()) return Parameters;

            for (var i = 0; i <= ListFilter.Count - 1; i++)
            {
                Parameters.Add(new SqlParam { ParameterName = ParamName + i, Value = ListFilter[i].Expression.Trim() });
            }
            return Parameters;
        }
    }

    [Serializable]
    public class NullFieldFilter : FieldFilter
    {
        public bool Null { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            return (Null ? string.Empty : "NOT ") + fieldName + " IS NULL";
        }
    }

    [Serializable]
    public class EqualFieldFilter : FieldFilter
    {
        public string Value { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (string.IsNullOrEmpty(ParamName)) return SearchCondition;
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            return SearchCondition = fieldName + " = " + ParamName;
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (string.IsNullOrEmpty(ParamName)) return Parameters;
            if (Parameters.Any()) return Parameters;

            Parameters.Add(new SqlParam { ParameterName = ParamName, Value = Value.Trim() });
            return Parameters;
        }
    }

    [Serializable]
    public class PropertyFieldFilter : FieldFilter
    {
        public Dictionary<int, List<int>> ListFilter { get; set; }
        public int CategoryId { get; set; }
        public bool GetSubCategoryes { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (string.IsNullOrEmpty(ParamName)) return SearchCondition;
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            var sqlSubcomand = string.Empty;
            const string template = @" (SELECT [ProductPropertyValue].[ProductID] FROM [Catalog].[ProductPropertyValue] INNER JOIN [Catalog].[ProductCategories] ON [ProductCategories].[ProductID] = [ProductPropertyValue].[ProductID] ";

            var categoryCondishion = GetSubCategoryes ? " WHERE [CategoryID] IN (SELECT id FROM [Settings].[GetChildCategoryByParent](" + ParamName + "_CatId" + ")) " : " WHERE [CategoryID]=" + ParamName + "_CatId ";
            
            foreach (var i in ListFilter.Keys)
            {
                SearchCondition = string.Empty;
                for (var j = 0; j <= ListFilter[i].Count - 1; j++)
                {
                    SearchCondition += string.IsNullOrEmpty(SearchCondition)
                                           ? ParamName + i + j
                                           : "," + ParamName + i + j;
                }
                if (string.IsNullOrEmpty(sqlSubcomand))
                    sqlSubcomand = sqlSubcomand + template + categoryCondishion + " AND [PropertyValueID] in (" + SearchCondition + "))";
                else
                    sqlSubcomand = sqlSubcomand + " INTERSECT " + template + categoryCondishion + " AND [PropertyValueID] in (" + SearchCondition + "))";
            }
            return SearchCondition = fieldName + " in (" + sqlSubcomand + ")";
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (string.IsNullOrEmpty(ParamName)) return Parameters;
            if (Parameters.Any()) return Parameters;

            Parameters.Add(new SqlParam { ParameterName = ParamName + "_CatId", Value = CategoryId });
            foreach (var i in ListFilter.Keys)
            {
                for (var j = 0; j <= ListFilter[i].Count - 1; j++)
                {
                    Parameters.Add(new SqlParam { ParameterName = ParamName + i + j, Value = ListFilter[i][j] });
                }
            }
            return Parameters;
        }
    }

    [Serializable]
    public class PriceFieldFilter : RangeFieldFilter
    {
        public int CategoryId { get; set; }
        public bool GetSubCategoryes { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (string.IsNullOrEmpty(ParamName)) return SearchCondition;
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            const string template = @"SELECT Offer.[ProductID] FROM [Catalog].Offer inner join Catalog.Product on Offer.ProductID=Product.ProductID inner join catalog.ProductCategories on Product.ProductID=ProductCategories.ProductID";

            var categoryCondition = GetSubCategoryes
                                           ? string.Format("WHERE [CategoryID] IN (SELECT id FROM [Settings].[GetChildCategoryByParent]({0}_CatId))", ParamName)
                                           : string.Format("WHERE [CategoryID]={0}_CatId", ParamName);

            if (From.HasValue)
            {
                SearchCondition += "Price - Price * Discount / 100" + " >= " + ParamName + "from";
            }
            if (To.HasValue)
            {
                SearchCondition += string.IsNullOrEmpty(SearchCondition)
                                      ? "Price - Price * Discount / 100" + " <= " + ParamName + "to"
                                       : " AND Price - Price * Discount / 100" + " <= " + ParamName + "to";
            }

            return SearchCondition = string.Format("{0} in ({1} {2} AND {3})", fieldName, template, categoryCondition, SearchCondition);
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (string.IsNullOrEmpty(ParamName)) return Parameters;
            if (Parameters.Any()) return Parameters;

            Parameters.Add(new SqlParam { ParameterName = ParamName + "_CatId", Value = CategoryId });

            if (From.HasValue)
                Parameters.Add(new SqlParam { ParameterName = ParamName + "from", Value = From.Value });

            if (To.HasValue)
                Parameters.Add(new SqlParam { ParameterName = ParamName + "to", Value = To.Value });
            return Parameters;
        }
    }

    [Serializable]
    public class PropertyRangeFieldFilter : FieldFilter
    {
        public Dictionary<int, KeyValuePair<int, int>> ListFilter { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (string.IsNullOrEmpty(ParamName)) return SearchCondition;
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            var sqlSubcomand = string.Empty;
            const string template =
                " (Select [ProductPropertyValue].[ProductID] From [Catalog].[ProductPropertyValue] " +
                " Left Join [Catalog].[PropertyValue] on [PropertyValue].[PropertyValueID] = [ProductPropertyValue].[PropertyValueID] Where";
            
            foreach (var i in ListFilter.Keys)
            {
                if (string.IsNullOrEmpty(sqlSubcomand))
                    sqlSubcomand += template +
                                    " [PropertyId] = " + ParamName + "_Prop_" + i + " And RangeValue >= " + ListFilter[i].Key + " And RangeValue <= " + ListFilter[i].Value + ")";
                else
                    sqlSubcomand += " INTERSECT " + template +
                                    " [PropertyId] = " + ParamName + "_Prop_" + i + " And RangeValue >= " + ListFilter[i].Key + " And RangeValue <= " + ListFilter[i].Value + ")";
            }

            return SearchCondition = fieldName + " in (" + sqlSubcomand + ")";
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (string.IsNullOrEmpty(ParamName)) return Parameters;
            if (Parameters.Any()) return Parameters;

            foreach (var i in ListFilter.Keys)
            {
                Parameters.Add(new SqlParam { ParameterName = ParamName + "_Prop_" + i, Value = i });
                Parameters.Add(new SqlParam { ParameterName = ParamName + "_Min_" + i, Value = ListFilter[i].Key });
                Parameters.Add(new SqlParam { ParameterName = ParamName + "_Max_" + i, Value = ListFilter[i].Value });
            }

            return Parameters;
        }
    }

    [Serializable]
    public class SizeFieldFilter : FieldFilter
    {
        public List<int> ListFilter { get; set; }
        public int CategoryId { get; set; }
        public bool GetSubCategoryes { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (string.IsNullOrEmpty(ParamName)) return SearchCondition;
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            string sqlSubcomand;
            const string template = @"SELECT Offer.[ProductID] FROM [Catalog].Offer inner join Catalog.Product on Offer.ProductID=Product.ProductID inner join catalog.ProductCategories on Product.ProductID=ProductCategories.ProductID";

            var categoryCondishion = GetSubCategoryes
                                            ? string.Format(
                                                "WHERE [CategoryID] IN (SELECT id FROM [Settings].[GetChildCategoryByParent]({0}_CatId)) ", ParamName)
                                            : string.Format("WHERE [CategoryID]={0}_CatId ", ParamName);

            if (ListFilter.Any())
            {
                sqlSubcomand = string.Format("{0} {1} AND [SizeID] in ({2}) and offer.amount > 0", template, categoryCondishion, ListFilter.AggregateString(','));
            }
            else
            {
                sqlSubcomand = template + categoryCondishion + ")";
            }

            return SearchCondition = string.Format("{0} in ({1})", fieldName, sqlSubcomand);
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (string.IsNullOrEmpty(ParamName)) return Parameters;
            if (Parameters.Any()) return Parameters;

            Parameters.Add(new SqlParam { ParameterName = ParamName + "_CatId", Value = CategoryId });
            Parameters.AddRange(ListFilter.Select(i => new SqlParam { ParameterName = ParamName + i, Value = i }));
            return Parameters;
        }
    }

    [Serializable]
    public class ColorFieldFilter : FieldFilter
    {
        public List<int> ListFilter { get; set; }
        public int CategoryId { get; set; }
        public bool GetSubCategoryes { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (string.IsNullOrEmpty(ParamName)) return SearchCondition;
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            string sqlSubcomand;
            const string template = @"SELECT [ProductID] FROM [Catalog].Offer";

            string categoryCondishion = GetSubCategoryes ? " WHERE [CategoryID] IN (SELECT id FROM [Settings].[GetChildCategoryByParent](" + ParamName + "_CatId" + ")) " : " WHERE [CategoryID]=" + ParamName + "_CatId ";

            if (ListFilter.Any())
            {
                sqlSubcomand = string.Format("{0}{1} AND [ColorID] in ({2}) and offer.amount > 0", template, categoryCondishion, ListFilter.AggregateString(','));
            }
            else
            {
                sqlSubcomand = template + categoryCondishion + ")";
            }

            return SearchCondition = fieldName + " in (" + sqlSubcomand + ")";
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (string.IsNullOrEmpty(ParamName)) return Parameters;
            if (Parameters.Any()) return Parameters;

            Parameters.Add(new SqlParam { ParameterName = ParamName + "_CatId", Value = CategoryId });
            Parameters.AddRange(ListFilter.Select(i => new SqlParam { ParameterName = ParamName + i, Value = i }));
            return Parameters;
        }
    }

    [Serializable]
    public class ProductIdInIds : FieldFilter
    {
        public List<int> ListFilter { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (string.IsNullOrEmpty(ParamName)) return SearchCondition;
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            var res = new StringBuilder();
            if (!string.IsNullOrEmpty(ParamName))
                for (var i = 0; i < ListFilter.Count; i++)
                {
                    if (res.Length == 0)
                        res.Append(ParamName + i);
                    else
                        res.Append("," + ParamName + i);
                }
            return SearchCondition = string.Format("{0} in ({1})", fieldName, res);
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (string.IsNullOrEmpty(ParamName)) return Parameters;
            if (Parameters.Any()) return Parameters;

            Parameters.AddRange(ListFilter.Select((t, i) => new SqlParam { ParameterName = ParamName + i, Value = t }));
            return Parameters;
        }
    }

    [Serializable]
    public class InSetFieldFilter : FieldFilter
    {
        public string[] Values { get; set; }
        public bool IncludeValues { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            if (!IncludeValues)
            {
                SearchCondition += "NOT ";
            }
            SearchCondition += fieldName + " IN (\'" + string.Join("\' ,\'", Values) + "\')";
            return SearchCondition;
        }
    }

    [Serializable]
    public class DateTimeRangeFieldFilter : FieldFilter
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (string.IsNullOrEmpty(ParamName)) return SearchCondition;
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            if (From.HasValue)
            {
                SearchCondition += fieldName + " >= " + ParamName + "_from";
            }
            if (To.HasValue)
            {
                SearchCondition += string.IsNullOrEmpty(SearchCondition)
                                       ? fieldName + " <= " + ParamName + "_to"
                                       : " AND " + fieldName + " <= " + ParamName + "_to";
            }
            return SearchCondition;
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (string.IsNullOrEmpty(ParamName)) return Parameters;
            if (Parameters.Any()) return Parameters;

            if (From.HasValue)
                Parameters.Add(new SqlParam { ParameterName = ParamName + "_from", Value = From });
            if (To.HasValue)
                Parameters.Add(new SqlParam { ParameterName = ParamName + "_to", Value = To });
            return Parameters;
        }
    }

    [Serializable]
    public class InChildCategoriesFieldFilter : FieldFilter
    {
        public string CategoryId { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (string.IsNullOrEmpty(ParamName)) return SearchCondition;
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            return SearchCondition = string.Format("{0} IN (SELECT id FROM [Settings].[GetChildCategoryByParent]({1}))", fieldName, ParamName);
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (string.IsNullOrEmpty(ParamName)) return Parameters;
            if (Parameters.Any()) return Parameters;

            Parameters.Add(new SqlParam { ParameterName = ParamName, Value = CategoryId });
            return Parameters;
        }
    }

    [Serializable]
    public class CountProductInCategory : FieldFilter
    {
        public override string GetSqlCondition(string fieldName)
        {
            return " ((select Count(CategoryID) from Catalog.ProductCategories where ProductID=[Product].[ProductID])<> 0) ";
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            return Parameters;
        }
    }

    [Serializable]
    public class NotEqualFieldFilter : FieldFilter
    {
        public string Value { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (string.IsNullOrEmpty(ParamName)) return SearchCondition;
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            return SearchCondition = fieldName + " <> " + ParamName;
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (string.IsNullOrEmpty(ParamName)) return Parameters;
            if (Parameters.Any()) return Parameters;

            Parameters.Add(new SqlParam { ParameterName = ParamName, Value = Value.Trim() });
            return Parameters;
        }
    }

    [Serializable]
    public class LogicalFilter : FieldFilter
    {
        public LogicalFilter()
        {
            _filterList = new List<FieldFilter>();
            _logicalOperation = new List<string>();
        }

        private readonly List<FieldFilter> _filterList;

        private readonly List<string> _logicalOperation;

        public void AddLogicalOperation(string op)
        {
            _logicalOperation.Add(op);
        }

        public void AddFilter(FieldFilter filter)
        {
            _filterList.Add(filter);
        }
        public int FilterCount()
        {
            return _filterList.Count;
        }

        public override string GetSqlCondition(string fieldName)
        {
            if (!string.IsNullOrEmpty(SearchCondition)) return SearchCondition;

            var outstr = new StringBuilder();
            var fs = (from f in _filterList select f.GetSqlCondition(fieldName)).ToArray();

            outstr.Append("(");
            for (var i = 0; i < fs.Length; i++)
            {
                if (i == 0)
                {
                    outstr.Append(" " + fs[i]);
                }
                else
                {
                    outstr.Append(" " + _logicalOperation[i - 1] + " " + fs[i]);
                }
            }
            outstr.Append(")");
            return SearchCondition = outstr.ToString();
        }

        public override IEnumerable<SqlParam> GetSqlConditionParameter()
        {
            if (Parameters.Any()) return Parameters;

            foreach (var f in _filterList)
            {
                Parameters.AddRange(f.GetSqlConditionParameter());
            }
            return Parameters;
        }
    }
}
