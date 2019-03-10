//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;

namespace AdvantShop.Core.SQL
{
    [Serializable]
    public class SqlParam
    {
        public string ParameterName;
        public object Value;
    }

    [Serializable]
    public class SqlPaging
    {
        public SqlPaging()
        {
            CurrentPageIndex = 1;
            ItemsPerPage = 10;
        }

        private IDictionary<string, Field> _fields;
        private IDictionary<string, Field> _fieldsUnionTable;

        public string TableName { get; set; }

        public string TableNameForUnion { get; set; }

        public string ExtensionWhere { get; set; }

        public IDictionary<string, Field> Fields
        {
            get { return _fields; }
        }

        public int ItemsPerPage { get; set; }

        public int CurrentPageIndex { get; set; }

        public int PageCount
        {
            get { return (int)(Math.Ceiling((double)TotalRowsCount / ItemsPerPage)); }
        }

        private int? _totalRowsCount;
        public int TotalRowsCount
        {
            get
            {
                // todo: Vladimir: Временно убрал кеширование свойства  TotalRowsCount. В админке при созранении SqlPaging во ViewState свойство не обновляется. Раскомментировать, когда уберется Viewstate.
                //if (_totalRowsCount != null)
                //    return (int)_totalRowsCount;

                var cmd = new List<SqlParameter>();
                var searchCondition = new StringBuilder();
                SetConditionByFieldsValues(Fields.Values.Where(f => f.Filter != null), searchCondition, cmd);
                SetConditionByListCondition(searchCondition);

                if (!string.IsNullOrWhiteSpace(ExtensionWhere))
                    searchCondition.Append(" " + ExtensionWhere);

                var distQuery = Fields.Values.Where(f => !f.NotInQuery && f.IsDistinct).Select(f => f.FilterExpression).ToList();

                var distinctField = distQuery.Any() ? "distinct " + distQuery.First() : "*";

                var query = "SELECT COUNT(" + distinctField + ") FROM " + TableName + searchCondition;

                if (_listParam != null)
                    cmd.AddRange(_listParam.Select(param => new SqlParameter { ParameterName = param.ParameterName, Value = param.Value }));

                _totalRowsCount = SQLDataAccess.ExecuteScalar<int>(query, CommandType.Text, cmd.ToArray());
                return (int)_totalRowsCount;
            }
        }

        public List<T> ItemsIds<T>(string idName) where T : IConvertible
        {
            var cmd = new List<SqlParameter>();
            var searchCondition = new StringBuilder();
            SetConditionByFieldsValues(Fields.Values.Where(f => f.Filter != null), searchCondition, cmd);

            if (!string.IsNullOrWhiteSpace(ExtensionWhere))
                searchCondition.Append(" " + ExtensionWhere);

            var query = "SELECT " + idName + " FROM " + TableName + searchCondition;

            if (_listParam != null)
                cmd.AddRange(_listParam.Select(param => new SqlParameter { ParameterName = param.ParameterName, Value = param.Value }));

            var itemsIds = SQLDataAccess.ExecuteReadList(query, CommandType.Text, reader => SQLDataHelper.GetValue<T>(reader, "ID"), cmd.ToArray());
            return itemsIds;
        }

        public List<T> ItemsUnionIds<T>(string idName) where T : IConvertible
        {
            var cmd = new List<SqlParameter>();
            var seachConditionUnionTable = string.Empty;
            var query = string.Empty;
            if (!string.IsNullOrEmpty(TableNameForUnion))
            {
                query = "SELECT " + idName + " FROM " + TableNameForUnion + seachConditionUnionTable;
            }

            if (_listParam != null)
                cmd.AddRange(_listParam.Select(param => new SqlParameter { ParameterName = param.ParameterName, Value = param.Value }));

            var itemsIds = new List<T>();
            if (string.IsNullOrEmpty(query))
                return itemsIds;

            itemsIds = SQLDataAccess.ExecuteReadList(query, CommandType.Text, reader => SQLDataHelper.GetValue<T>(reader, "ID"), cmd.ToArray());

            return itemsIds;
        }

        public string ExtendedSorting { get; set; }
        public SortDirection ExtendedSortingDirection { get; set; }

