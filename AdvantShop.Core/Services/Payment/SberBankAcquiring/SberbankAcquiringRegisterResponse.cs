namespace AdvantShop.Core.Services.Payment.SberBankAcquiring
{
    // Documentation: https://developer.sberbank.ru/acquiring-api-rest-requests1pay


    public class SberbankAcquiringRegisterResponse
    {
        public string OrderId { get; set; }
        public string FormUrl { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class SberbankAcquiringOrderStatusResponse
    {
        public string OrderStatus { get; set; }
        public string ErrorCode { get; set; }
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
