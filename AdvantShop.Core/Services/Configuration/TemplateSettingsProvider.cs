//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Services.CMS;
using AdvantShop.Design;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;

namespace AdvantShop.Configuration
{
    public class TemplateSettingsProvider
    {
        private const string TemplateFileConfigName = "template.config";

        public sealed class TemplateSettingIndexer
        {
            public string this[string name]
            {
                get { return GetSettingValue(name); }
                set { SetSettingValue(name, value); }
            }
        }

        private static TemplateSettingIndexer _staticIndexer;
        public static TemplateSettingIndexer Items
        {
            get { return _staticIndexer ?? (_staticIndexer = new TemplateSettingIndexer()); }
        }

        #region Get/Set settings value

        public static string GetSettingValue(string key, string template = null)
        {
            var settings = GetAllTemplateSettings(template ?? SettingsDesign.Template);

            string value = null;

            if (settings != null && settings.TryGetValue(key, out value))
                return value;

            return null;
        }


        public static bool SetSettingValue(string name, string value, string template = null)
        {
            var tpl = template ?? SettingsDesign.Template;

            SQLDataAccess.ExecuteNonQuery(
                "IF (SELECT COUNT(*) FROM [Settings].[TemplateSettings] WHERE [Template] = @Template and [Name] = @Name) = 0" +
                    "BEGIN " +
                        "INSERT INTO [Settings].[TemplateSettings] ([Template], [Name], [Value]) VALUES (@Template, @Name, @Value) " +
                    "END " +
                "ELSE " +
                    "BEGIN " +
                        "UPDATE [Settings].[TemplateSettings] SET [Value] = @Value WHERE [Template] = @Template and [Name] = @Name " +
                    "END  ",
                CommandType.Text,
                new SqlParameter("@Template", tpl),
                new SqlParameter("@Name", name),
                new SqlParameter("@Value", value));

            CacheManager.RemoveByPattern(CacheNames.GetTemplateSettings(tpl));

            return true;
        }

        #endregion

        #region Get/Set settings service

        public static Dictionary<string, string> GetAllTemplateSettings(string template)
        {
            var settings = CacheManager.Get(CacheNames.GetTemplateSettings(template), 60,
                () =>
                    SQLDataAccess.ExecuteReadDictionary<string, string>(
                        "SELECT [Name],[Value] FROM [Settings].[TemplateSettings] Where Template = @Template",
                        CommandType.Text, "Name", "Value",
                        new SqlParameter("@Template", template)));

            return settings;
        }


