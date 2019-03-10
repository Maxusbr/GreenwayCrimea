<%@ WebHandler Language="C#" Class="AdvantShop.Admin.HttpHandlers.Design.ThemeTreeView" %>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.Security;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Design;
using AdvantShop.Helpers;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using Newtonsoft.Json;
using Resources;

namespace AdvantShop.Admin.HttpHandlers.Design
{
    [AuthorizeRole(RoleAction.Design)]
    public class ThemeTreeView : AdminHandler, IHttpHandler
    {
        private readonly string[] _imgExtensions = new string[] { ".jpg", ".png", ".jpeg", ".gif", ".bmp" };
        private string _baseUrl;
        private string _themeFolder;
        private string _themeFolderPath;
        
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            context.Response.ContentType = "application/json";
            
            var design = context.Request["design"];
            var theme = context.Request["theme"];
            var folder = context.Request["folder"];
            
            if (string.IsNullOrEmpty(design) || string.IsNullOrEmpty(theme))
                return;

            _baseUrl = UrlService.GetUrl();

            var designFolderPath = SettingsDesign.Template != TemplateService.DefaultTemplateId
                                            ? "templates/" + SettingsDesign.Template + "/"
                                            : "";

            _themeFolderPath = string.Format("{0}design/{1}s/{2}/{3}", designFolderPath, design, theme, folder);
            _themeFolder = HttpContext.Current.Server.MapPath("~/" +_themeFolderPath);
            
            switch (context.Request["action"])
            {
                case "getfolder":
                    GetFolder(context);
                    break;
                case "remove":
                    RemoveFile(context);
                    break;
                case "add":
                    AddFiles(context);
                    break;
            }
        }

        private void GetFolder(HttpContext context)
        {
            var folderPath = _themeFolder;

            var directoryInfo = new DirectoryInfo(folderPath);
            if (!directoryInfo.Exists)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { error = true }));
                return;
            }

            var files = new List<FileItemInfo>();

            foreach (var file in directoryInfo.GetFiles())
            {
                var fileInfo = new FileItemInfo()
                {
                    Name = file.Name,
                    Preview = IsImage(file)
                                ? Path.Combine(_baseUrl, _themeFolderPath, file.Name)
                                : null
                };

                files.Add(fileInfo);
            }

            context.Response.Write(JsonConvert.SerializeObject(files));
        }

        private void AddFiles(HttpContext context)
        {
            var filesCount = context.Request.Files.Count;
            if (filesCount < 1)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { error = true, msg = "" }));
                return;
            }

            var folderPath =  _themeFolder;

            var directoryInfo = new DirectoryInfo(folderPath);
            if (!directoryInfo.Exists)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { error = true, msg = "" }));
                return;
            }

            try
            {
                for (int i = 0; i < filesCount; i++)
                {
                    var file = context.Request.Files.Get(i);
                    if (file.ContentLength > 5000000 || !FileHelpers.CheckFileExtension(file.FileName, EAdvantShopFileTypes.Image))
                    {
                        context.Response.Write(JsonConvert.SerializeObject(new { error = true, msg = Resource.Admin_FileUploadError }));
                        return;
                    }

                    file.SaveAs(Path.Combine(folderPath, file.FileName));
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            context.Response.Write(JsonConvert.SerializeObject(new { error = false, msg = Resource.Admin_FileUploadSuccess }));
        }

        private void RemoveFile(HttpContext context)
        {
            var filePath = _themeFolder + "/" + context.Request["file"];

            if (!File.Exists(filePath))
            {
                context.Response.Write(JsonConvert.SerializeObject(new { error = true }));
                return;
            }

            FileHelpers.DeleteFile(filePath);

            context.Response.Write(
                JsonConvert.SerializeObject(
                    new {error = false, msg = string.Format(Resource.Admin_FileRemoved, context.Request["file"])}));
        }
        
        public class FileItemInfo
        {
            public string Name { get; set; }
            public string Preview { get; set; }
        }

        private bool IsImage(FileInfo fileInfo)
        {
            return _imgExtensions.Any(ext => Path.GetExtension(fileInfo.FullName) == ext);
        }
    }
}