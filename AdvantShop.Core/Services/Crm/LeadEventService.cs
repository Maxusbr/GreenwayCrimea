using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.SQL;

namespace AdvantShop.Core.Services.Crm
{
    public class LeadEventService
    {
        public static List<LeadEvent> GetEvents(int leadId)
        {
            return
                SQLDataAccess.Query<LeadEvent>("Select * From [Order].[LeadEvent] Where LeadId=@leadId", new {leadId})
                    .ToList();
        }

        public static int AddEvent(LeadEvent leadEvent)
        {
            leadEvent.Id = SQLDataAccess.ExecuteScalar<int>(
                "Insert Into [Order].[LeadEvent] ([LeadId],[Type],[Title],[Message],[CreatedDate],[CreatedBy],[TaskId]) Values (@LeadId, @Type, @Title, @Message, GetDate(),@CreatedBy,@TaskId); Select scope_identity();",
                CommandType.Text,
                new SqlParameter("@LeadId", leadEvent.LeadId),
                new SqlParameter("@Type", (int) leadEvent.Type),
                new SqlParameter("@Title", leadEvent.Title ?? ""),
                new SqlParameter("@Message", leadEvent.Message ?? ""),
                new SqlParameter("@CreatedBy", leadEvent.CreatedBy ?? ""),
                new SqlParameter("@TaskId", leadEvent.TaskId ?? (object)DBNull.Value));

            return leadEvent.Id;
        }

        public static void DeleteEvent(int id)
        {
            SQLDataAccess.ExecuteNonQuery("Delete from [Order].[LeadEvent] Where Id=@Id", CommandType.Text, new SqlParameter("@Id", id));
        }
    }
}