        /// <summary>
        /// Get localized template settings
        /// </summary>
        /// <returns></returns>
        public static TemplateSettingBox GetTemplateSettingsBox()
        {
            var settingsBox = new TemplateSettingBox() { Settings = new List<TemplateSetting>() };


            var templateConfigFile = SettingsGeneral.AbsolutePath
                                            + (SettingsDesign.Template != TemplateService.DefaultTemplateId ? ("templates\\" + SettingsDesign.Template + "\\") : "App_Data\\")
                                            + TemplateFileConfigName;

            if (!File.Exists(templateConfigFile))
            {
                settingsBox.Message = LocalizationService.GetResource("Core.Configuration.TemplateSettings.ConfigNotExist");
                return settingsBox;
            }

            var allSettings = GetAllTemplateSettings(SettingsDesign.Template) ?? new Dictionary<string, string>();

            try
            {
                var settings = settingsBox.Settings;

                var doc = XDocument.Load(templateConfigFile);

                foreach (var elSection in doc.Root.Elements("SettingSection"))
                {
                    string sectionName = LocalizationService.GetResource("Core.Configuration.TemplateSettings_" + elSection.Attribute("Title").Value).Default(elSection.Attribute("Title").Value);

                    if (elSection.Attribute("Hidden") != null && Convert.ToBoolean(elSection.Attribute("Hidden").Value))
                        continue;

                    foreach (var elSetting in elSection.Elements())
                    {
                        var name = elSetting.Attribute("Name").Value;
                        var value = allSettings.ContainsKey(name)
                            ? allSettings[name]
                            : elSetting.Element("Value") != null ? elSetting.Element("Value").Value : "";

                        var setting = new TemplateSetting
                        {
                            Name = name,
                            Value = value,
                            Type = elSetting.Attribute("Type").Value,
                            SectionName = sectionName,
                            DataType = elSetting.Attribute("DataType") != null ? elSetting.Attribute("DataType").Value : "string",
                        };

                        setting.Title = elSetting.Attribute("Title") != null
                                                ? LocalizationService.GetResource("Core.Configuration.TemplateSettings_" + elSetting.Attribute("Title").Value).Default(elSetting.Attribute("Title").Value)
                                                : LocalizationService.GetResource("Core.Configuration.TemplateSettings_" + setting.Name).Default(setting.Name);

                        if (elSetting.Attribute("Name") != null)
                        {
                            var resourceKey = "Core.Configuration.TemplateSettings_" + elSetting.Attribute("Name").Value +
                                              "_Description";

                            var localizedString = LocalizationService.GetResource(resourceKey);

                            setting.Description = localizedString.ToLower() != resourceKey.ToLower()
                                                    ? localizedString
                                                    : string.Empty;
                        }

                        var options = new List<TemplateOptionSetting>();
                        foreach (var elOption in elSetting.Elements("option"))
                        {
                            string title = elOption.Attribute("Title").Value;

                            options.Add(new TemplateOptionSetting
                            {
                                Title = LocalizationService.GetResource("Core.Configuration.TemplateSettings_" + title).Default(title),
                                Value = elOption.Attribute("Value").Value
                            });
                        }
                        setting.Options = options;
                        settings.Add(setting);
                    }
                }
            }
            catch (Exception ex)
            {
                settingsBox.Message = LocalizationService.GetResource("Core.Configuration.TemplateSettings.ErrorReadConfig");
                Debug.Log.Error(ex);
            }

            return settingsBox;
        }

        /// <summary>
        /// Set template settings from config if not exist
        /// </summary>
        /// <param name="template">Template name</param>
        /// <param name="setDefault">Rewrite setting value</param>
        /// <returns></returns>
        public static TemplateSettingBox SetTemplateSettings(string template = null, bool setDefault = false)
        {
            if (template == null)
                template = SettingsDesign.Template;

            var settingsBox = new TemplateSettingBox();

            var templateConfigFile = SettingsGeneral.AbsolutePath
                                     + (template != TemplateService.DefaultTemplateId
                                         ? "templates\\" + template + "\\"
                                         : "App_Data\\")
                                     + TemplateFileConfigName;

            if (!File.Exists(templateConfigFile))
            {
                settingsBox.Message = LocalizationService.GetResource("Core.Configuration.TemplateSettings.ConfigNotExist");
                return settingsBox;
            }

            try
            {
                var doc = XDocument.Load(templateConfigFile);

                foreach (var elSection in doc.Root.Elements("SettingSection"))
                {
                    foreach (var elSetting in elSection.Elements())
                    {
                        var name = elSetting.Attribute("Name").Value;
                        var value = elSetting.Element("Value") != null ? elSetting.Element("Value").Value : "";
                        
                        if (setDefault || GetSettingValue(name, template).IsNullOrEmpty())
                            SetSettingValue(name, value, template);
                    }
                }

                InstallStaticBlocks(doc);

                InstallLogo(doc, template);
                InstallCarouselSlides(doc, template);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                settingsBox.Message = LocalizationService.GetResource("Core.Configuration.TemplateSettings.ErrorReadConfig");
            }

            return settingsBox;
        }

