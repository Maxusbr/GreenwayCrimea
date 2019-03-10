using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdvantShop.Module.MoySklad.Domain
{
    #region Base

    public class MetaData
    {
        public string Href { get; set; }
        public string MetadataHref { get; set; }
        public string Type { get; set; }
        public string MediaType { get; set; }
        public int? Size { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }

    public class EntityMetaContainer
    {
        public MetaData Meta { get; set; }
    }

    #endregion

    #region Product

    public class PriceType
    {
        /// <summary>
        /// Наименование типа цен
        /// </summary>
        public string Name { get; set; }
    }

    public class Price
    {
        /// <summary>
        /// Значение цены (в копейках)
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Валюта
        /// </summary>
        public string Currency { get; set; }
    }

    public class SalePrice : Price
    {
        /// <summary>
        /// Тип цены
        /// </summary>
        public string PriceType { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnTypeAttribute
    {
        [EnumMember(Value = "string")]
        String,

        [EnumMember(Value = "long")]
        Long,

        [EnumMember(Value = "time")]
        Time,

        [EnumMember(Value = "file")]
        File,

        [EnumMember(Value = "double")]
        Double,

        [EnumMember(Value = "boolean")]
        Boolean,

        [EnumMember(Value = "text")]
        Text,

        [EnumMember(Value = "link")]
        Link
    }

    public class AttributeWithValue : Attribute
    {
        /// <summary>
        /// Значение доп. поля (должно соответствовать типу доп. поля)
        /// </summary>
        [JsonProperty(Required = Newtonsoft.Json.Required.Always)]
        public string Value { get; set; }
    }

    public class Attribute
    {
        /// <summary>
        /// ID доп. поля
        /// </summary>
        [JsonProperty(Required = Newtonsoft.Json.Required.Always)]
        public string AttributeId { get; set; }

        /// <summary>
        /// Наименование доп. поля
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип доп. поля
        /// </summary>
        public EnTypeAttribute? Type { get; set; }

        /// <summary>
        /// Флажок о том, является ли доп. поле обязательным
        /// </summary>
        public bool? Required { get; set; }
    }

    public class EntityProduct
    {
        /// <summary>
        /// Имя Товара
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Описание Товара
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Артикул
        /// </summary>
        public string Article { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Внешний код
        /// </summary>
        public string ExternalCode { get; set; }

        ///// <summary>
        ///// Наименование группы, в которую входит товар (только чтение)
        ///// </summary>
        //public string PathName { get; set; }

        /// <summary>
        /// Групппа товара
        /// </summary>
        public EntityMetaContainer ProductFolder { get; set; }

        /// <summary>
        /// НДС %
        /// </summary>
        public string Vat { get; set; }

        /// <summary>
        /// Реальный НДС %
        /// </summary>
        public string EffectiveVat { get; set; }

        /// <summary>
        /// Единицы измерения
        /// </summary>
        public string Uom { get; set; }

        /// <summary>
        /// Минимальная цена (в копейках)
        /// </summary>
        public float MinPrice { get; set; }

        /// <summary>
        /// Цена закупки
        /// </summary>
        public Price BuyPrice { get; set; }

        /// <summary>
        /// Цены продажи
        /// </summary>
        public List<SalePrice> SalePrices { get; set; }

        /// <summary>
        /// Вес
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// Объём
        /// </summary>
        public float Volume { get; set; }

        /// <summary>
        /// Доп. поля
        /// </summary>
        public List<AttributeWithValue> Attributes { get; set; }
    }

    public class EntityVariantProduct
    {
        /// <summary>
        /// Ссылка на продукт, к которому относится модификация
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public MetaData Meta { get; set; }
    }

    public class EntityVariant
    {
        /// <summary>
        /// Имя Товара
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Внешний код
        /// </summary>
        public string ExternalCode { get; set; }

        /// <summary>
        /// Цена закупки
        /// </summary>
        public Price BuyPrice { get; set; }

        /// <summary>
        /// Цены продажи
        /// </summary>
        public List<SalePrice> SalePrices { get; set; }

        /// <summary>
        /// Ссылка на продукт, к которому относится модификация
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public EntityVariantProduct Product { get; set; }

        /// <summary>
        /// Объект, свойства которого представляют собой пару имя характеристики - её значение
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ExpandoObject Characteristics { get; set; }
    }

    #endregion

    #region Category

    public class EntityProductFolder
    {
        /// <summary>
        /// Наименование Группы
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Внешний код
        /// </summary>
        public string ExternalCode { get; set; }

        /// <summary>
        /// НДС %
        /// </summary>
        public string Vat { get; set; }

        /// <summary>
        /// Ссылка на родительскую группу
        /// </summary>
        public EntityMetaContainer ProductFolder { get; set; }
    }

    public class EntityProductFolderFromList : EntityProductFolder
    {
        public string Id { get; set; }

        ///// <summary>
        ///// Наименование Группы
        ///// </summary>
        //public string Name { get; set; }

        ///// <summary>
        ///// Ссылка на родительскую группу
        ///// </summary>
        //public EntityMetaProductFolder ProductFolder { get; set; }
    }

    #endregion

    #region State

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnTypeState
    {
        /// <summary>
        /// Обычный
        /// </summary>
        [EnumMember(Value = "Regular")]
        Regular,

        /// <summary>
        /// Финальный положитеьный
        /// </summary>
        [EnumMember(Value = "Successful")]
        Successful,

        /// <summary>
        /// Финальный отрицательный
        /// </summary>
        [EnumMember(Value = "Unsuccessful")]
        Unsuccessful,
    }

    public class EntityState
    {
        /// <summary>
        /// ID в формате UUID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Наименование Статуса
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Цвет статуса ARGB
        /// </summary>
        public int Color { get; set; }

        /// <summary>
        /// Тип статуса
        /// </summary>
        public EnTypeState StateType { get; set; }

        /// <summary>
        /// Тип сущности, к которой привязан статус
        /// </summary>
        public string EntityType { get; set; }
    }

    #endregion

    #region CustomerOrder

    public class EntityCustomerOrder
    {
        /// <summary>
        /// Внешний код заказа покупателя
        /// </summary>
        public string ExternalCode { get; set; }

        /// <summary>
        /// Наименование заказа покупателя
        /// </summary>
        public string Name { get; set; }

        public EntityState State { get; set; }

    }

    #endregion

    #region Counterparty

    public class EntityCounterparty
    {
        public string Id { get; set; }

        public string ExternalCode { get; set; }

        /// <summary>
        /// Имя агента
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Кодовый идентификатор
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Номер мобильного
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Факс
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string ActualAddress { get; set; }

        /// <summary>
        /// Описание Контрагента
        /// </summary>
        public string Description { get; set; }
    }

    public class EntityCounterpartyContactperson
    {
        public string Id { get; set; }

        public string ExternalCode { get; set; }

        /// <summary>
        /// Имя контактного лица
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Описание контактного лица
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Должность, занимаемая контактным лицом
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Ссылка на контрагента
        /// </summary>
        //[JsonProperty(Required = Required.Always)]
        public EntityMetaContainer Agent { get; set; }

    }

    #endregion
}