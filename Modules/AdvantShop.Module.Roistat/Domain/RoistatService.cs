using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Helpers;
using AdvantShop.Orders;

namespace AdvantShop.Module.Roistat.Domain
{
    /// <summary>
    /// Тип сущности (order, lead)
    /// </summary>
    public enum RoistatEntityType
    {
        Order = 0,
        Lead = 1
    }

    public class RoistatService
    {
        private const string RoistatCookieName = "roistat_visit";

        public static bool Install()
        {
            if (!ModulesRepository.IsExistsModuleTable("Module", "RoistatOrder"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
                    @"CREATE TABLE [Module].[RoistatOrder](
                        [EntityId] [int] NOT NULL,
                        [EntityType] [int] NOT NULL,
                        [RoistatCookie] [nvarchar](50) NOT NULL,                        
                        CONSTRAINT [PK_RoistatOrder] PRIMARY KEY CLUSTERED
                    ([EntityId] ASC, [EntityType] ASC) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]) ON [PRIMARY]",
                    CommandType.Text);
            }

            return true;
        }

        public static void AddUpdateRoistatOrder(int entityId, RoistatEntityType type, string roistatCookie)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "if ((Select Count(*) From [Module].[RoistatOrder] Where EntityId=@EntityId and EntityType=@EntityType) = 0) " +
                "  Insert [Module].[RoistatOrder] (EntityId, EntityType, RoistatCookie) Values (@EntityId, @EntityType, @RoistatCookie) " +
                "else " +
                "  Update [Module].[RoistatOrder] Set RoistatCookie = @RoistatCookie Where EntityId=@EntityId and EntityType=@EntityType ",
                CommandType.Text,
                new SqlParameter("@EntityId", entityId),
                new SqlParameter("@EntityType", (int)type),
                new SqlParameter("@RoistatCookie", roistatCookie)
                );
        }
        
        public static string GetRoistatOrderCookie(int entityId, RoistatEntityType type)
        {
            return ModulesRepository.ModuleExecuteScalar<string>(
                "Select RoistatCookie From [Module].[RoistatOrder] Where EntityId=@EntityId and EntityType=@EntityType",
                CommandType.Text,
                new SqlParameter("@EntityId", entityId),
                new SqlParameter("@EntityType", (int) type));
        }


        public static void OnOrderAdding(IOrder order)
        {
            if (order == null || order.IsDraft)
                return;

            if (order.LeadId != null)
            {
                var leadCookieValue = GetRoistatOrderCookie(order.LeadId.Value, RoistatEntityType.Lead);
                if (!string.IsNullOrEmpty(leadCookieValue))
                    AddUpdateRoistatOrder(order.OrderID, RoistatEntityType.Order, leadCookieValue);
            }

            if (order.IsFromAdminArea)
                return;

            var cookieValue = CommonHelper.GetCookieString(RoistatCookieName);
            if (!string.IsNullOrWhiteSpace(cookieValue))
                AddUpdateRoistatOrder(order.OrderID, RoistatEntityType.Order, cookieValue);
        }

        public static void OnLeadAdding(Lead lead)
        {
            if (lead == null || lead.IsFromAdminArea)
                return;

            var cookieValue = CommonHelper.GetCookieString(RoistatCookieName);
            if (!string.IsNullOrWhiteSpace(cookieValue))
                AddUpdateRoistatOrder(lead.Id, RoistatEntityType.Lead, cookieValue);
        }
    }
}
