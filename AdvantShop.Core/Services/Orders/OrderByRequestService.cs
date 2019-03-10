//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Orders
{
    public class OrderByRequestService
    {
        public static OrderByRequest GetOrderByRequest(int orderByRequestId)
        {
            return SQLDataAccess.ExecuteReadOne<OrderByRequest>(
                "SELECT * FROM [Order].[OrderByRequest] WHERE OrderByRequestId = @OrderByRequestId ",
                CommandType.Text, GetOrderByRequestFromReader,
                new SqlParameter("@OrderByRequestId", orderByRequestId));
        }

        public static OrderByRequest GetOrderByRequest(string code)
        {
            return SQLDataAccess.ExecuteReadOne<OrderByRequest>(
                "SELECT TOP(1) * FROM [Order].[OrderByRequest] WHERE Code = @code  ",
                CommandType.Text, GetOrderByRequestFromReader,
                new SqlParameter("@Code", code));
        }

        public static List<int> GetIdList()
        {
            return SQLDataAccess.ExecuteReadList<int>(
                "SELECT [OrderByRequestId] FROM [Order].[OrderByRequest]",
                CommandType.Text, reader => SQLDataHelper.GetInt(reader, "OrderByRequestId"));
        }

        public static List<OrderByRequest> GetOrderByRequestList()
        {
            return SQLDataAccess.ExecuteReadList<OrderByRequest>(
                "SELECT [OrderByRequestId] FROM [Order].[OrderByRequest]",
                CommandType.Text, GetOrderByRequestFromReader);
        }

        private static OrderByRequest GetOrderByRequestFromReader(SqlDataReader reader)
        {
            return new OrderByRequest
            {
                OrderByRequestId = SQLDataHelper.GetInt(reader, "OrderByRequestId"),
                ProductId = SQLDataHelper.GetInt(reader, "ProductID"),
                OfferId = SQLDataHelper.GetInt(reader, "OfferID"),
                ProductName = SQLDataHelper.GetString(reader, "ProductName"),
                ArtNo = SQLDataHelper.GetString(reader, "ArtNo"),
                Quantity = SQLDataHelper.GetFloat(reader, "Quantity"),
                UserName = SQLDataHelper.GetString(reader, "UserName"),
                Email = SQLDataHelper.GetString(reader, "Email"),
                Phone = SQLDataHelper.GetString(reader, "Phone"),
                Comment = SQLDataHelper.GetString(reader, "Comment"),
                IsComplete = SQLDataHelper.GetBoolean(reader, "IsComplete"),
                RequestDate = SQLDataHelper.GetDateTime(reader, "RequestDate"),
                Code = SQLDataHelper.GetString(reader, "Code"),
                CodeCreateDate = SQLDataHelper.GetDateTime(reader, "CodeCreateDate", DateTime.MinValue),
                LetterComment = SQLDataHelper.GetString(reader, "LetterComment"),
                Options = SQLDataHelper.GetString(reader, "Options"),
            };
        }

        public static void AddOrderByRequest(OrderByRequest orderByRequest)
        {
            orderByRequest.OrderByRequestId = SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO [Order].[OrderByRequest] " +
                "([ProductID], [ProductName], [ArtNo], [Quantity], [UserName], [Email], [Phone], [Comment], [IsComplete], [RequestDate], [OfferID], LetterComment, Options) " +
                "VALUES (@ProductID, @ProductName, @ArtNo, @Quantity, @UserName, @Email, @Phone, @Comment, @IsComplete, @RequestDate, @OfferID, @LetterComment, @Options); " +
                "SELECT SCOPE_IDENTITY();",
                CommandType.Text,
                new SqlParameter("@ProductID", orderByRequest.ProductId),
                new SqlParameter("@ProductName", orderByRequest.ProductName),
                new SqlParameter("@ArtNo", orderByRequest.ArtNo),
                new SqlParameter("@Quantity", orderByRequest.Quantity),
                new SqlParameter("@UserName", orderByRequest.UserName),
                new SqlParameter("@Email", orderByRequest.Email),
                new SqlParameter("@Phone", orderByRequest.Phone),
                new SqlParameter("@Comment", orderByRequest.Comment ?? string.Empty),
                new SqlParameter("@IsComplete", orderByRequest.IsComplete),
                new SqlParameter("@RequestDate", orderByRequest.RequestDate),
                new SqlParameter("@OfferID", orderByRequest.OfferId),
                new SqlParameter("@LetterComment", orderByRequest.LetterComment ?? string.Empty),
                new SqlParameter("@Options", orderByRequest.Options ?? string.Empty)
                );
        }

        public static void UpdateOrderByRequest(OrderByRequest orderByRequest)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Order].[OrderByRequest] SET [ProductID]=@ProductID, [ProductName]=@ProductName, [ArtNo]=@ArtNo, [Quantity]=@Quantity, [UserName]=@UserName, [Email]=@Email, " +
                "[Phone]=@Phone, [Comment]=@Comment, [IsComplete]=@IsComplete, [RequestDate]=@RequestDate, [OfferID]=@OfferID, LetterComment=@LetterComment, Options=@Options " +
                "WHERE OrderByRequestId=@OrderByRequestId",
                CommandType.Text,
                new SqlParameter("@OrderByRequestId", orderByRequest.OrderByRequestId),
                new SqlParameter("@ProductID", orderByRequest.ProductId),
                new SqlParameter("@ProductName", orderByRequest.ProductName),
                new SqlParameter("@ArtNo", orderByRequest.ArtNo),
                new SqlParameter("@Quantity", orderByRequest.Quantity),
                new SqlParameter("@UserName", orderByRequest.UserName),
                new SqlParameter("@Email", orderByRequest.Email),
                new SqlParameter("@Phone", orderByRequest.Phone),
                new SqlParameter("@Comment", orderByRequest.Comment ?? string.Empty),
                new SqlParameter("@IsComplete", orderByRequest.IsComplete),
                new SqlParameter("@RequestDate", orderByRequest.RequestDate),
                new SqlParameter("@OfferID", orderByRequest.OfferId),
                new SqlParameter("@LetterComment", orderByRequest.LetterComment ?? string.Empty),
                new SqlParameter("@Options", orderByRequest.Options ?? string.Empty)
                );
        }

        public static void DeleteOrderByRequest(int orderByRequestId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM [Order].[OrderByRequest] WHERE OrderByRequestId = @OrderByRequestId",
                CommandType.Text, new SqlParameter("@OrderByRequestId", orderByRequestId));
        }

        public static string CreateCode(int orderByRequestId)
        {
            var code = Guid.NewGuid().ToString();
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Order].[OrderByRequest] SET [Code] = @Code, [CodeCreateDate] = GETDATE() WHERE OrderByRequestId = @OrderByRequestId",
                CommandType.Text,
                new SqlParameter("@OrderByRequestId", orderByRequestId),
                new SqlParameter("@Code", code));
            return code;
        }

        public static void DeleteCode(string code)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [Order].[OrderByRequest] SET [Code] = '' WHERE [Code] = @Code",
                CommandType.Text, new SqlParameter("@Code", code));
        }

        public static void SendConfirmationMessage(int orderByRequestId)
        {
            var orderByRequest = GetOrderByRequest(orderByRequestId);
            var code = CreateCode(orderByRequestId);

            var offer = OfferService.GetOffer(orderByRequest.OfferId);
            if (offer == null)
            {
                offer = OfferService.GetOffer(orderByRequest.ArtNo);
                if (offer == null)
                {
                    return;
                }
            }
            string optionsRendered = null;
            if (orderByRequest.Options.IsNotEmpty())
            {
                try
                {
                    var listOptions = CustomOptionsService.DeserializeFromXml(orderByRequest.Options, offer != null ? offer.Product.Currency.Rate : CurrencyService.CurrentCurrency.Rate);
                    optionsRendered = OrderService.RenderSelectedOptions(listOptions, offer != null ? offer.Product.Currency : CurrencyService.CurrentCurrency);
                }
                catch { }
            }

            var linkByRequestMailTemplate = new LinkByRequestMailTemplate(
                orderByRequest.OrderByRequestId.ToString(),
                orderByRequest.ArtNo,
                orderByRequest.ProductName + " " + (optionsRendered ?? string.Empty),
                orderByRequest.Quantity.ToString(), code,
                orderByRequest.UserName,
                orderByRequest.LetterComment,
                offer != null && offer.Color != null ? offer.Color.ColorName : "",
                offer != null && offer.Size != null ? offer.Size.SizeName : "");

            linkByRequestMailTemplate.BuildMail();
            SendMail.SendMailNow(Guid.Empty, orderByRequest.Email, linkByRequestMailTemplate.Subject, linkByRequestMailTemplate.Body, true);
        }

        public static void SendFailureMessage(int orderByRequestId)
        {
            var orderByRequest = GetOrderByRequest(orderByRequestId);
            var offer = OfferService.GetOffer(orderByRequest.OfferId);

            string optionsRendered = null;
            if (orderByRequest.Options.IsNotEmpty())
            {
                try
                {
                    var listOptions = CustomOptionsService.DeserializeFromXml(orderByRequest.Options, offer != null ? offer.Product.Currency.Rate : CurrencyService.CurrentCurrency.Rate);
                    optionsRendered = OrderService.RenderSelectedOptions(listOptions, offer != null ? offer.Product.Currency : CurrencyService.CurrentCurrency);
                }
                catch { }
            }

            var failureByRequestMail = new FailureByRequestMailTemplate(
                orderByRequest.OrderByRequestId.ToString(),
                orderByRequest.ArtNo, orderByRequest.ProductName + " " + (optionsRendered ?? string.Empty),
                orderByRequest.Quantity.ToString(),
                orderByRequest.UserName,
                orderByRequest.LetterComment,
                offer != null && offer.Color != null ? offer.Color.ColorName : "",
                offer != null && offer.Size != null ? offer.Size.SizeName : "");

            failureByRequestMail.BuildMail();
            SendMail.SendMailNow(Guid.Empty, orderByRequest.Email, failureByRequestMail.Subject, failureByRequestMail.Body, true);
        }
    }
}