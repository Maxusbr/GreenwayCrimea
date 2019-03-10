using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public class BizProcessRuleService
    {
        public static IBizObjectFilter GetBizObjectFilterFromJson<T>(string json) where T : BizProcessRule, new()
        {
            if (json.IsNullOrEmpty())
                return null;

            var jsonObject = JsonConvert.DeserializeObject<dynamic>(json);
            var jsonFieldComparers = jsonObject["Comparers"];

            var objectType = new T().ObjectType;
            switch (objectType)
            {
                case EBizProcessObjectType.Order:
                    var orderFilter = new OrderFilter();

                    foreach (var jsonOrderFieldComparer in jsonFieldComparers)
                    {
                        var orderFieldComparer = new OrderFieldComparer();
                        orderFieldComparer.FieldType = (EOrderFieldType)jsonOrderFieldComparer["FieldType"]; // EOrderFieldType FieldType

                        var jsonFieldComparer = jsonOrderFieldComparer["FieldComparer"];    // FieldComparer FieldComparer

                        FieldComparer fieldComparer;
                        if (GetFieldComparerFromJson(jsonFieldComparer, orderFieldComparer.FieldType, orderFieldComparer.FieldType == EOrderFieldType.CustomerField, out fieldComparer))
                        {
                            orderFieldComparer.FieldComparer = fieldComparer;
                            orderFilter.Comparers.Add(orderFieldComparer);
                        }
                    }
                    return orderFilter;
                case EBizProcessObjectType.Lead:
                    var leadFilter = new LeadFilter();

                    foreach (var jsonLeadFieldComparer in jsonFieldComparers)
                    {
                        var leadFieldComparer = new LeadFieldComparer();
                        leadFieldComparer.FieldType = (ELeadFieldType)jsonLeadFieldComparer["FieldType"]; // ELeadFieldType FieldType

                        var jsonFieldComparer = jsonLeadFieldComparer["FieldComparer"];    // FieldComparer FieldComparer

                        FieldComparer fieldComparer;
                        if (GetFieldComparerFromJson(jsonFieldComparer, leadFieldComparer.FieldType, leadFieldComparer.FieldType == ELeadFieldType.CustomerField, out fieldComparer))
                        {
                            leadFieldComparer.FieldComparer = fieldComparer;
                            leadFilter.Comparers.Add(leadFieldComparer);
                        }
                    }
                    return leadFilter;
                case EBizProcessObjectType.Call:
                    var callFilter = new CallFilter();

                    foreach (var jsonCallFieldComparer in jsonFieldComparers)
                    {
                        var callFieldComparer = new CallFieldComparer();
                        callFieldComparer.FieldType = (ECallFieldType)jsonCallFieldComparer["FieldType"]; // ECallFieldType FieldType

                        var jsonFieldComparer = jsonCallFieldComparer["FieldComparer"];    // FieldComparer FieldComparer

                        FieldComparer fieldComparer;
                        if (GetFieldComparerFromJson(jsonFieldComparer, callFieldComparer.FieldType, callFieldComparer.FieldType == ECallFieldType.CustomerField, out fieldComparer))
                        {
                            callFieldComparer.FieldComparer = fieldComparer;
                            callFilter.Comparers.Add(callFieldComparer);
                        }
                    }
                    return callFilter;
                case EBizProcessObjectType.Review:
                    return new ReviewFilter();
                case EBizProcessObjectType.Customer:
                    return new CustomerFilter();
                default:
                    throw new NotImplementedException("No implementation for object type " + objectType);
            }
        }

        private static bool GetFieldComparerFromJson(dynamic jsonFieldComparer, Enum bizObjFieldType, bool isCustomerField, out FieldComparer fieldComparer)
        {
            var fieldType = GetFieldType(bizObjFieldType, isCustomerField, (int?)jsonFieldComparer["FieldObjId"]);
            var fieldComparerType = (EFieldComparerType)jsonFieldComparer["Type"];  // EFieldComparerType Type
            switch (fieldComparerType)
            {
                case EFieldComparerType.Equal:
                    var value = jsonFieldComparer["Value"];
                    fieldComparer = new FieldEqualityComparer
                    {
                        Value = value,             // string Value
                    };
                    break;
                case EFieldComparerType.Range:
                    if (fieldType == EFieldType.Date)
                    {
                        var dateFrom = (object)jsonFieldComparer["DateFrom"];
                        var dateTo = (object)jsonFieldComparer["DateTo"];
                        fieldComparer = new FieldRangeComparer
                        {
                            DateFrom = dateFrom != null ? dateFrom.ToString().TryParseDateTime(true) : null,       // DateTime? From 
                            DateTo = dateTo != null ? dateTo.ToString().TryParseDateTime(true) : null          // DateTime? To
                        };
                    }
                    else
                    {
                        var from = (object)jsonFieldComparer["From"];
                        var to = (object)jsonFieldComparer["To"];
                        fieldComparer = new FieldRangeComparer
                        {
                            From = from != null ? from.ToString().TryParseFloat(true) : null,       // float? From 
                            To = to != null ? to.ToString().TryParseFloat(true) : null              // float? To
                        };
                    }
                    break;
                case EFieldComparerType.Flag:
                    var flag = (object)jsonFieldComparer["Flag"];
                    fieldComparer = new FieldFlagComparer
                    {
                        Flag = flag != null ? flag.ToString().TryParseBool() : false,       // bool From 
                    };
                    break;
                case EFieldComparerType.Contains:
                    var valueContains = jsonFieldComparer["Value"];
                    fieldComparer = new FieldContainsComparer
                    {
                        Value = valueContains,       // string Value
                    };
                    break;
                default:
                    fieldComparer = null;
                    return false;
            }
            fieldComparer.FieldObjId = jsonFieldComparer["FieldObjId"];      // int? FieldObjId
            fieldComparer.ValueObjId = jsonFieldComparer["ValueObjId"];      // int? ValueObjId

            return true;
        }

        public static EFieldType GetFieldType(Enum bizObjFieldType, bool isCustomerField, int? fieldObjId)
        {
            if (!isCustomerField || !fieldObjId.HasValue)
                return bizObjFieldType.FieldType();
            var customerField = CustomerFieldService.GetCustomerField(fieldObjId.Value);
            if (customerField == null)
                return bizObjFieldType.FieldType();
            return customerField.FieldType.FieldType();
        }

        public static T GetBizProcessRuleFromReader<T>(SqlDataReader reader) where T : BizProcessRule, new()
        {
            var rule = new T
            {
                Id = SQLDataHelper.GetInt(reader, "Id"),
                EventObjId = SQLDataHelper.GetNullableInt(reader, "EventObjId"),
                Priority = SQLDataHelper.GetInt(reader, "Priority"),
                TaskName = SQLDataHelper.GetString(reader, "TaskName"),
                TaskDescription = SQLDataHelper.GetString(reader, "TaskDescription"),
                TaskDueDateInterval = JsonConvert.DeserializeObject<TimeInterval>(SQLDataHelper.GetString(reader, "TaskDueDateInterval")),
                TaskCreateInterval = JsonConvert.DeserializeObject<TimeInterval>(SQLDataHelper.GetString(reader, "TaskCreateInterval")),
                TaskPriority = (TaskPriority)SQLDataHelper.GetInt(reader, "TaskPriority"),
                TaskGroupId = SQLDataHelper.GetNullableInt(reader, "TaskGroupId"),
                ManagerFilter = JsonConvert.DeserializeObject<ManagerFilter>(SQLDataHelper.GetString(reader, "ManagerFilter")),
                DateCreated = SQLDataHelper.GetDateTime(reader, "DateCreated"),
                DateModified = SQLDataHelper.GetDateTime(reader, "DateModified"),
            };
            rule.Filter = GetBizObjectFilterFromJson<T>(SQLDataHelper.GetString(reader, "Filter"));
            rule.RemoveInvalidComparers<T>();
            return rule;
        }

        public static bool ExistBizProcessRules()
        {
            return SQLDataAccess.ExecuteScalar<int>("SELECT COUNT(*) FROM CRM.BizProcessRule", CommandType.Text) > 0;
        }

        public static T GetBizProcessRule<T>(int id) where T : BizProcessRule, new()
        {
            return SQLDataAccess.ExecuteReadOne<T>(
                "SELECT * FROM CRM.BizProcessRule WHERE Id = @Id", CommandType.Text,
                GetBizProcessRuleFromReader<T>, new SqlParameter("@Id", id));
        }

        public static List<T> GetBizProcessRules<T>(int? eventObjId = null) where T : BizProcessRule, new()
        {
            var type = new T().EventType;
            return SQLDataAccess.ExecuteReadList<T>(
                "SELECT * FROM CRM.BizProcessRule WHERE EventType = @EventType " +
                (eventObjId.HasValue ? "AND EventObjId = @EventObjId " : string.Empty) +
                "ORDER BY Priority", 
                CommandType.Text, GetBizProcessRuleFromReader<T>, 
                new SqlParameter("@EventType", type), 
                eventObjId.HasValue ? new SqlParameter("@EventObjId", eventObjId.Value) : null);
        }

        public static int AddBizProcessRule(BizProcessRule rule)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO CRM.BizProcessRule (EventType, ObjectType, EventObjId, Priority, TaskName, TaskDescription, TaskDueDateInterval, TaskCreateInterval, TaskPriority, TaskGroupId, ManagerFilter, Filter, DateCreated, DateModified) " +
                "VALUES (@EventType, @ObjectType, @EventObjId, @Priority, @TaskName, @TaskDescription, @TaskDueDateInterval, @TaskCreateInterval, @TaskPriority, @TaskGroupId, @ManagerFilter, @Filter, GETDATE(), GETDATE()); " +
                "SELECT SCOPE_IDENTITY()",
                CommandType.Text,
                new SqlParameter("@EventType", rule.EventType),
                new SqlParameter("@ObjectType", rule.ObjectType),
                new SqlParameter("@EventObjId", rule.EventObjId ?? (object)DBNull.Value),
                new SqlParameter("@Priority", rule.Priority),
                new SqlParameter("@TaskName", rule.TaskName),
                new SqlParameter("@TaskDescription", rule.TaskDescription),
                new SqlParameter("@TaskDueDateInterval", rule.TaskDueDateInterval != null ? JsonConvert.SerializeObject(rule.TaskDueDateInterval) : (object)DBNull.Value),
                new SqlParameter("@TaskCreateInterval", rule.TaskCreateInterval != null ? JsonConvert.SerializeObject(rule.TaskCreateInterval) : (object)DBNull.Value),
                new SqlParameter("@TaskPriority", rule.TaskPriority),
                new SqlParameter("@TaskGroupId", rule.TaskGroupId ?? (object)DBNull.Value),
                new SqlParameter("@ManagerFilter", rule.ManagerFilter != null ? JsonConvert.SerializeObject(rule.ManagerFilter) : (object)DBNull.Value),
                new SqlParameter("@Filter", rule.Filter != null ? JsonConvert.SerializeObject(rule.Filter) : (object)DBNull.Value)
                );
        }

        public static int UpdateBizProcessRule(BizProcessRule rule)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "UPDATE CRM.BizProcessRule SET EventType = @EventType, ObjectType = @ObjectType, EventObjId = @EventObjId, Priority = @Priority, TaskName = @TaskName, " +
                "TaskDescription = @TaskDescription, TaskDueDateInterval = @TaskDueDateInterval, TaskCreateInterval = @TaskCreateInterval, TaskPriority = @TaskPriority, " +
                "TaskGroupId = @TaskGroupId, ManagerFilter = @ManagerFilter, Filter = @Filter, DateModified = GETDATE() WHERE Id = @Id",
                CommandType.Text,
                new SqlParameter("@Id", rule.Id),
                new SqlParameter("@EventType", rule.EventType),
                new SqlParameter("@ObjectType", rule.ObjectType),
                new SqlParameter("@EventObjId", rule.EventObjId ?? (object)DBNull.Value),
                new SqlParameter("@Priority", rule.Priority),
                new SqlParameter("@TaskName", rule.TaskName),
                new SqlParameter("@TaskDescription", rule.TaskDescription),
                new SqlParameter("@TaskDueDateInterval", rule.TaskDueDateInterval != null ? JsonConvert.SerializeObject(rule.TaskDueDateInterval) : (object)DBNull.Value),
                new SqlParameter("@TaskCreateInterval", rule.TaskCreateInterval != null ? JsonConvert.SerializeObject(rule.TaskCreateInterval) : (object)DBNull.Value),
                new SqlParameter("@TaskPriority", rule.TaskPriority),
                new SqlParameter("@TaskGroupId", rule.TaskGroupId ?? (object)DBNull.Value),
                new SqlParameter("@ManagerFilter", rule.ManagerFilter != null ? JsonConvert.SerializeObject(rule.ManagerFilter) : (object)DBNull.Value),
                new SqlParameter("@Filter", rule.Filter != null ? JsonConvert.SerializeObject(rule.Filter) : (object)DBNull.Value)
                );
        }

        public static void DeleteBizProcessRule(int id)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM CRM.BizProcessRule WHERE Id = @Id", CommandType.Text, new SqlParameter("@Id", id));
        }
    }
}

