//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Module.Watermark
{
    public class Watermark : IProcessPhoto
    {
        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Водяные знаки";

                    case "en":
                        return "Watermarks";

                    default:
                        return "Watermarks";
                }
            }
        }

        public string ModuleStringId
        {
            get { return "Watermark"; }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> {new WatermarkSetting()}; }
        }

        public bool HasSettings
        {
            get { return true; }
        }

        public Image DoProcessPhoto(Image photo)
        {
            string fileName = ModuleSettingsProvider.GetAbsolutePath() + "Modules/" + ModuleStringId + "/" +
                              ModuleSettingsProvider.GetSettingValue<string>("WatermarkImage", ModuleStringId);
            if (!File.Exists(fileName))
            {
                return photo;
            }

            using (Stream streamOpen = new FileStream(fileName, FileMode.Open))
            {
                using (Image watermarkImage = Image.FromStream(streamOpen))
                {
                    using (Graphics graphics = Graphics.FromImage(photo))
                    {
                        int posX = Convert.ToInt32((photo.Width * ModuleSettingsProvider.GetSettingValue<decimal>("WatermarkPositionX", ModuleStringId) / 100) -
                            (watermarkImage.Width * ModuleSettingsProvider.GetSettingValue<decimal>("WatermarkPositionX", ModuleStringId) / 100));
                        int posY = Convert.ToInt32((photo.Height * ModuleSettingsProvider.GetSettingValue<decimal>("WatermarkPositionY", ModuleStringId) / 100) -
                            (watermarkImage.Height * ModuleSettingsProvider.GetSettingValue<decimal>("WatermarkPositionY", ModuleStringId) / 100));
                        float? width = null, height = null;
                        if (photo.Width < watermarkImage.Width || photo.Height < watermarkImage.Height)
                        {
                            if ((watermarkImage.Width - photo.Width) < (watermarkImage.Height - photo.Height))
                            {
                                float percent = photo.Height / (watermarkImage.Height / 100f);
                                height = watermarkImage.Height * (percent / 100f);
                                width = watermarkImage.Width * (percent / 100f);
                            }
                            else
                            {
                                float percent = photo.Width / (watermarkImage.Width / 100f);
                                height = watermarkImage.Height * (percent / 100f);
                                width = watermarkImage.Width * (percent / 100f);
                            }
                        }
                        graphics.DrawImage(watermarkImage, posX, posY,
                             width != null ? width.Value : watermarkImage.Width,
                             height != null ? height.Value : watermarkImage.Height);
                    }
                }
                streamOpen.Close();
            }

            return photo;
        }

        public bool CheckAlive()
        {
            return ModulesRepository.IsInstallModule(ModuleStringId);
        }

        public bool InstallModule()
        {
            ModuleSettingsProvider.SetSettingValue("WatermarkPositionX", 0m, ModuleStringId);
            ModuleSettingsProvider.SetSettingValue("WatermarkPositionY", 0m, ModuleStringId);
            return true;
        }

        public bool UninstallModule()
        {
            ModuleSettingsProvider.RemoveSqlSetting("WatermarkPositionX", ModuleStringId);
            ModuleSettingsProvider.RemoveSqlSetting("WatermarkPositionY", ModuleStringId);
            return true;
        }

        public bool UpdateModule()
        {
            return true;
        }

        private class WatermarkSetting : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Настройки";

                        case "en":
                            return "Settings";

                        default:
                            return "Settings";
                    }
                }
            }

            public string File
            {
                get { return "WatermarkModule.ascx"; }
            }

            #endregion
        }
    }
}