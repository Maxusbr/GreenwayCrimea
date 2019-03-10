//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Core.Services.Payment.Alfabank
{
    public class AlfabankRegisterResponse
    {
        public string OrderId { get; set; }
        public string FormUrl { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class AlfabankOrderStatusResponse
    {
        public string OrderStatus { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string OrderNumber { get; set; }

        /// <summary>
        /// Номер (идентификатор) клиента в системе магазина, переданный при регистрации заказа. Присутствует только если магазину разрешено создание связок.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Идентификатор связки созданной при оплате заказа или использованной для оплаты. Присутствует только если магазину разрешено создание связок.
        /// </summary>
        public string BindingId { get; set; }
    }
}
