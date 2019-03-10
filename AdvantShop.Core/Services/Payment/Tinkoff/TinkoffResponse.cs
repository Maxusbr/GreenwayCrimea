//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------


namespace AdvantShop.Core.Services.Payment.Tinkoff
{
    public class TinkoffBaseResponse
    {
        public string TerminalKey { get; set; }
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }

    public class TinkoffInitResponse : TinkoffBaseResponse
    {
        public int Amount { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public string PaymentId { get; set; }
        public string PaymentURL { get; set; }
    }
}
