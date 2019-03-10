using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.Core.Caching;
using System.Xml.Serialization;
using AdvantShop.Helpers;

namespace AdvantShop.Design
{
    public enum eDesign
    {
        Theme,
        Color,
        Background
    }

    public static class DesignService
    {
        private const int DesignCacheTime = 60;

        private static readonly Dictionary<eDesign, string> TypeAndPath = new Dictionary<eDesign, string>()
        {
            {eDesign.Background, "backgrounds"},
            {eDesign.Theme, "themes"},
            {eDesign.Color, "colors"},
        };

        public static string TemplatePath
        {
            get
            {
                return SettingsDesign.Template != TemplateService.DefaultTemplateId
                    ? ("Templates/" + SettingsDesign.Template + "/")
                    : "";
            }
        }


        public static List<Theme> GetDesigns(eDesign design, bool withCache = true)
        {
            if (withCache)
            {
                var strCacheName = CacheNames.GetDesignCacheObjectName(design.ToString()) + SettingsDesign.Template;
                return CacheManager.Get(strCacheName, DesignCacheTime, () => GetDesignsFromConfig(design));
            }
            else
            {
                return GetDesignsFromConfig(design, true);
            }
        }

        private static List<Theme> GetDesignsFromConfig(eDesign design, bool fromDb = false)
        {
            var themes = new List<Theme>();
            var temp = new List<Theme>();

            var tmplPath = (!fromDb && SettingsDesign.Template != TemplateService.DefaultTemplateId) || (fromDb && SettingsDesign.TemplateInDb  != TemplateService.DefaultTemplateId)
                                    ? "Templates\\" + (fromDb ? SettingsDesign.TemplateInDb : SettingsDesign.Template) + "\\"
                                    : "";

            var designPath = SettingsGeneral.AbsolutePath + tmplPath + "design\\" + TypeAndPath[design];

            if (Directory.Exists(designPath))
            {
                foreach (var configPath in Directory.GetDirectories(designPath))
                {
                    var themeName = configPath.Split('\\').Last();
                    var themeConfig = configPath + "\\" + design.ToString() + ".config";
                    
                    try
                    {
                        Theme theme = null;

                        if (File.Exists(themeConfig))
                        {
                            using (var myReader = new StreamReader(themeConfig))
                            {
                                var mySerializer = new XmlSerializer(typeof (Theme));
                                theme = (Theme) mySerializer.Deserialize(myReader);

                                var themeTitle = theme.Names.Find(t => t.Lang == SettingsMain.Language);
                                theme.Title = themeTitle != null ? themeTitle.Value : themeName;
                                theme.Name = themeName;
                                theme.PreviewImage = theme.PreviewImage.IsNotEmpty() && File.Exists(configPath + "\\" + theme.PreviewImage)
                                    ? UrlService.GetAbsoluteLink(tmplPath + "design\\" + TypeAndPath[design] + "\\" + themeName + "\\" + theme.PreviewImage)
                                    : null;
                                myReader.Close();
                            }
                        }
                        else
                        {
                            theme = new Theme()
                            {
                                Title = themeName,
                                Name = themeName,
                            };
                        }

                        if (themeName != "_none")
                            temp.Add(theme);
                        else
                            themes.Add(theme);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                }
            }
            themes.AddRange(temp.OrderBy(t => t.Title));

            return themes;
        }

        public static string GetDesign(string type)
        {
            var template = SettingsDesign.Template != TemplateService.DefaultTemplateId
                                ? "templates/" + SettingsDesign.Template.ToLower() + "/"
                                : "";

            switch (type)
            {
                case "colorscheme":
                    return template + "design/colors/" + GetTypeDesign(type, SettingsDesign.ColorScheme);
                case "theme":
                    return template + "design/themes/" + GetTypeDesign(type, SettingsDesign.Theme);
                case "background":
                    return template + "design/backgrounds/" + GetTypeDesign(type, SettingsDesign.Background);
                default:
                    throw new Exception("Design type is undefined");
            }
        }

        private static string GetTypeDesign(string type, string currentType)
        {
            if (string.IsNullOrEmpty(currentType))
                currentType = "_none";

            if (!Demo.IsDemoEnabled)
                return currentType;

            var styleCss = CommonHelper.GetCookieString(type);
            if (string.IsNullOrEmpty(styleCss))
            {
                CommonHelper.SetCookie(type, currentType);
                return currentType;
            }

            return styleCss.ToLower();
        }

        //private static string GetThemesOnLine(eDesign designType)
        //{
        //    string responseFromServer = "";

        //    try
        //    {
        //        var requestUrl = string.Format("{0}?type={1}&lang={2}{3}", RequestBaseUrl, designType.ToString(), CultureInfo.CurrentCulture.TwoLetterISOLanguageName,
        //                                                                   (SettingsDesign.Template != TemplateService.DefaultTemplateId
        //                                                                                ? "&template=" + SettingsDesign.Template : ""));

        //        var request = WebRequest.Create(requestUrl);

        //        using (var response = request.GetResponse())
        //        using (var dataStream = response.GetResponseStream())
        //        {
        //            if (dataStream != null)
        //                using (var reader = new StreamReader(dataStream))
        //                {
        //                    responseFromServer = reader.ReadToEnd();
        //                }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.Log.Error(ex);
        //    }

        //    return responseFromServer;
        //}

        //public static List<Theme> GetAvaliableDesignsOnLine(eDesign designType)
        //{
        //    string response = GetThemesOnLine(designType);
        //    if (!string.IsNullOrEmpty(response))
        //    {
        //        return JsonConvert.DeserializeObject<List<Theme>>(response);
        //    }
        //    return null;
        //}
    }
}