//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Payment
{
    /// <summary>
    /// Summary description for WebMoneyTemplate
    /// </summary>
    public struct MasterBankTemplate
    {
        //General params
        public const string Order = "ORDER";
        public const string Sign = "SIGN";
        public const string Timestamp = "TIMESTAMP";

        //POST params
        //1.	AMOUNT (Сумма к оплате. Разделитель копеек – точка)
        //2.	ORDER (Внутренний номер заказа. Должен быть уникальным. Использоваться для завершения расчёта. Содержать только цифры длинной 6-32 значения.)
        //3.	MERCH_URL (URL, который подставляется по ссылке «Назад в магазин». Если не задан, берется из базы настроек терминала)
        //4.	TERMINAL (Код терминала, присваиваемый банком)
        //5.	COUNTRY (Страна. Обязательно передавать, если торговец находится не в России)
        //6.	TIMESTAMP (Время проведения операции в GMT (-4 часа от московского). Формат YYYYMMDDHHMMSS)
        //7.	SIGN (Цифровая подпись. Шифруется по алгоритму: md5(TERMINAL. TIMESTAMP.ORDER.AMOUNT.<секретное слово>) Точка между параметрами – операция конкатенации)
        public const string Amount = "AMOUNT";
        public const string Merch_url = "MERCH_URL";
        public const string Terminal = "TERMINAL";
        public const string Country = "COUNTRY";

        public const string SecretWord = "SecretWord";
        
        //Responce params
        //1.	RESULT (Результат операции. 0 – одобрено 2 – отклонена 3 – технические проблемы )
        //2.	RC (Код ответа ISO8583)
        //3.	CURRENCY (Валюта)
        //4.	ORDER
        //5.	RRN (Номер операции в платёжной системе)
        //6.	INT_REF (Внутренний код операции)
        //7.	AUTHCODE (Код авторизации. Может отсутствовать)
        //8.	PAN (Замаскированный номер карты)
        //9.	TRTYPE (Тип операции. 0 – авторизация (начальный платеж пользователя), 21 – завершение расчёта, 24 – возврат.)
        //10.	TIMESTAMP
        //11.	SIGN (Используется для безопасности клиента)
        //12.	AMOUNT
        public const string Result = "RESULT";
        public const string Rc = "RC";
        public const string Currency = "CURRENCY";
        public const string Rrn = "RRN";
        public const string Int_ref = "INT_REF";
        public const string Authcode = "AUTHCODE";
        public const string Pan = "PAN";
    }
}