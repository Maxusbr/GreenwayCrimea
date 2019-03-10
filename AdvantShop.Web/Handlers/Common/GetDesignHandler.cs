using System;
using System.Globalization;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Design;
using AdvantShop.Helpers;
using AdvantShop.Trial;

namespace AdvantShop.Handlers.Common
{
    public class GetDesignHandler
    {

        private object GetCurrentDesign()
        {
            var isDemoEnabled = Demo.IsDemoEnabled;
            return new
            {
                Background =
                    isDemoEnabled && CommonHelper.GetCookieString("background").IsNotEmpty()
                        ? CommonHelper.GetCookieString("background")
                        : SettingsDesign.Background,
                Theme =
                    isDemoEnabled && CommonHelper.GetCookieString("theme").IsNotEmpty()
                        ? CommonHelper.GetCookieString("theme")
                        : SettingsDesign.Theme,
                ColorScheme =
                    isDemoEnabled && CommonHelper.GetCookieString("colorscheme").IsNotEmpty()
                        ? CommonHelper.GetCookieString("colorscheme")
                        : SettingsDesign.ColorScheme,
                Structure =
                    isDemoEnabled && CommonHelper.GetCookieString("structure").IsNotEmpty()
                        ? CommonHelper.GetCookieString("structure")
                        : SettingsDesign.MainPageMode.ToString(),
                Template =
                    SettingsDesign.Template != TemplateService.DefaultTemplateId
                        ? SettingsDesign.Template
                        : string.Empty
            };
        }

        public object Get()
        {
            object obj = new
            {
                Backgrounds = DesignService.GetDesigns(eDesign.Background),
                Themes = DesignService.GetDesigns(eDesign.Theme),
                Colors = DesignService.GetDesigns(eDesign.Color),
                Structures = Enum.GetNames(typeof(SettingsDesign.eMainPageMode)),
                DesignCurrent = GetCurrentDesign(),
                isTrial = TrialService.IsTrialEnabled && CommonHelper.GetCookieString("isTrialService").IsNullOrEmpty(),
                isTemplate = SettingsDesign.Template != TemplateService.DefaultTemplateId
            };

            CommonHelper.SetCookie("isTrialService", TrialService.IsTrialEnabled.ToString(CultureInfo.InvariantCulture));

            return obj;
        }
    }
}