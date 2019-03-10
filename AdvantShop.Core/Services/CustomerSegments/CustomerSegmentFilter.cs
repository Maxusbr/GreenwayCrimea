using System;
using System.Collections.Generic;

namespace AdvantShop.Core.Services.CustomerSegments
{
    public class CustomerSegmentFilter
    {
        /// <summary>
        /// Сумма всех заказов (От)
        /// </summary>
        public float? OrdersSumFrom { get; set; }

        /// <summary>
        /// Сумма всех заказов (До)
        /// </summary>
        public float? OrdersSumTo { get; set; }
        
        /// <summary>
        /// Сумма выполненых заказов (От)
        /// </summary>
        public float? OrdersPaidSumFrom { get; set; }

        /// <summary>
        /// Сумма выполненых заказов (До)
        /// </summary>
        public float? OrdersPaidSumTo { get; set; }
        
        /// <summary>
        /// Кол-во выволненых заказов (От)
        /// </summary>
        public int? OrdersCountFrom { get; set; }

        /// <summary>
        /// Кол-во выволненых заказов (До)
        /// </summary>
        public int? OrdersCountTo { get; set; }
        
        /// <summary>
        /// Средний чек (От)
        /// </summary>
        public float? AverageCheckFrom { get; set; }

        /// <summary>
        /// Средний чек (До)
        /// </summary>
        public float? AverageCheckTo { get; set; }
        
        /// <summary>
        /// Страны
        /// </summary>
        public List<string> Countries { get; set; }

        /// <summary>
        /// Города
        /// </summary>
        public List<string> Cities { get; set; }
        
        /// <summary>
        /// Категории
        /// </summary>
        public List<int> Categories { get; set; }

        /// <summary>
        /// Дата последнего заказа (От)
        /// </summary>
        public DateTime? LastOrderDateFrom { get; set; }

        /// <summary>
        /// Дата последнего заказа (До)
        /// </summary>
        public DateTime? LastOrderDateTo { get; set; }

        /// <summary>
        /// Дата рождения (От)
        /// </summary>
        public DateTime? BirthDayFrom { get; set; }

        /// <summary>
        /// Дата рождения (До)
        /// </summary>
        public DateTime? BirthDayTo { get; set; }


        public List<KeyValuePair<int, string>> CustomerFields { get; set; }
    }
}
