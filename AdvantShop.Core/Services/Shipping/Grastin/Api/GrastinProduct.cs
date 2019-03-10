using System;
using System.Xml.Serialization;

namespace AdvantShop.Core.Services.Shipping.Grastin.Api
{
    [Serializable]
    public class GrastinProduct
    {
        /// <summary>
        /// Артикул товара
        /// </summary>
        [XmlAttribute("article")]
        public string ArtNo { get; set; }

        /// <summary>
        /// Наименование товара
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Стоимость товара
        /// </summary>
        [XmlAttribute("cost")]
        public float Price { get; set; }

        /// <summary>
        /// Количество товара
        /// </summary>
        [XmlAttribute("amount")]
        public float Amount { get; set; }
    }
}
