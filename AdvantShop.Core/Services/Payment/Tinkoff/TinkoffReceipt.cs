//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.Payment.Tinkoff
{
    public class Receipt
    {
        [JsonProperty(Required = Required.Always)]
        public string Email { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Taxation { get; set; }

        [JsonProperty(Required = Required.Always)]
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        /// <summary>
        /// Наименование товара. Максимальная длина строки – 128 символов.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Сумма в копейках. *Целочисленное значение не более 10 знаков.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Price { get; set; }

        /// <summary>
        /// Количество/вес: целая часть не более 8 знаков; дробная часть не более 3 знаков.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public float Quantity { get; set; }

        /// <summary>
        /// Сумма в копейках. Целочисленное значение не более 10 знаков.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Amount { get; set; }

        /// <summary>
        /// штрих-код
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Ean13 { get; set; }

        /// <summary>
        /// Код магазина
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ShopCode { get; set; }

        /// <summary>
        /// Ставка налога Перечисление со значениями:
        ///«none» – без НДС;
        ///«vat0» – НДС по ставке 0%;
        ///«vat10» – НДС чека по ставке 10%;
        ///«vat18» – НДС чека по ставке 18%;
        ///«vat110» – НДС чека по расчетной ставке 10/110;
        ///«vat118» – НДС чека по расчетной ставке 18/118.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Tax { get; set; }

    }
}
