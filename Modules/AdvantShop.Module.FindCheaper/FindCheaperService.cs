//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Repository.Currencies;
using AdvantShop.Saas;
using AdvantShop.Orders;

namespace AdvantShop.Module.FindCheaper
{
    public class FindCheaperService
    {

        #region Sql Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static FindCheaperRequest GetFindCheaperRequestFromReader(SqlDataReader reader)
        {
            return new FindCheaperRequest
            {
                Id = SQLDataHelper.GetInt(reader, "Id"),
                ClientName = SQLDataHelper.GetString(reader, "ClientName"),
                ClientPhone = SQLDataHelper.GetString(reader, "ClientPhone"),
                WishPrice = SQLDataHelper.GetFloat(reader, "WishPrice"),
                WhereCheaper = SQLDataHelper.GetString(reader, "WhereCheaper"),

                IsProcessed = SQLDataHelper.GetBoolean(reader, "IsProcessed"),
                RequestDate = SQLDataHelper.GetDateTime(reader, "RequestDate"),
                ManagerComment = SQLDataHelper.GetString(reader, "ManagerComment"),

                Price = SQLDataHelper.GetFloat(reader, "Price"),
                OfferArtNo = SQLDataHelper.GetString(reader, "OfferArtNo"),
                ProductName = SQLDataHelper.GetString(reader, "ProductName"),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        public static void AddRequest(FindCheaperRequest request)
        {
            var offer = OfferService.GetOffer(Convert.ToInt32(request.OfferArtNo));
            if (offer == null)
            {
                return;
            }

            request.ClientName = HttpUtility.HtmlEncode(request.ClientName);
            request.ClientPhone = HttpUtility.HtmlEncode(request.ClientPhone).Reduce(20);
            request.WhereCheaper = HttpUtility.HtmlEncode(request.WhereCheaper);
            request.ManagerComment = HttpUtility.HtmlEncode(request.ManagerComment);
            request.ProductName = HttpUtility.HtmlEncode(request.ProductName);

            SQLDataAccess.ExecuteNonQuery(
                "INSERT INTO [Module].[FindCheaper] ([ClientName],[ClientPhone],[WishPrice],[WhereCheaper],[IsProcessed],[ManagerComment],[Price],[OfferArtNo],[ProductName],[RequestDate]) " +
                "VALUES (@Name,@Phone,@WishPrice,@WhereCheaper,@IsProcessed,@ManagerComment,@Price,@OfferArtNo,@ProductName,GETDATE())",
                CommandType.Text,
                new SqlParameter("@Name", request.ClientName),
                new SqlParameter("@Phone", request.ClientPhone),
                new SqlParameter("@WishPrice", request.WishPrice),
                new SqlParameter("@WhereCheaper", request.WhereCheaper),

                new SqlParameter("@IsProcessed", request.IsProcessed),
                new SqlParameter("@ManagerComment", request.ManagerComment ?? (object)DBNull.Value),

                new SqlParameter("@Price", request.Price),
                new SqlParameter("@OfferArtNo", offer.ArtNo),
                new SqlParameter("@ProductName", request.ProductName));

            if ((SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveCrm) ||
                (!SaasDataService.IsSaasEnabled))
            {
                var productId = ProductService.GetProductIDByOfferArtNo(offer.ArtNo);
                if (productId != 0)
                {
                    var orederSource = OrderSourceService.GetOrderSource(OrderType.FindCheaper);

                    LeadService.AddLead(new Lead
                    {
                        Phone = request.ClientPhone,
                        FirstName = request.ClientName,
                        OrderSourceId = orederSource.Id,
                        Comment =
                            string.Format("Модуль \'нашли дешевле\', желаемая цена: {0}, где нашли дешевле: {1}",
                                request.WishPrice, request.WhereCheaper),
                        LeadItems = new List<LeadItem>
                        {
                            new LeadItem
                            {
                                ArtNo = request.OfferArtNo,
                                ProductId = productId,
                                Price = request.Price,
                                Name = request.ProductName,
                                Amount = 1
                            }
                        },
                        LeadCurrency = CurrencyService.CurrentCurrency,
                        Customer = new Customer(CustomerGroupService.DefaultCustomerGroup)
                        {
                            FirstName = request.ClientName,
                            Phone = request.ClientPhone,
                            StandardPhone = StringHelper.ConvertToStandardPhone(request.ClientPhone),
                            CustomerRole = Role.User
                        }
                    }, true);
                }
            }

            const string emailHtml = "<h4>Клиент нашел дешевле</h4><br/>Продукт:{0} артикул {1} цена {2} <br/>Имя: {3} <br/>Телефон: {4} <br/>Желаемая цена: {5} <br/>Где нашли дешевле: {6} <br/>";

            ModulesService.SendModuleMail(
                Guid.Empty,
                "Модуль \'Нашли дешевле\'?",
                string.Format(emailHtml, request.ProductName, request.OfferArtNo, request.Price, request.ClientName, request.ClientPhone, request.WishPrice, request.WhereCheaper),
                ModuleSettingsProvider.GetSettingValue<string>("EmailTo", FindCheaperModule.ModuleStringId),
                true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        public static void UpdateRequest(FindCheaperRequest request)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Module].[FindCheaper] SET [ClientName]=@Name,[ClientPhone]=@Phone,[WishPrice]=@WishPrice,[WhereCheaper]=@WhereCheaper,[IsProcessed]=@IsProcessed,[ManagerComment]=@ManagerComment,[Price]=@Price,[OfferArtNo]=@OfferArtNo,[ProductName]=@ProductName WHERE [Id]=@Id ",
                CommandType.Text,
                new SqlParameter("@Id", request.Id),
                new SqlParameter("@Name", request.ClientName),
                new SqlParameter("@Phone", request.ClientPhone),
                new SqlParameter("@WishPrice", request.WishPrice),
                new SqlParameter("@WhereCheaper", request.WhereCheaper),

                new SqlParameter("@IsProcessed", request.IsProcessed),
                new SqlParameter("@ManagerComment", request.ManagerComment),

                new SqlParameter("@Price", request.Price),
                new SqlParameter("@OfferArtNo", request.OfferArtNo),
                new SqlParameter("@ProductName", request.ProductName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static FindCheaperRequest GetRequest(int id)
        {
            return SQLDataAccess.ExecuteReadOne<FindCheaperRequest>(
                 "Select * From  [Module].[FindCheaper] WHERE [Id] = @Id",
                 CommandType.Text,
                 GetFindCheaperRequestFromReader,
                 new SqlParameter("@Id", id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<FindCheaperRequest> GetRequests()
        {
            return SQLDataAccess.ExecuteReadList<FindCheaperRequest>(
                "Select * From  [Module].[FindCheaper] Order By [RequestDate] Desc",
                CommandType.Text,
                GetFindCheaperRequestFromReader);
        }

        public static void DeleteRequest(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
               "DELETE From  [Module].[FindCheaper] WHERE [Id] = @id",
               CommandType.Text,
               new SqlParameter("@id", id));
        }

        #endregion


        public static bool InstalModule()
        {

            if (!ModulesRepository.IsExistsModuleTable("Module", "FindCheaper"))
            {
                ModulesRepository.ModuleExecuteNonQuery(
                    @"CREATE TABLE [Module].[FindCheaper](
	                    [Id] [int] IDENTITY(1,1) NOT NULL,
	                    [ClientName] [nvarchar](max) NOT NULL,
	                    [ClientPhone] [nvarchar](20) NOT NULL,
	                    [WishPrice] [float] NOT NULL,
	                    [WhereCheaper] [nvarchar](max) NOT NULL,
	                    [RequestDate] [datetime] NOT NULL,
	                    [ManagerComment] [nvarchar](max) NULL,
	                    [Price] [float] NOT NULL,
	                    [OfferArtNo] [nvarchar](50) NOT NULL,
	                    [ProductName] [nvarchar](max) NOT NULL,
	                    [IsProcessed] [bit] NOT NULL,
                     CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
                     ([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]",
                    CommandType.Text);
            }

            ModuleSettingsProvider.SetSettingValue("Title", "Нашли дешевле?", FindCheaperModule.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("TopText", "Нашли дешевле? расскажите об этом нам и мы компенсируем разницу!", FindCheaperModule.ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("FinalText", "Спасибо за ваш запрос! мы вам ответим потому что нам не все равно!!!", FindCheaperModule.ModuleStringId);

            return ModulesRepository.IsExistsModuleTable("Module", "FindCheaper");
        }
    }
}