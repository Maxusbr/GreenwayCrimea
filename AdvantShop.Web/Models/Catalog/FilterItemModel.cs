using System.Collections.Generic;

namespace AdvantShop.Models.Catalog
{
    public class FilterItemModel
    {
        public FilterItemModel()
        {
            Values = new List<object>();
        }

        public bool Expanded { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Control { get; set; }

		public string Description { get; set; }

        public List<object> Values { get; set; }
    }

    public class FilterRangeItemModel
    {
        public float Min { get; set; }
        public float Max { get; set; }
        public float CurrentMin { get; set; }
        public float CurrentMax { get; set; }
        public float Step { get; set; }
        public float DecimalPlaces { get; set; }
        public int Id { get; set; }
    }

    public class FilterListItemModel
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public bool Selected { get; set; }

        public bool Available { get; set; }
    }
}