using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Services.IPTelephony
{
    public class CallService
    {
        private static readonly Object SyncObject = new Object();

        public static Call GetCallFromReader(SqlDataReader reader)
        {
            return new Call
            {
                Id = SQLDataHelper.GetInt(reader, "Id"),
                CallId = SQLDataHelper.GetString(reader, "CallId"),
                Type = SQLDataHelper.GetString(reader, "Type").TryParseEnum<ECallType>(),
                SrcNum = SQLDataHelper.GetString(reader, "SrcNum"),
                DstNum = SQLDataHelper.GetString(reader, "DstNum"),
                Extension = SQLDataHelper.GetString(reader, "Extension"),
                CallDate = SQLDataHelper.GetDateTime(reader, "CallDate"),
                CallAnswerDate = SQLDataHelper.GetNullableDateTime(reader, "CallAnswerDate"),
                Duration = SQLDataHelper.GetInt(reader, "Duration"),
                RecordLink = SQLDataHelper.GetString(reader, "RecordLink"),
                CalledBack = SQLDataHelper.GetBoolean(reader, "CalledBack"),
                HangupStatus = SQLDataHelper.GetString(reader, "HangupStatus").TryParseEnum<ECallHangupStatus>(),
                OperatorType = SQLDataHelper.GetString(reader, "OperatorType").TryParseEnum<EOperatorType>(),
                ManagerId = SQLDataHelper.GetNullableInt(reader, "ManagerId"),
                Phone = SQLDataHelper.GetString(reader, "Phone"),
                IsComplete = SQLDataHelper.GetBoolean(reader, "IsComplete"),
            };
        }

        public static Call GetCall(int id)
        {
            return SQLDataAccess.ExecuteReadOne<Call>(
                "SELECT * FROM Customers.Call WHERE Id = @Id",
                CommandType.Text, GetCallFromReader,
                new SqlParameter("@Id", id));
        }

        public static Call GetCall(string callId, EOperatorType operatorType)
        {
            return SQLDataAccess.ExecuteReadOne<Call>(
                "SELECT * FROM Customers.Call WHERE CallId = @CallId AND OperatorType = @OperatorType",
                CommandType.Text, GetCallFromReader,
                new SqlParameter("@CallId", callId),
                new SqlParameter("@OperatorType", operatorType.ToString()));
        }

        public static Call GetCall(string callId, string dstNum, EOperatorType operatorType)
        {
            return SQLDataAccess.ExecuteReadOne<Call>(
                "SELECT * FROM Customers.Call WHERE CallId = @CallId AND DstNum = @DstNum AND OperatorType = @OperatorType",
                CommandType.Text, GetCallFromReader,
                new SqlParameter("@CallId", callId),
                new SqlParameter("@DstNum", dstNum),
                new SqlParameter("@OperatorType", operatorType.ToString()));
        }

        public static List<Call> GetAllCalls()
        {
            return SQLDataAccess.ExecuteReadList<Call>(
                "SELECT * FROM Customers.Call", CommandType.Text, GetCallFromReader);
        }

        public static List<Call> GetCalls(long phone)
        {
            if (phone.ToString().Length < 6)
            {
                return null;
            }

            return SQLDataAccess.ExecuteReadList<Call>(
                "SELECT * FROM Customers.Call WHERE SrcNum LIKE '%' + @Phone + '%' OR DstNum LIKE '%' + @Phone + '%' ORDER BY CallDate DESC",
                CommandType.Text, GetCallFromReader,
                new SqlParameter("@Phone", phone.ToString()));
        }

        public static List<Call> GetCalls(ECallType type, string srcNum)
        {
            if (srcNum.Length < 6)
            {
                return null;
            }

            return SQLDataAccess.ExecuteReadList<Call>(
                "SELECT * FROM Customers.Call WHERE Type = @Type AND SrcNum = @SrcNum ORDER BY CallDate DESC",
                CommandType.Text, GetCallFromReader,
                new SqlParameter("@Type", type.ToString()),
                new SqlParameter("@SrcNum", srcNum));
        }

        public static Call DeleteCall(int id)
        {
            return SQLDataAccess.ExecuteReadOne<Call>(
                "DELETE FROM Customers.Call WHERE Id = @Id",
                CommandType.Text, GetCallFromReader,
                new SqlParameter("@Id", id));
        }

        private static bool IsExistCall(string callId)
        {
            return SQLDataAccess.ExecuteScalar<int>("SELECT COUNT(*) FROM Customers.Call WHERE CallId = @CallId",
                CommandType.Text, new SqlParameter("@CallId", callId)) > 0;
        }

        public static int AddCall(Call call)
        {
            lock (SyncObject)
            {
                if (IsExistCall(call.CallId))
                    return 0;
                return SQLDataAccess.ExecuteScalar<int>(
                    "INSERT INTO Customers.Call (CallId, Type, SrcNum, DstNum, Extension, CallDate, CallAnswerDate, Duration, RecordLink, CalledBack, HangupStatus, OperatorType, ManagerId, Phone, IsComplete) " +
                    "VALUES (@CallId, @Type, @SrcNum, @DstNum, @Extension, @CallDate, @CallAnswerDate, @Duration, @RecordLink, @CalledBack, @HangupStatus, @OperatorType, @ManagerId, @Phone, @IsComplete); " +
                    "SELECT SCOPE_IDENTITY();",
                    CommandType.Text,
                    new SqlParameter("@CallId", call.CallId),
                    new SqlParameter("@Type", call.Type.ToString()),
                    new SqlParameter("@SrcNum", !string.IsNullOrEmpty(call.SrcNum) 
                                                    ? StringHelper.ConvertToStandardPhone(call.SrcNum).ToString() 
                                                    : string.Empty),
                    new SqlParameter("@DstNum", !string.IsNullOrEmpty(call.DstNum) 
                                                    ? StringHelper.ConvertToStandardPhone(call.DstNum).ToString() 
                                                    : string.Empty),
                    new SqlParameter("@Extension", call.Extension ?? string.Empty),
                    new SqlParameter("@CallDate", call.CallDate),
                    new SqlParameter("@CallAnswerDate", call.CallAnswerDate ?? (object) DBNull.Value),
                    new SqlParameter("@Duration", call.Duration),
                    new SqlParameter("@RecordLink", call.RecordLink ?? string.Empty),
                    new SqlParameter("@CalledBack", call.CalledBack),
                    new SqlParameter("@HangupStatus", call.HangupStatus.ToString()),
                    new SqlParameter("@OperatorType", call.OperatorType.ToString()),
                    new SqlParameter("@ManagerId", call.ManagerId ?? (object) DBNull.Value),
                    new SqlParameter("@Phone", !string.IsNullOrEmpty(call.Phone)
                                                    ? StringHelper.ConvertToStandardPhone(call.Phone).ToString()
                                                    : string.Empty),
                    new SqlParameter("@IsComplete", call.IsComplete)
                    );
            }
        }

        public static int UpdateCall(Call call)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "UPDATE Customers.Call SET CallId=@CallId, Type=@Type, SrcNum=@SrcNum, DstNum=@DstNum, Extension=@Extension, CallDate=@CallDate, CallAnswerDate=@CallAnswerDate, " +
                "Duration=@Duration, RecordLink=@RecordLink, CalledBack=@CalledBack, HangupStatus=@HangupStatus, OperatorType=@OperatorType, ManagerId=@ManagerId, Phone=@Phone, IsComplete=@IsComplete " +
                "WHERE Id = @Id",
                CommandType.Text,
                new SqlParameter("@Id", call.Id),
                new SqlParameter("@CallId", call.CallId),
                new SqlParameter("@Type", call.Type.ToString()),
                new SqlParameter("@SrcNum", call.SrcNum ?? string.Empty),
                new SqlParameter("@DstNum", call.DstNum ?? string.Empty),
                new SqlParameter("@Extension", call.Extension ?? string.Empty),
                new SqlParameter("@CallDate", call.CallDate),
                new SqlParameter("@CallAnswerDate", call.CallAnswerDate ?? (object)DBNull.Value),
                new SqlParameter("@Duration", call.Duration),
                new SqlParameter("@RecordLink", call.RecordLink ?? string.Empty),
                new SqlParameter("@CalledBack", call.CalledBack),
                new SqlParameter("@HangupStatus", call.HangupStatus.ToString()),
                new SqlParameter("@OperatorType", call.OperatorType.ToString()),
                new SqlParameter("@ManagerId", call.ManagerId ?? (object) DBNull.Value),
                new SqlParameter("@Phone", call.Phone ?? string.Empty),
                new SqlParameter("@IsComplete", call.IsComplete)
                );
        }

        public static void SetCallCalledBack(int id, bool calledBack)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE Customers.Call SET CalledBack=@CalledBack WHERE Id = @Id",
                CommandType.Text, new SqlParameter("@Id", id), new SqlParameter("@CalledBack", calledBack));
        }

        public static void ProcessCall(Call call)
        {
            if (call.Type == ECallType.Out && call.Duration > 0)
            {
                // ранее пропущенные вызовы от номера, на который осуществляется вызов
                foreach (var missedCall in GetCalls(ECallType.Missed, call.DstNum).Where(c => c.CallDate < call.CallDate && !c.CalledBack))
                {
                    SetCallCalledBack(missedCall.Id, true);
                }
            }
        }

        public static List<Call> GetNotCompletedIncomingCalls()
        {
            return SQLDataAccess.ExecuteReadList<Call>(
                "SELECT * FROM Customers.Call WHERE [Type] = @Type AND CallDate > @DateFrom AND IsComplete = 0", CommandType.Text, GetCallFromReader,
                new SqlParameter("@Type", ECallType.In.ToString()),
                new SqlParameter("@DateFrom", DateTime.Now.AddMinutes(-5)));
        }
    }
}
