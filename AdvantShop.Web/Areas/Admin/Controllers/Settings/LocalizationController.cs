using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings.System;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Filters;
using CsvHelper.Configuration;
using AdvantShop.Web.Admin.Models.Import;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class LocalizationController : BaseAdminController
    {
        #region Add/Edit/Get/Delete
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddEditLocalization(LocalizedResource model)
        {
            try
            {
                LocalizationService.AddOrUpdateResource(model.LanguageId, model.ResourceKey, model.ResourceValue);

                if (model.ResourceKey.ToLower().StartsWith("js."))
                {
                    LocalizationService.GenerateJsResourcesFile();
                }
                    
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        public JsonResult GetLocalizeItem(int LanguageId, string ResourceKey)
        {
            var localize = SQLDataAccess.Query<LocalizedResource>(
                    "Select Top(1) LanguageId,ResourceKey,ResourceValue From Settings.Localization " +
                    "Where LanguageId=@LanguageId and ResourceKey=@ResourceKey", new { LanguageId, ResourceKey }, CommandType.Text);
            if (localize == null)
            {
                return Json(false);
            }
            return Json(localize);
        }

        public JsonResult GetLocalizations(AdminLocalizationsFilterModel model)
        {
            var hendler = new GetLocalizations(model);
            var result = hendler.Execute();

            return Json(result);
        }

        public JsonResult GetLanguage(int? lang)
        {
            var result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "Русский", Value = "1", Selected = lang == 1 });
            result.Add(new SelectListItem() { Text = "Английский", Value = "2", Selected = lang == 2 });
            return Json(result);
        }

        #endregion

        public ActionResult Export(int lang)
        {
            var language = LanguageService.GetLanguage(lang);
            var fileName = "localization.csv";
            var fileDirectory = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);

            if (!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);

            using (var csvWriter = new CsvHelper.CsvWriter(new StreamWriter(fileDirectory + fileName, false, Encoding.UTF8), new CsvConfiguration() { Delimiter = ";" }))
            {
                foreach (var item in new[] { "ResourceKey", "ResourceValue", "LanguageCode" })
                    csvWriter.WriteField(item);

                csvWriter.NextRecord();

                foreach (var resource in LocalizationService.GetResources(language.LanguageCode))
                {
                    csvWriter.WriteField(resource.Key);
                    csvWriter.WriteField(resource.Value);
                    csvWriter.WriteField(language.LanguageCode);

                    csvWriter.NextRecord();
                }
            }

            return File(fileDirectory + fileName, "application/octet-stream", fileName);
        }

        [Auth(RoleAction.Customers)]
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Import(HttpPostedFileBase file)
        {
            if (file.ContentLength < 1)
                return Json(new { result = false });

            var filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            var fileName = "localizationImport.csv";
            var fullFileName = filePath + fileName;

            try
            {
                FileHelpers.CreateDirectory(filePath);

                file.SaveAs(fullFileName);

                using (var csvReader = new CsvHelper.CsvReader(new StreamReader(fullFileName), new CsvConfiguration() { Delimiter = ";" }))
                {
                    while (csvReader.Read())
                    {
                        var cultureName = csvReader.GetField<string>("LanguageCode");
                        var languageId = 0;

                        if (!string.IsNullOrEmpty(cultureName))
                        {
                            var language = LanguageService.GetLanguage(cultureName);
                            if (language != null)
                            {
                                languageId = language.LanguageId;
                            }
                            else
                            {
                                try
                                {
                                    var ci = CultureInfo.GetCultureInfo(cultureName);
                                    languageId = LanguageService.Add(new Language() { Name = ci.DisplayName, LanguageCode = ci.Name });
                                }
                                catch (Exception ex)
                                {
                                    Debug.Log.Error(ex);
                                }
                            }
                        }

                        if (languageId == 0)
                            continue;

                        LocalizationService.AddOrUpdateResource(
                            languageId,
                            csvReader.GetField<string>("ResourceKey"),
                            csvReader.GetField<string>("ResourceValue") ?? "");
                    }
                }

                FileHelpers.DeleteFile(fullFileName);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return Json(new UploadFileResult { Result = false });
            }
            return Json(new UploadFileResult { Result = true });
        }

        #region HelpMethod

        #endregion
    }
}