        public DataTable PageItems
        {
            get
            {
                string query;
                var columns = string.Join(", ", (from f in Fields.Values where !f.NotInQuery select f.SelectExpression).ToArray());
                var columnsUnionTable = string.Empty;

                if ((from f in Fields.Values where f.IsDistinct select f).Any())
                {
                    columns = "distinct " + columns;
                }

                var order = Fields.Values.Where(f => f.Sorting.HasValue)
                                         .Aggregate(string.Empty, (current, f) => current + ((f.Name.Contains(".") ? "Temp." + f.Name.Split(new[] { '.' })[1] : f.Name)
                                             + (f.Sorting != null && f.Sorting.Value == SortDirection.Ascending ? " ASC" : " DESC") + ", "), current => current.TrimEnd(' ', ','));

                if (!string.IsNullOrEmpty(ExtendedSorting))
                    order = (!string.IsNullOrEmpty(order) ? order + " , " : "") + ExtendedSorting +
                        (ExtendedSortingDirection == SortDirection.Ascending ? " ASC " : " DESC ");

                if (string.IsNullOrEmpty(order))
                {
                    order = Fields.FirstOrDefault().Value.Name;
                    order = order.Contains(".") ? "Temp." + order.Split(new[] { '.' })[1] : order;
                }

                var searchCondition = new StringBuilder();
                var cmd = new List<SqlParameter>();
                SetConditionByFieldsValues(Fields.Values.Where(f => f.Filter != null), searchCondition, cmd);
                SetConditionByListCondition(searchCondition);

                if (!string.IsNullOrWhiteSpace(ExtensionWhere))
                    searchCondition.Append(" " + ExtensionWhere);

                var seachConditionUnionTable = string.Empty;

                int needRow = CurrentPageIndex * ItemsPerPage;

                int keyid = (CurrentPageIndex - 1) * ItemsPerPage;

                if (string.IsNullOrEmpty(TableNameForUnion))
                {
                    query =
                        string.Format(
                            "WITH Temp AS(SELECT {2} FROM {3} {4})SELECT * FROM (SELECT TOP ({0})  Row_Number() OVER (ORDER BY {1} )AS RowNum,* from Temp) as t WHERE RowNum > {5}",
                            needRow, order, columns, TableName, searchCondition, keyid);
                }
                else
                {
                    query =
                        string.Format(
                            "WITH Temp AS( SELECT {6} from {7} {8} union all SELECT {2} FROM {3} {4})SELECT * FROM (SELECT TOP ({0})  Row_Number() OVER (ORDER BY {1} )AS RowNum,* from Temp) as t WHERE RowNum > {5}",
                            needRow, order, columns, TableName, searchCondition, keyid, columnsUnionTable,
                            TableNameForUnion, seachConditionUnionTable);
                }

                if (_listParam != null)
                    cmd.AddRange(_listParam.Select(param => new SqlParameter { ParameterName = param.ParameterName, Value = param.Value }));

                DataTable tbl = SQLDataAccess.ExecuteTable(query, CommandType.Text, cmd.ToArray());
                return tbl;
            }
        }

        public string TableNameHint { get; set; }

        public DataTable PageItemsHint
        {
            get
            {
                string query;
                var columns = string.Join(", ", (from f in Fields.Values where !f.NotInQuery && !f.Sorting.HasValue select f.SelectExpression).ToArray());

                var solSort = (from f in Fields.Values where f.Sorting.HasValue select f.SelectExpression).ToList();
                solSort.Insert(0, Fields.Values.First().SelectExpression);
                var columnsSort = string.Join(", ", solSort);

                if ((from f in Fields.Values where f.IsDistinct select f).Any())
                {
                    columnsSort = "distinct " + columnsSort;
                }

                var order = Fields.Values.Where(f => f.Sorting.HasValue).Union(new[] { Fields.Values.First() })
                                         .Aggregate(string.Empty, (current, f) => current + ((f.Name.Contains(".") ? "Temp." + f.Name.Split(new[] { '.' })[1] : f.Name)
                                             + (f.Sorting != null && f.Sorting.Value == SortDirection.Ascending ? " ASC" : " DESC") + ", "), current => current.TrimEnd(' ', ','));

                if (!string.IsNullOrEmpty(ExtendedSorting))
                    order = (!string.IsNullOrEmpty(order) ? order + " , " : "") + ExtendedSorting + (ExtendedSortingDirection == SortDirection.Ascending ? " ASC " : " DESC ");

                if (string.IsNullOrEmpty(order))
                {
                    order = Fields.FirstOrDefault().Value.Name;
                    order = order.Contains(".") ? "Temp." + order.Split(new[] { '.' })[1] : order;
                }

                var searchCondition = new StringBuilder();
                var cmd = new List<SqlParameter>();
                SetConditionByFieldsValues(Fields.Values.Where(f => f.Filter != null), searchCondition, cmd);
                SetConditionByListCondition(searchCondition);

                if (!string.IsNullOrWhiteSpace(ExtensionWhere))
                    searchCondition.Append(" " + ExtensionWhere);

                var needRow = CurrentPageIndex * ItemsPerPage;

                var keyid = (CurrentPageIndex - 1) * ItemsPerPage;

                if (!string.IsNullOrEmpty(TableNameHint))
                {
                    query =
                        string.Format(
                            "WITH Temp AS(SELECT {2} FROM {3} {4})SELECT {6} FROM (SELECT TOP ({0})  Row_Number() OVER (ORDER BY {1} )AS RowNum,* from Temp) as t {7} WHERE RowNum > {5}",
                            needRow, order, columnsSort, TableName, searchCondition, keyid, columns, TableNameHint);
                }
                else
                {
                    throw new Exception("TableNameHint don't set");
                }

                if (_listParam != null)
                    cmd.AddRange(_listParam.Select(param => new SqlParameter { ParameterName = param.ParameterName, Value = param.Value }));

                var hashKey = GetHashFromCommand(query, cmd);
                if (CacheManager.Contains(hashKey))
                    return CacheManager.Get<DataTable>(hashKey);

                var tbl = SQLDataAccess.ExecuteTable(query, CommandType.Text, cmd.ToArray());
                CacheManager.Insert(hashKey, tbl);
                return tbl;
            }
        }

