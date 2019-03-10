using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Module.VkMarket.Domain
{
    public class VkProduct
    {
        /// <summary>
        /// Идентификатор товара ВКонтакте.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ProductId in shop
        /// </summary>
        public int ProductId { get; set; }


        public long AlbumId { get; set; }

        /// <summary>
        /// Идентификатор владельца товара. Обратите внимание, идентификатор сообщества в параметре owner_id необходимо указывать со знаком "-" — например, owner_id=-1 соответствует идентификатору сообщества ВКонтакте API (club1)  целое число, обязательный параметр (целое число, обязательный параметр).
        /// </summary>
        public long OwnerId { get; set; }
        
        /// <summary>
        /// Название товара. строка, минимальная длина 4, максимальная длина 100, обязательный параметр (строка, минимальная длина 4, максимальная длина 100, обязательный параметр).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание товара. строка, минимальная длина 10, обязательный параметр (строка, минимальная длина 10, обязательный параметр).
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Идентификатор категории товара. положительное число, обязательный параметр (положительное число, обязательный параметр).
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// Цена товара. дробное число, обязательный параметр, минимальное значение 0.01 (дробное число, обязательный параметр, минимальное значение 0.01).
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Статус товара (1 — товар удален, 0 — товар не удален). флаг, может принимать значения 1 или 0 (флаг, может принимать значения 1 или 0).
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Идентификатор фотографии обложки товара. положительное число, обязательный параметр (положительное число, обязательный параметр).
        /// </summary>
        public long MainPhotoId { get; set; }

        /// <summary>
        /// Идентификаторы дополнительных фотографий товара. список положительных чисел, разделенных запятыми, количество элементов должно составлять не более 4 (список положительных чисел, разделенных запятыми, количество элементов должно составлять не более 4).
        /// </summary>
        public IEnumerable<long> PhotoIdsList { get; set; }

        public string PhotoIds
        {
            get { return PhotoIdsList != null ? String.Join(",", PhotoIdsList) : ""; }
            set
            {
                PhotoIdsList = value != null ? value.Split(',').Select(x => x.TryParseLong()).Where(x => x != 0) : null;
            }
        }
    }
}
