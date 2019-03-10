namespace AdvantShop.Payment
{
    public class QiwiTemplate
    {
        public enum CreateCode
        {
            Sucsses = 0,            //0 //Успех
            WrongParams = 5,        //5 //Неверный формат параметров запроса
            Busy = 13,              //13 //Сервер занят, повторите запрос позже
            ErrorAuth = 150,        //150 //Ошибка авторизации (неверный логин/пароль)
            BillNotFind = 210,      //210 //Счет не найден
            BillIsExist = 215,      //215 //Счет с таким txn-id уже существует
            OutOfMinSum = 241,      //241 //Сумма слишком мала.
            OutOfMaxSum = 242,      //242 //Сумма слишком велика
            OutTimeGetBill = 278,   //278 //Превышение максимального интервала получения списка счетов
            AgentNotExist = 298,    //298 //Кошелек с таким номером не зарегистрирован
            TechnicalErr = 300,     //300 //Техническая ошибка
            Unknown = 9999
        }

        public enum CheckCode
        {
            Success = 0,           //0 Успех
            WrongParams = 5,       //5 Ошибка формата параметров запроса
            DatabaseError = 5,     //13 Ошибка соединения с базой данных
            PasswordErr = 150,     //150 Ошибка проверки пароля
            SignatureErr = 151,    //151 Ошибка проверки подписи
            ServerErr = 300,       //300 Ошибка связи с сервером
            Unknown = 9999
        }

        public const string ProviderID = "QIWI_prv_id";
        public const string RestID = "QIWI_rest_id";
        public const string ProviderName = "QIWI_prv_name";
        public const string Password = "QIWI_password";
        public const string PasswordNotify = "QIWI_passwordNotify";

        public const string CurrencyCode = "QIWI_ccy";
        public const string CurrencyValue = "QIWI_CurrencyValue";

        public class QIWIAnswer
        {
            public Response response { get; set; }
        }


        public class Response
        {
            public int result_code { get; set; }
            public Bill bill { get; set; }
            public string description { get; set; }
        }

        public class Bill
        {
            public string bill_id { get; set; }
            public float amount { get; set; }
            public string ccy { get; set; }
            public string status { get; set; }
            public int error { get; set; }
            public string user { get; set; }
            public string comment { get; set; }
        }
    }
}