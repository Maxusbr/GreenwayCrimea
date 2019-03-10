using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Helpers;
using AdvantShop.Trial;

namespace AdvantShop.Handlers.Common
{
    public class SaveDesignHandler
    {
        public void Save(string theme, string colorscheme, string structure, string background)
        {
            if (Demo.IsDemoEnabled)
            {
                CommonHelper.SetCookie("theme", theme);
                CommonHelper.SetCookie("background", theme);
                CommonHelper.SetCookie("colorscheme", colorscheme);
                CommonHelper.SetCookie("structure", structure);
            }
            else
            {
                SettingsDesign.Theme = theme;
                SettingsDesign.Background = background;
                SettingsDesign.ColorScheme = colorscheme;
                SettingsDesign.MainPageMode = (SettingsDesign.eMainPageMode)Enum.Parse(typeof(SettingsDesign.eMainPageMode), structure);
            }

            TrialService.TrackEvent(TrialEvents.ChangeColorScheme, colorscheme);
            TrialService.TrackEvent(TrialEvents.ChangeBackGround, background);
            TrialService.TrackEvent(TrialEvents.ChangeTheme, theme);
            TrialService.TrackEvent(TrialEvents.ChangeMainPageMode, structure);

            TrialService.TrackEvent(ETrackEvent.Trial_ChangeDesignTransformer);

            if (SettingsDesign.MainPageMode == SettingsDesign.eMainPageMode.Default)
            {
                SettingsDesign.CountMainPageProductInLine = 4;
                SettingsDesign.CountMainPageProductInSection = 4;
            }
            if(SettingsDesign.MainPageMode == SettingsDesign.eMainPageMode.TwoColumns)
            {
                SettingsDesign.CountMainPageProductInLine = 3;
                SettingsDesign.CountMainPageProductInSection = 3;
            }


            CacheManager.Clean();
        }

    }
}