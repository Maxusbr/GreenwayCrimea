//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Payment
{
    public class AvangardResponse
    {
        //Идентификатор запроса в АБС банка Авангард
        public int Id { get; set; }

        //Тикет
        public string Ticket { get; set; }

        //Код, возвращаемый в случае успешной оплаты
        public string OkCode { get; set; }

        //Код, возвращаемый в случае каких-либо ошибок в процессе оплаты
        public string FailureCode { get; set; }

        //Код ответа для операции регистрации заказа
        public int ResponseCode { get; set; }

        //Детальное сообщение ответа
        public string ResponseMessage { get; set; }
    }
}