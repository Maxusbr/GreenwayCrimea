//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

namespace AdvantShop.Configuration
{
    public class TemplateOptionSetting
    {
        public string Title { get; set; }
        public string Value { get; set; }
    }

    public class TemplateSetting
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Hidden { get; set; }

        public string DataType { get; set; }

        public string SectionName { get; set; }

        public List<TemplateOptionSetting> Options { get; set; }
    }


    public class TemplateSettingBox
    {
        public string Message { get; set; }
        public List<TemplateSetting> Settings { get; set; }
    }

    public class TemplateSettingSection
    {
        public string Name { get; set; }
        public List<TemplateSetting> Settings { get; set; }
    }
}