        private static void InstallStaticBlocks(XDocument doc)
        {
            var sbBlock = doc.Root.Element("StaticBlocks");
            if (sbBlock == null)
                return;

            foreach (var block in sbBlock.Elements("StaticBlock"))
            {
                var key = block.Attribute("Key");
                var name = block.Attribute("Name");

                if (key == null || name == null || 
                    string.IsNullOrWhiteSpace(key.Value) || string.IsNullOrWhiteSpace(name.Value) ||
                    StaticBlockService.GetPagePartByKey(key.Value) != null)
                {
                    continue;
                }

                var sb = new StaticBlock()
                {
                    Key = key.Value,
                    InnerName = name.Value,
                    Added = DateTime.Now,
                    Modified = DateTime.Now,
                    Content = block.Value ?? ""
                };

                var enabled = block.Attribute("Enabled");
                sb.Enabled = enabled == null || enabled.Value.TryParseBool();

                StaticBlockService.AddStaticBlock(sb);
            }
        }

        private static void InstallLogo(XDocument doc, string template)
        {
            var isPreview = SettingsDesign.PreviewTemplate != null;

            if (!SettingsMain.IsDefaultLogo && !string.IsNullOrWhiteSpace(SettingsMain.LogoImageName) && !isPreview)
                return;
            
            try
            {
                var logoEl = doc.Root.Element("LogoPicture");
                if (logoEl == null)
                    return;

                var pathAttr = logoEl.Attribute("Path");
                if (pathAttr == null || string.IsNullOrWhiteSpace(pathAttr.Value))
                    return;

                var logoPath = SettingsGeneral.AbsolutePath + "templates\\" + template + "\\" + pathAttr.Value;

                if (!File.Exists(logoPath))
                    return;

                SettingsDesign.DefaultLogo = "templates/" + template + "/" + pathAttr.Value;

                if (isPreview)
                    return;

                FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsMain.LogoImageName));

                var logoName = pathAttr.Value.Split(new char[] {'\\', '/'}).LastOrDefault();

                var newName = logoName.FileNamePlusDate("logo");
                File.Copy(logoPath, FoldersHelper.GetPathAbsolut(FolderType.Pictures, newName));

                SettingsMain.LogoImageName = newName;
                SettingsMain.IsDefaultLogo = true;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        private static void InstallCarouselSlides(XDocument doc, string template)
        {
            var isPreview = SettingsDesign.PreviewTemplate != null;
            var allSlides = CarouselService.GetAllCarousels();

            if (!SettingsDesign.IsDefaultSlides && allSlides.Count > 0 && !isPreview)
                return;

            try
            {
                var carouselEl = doc.Root.Element("Carousel");
                if (carouselEl == null)
                    return;
                
                var slides = new List<string>();

                foreach (var slideEl in carouselEl.Elements("Slide"))
                {
                    var path = slideEl.Attribute("Path");
                    if (path == null || string.IsNullOrEmpty(path.Value))
                        continue;
                    
                    slides.Add(path.Value);
                }

                SettingsDesign.DefaultSlides = String.Join(";", slides);

                if (isPreview || slides.Count == 0)
                    return;

                var maxSortOrder = Int32.MinValue;

                foreach (var slide in allSlides)
                {
                    if (slide.Picture != null && slides.Contains(slide.Picture.PhotoName))
                    {
                        slide.Enabled = true;
                        slides.Remove(slide.Picture.PhotoName);
                    }
                    else
                        slide.Enabled = false;

                    CarouselService.UpdateCarousel(slide);

                    if (maxSortOrder < slide.SortOrder)
                        maxSortOrder = slide.SortOrder;
                }

                for(var i = 0; i < slides.Count; i++)
                {
                    var carousel = new Carousel()
                    {
                        Url = "/",
                        DisplayInMobile = true,
                        DisplayInOneColumn = true,
                        DisplayInTwoColumns = true,
                        Enabled = true,
                        SortOrder = maxSortOrder + i*10
                    };
                    CarouselService.AddCarousel(carousel);

                    PhotoService.AddPhotoWithOrignName(new Photo(0, carousel.CarouselId, PhotoType.Carousel){PhotoName = slides[i]});
                }

                SettingsDesign.IsDefaultSlides = true;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        #endregion
    }
}