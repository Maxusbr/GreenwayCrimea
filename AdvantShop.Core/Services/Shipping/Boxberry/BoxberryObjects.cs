using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AdvantShop.Core.Services.Shipping.Boxberry
{
    [Serializable]
    public class BoxberryErrorObject
    {
        [JsonProperty(PropertyName = "err")]
        public string Error { get; set; }
    }

    [Serializable]
    public class BoxberryCity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Prefix { get; set; }
        // 'ReceptionLaP' => 'Прием пип', 
        public string ReceptionLaP { get; set; }
        // 'DeliveryLaP' => 'Выдача пип',
        public string DeliveryLaP { get; set; }
        // 'Reception' => 'Прием МиМ', 
        public string Reception { get; set; }
        // 'ForeignReceptionReturns' => 'Прием международных возвратов',
        public string ForeignReceptionReturns { get; set; }
        // 'Terminal' => 'Наличие терминала', 
        public string Terminal { get; set; }
        // 'Kladr' => 'ИД КЛАДРа', 
        public string Kladr { get; set; }
        // 'Region' => 'Регион', 
        public string Region { get; set; }
        // 'CountryCode' => 'Код страны', 
        public string CountryCode { get; set; }
        // 'UniqName' => 'Составное уникальное имя',
        public string UniqName { get; set; }
        // 'District' => 'Район'   
        public string District { get; set; }
        // 'PickupPoint' => 'Наличие ПВЗ',
        public string PickupPoint { get; set; }
    }

    [Serializable]
    public class BoxberryCityCourier
    {
        public string City { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
    }

    [Serializable]
    public class BoxberryObjectPoint : BoxberryErrorObject
    {
        // 'Code'=>'Код в базе boxberry',
        public string Code { get; set; }
        // 'Name'=>'Наименование ПВЗ',
        public string Name { get; set; }
        // 'Address'=>'Полный адрес',
        public string Address { get; set; }
        // 'Phone'=>'Телефон или телефоны',
        public string Phone { get; set; }
        // 'WorkSchedule'=>'График работы',
        public string WorkSchedule { get; set; }
        // 'TripDescription'=>'Описание проезда',
        public string TripDescription { get; set; }
        // 'DeliveryPeriod'=>'Срок доставки',
        public string DeliveryPeriod { get; set; }
        // 'CityCode'=>'Код города в boxberry',
        public string CityCode { get; set; }
        // 'CityName'=>'Наименование города',
        public string CityName { get; set; }
        // 'TariffZone'=>'Тарифная зона',
        public string TariffZone { get; set; }
        // 'Settlement'=>'Населенный пункт',
        public string Settlement { get; set; }
        // 'Area'=>'Регион',
        public string Area { get; set; }
        // 'Country'=>'Страна',
        public string Country { get; set; }
        // 'GPS'=>'Координаты gps',
        public string GPS { get; set; }
        // 'OnlyPrepaidOrders'=>'Если значение "Yes" - точка работает только с полностью оплаченными заказами',
        public string OnlyPrepaidOrders { get; set; }
        // 'Acquiring'=>'Если значение "Yes" - Есть возможность оплаты платежными(банковскими) картами',
        public string Acquiring { get; set; }
        // 'DigitalSignature'=>'Если значение "Yes" - Подпись получателя будет хранится в системе boxberry в электронном виде'
        public string DigitalSignature { get; set; }
    }

    [Serializable]
    public class BoxberryDeliveryCost
    {
        [JsonProperty(PropertyName = "price")]
        public float Price { get; set; }

        [JsonProperty(PropertyName = "price_base")]
        public float PriceBase { get; set; }

        [JsonProperty(PropertyName = "price_service")]
        public float PriceService { get; set; }

        [JsonProperty(PropertyName = "delivery_period")]
        public int DeliveryPeriod { get; set; }
    }

    [Serializable]
    public class BoxberryZipCheck
    {
        public string ExpressDelivery { get; set; }
        public string ZoneExpressDelivery { get; set; }
    }

    [Serializable]
    public class BoxberryZip
    {
        public string Zip { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        //'Area'=>'Позволяет получить информацию о возможности осуществления
        //  курьерской доставки в заданном индексе.Обязательно наличие параметра("zip" почтовый код
        //  для которого осуществляется проверка)'
        public string Area { get; set; }
        //Тарифная зона'
        public string ZoneExpressDelivery { get; set; }
    }

    #region Boxberry Order objects

    [Serializable]
    public class BoxberryOrder
    {
        [JsonProperty(PropertyName = "updateByTrack")]
        public string TrackCode { get; set; }

        [JsonProperty(PropertyName = "order_id")]
        public string OrderId { get; set; }

        //'Объявленная стоимость
        [JsonProperty(PropertyName = "price")]
        public string Price { get; set; }

        [JsonProperty(PropertyName = "payment_sum")]
        public string PaymentSum { get; set; }

        [JsonProperty(PropertyName = "delivery_sum")]
        public string DeliverySum { get; set; }

        //'Тип доставки (1/2)'
        [JsonProperty(PropertyName = "vid")]
        public string DeliveryType { get; set; }

        [JsonProperty(PropertyName = "shop")]
        public BoxberryOrderShopInfo Shop { get; set; }

        [JsonProperty(PropertyName = "customer")]
        public BoxberryOrderCustomer Customer { get; set; }

        //для курьерской доставки
        [JsonProperty(PropertyName = "kurdost")]
        public BoxberryOrderShippingInfo Kurdost { get; set; }

        [JsonProperty(PropertyName = "items")]
        public List<BoxberryOrderItem> Items { get; set; }

        [JsonProperty(PropertyName = "weights")]
        public BoxberryOrderWeight Weights { get; set; }
    }

    [Serializable]
    public class BoxberryOrderShopInfo
    {
        //'name'=>'Код ПВЗ'
        [JsonProperty(PropertyName = "name")]
        public string CodeDestination { get; set; }

        //'name1'=>'Код пункта поступления'
        [JsonProperty(PropertyName = "name1")]
        public string CodeDeparture { get; set; }
    }

    [Serializable]
    public class BoxberryOrderCustomer
    {
        [JsonProperty(PropertyName = "fio")]
        public string Fio { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "phone2")]
        public string Phone2 { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        //'name'=>'Наименование организации',
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "inn")]
        public string Inn { get; set; }

        [JsonProperty(PropertyName = "kpp")]
        public string Kpp { get; set; }

        //'r_s'=>'Расчетный счет',
        [JsonProperty(PropertyName = "r_s")]
        public string CheckingAccount { get; set; }

        //'bank'=>'Наименование банка',
        [JsonProperty(PropertyName = "bank")]
        public string bank { get; set; }

        //'kor_s'=>'Кор. счет',
        [JsonProperty(PropertyName = "kor_s")]
        public string CorrespondentAccount { get; set; }

        [JsonProperty(PropertyName = "bik")]
        public string Bik { get; set; }
    }

    [Serializable]
    public class BoxberryOrderShippingInfo
    {
        [JsonProperty(PropertyName = "index")]
        public string Index { get; set; }

        [JsonProperty(PropertyName = "citi")]
        public string City { get; set; }

        ////'addressp' => 'Адрес получателя',
        [JsonProperty(PropertyName = "addressp")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "timesfrom1")]
        public string DeliveryTimeFrom { get; set; }

        [JsonProperty(PropertyName = "timesto1")]
        public string DeliveryTimeTo { get; set; }

        [JsonProperty(PropertyName = "timesfrom2")]
        public string AltDeliveryTimeFrom { get; set; }

        [JsonProperty(PropertyName = "timesto2")]
        public string AltDeliveryTimeTo { get; set; }

        [JsonProperty(PropertyName = "timep")]
        public string Timep { get; set; }

        [JsonProperty(PropertyName = "comentk")]
        public string Comment { get; set; }
    }

    [Serializable]
    public class BoxberryOrderItem
    {
        //'id'=>'Артикул товара в БД',
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        ////'UnitName'=>'Единица измерения',
        [JsonProperty(PropertyName = "UnitName")]
        public string UnitName { get; set; }

        [JsonProperty(PropertyName = "nds")]
        public string Nds { get; set; }

        [JsonProperty(PropertyName = "price")]
        public float Price { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public float Quantity { get; set; }
    }

    [Serializable]
    public class BoxberryOrderWeight
    {
        [JsonProperty(PropertyName = "weight")]
        public string Weight { get; set; }

        [JsonProperty(PropertyName = "barcode")]
        public string Barcode { get; set; }

        [JsonProperty(PropertyName = "weight2")]
        public string Weight2 { get; set; }

        [JsonProperty(PropertyName = "barcode2")]
        public string Barcode2 { get; set; }

        [JsonProperty(PropertyName = "weight3")]
        public string Weight3 { get; set; }

        [JsonProperty(PropertyName = "barcode3")]
        public string Barcode3 { get; set; }

        [JsonProperty(PropertyName = "weight4")]
        public string Weight4 { get; set; }

        [JsonProperty(PropertyName = "barcode4")]
        public string Barcode4 { get; set; }

        [JsonProperty(PropertyName = "weight5")]
        public string Weight5 { get; set; }

        [JsonProperty(PropertyName = "barcode5")]
        public string Barcode5 { get; set; }
    }

    [Serializable]
    public class BoxberryOrderTrackNumber : BoxberryApiResponse
    {
        //track'=>'XXXXXXXX', // Трекинг код для посылки.
        //'label'=>
        [JsonProperty(PropertyName = "track")]
        public string TrackNumber { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string LinkLabelsFile { get; set; }
    }

    #endregion Boxberry Order objects



    [Serializable]
    public class BoxberryParcelPoint
    {
        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }
    }

    [Serializable]
    public class BoxberryObjectOptions
    {
        [JsonProperty(PropertyName = "result")]
        public BoxberryOptions Result { get; set; }
    }

    [Serializable]
    public class BoxberryOptions
    {
        [JsonProperty(PropertyName = "1")]
        public BoxberrySettings1 Settings1 { get; set; }

        [JsonProperty(PropertyName = "2")]
        public BoxberrySettings2 Settings2 { get; set; }

        [JsonProperty(PropertyName = "3")]
        public BoxberrySettings3 Settings3 { get; set; }
    }

    [Serializable]
    public class BoxberrySettings1
    {
        //          "prepaid": false,
        //			"CityCode": [],
        //			"Code": []        
    }

    [Serializable]
    public class BoxberrySettings2
    {
        //			"power": 0,
        //			"length": 1,
        //			"round": 2,
        //			"surcharges": 2,
        //			"surcharges_value": 0,
        //			"surcharges_percent": 0,
        //			"sum_min": 0,
        //			"sum_max": 3000,
        //			"tariff": 1,
        //			"tariff_value": 121,
        //			"value_sum_max": 3000,
        //			"tariff_max": 1,
        //			"tariff_max_value": 121,
        //			"limit_min": 0,
        //			"limit_min_value": 0,
        //			"limit_max": 0,
        //			"limit_max_value": 3000
    }

    [Serializable]
    public class BoxberrySettings3
    {
        [JsonProperty(PropertyName = "hide_delivery_day")]
        public int HideDeliveryDay { get; set; }

        [JsonProperty(PropertyName = "add_delivery_day")]
        public int AddDeliveryDay { get; set; }
    }

}
