using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.SEO;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings;
using AdvantShop.Web.Admin.Handlers.Settings.System;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Filters;
using CsvHelper.Configuration;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class Redirect301Controller : BaseAdminController
    {
        #region Add/Edit/Get/Delete
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Add301Redirect(RedirectSeo model)
        {

            try
            {
                var redirect = new RedirectSeo()
                {
                    RedirectFrom = model.RedirectFrom,
                    RedirectTo = model.RedirectTo ?? "/",
                    ProductArtNo = model.ProductArtNo ?? string.Empty
                };

                RedirectSeoService.AddRedirectSeo(redirect);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Edit301Redirect(RedirectSeo model)
        {
            try
            {
                var redirect = new RedirectSeo()
                {
                    ID = model.ID,
                    RedirectFrom = model.RedirectFrom,
                    RedirectTo = model.RedirectTo ?? "/",
                    ProductArtNo = model.ProductArtNo ?? string.Empty
                };

                RedirectSeoService.UpdateRedirectSeo(redirect);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        public JsonResult GetRedirect301Item(int ID)
        {
            var redirect = RedirectSeoService.GetRedirectSeoById(ID);
            if(redirect == null)
            {
                return Json(false);
            }
            return Json(redirect);
        }

        public JsonResult GetRedirect301(Admin301RedirectFilterModel model)
        {
            var hendler = new Get301Redirect(model);
            var result = hendler.Execute();

            return Json(result);
        }

        public JsonResult DeleteRedirect301(Admin301RedirectFilterModel model)
        {
            Command(model, (id, c) =>
            {
                RedirectSeoService.DeleteRedirectSeo(id);
                return true;
            });

            return Json(true);
        }
        #endregion

        public JsonResult GetActive(bool? active)
        {
            if(active == null)
            {
                return Json(SettingsSEO.Enabled301Redirects);
            }
            var currentActive = SettingsSEO.Enabled301Redirects;
            if (active != currentActive)
            {
                SettingsSEO.Enabled301Redirects = (bool)active;
            }
            return Json((bool)active);
        }

        public ActionResult Export()
        {
            var fileName = "redirects.csv";
            var fileDirectory = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);

            if (System.IO.File.Exists(fileDirectory + fileName))
            {
                System.IO.File.Delete(fileDirectory + fileName);
            }

            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            try
            {
                using (var csvWriter = new CsvHelper.CsvWriter(new StreamWriter(fileDirectory + fileName), new CsvConfiguration() { Delimiter = ";", SkipEmptyRecords = false }))
                {

                    foreach (var item in new[] { "RedirectFrom", "RedirectTo", "ProductArtNo" })
                        csvWriter.WriteField(item);
                    csvWriter.NextRecord();

                    foreach (var redirect in RedirectSeoService.GetRedirectsSeo())
                    {
                        csvWriter.WriteField(redirect.RedirectFrom);
                        csvWriter.WriteField(redirect.RedirectTo);
                        csvWriter.WriteField(redirect.ProductArtNo);

                        csvWriter.NextRecord();
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.Log.Error(ex);
                return Json(false);
            }


            return File(fileDirectory + fileName, "application/octet-stream", fileName);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Import(HttpPostedFileBase file)
        {
            if (file == null || string.IsNullOrEmpty(file.FileName))               
            {
                return Json(new { Result = false });
            }

            var filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            var fileName = "redirectsImport.csv";
            var fullFileName = filePath + fileName.FileNamePlusDate();

            try
            {
                FileHelpers.CreateDirectory(filePath);

                file.SaveAs(fullFileName);

                using (var csvReader = new CsvHelper.CsvReader(new StreamReader(fullFileName), new CsvConfiguration() { Delimiter = ";" }))
                {
                    while (csvReader.Read())
                    {
                        var currentRecord = new RedirectSeo
                        {
                            RedirectFrom = csvReader.GetField<string>("RedirectFrom").ToLower(),
                            RedirectTo = csvReader.GetField<string>("RedirectTo").ToLower(),
                            ProductArtNo = csvReader.GetField<string>("ProductArtNo")
                        };

                        if (string.IsNullOrWhiteSpace(currentRecord.RedirectFrom) || currentRecord.RedirectFrom == "*")
                            continue;

                        var redirect = RedirectSeoService.GetRedirectsSeoByRedirectFrom(currentRecord.RedirectFrom);
                        if (redirect == null)
                        {
                            RedirectSeoService.AddRedirectSeo(currentRecord);
                        }
                        else
                        {
                            currentRecord.ID = redirect.ID;
                            RedirectSeoService.UpdateRedirectSeo(currentRecord);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return Json(new { Result = false });
            }
            return Json(new { Result = true });
        }

        #region Inplace

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InplaceRedirect301(Admin301RedirectFilterModel model)
        {

            int id = 0;
            Int32.TryParse(model.ID, out id);
            if(id == 0)
            {
                return Json(new { result = false });
            }
            var redirect = RedirectSeoService.GetRedirectSeoById(id);

            redirect.RedirectFrom = model.RedirectFrom;
            redirect.RedirectTo = model.RedirectTo ?? "/";
            redirect.ProductArtNo = model.ProductArtNo ?? string.Empty;

            RedirectSeoService.UpdateRedirectSeo(redirect);

            return Json(new { result = true });
        }

        #endregion

        #region Command

        private void Command(Admin301RedirectFilterModel model, Func<int, Admin301RedirectFilterModel, bool> func)
        {
            if (model.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in model.Ids)
                {
                    func(id, model);
                }
            }
            else
            {
                var handler = new Get301Redirect(model);
                var Ids = handler.GetItemsIds();

                foreach (int id in Ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

        #endregion
    }
}
