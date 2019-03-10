using System;
using System.IO;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Mobiles
{
    public class LoadSaveMobileSettingsHandler
    {
        private readonly MobileVersionSettingsModel _model;
        private readonly string _filename = SettingsGeneral.AbsolutePath + "/areas/mobile/robots.txt";

        public LoadSaveMobileSettingsHandler(MobileVersionSettingsModel model)
        {
            _model = model;
        }

        public MobileVersionSettingsModel Load()
        {
            var model = new MobileVersionSettingsModel
            {
                Enabled = SettingsMobile.IsMobileTemplateActive,
                MainPageProductCountMobile = SettingsMobile.MainPageProductsCount.ToString(),
                ShowCity = SettingsMobile.DisplayCity,
                ShowSlider = SettingsMobile.DisplaySlider,
                DisplayHeaderTitle = SettingsMobile.DisplayHeaderTitle,
                HeaderCustomTitle = SettingsMobile.HeaderCustomTitle,
                IsFullCheckout = SettingsMobile.IsFullCheckout,
                RedirectToSubdomain = SettingsMobile.RedirectToSubdomain,
                MobilePhone = SettingsMain.MobilePhone
            };
            
            if (!File.Exists(_filename))
                File.Create(_filename);

            using (var sr = new StreamReader(_filename))
                model.Robots = sr.ReadToEnd();

            return model;
        }

        public void Save()
        {
            SettingsMobile.IsMobileTemplateActive = _model.Enabled;
            SettingsMobile.MainPageProductsCount = Convert.ToInt32(_model.MainPageProductCountMobile);
            SettingsMobile.DisplayCity = _model.ShowCity;
            SettingsMobile.DisplaySlider = _model.ShowSlider;
            SettingsMobile.DisplayHeaderTitle = _model.DisplayHeaderTitle;
            SettingsMobile.HeaderCustomTitle = _model.HeaderCustomTitle == null ? string.Empty : _model.HeaderCustomTitle;
            SettingsMobile.IsFullCheckout = _model.IsFullCheckout;
            SettingsMobile.RedirectToSubdomain = _model.RedirectToSubdomain;
            SettingsMain.MobilePhone = _model.MobilePhone;

            using (var wr = new StreamWriter(_filename))
                wr.Write(_model.Robots ?? string.Empty);
        }
    }
}
