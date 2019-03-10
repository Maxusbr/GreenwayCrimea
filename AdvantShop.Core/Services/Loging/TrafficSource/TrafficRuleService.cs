using System.Collections.Generic;

namespace AdvantShop.Core.Services.Loging.TrafficSource
{
    public class TrafficRule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RuleRegex { get; set; }
        public string utm_source { get; set; }
        public string utm_medium { get; set; }
        public int SortOrder { get; set; }
    }

    public class TrafficRuleService
    {
        public static List<TrafficRule> DefaultRules = new List<TrafficRule>()
        {
            new TrafficRule()
            {
                Id = 1000,
                Name = "Поиск Яндекс",
                RuleRegex = @"^(http(s)?:\/\/)?yandex\.[a-z]{2,6}\/yandsearch\?text=",
                SortOrder = 1000
            },
            new TrafficRule()
            {
                Id = 1001,
                Name = "Мобильный Яндекс",
                RuleRegex = @"^(http(s)?:\/\/)?mobile\.yandex\.[a-z]{2,6}\/",
                SortOrder = 1010
            },
            new TrafficRule()
            {
                Id = 1002,
                Name = "Яндекс (остальное)",
                RuleRegex = @"^(http(s)?:\/\/)?[\w_-]+\.yandex\.[a-z]{2,6}\/",
                SortOrder = 1020
            },

            new TrafficRule()
            {
                Id = 1010,
                Name = "Поиск Google",
                RuleRegex = @"^(http(s)?:\/\/)?google\.(\w{2,6}|(com\.ua))\/(search|url|m)\?",
                SortOrder = 1050
            },
            new TrafficRule()
            {
                Id = 1011,
                Name = "Google (остальное)",
                RuleRegex = @"^(http(s)?:\/\/)?google\.(\w{2,6}|(com\.ua))\/",
                SortOrder = 1060
            },

            new TrafficRule()
            {
                Id = 1020,
                Name = "Поиск Mail.ru",
                RuleRegex = @"^(http(s)?:\/\/)?go\.mail\.ru\/search\?",
                SortOrder = 1070
            },

            new TrafficRule()
            {
                Id = 1021,
                Name = "Авито",
                RuleRegex = @"^(http(s)?:\/\/)?avito\.ru\/",
                SortOrder = 1080
            },
            new TrafficRule()
            {
                Id = 1022,
                Name = "Вконтакте",
                RuleRegex = @"^(http(s)?:\/\/)?vk\.com\/",
                SortOrder = 1090
            },
            new TrafficRule()
            {
                Id = 1023,
                Name = "Facebook",
                RuleRegex = @"^(http(s)?:\/\/)?www\.facebook\.com\/",
                SortOrder = 1100
            },
            
            new TrafficRule()
            {
                Id = 1024,
                Name = "Twitter",
                RuleRegex = @"^(http(s)?:\/\/)?twitter\.com\/",
                SortOrder = 1110
            },
            new TrafficRule()
            {
                Id = 1025,
                Name = "Поиск Рамблер",
                RuleRegex = @"^(http(s)?:\/\/)?nova\.rambler\.ru\/",
                SortOrder = 1120
            },
            new TrafficRule()
            {
                Id = 1026,
                Name = "Поиск Нигма",
                RuleRegex = @"^(http(s)?:\/\/)?nigma\.ru\/",
                SortOrder = 1130
            },
            new TrafficRule()
            {
                Id = 1027,
                Name = "Поиск Bing",
                RuleRegex = @"^(http(s)?:\/\/)?bing\.com\/search",
                SortOrder = 1140
            },
            new TrafficRule()
            {
                Id = 1100,
                Name = "Прямой заход",
                RuleRegex = @"^$",
                SortOrder = 1200
            },

            new TrafficRule()
            {
                Id = 1110,
                Name = "Остальное",
                RuleRegex = @"",
                SortOrder = 100000
            },
        };


        public static List<TrafficRule> GetRules()
        {

            return DefaultRules;
        }

    }
}
