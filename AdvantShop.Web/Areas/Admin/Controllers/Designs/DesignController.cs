using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Design;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Statistic;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Design;
using AdvantShop.Web.Admin.Models.Designs;
using AdvantShop.Web.Infrastructure.ActionResults;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Designs
{
    [Auth(RoleAction.Design)]
    public partial class DesignController : BaseAdminController
    {
        public ActionResult Index()
        {
            DesignHandler handler = new DesignHandler(Request["stringid"]);
            var model = handler.Execute();

            SetMetaInformation(T("Admin.Design.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.DesignCtrl);

            return View(model);
        }

        public ActionResult ApplyTemplate(string templateId)
        {
            if (templateId.IsNullOrEmpty())
            {
                ShowMessage(Core.Controls.NotifyType.Error, "Ошибка применения шаблона");
            }
            else
            {
                SettingsDesign.ChangeTemplate(templateId);
                CacheManager.Clean();
                TrialService.TrackEvent(ETrackEvent.Trial_ApplyDesignTemplate);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult PreviewTemplate(int id, string previewTemplateId)
        {
            var installTemplate = TemplateService.InstallTemplate(id, previewTemplateId, true);

            if (!installTemplate)
            {
                return Json(false);
            }

            SettingsDesign.PreviewTemplate = previewTemplateId;
            CacheManager.Clean();

            TrialService.TrackEvent(ETrackEvent.Trial_PreviewDesignTemplate);

            return Json(true);
        }

        public ActionResult CancelPreviewTemplate()
        {
            SettingsDesign.PreviewTemplate = null;
            CacheManager.Clean();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetDesigns(string stringId = null)
        {
            var handler = new DesignHandler(stringId);
            var model = handler.Execute();

            var extModel = new
            {
                Themes = model.Themes,
                BackGrounds = model.BackGrounds,
                ColorSchemes = model.ColorSchemes,
                DesignCurrent = new
                {
                    model.CurrentTheme,
                    model.CurrentBackGround,
                    model.CurrentColorScheme
                }
            };

            return Json(extModel);
        }


        [HttpPost]
        public JsonResult SaveDesign(eDesign designType, string name)
        {
            var handler = new DesignHandler();
            return Json(handler.SaveDesign(designType, name));
        }

        [HttpPost]
        public JsonResult UploadDesign(eDesign designType, HttpPostedFileBase file)
        {
            var handler = new DesignHandler();
            return Json(handler.UploadDesignFile(designType, file));
        }

        [HttpPost]
        public JsonResult DeleteDesign(eDesign designType, string name)
        {
            var handler = new DesignHandler();
            return Json(handler.DeleteDesign(designType, name));
        }


        public ActionResult CssEditor()
        {
            var handler = new CssEditorHandler();
            string css = handler.GetFileContent();

            SetMetaInformation(T("Admin.Design.CssEditor.Title"));
            SetNgController(NgControllers.NgControllersTypes.CssEditorCtrl);

            return View((object)css);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult CssEditor(string value)
        {
            var result = new CssEditorHandler().SaveFileContent(value);

            return Json(new { result = result });
        }


        public JsonResult TemplateSettings()
        {
            var handler = new TemplateSettingsHandler();
            var model = handler.Execute();

            return Json(model);
        }

        [HttpPost]
        public JsonResult SaveTemplateSettings(string settings)
        {
            var handler = new TemplateSettingsHandler();
            var model = handler.SaveSettings(settings);

            return Json(model);
        }

        [HttpPost]
        public JsonResult ResizePictures()
        {

            if (CommonStatistic.IsRun) return Json(new CommandResult() { Result = false, Error = "Не возможно пережать фотографии, выполняется другой процесс" });

            CommonStatistic.Init();
            CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
            CommonStatistic.CurrentProcessName = Request.Url.PathAndQuery;
            CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
            try
            {
                CommonStatistic.TotalRow = PhotoService.GetCountPhotos(0, PhotoType.Product);
                CommonStatistic.StartNew(Helpers.FileHelpers.ResizeAllProductPhotos);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return Json(new CommandResult() { Result = true });
        }


        #region Install, update, uninstall, enable module

        [HttpPost]
        public ActionResult InstallTemplate(string stringId, int id, string version)
        {
            if (string.IsNullOrWhiteSpace(stringId) ||                
                string.IsNullOrWhiteSpace(version))
            {
                ShowMessage(Core.Controls.NotifyType.Error, "Шаблон не найден");

            }
            else if (!TemplateService.InstallTemplate(id, stringId, false))
            {
                ShowMessage(Core.Controls.NotifyType.Error, "Ошибка установки шаблона");
            }
            else
            {
                ShowMessage(Core.Controls.NotifyType.Success, "Шаблон установлен");
            }

            return Redirect("Index#?tabsDesignTemplates=1");
        }

        [HttpPost]
        public ActionResult UpdateTemplate(int id, string stringId)
        {
            if (!TemplateService.InstallLastTemplate(id, stringId))
            {
                ShowMessage(Core.Controls.NotifyType.Error, "Ошибка обновления шаблона");
            }
            else
            {
                ShowMessage(Core.Controls.NotifyType.Success, "Шаблон обновлен");
            }
            return Redirect("Index#?tabsDesignTemplates=1");
        }

        [HttpPost]
        public ActionResult DeleteTemplate(string stringId)
        {
            if (!TemplateService.UninstallTemplate(stringId))
            {
                ShowMessage(Core.Controls.NotifyType.Error, "Ошибка удаления шаблона");
            }
            else
            {
                ShowMessage(Core.Controls.NotifyType.Success, "Шаблон удален");
            }
            return Redirect("Index#?tabsDesignTemplates=1");
        }

        #endregion

        #region Edit theme

        public ActionResult Theme(string theme, eDesign design)
        {
            var themes = DesignService.GetDesigns(design);

            var selectedTheme = themes.Find(x => x.Name.ToLower() == theme.ToLower());
            if (selectedTheme == null)
                return RedirectToAction("Index");

            var model = new GetTheme(selectedTheme, design).Execute();

            SetMetaInformation(T("Admin.Design.Theme.Title"));
            SetNgController(NgControllers.NgControllersTypes.DesignCtrl);

            return View(model);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveTheme(string theme, eDesign design, string themeCss)
        {
            var themes = DesignService.GetDesigns(design);

            var selectedTheme = themes.Find(x => x.Name.ToLower() == theme.ToLower());
            if (selectedTheme == null)
                return JsonError("Theme not exist");

            new SaveTheme(selectedTheme, design, themeCss).Execute();

            return JsonOk();
        }

        [HttpPost]
        public JsonResult ThemeFiles(ThemeFilesModel model)
        {
            var themes = DesignService.GetDesigns(model.Design);

            var selectedTheme = themes.Find(x => x.Name.ToLower() == model.Theme.ToLower());
            if (selectedTheme == null)
                return JsonError("Theme not exist");

            var designFolderPath = SettingsDesign.Template != TemplateService.DefaultTemplateId
                                            ? "Templates/" + SettingsDesign.Template + "/"
                                            : "";

            var themeFolderPath = string.Format("{0}design/{1}s/{2}/images", designFolderPath, model.Design, selectedTheme.Name);
            var themeFolder = HostingEnvironment.MapPath("~/" + themeFolderPath);

            switch (model.Action)
            {
                case "getfiles":
                    var dir = new DirectoryInfo(themeFolder);
                    if (!dir.Exists)
                        FileHelpers.CreateDirectory(themeFolder);

                    var files =
                        dir.GetFiles().Select(x =>
                            new
                            {
                                x.Name,
                                Preview = 
                                    new [] { ".jpg", ".png", ".jpeg", ".gif", ".bmp" }.Any(ext => Path.GetExtension(x.FullName) == ext)
                                        ? Path.Combine(UrlService.GetUrl(), themeFolderPath, x.Name)
                                        : null
                            });
                    return Json(new {files});

                case "remove":
                    FileHelpers.DeleteFile(themeFolder + "/" + model.RemoveFile);
                    break;

                case "upload":
                    if (Request.Files == null || Request.Files.Count == 0)
                        return JsonError("file is null");

                    var hasErrors = false;

                    for (var i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files.Get(i);
                        if (file == null)
                            continue;

                        if (file.ContentLength > 5000000 ||
                            !FileHelpers.CheckFileExtension(file.FileName, EAdvantShopFileTypes.Image))
                        {
                            hasErrors = true;
                            continue;
                        }
                        file.SaveAs(Path.Combine(themeFolder, file.FileName));
                    }
                    if (hasErrors)
                        return JsonError("Ограничение на размер файла 5 МБ. Допустимые форматы jpg, jpeg, gif, png, bmp.");
                    break;
            }
            return JsonOk();
        }

        #endregion
    }
}
