using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using Newtonsoft.Json;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.DownloadableContent;
using AdvantShop.Core.Caching;

namespace AdvantShop.Design
{
    public class TemplateService
    {
        public const string DefaultTemplateId = "_default";

        private const string RequestUrlGetTemplates = "http://modules.advantshop.net/DownloadableContent/GetDlcs?id={0}&dlctype=Template&storeversion={1}";
        private const string RequestUrlGetTemplateArchive = "http://modules.advantshop.net/DownloadableContent/GetDlc?lickey={0}&dlcId={1}&storeversion={2}&forpreview={3}";

        public static DownloadableContentBox GetTemplates()
        {
            var templatesFromServer = GetTemplatesFromRemoteServer();

            templatesFromServer.Items.Insert(0, new DownloadableContentObject()
            {
                StringId = DefaultTemplateId,
                Name = LocalizationService.GetResource("Core.Design.Template.DefaultTemplate"),
                IsInstall = true,
                Active = true,
                Icon = "../images/design/preview.jpg"
            });

            if (Directory.Exists(SettingsGeneral.AbsolutePath + "Templates"))
            {
                foreach (var templateFolder in Directory.GetDirectories(SettingsGeneral.AbsolutePath + "Templates"))
                {
                    if (!File.Exists(templateFolder + "\\template.config"))
                        continue;

                    var stringId = templateFolder.Split('\\').Last();
                    var curTemplate = templatesFromServer.Items.Find(t => t.StringId.ToLower() == stringId.ToLower());

                    if (curTemplate != null)
                    {
                        var templateFromDb = DownloadableContentService.GetOne(stringId);

                        curTemplate.IsInstall = templateFromDb != null ? templateFromDb.IsInstall : false;
                        //curTemplate.Active = true; // templateFromDb != null ? templateFromDb.Active : false;
                        curTemplate.CurrentVersion = templateFromDb != null ? templateFromDb.Version : LocalizationService.GetResource("Core.Design.Template.InDebug");
                    }
                    else
                    {
                        templatesFromServer.Items.Add(new DownloadableContentObject
                        {
                            StringId = stringId,
                            Name = stringId,
                            IsInstall = DownloadableContentService.IsInstall(stringId),
                            Icon = string.Format("../Templates/{0}/preview.jpg", stringId),
                            Active = true,
                            Price = 0,
                            Version = LocalizationService.GetResource("Core.Design.Template.InDebug")
                        });
                    }
                }
            }

            templatesFromServer.Items = templatesFromServer.Items.OrderByDescending(t => t.IsInstall).ToList();

            var resultTemplateBox = new DownloadableContentBox()
            {
                Message = templatesFromServer.Message,
                Items = new List<DownloadableContentObject>()
            };

            resultTemplateBox.Items.Add(templatesFromServer.Items.FirstOrDefault(t => t.StringId == SettingsDesign.Template));
            resultTemplateBox.Items.AddRange(templatesFromServer.Items.Where(t => t.StringId != SettingsDesign.Template));
            

            return resultTemplateBox;
        }

        private static DownloadableContentBox GetTemplatesFromRemoteServer()
        {
            var templateBox = new DownloadableContentBox() { Items = new List<DownloadableContentObject>() };

            try
            {
                var request = WebRequest.Create(string.Format(RequestUrlGetTemplates, SettingsLic.LicKey, SettingsGeneral.SiteVersionDev));
                request.Method = "GET";

                using (var dataStream = request.GetResponse().GetResponseStream())
                using (var reader = new StreamReader(dataStream))
                {
                    var responseFromServer = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(responseFromServer))
                    {
                        templateBox = JsonConvert.DeserializeObject<DownloadableContentBox>(responseFromServer);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return templateBox;
        }

        public static bool IsExistTemplate(string templateId)
        {
            if (SettingsDesign.Template == DefaultTemplateId)
                return true;
            return Directory.Exists(SettingsGeneral.AbsolutePath + "Templates\\" + templateId + "\\template.config");
        }
        
        public static bool UninstallTemplate(string stringId)
        {
            if (stringId == DefaultTemplateId)
                return false;

            try
            {
                if (SettingsDesign.Template == stringId)
                    SettingsDesign.ChangeTemplate(DefaultTemplateId);

                FileHelpers.DeleteDirectory(HttpContext.Current.Server.MapPath("~/Templates/" + stringId));
                DownloadableContentService.Uninstall(stringId, "template");
                CacheManager.Clean();
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return !DownloadableContentService.IsInstall(stringId);
        }
        
        public static bool InstallTemplate(int id, string stringid, bool isPreview)
        {
            if (stringid == DefaultTemplateId)
                return true;

            var templateConfig = HttpContext.Current.Server.MapPath("~/Templates/" + stringid + "/template.config");

            if (File.Exists(templateConfig))
            {
                DownloadableContentService.Install(
                    new DownloadableContentObject
                    {
                        StringId = stringid,
                        Version = "В режиме отладки",
                        IsInstall = true,
                        DcType = "template",
                        Active = !isPreview
                    });
                CacheManager.Clean();
                return DownloadableContentService.IsInstall(stringid);
            }

            var message = GetTemplateArchiveFromRemoteServer(id, isPreview);
            if (string.IsNullOrEmpty(message))
            {
                var templatesFromServer = GetTemplatesFromRemoteServer();
                var templateInfoFromServer = new DownloadableContentObject();
                
                if (templatesFromServer != null &&
                    templatesFromServer.Items.Count > 0 &&
                    (templateInfoFromServer = templatesFromServer.Items.Find(t => t.StringId == stringid)) != null)
                {
                    DownloadableContentService.Install(
                        new DownloadableContentObject
                        {
                            Version = templateInfoFromServer.Version,
                            StringId = templateInfoFromServer.StringId,
                            IsInstall = true,
                            DcType = "template",
                            Active = templateInfoFromServer.Active
                        });
                    CacheManager.Clean();

                    return DownloadableContentService.IsInstall(templateInfoFromServer.StringId);
                }

            }
            return DownloadableContentService.IsInstall(stringid);
        }

        public static bool InstallLastTemplate(int id)
        {
            var template = DownloadableContentService.GetOne(id);
            if (template != null && !UninstallTemplate(template.StringId))
                return false;

            return InstallTemplate(id, null, false);
        }

        public static bool InstallLastTemplate(int id, string stringId)
        {
            var template = DownloadableContentService.GetOne(stringId);
            if (template != null && !UninstallTemplate(template.StringId))
                return false;

            return InstallTemplate(id, stringId, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        private static string GetTemplateArchiveFromRemoteServer(int templateId, bool preview)
        {
            var zipFileName = HttpContext.Current.Server.MapPath("~/App_Data/" + Guid.NewGuid() + ".Zip");
            try
            {
                using (var wc = new WebClient())
                {
                    wc.DownloadFile(string.Format(RequestUrlGetTemplateArchive, SettingsLic.LicKey, templateId, SettingsGeneral.SiteVersionDev, preview),
                                                    zipFileName);
                }

                if (!FileHelpers.UnZipFile(zipFileName, HttpContext.Current.Server.MapPath("~/Templates/")))
                {
                    if (File.Exists(zipFileName))
                        File.Delete(zipFileName);

                    return "error on UnZipFile";
                }

                FileHelpers.DeleteFile(zipFileName);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return "error on download or unzip";
            }

            return string.Empty;
        }
    }
}