        private static string GetHashFromCommand(string query, IEnumerable<SqlParameter> parm)
        {
            var str = parm.Aggregate(query, (current, item) => current + (item.ParameterName + ":" + item.SqlValue + ","));
            return GetStringSha1Hash(str);
        }

        private static string GetStringSha1Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha1 = new System.Security.Cryptography.SHA1Managed())
            {
                byte[] textData = Encoding.UTF8.GetBytes(text);

                byte[] hash = sha1.ComputeHash(textData);

                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        public List<T> GetCustomData<T>(string selectFields, string newCondition, Func<SqlDataReader, T> getFromReader)
        {
            var cmd = new List<SqlParameter>();
            var searchCondition = new StringBuilder();
            SetConditionByFieldsValues(Fields.Values.Where(field => field.Filter != null && !field.Filter.HideInCustomData), searchCondition, cmd);
            if (_listParam != null)
                cmd.AddRange(_listParam.Select(param => new SqlParameter { ParameterName = param.ParameterName, Value = param.Value }));

            var query = String.Format("SELECT distinct {0} FROM {1} {2} {3}", selectFields, TableName, searchCondition, newCondition);
            var table = SQLDataAccess.ExecuteReadList(query, CommandType.Text, getFromReader, cmd.ToArray());
            return table;
        }

        private IList<string> _listCondition;
        public void AddCondition(string f)
        {
            if (_listCondition == null)
            {
                _listCondition = new List<string>();
            }
            _listCondition.Add(f.Trim());
        }

        private IList<SqlParam> _listParam;
        public void AddParam(SqlParam f)
        {
            if (_listParam == null)
            {
                _listParam = new List<SqlParam>();
            }

            var param = _listParam.FirstOrDefault(p => p.ParameterName == f.ParameterName);
            if (param == null)
            {
                _listParam.Add(f);
            }
            else
            {
                param.Value = f.Value;
            }
        }

        public void AddFieldsRange(IEnumerable<Field> fields)
        {
            if (_fields == null)
            {
                _fields = new Dictionary<string, Field>();
            }
            _fields.AddRange(fields.ToDictionary(f => f.Name));
        }

        public void AddFieldsRange(params Field[] fields)
        {
            if (_fields == null)
            {
                _fields = new Dictionary<string, Field>();
            }
            _fields.AddRange(fields.ToDictionary(f => f.Name));
        }

        public void AddField(Field f)
        {
            if (_fields == null)
            {
                _fields = new Dictionary<string, Field>();
            }
            _fields.Add(f.Name, f);
        }

        public void AddFieldsRangeUnionTable(params Field[] fields)
        {
            if (_fieldsUnionTable == null)
            {
                _fieldsUnionTable = new Dictionary<string, Field>();
            }
            _fieldsUnionTable.AddRange(fields.ToDictionary(f => f.Name));
        }

        public void AddFieldUnionTable(Field f)
        {
            if (_fieldsUnionTable == null)
            {
                _fieldsUnionTable = new Dictionary<string, Field>();
            }
            _fieldsUnionTable.Add(f.Name, f);
        }

        private void SetConditionByFieldsValues(IEnumerable<Field> fields, StringBuilder searchCondition, List<SqlParameter> cmdparams)
        {
            var first = true;
            foreach (var f in fields)
            {
                searchCondition.Append(first ? " WHERE " : " AND ");
                searchCondition.Append(f.Filter.GetSqlCondition(f.FilterExpression));
                first = false;
                cmdparams.AddRange(f.Filter.GetSqlConditionParameter().Select(item => new SqlParameter(item.ParameterName, item.Value)));
            }
        }

        private void SetConditionByListCondition(StringBuilder searchCondition)
        {
            if (_listCondition == null) return;
            var first = true;
            foreach (var t in _listCondition)
            {
                searchCondition.Append(first ? " WHERE " : " AND ");
                searchCondition.Append(t);
                first = false;
            }
        }
    }
}