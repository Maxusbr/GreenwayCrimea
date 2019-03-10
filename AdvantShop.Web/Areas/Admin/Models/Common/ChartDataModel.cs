using System.Collections.Generic;

namespace AdvantShop.Web.Admin.Models.Common
{
    public class ChartDataModel
    {
        public ChartDataModel()
        {
            Colors = "['#71c73e', '#77b7c4']";
        }

        public string Data { get; set; }
        public string Labels { get; set; }
        public string Series { get; set; }
        public string Colors { get; set; }
    }

    public class ChartDataJsonModel
    {
        public ChartDataJsonModel()
        {
            Colors = new List<string>() {"#71c73e", "#77b7c4"};
        }

        public List<object> Data { get; set; }
        public List<string> Labels { get; set; }
        public List<string> Series { get; set; }
        public List<string> Colors { get; set; }
    }
}