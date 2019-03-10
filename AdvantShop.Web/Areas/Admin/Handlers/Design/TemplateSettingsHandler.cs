using System.Collections.Generic;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Web.Admin.ViewModels.Design;


namespace AdvantShop.Web.Admin.Handlers.Design
{
    public class TemplateSettingsHandler
    {

        public TemplateSettingsModel Execute()
        {
            var settings = TemplateSettingsProvider.GetTemplateSettingsBox();
            if (settings != null)
            {
                var sections = new List<TemplateSettingSection>();

                foreach (var setting in settings.Settings)
                {
                    var section = sections.Find(x => x.Name == setting.SectionName);
                    if (section == null)
                    {
                        sections.Add(new TemplateSettingSection()
                        {
                            Name = setting.SectionName,
                            Settings = new List<TemplateSetting>() {setting}
                        });
                    }
                    else
                    {
                        section.Settings.Add(setting);
                    }
                }

                return new TemplateSettingsModel() {Sections = sections};
            }

            return null;
        }

        public bool SaveSettings(string settings)
        {
            foreach (var setting in settings.Split(','))
            {
                var settingArr = setting.Split('~');

                if (settingArr.Length == 3)
                {
                    if (!ValidateSettingByType(settingArr[1], settingArr[2]) || !TemplateSettingsProvider.SetSettingValue(settingArr[0], settingArr[1]))
                    {
                        return false;
                    }
                }
            }

            CacheManager.Clean();
            return true;
        }

        private bool ValidateSettingByType(string settingValue, string type)
        {

            if (string.Equals(type, "int"))
            {
                var intValue = 0;
                return int.TryParse(settingValue, out intValue);
            }
            else if (string.Equals(type, "string"))
            {
                return !string.IsNullOrEmpty(settingValue);
            }
            else if (string.Equals(type, "bool"))
            {
                return true;
            }
            return false;

        }


    }
}
