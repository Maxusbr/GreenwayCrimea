using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Models.Files;
using AdvantShop.Web.Infrastructure.ActionResults;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Cms
{
    [Auth(RoleAction.Cms)]
    public partial class FilesController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Files.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.FilesCtrl);

            var model = FileHelpers.GetAllowedFileExtensions(EAdvantShopFileTypes.FileInRootFolder).Aggregate((current, next) => current + "," + next);

            return View((object)model);
        }

        public JsonResult GetFiles(FilesFilterModel model)
        {
            var allFiles = FileHelpers.GetFilesInRootDirectory().Where(file => model.Search.IsNullOrEmpty() || file.Name.ToLower().Contains(model.Search.ToLower()));
            var files = allFiles.Skip(model.ItemsPerPage * (model.Page - 1)).Take(model.ItemsPerPage);

            var result = new FilterResult<FilesModel>();
            result.TotalItemsCount = allFiles.Count();
            result.TotalPageCount = (int)Math.Ceiling((decimal)result.TotalItemsCount / model.ItemsPerPage);

            switch (model.Sorting)
            {
                case "FileName":
                    files = files.OrderBy(file => file.Name);
                    break;
                case "DateCreated":
                    files = files.OrderBy(file => file.CreationTime);
                    break;
                case "DateModified":
                    files = files.OrderBy(file => file.LastWriteTime);
                    break;
                case "FileSizeString":
                    files = files.OrderBy(file => file.Length);
                    break;
            }

            if (model.SortingType == FilterSortingType.Desc)
            {
                files = files.Reverse();
            }

            result.DataItems = files.Select(file => new FilesModel()
            {
                Id = file.Name,
                FileName = file.Name,
                FileSize = file.Length,
                DateCreated = file.CreationTime,
                DateModified = file.LastWriteTime,
                Link = UrlService.GetAbsoluteLink(file.Name)
            }).ToList();
            return Json(result);
        }

        public JsonResult DeleteFile(FilesFilterModel model)
        {
            Command(model, (id, c) =>
            {
                FileHelpers.DeleteFile(SettingsGeneral.AbsolutePath + id);
                return true;
            });

            return Json(true);
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadFile(HttpPostedFileBase file)
        {
            if (!FileHelpers.CheckFileExtension(file.FileName, EAdvantShopFileTypes.FileInRootFolder))
            {
                return Json(new CommandResult() { Result = false, Error = T("Admin.Files.Index.InvalidFileNameExtension") });
            }
            if (file != null)
            //if (file != null && file.ContentLength > 0)
            {
                if (System.IO.File.Exists(SettingsGeneral.AbsolutePath + file.FileName))
                {
                    return Json(new { Result = false, error = T("Admin.Files.Index.FileAlreadyExists") });
                }

                file.SaveAs(SettingsGeneral.AbsolutePath + file.FileName);
                return Json(new { Result = true });
            }
            else
                return Json(new { Result = false, error = T("Admin.Files.Index.FileNotFound") });

        }


        public JsonResult RenameFile(FilesModel model)
        {
            if (!FileHelpers.CheckFileExtension(model.FileName, EAdvantShopFileTypes.FileInRootFolder))
            {
                return Json(new { result = false, error = T("Admin.Files.Index.InvalidFileNameExtension") });
            }

            FileHelpers.RenameFile(SettingsGeneral.AbsolutePath, model.Id, model.FileName);
            model.Id = model.FileName;
            model.Link = UrlService.GetAbsoluteLink(model.FileName);
            return Json(new { result = true, entity = model });
        }


        private void Command(FilesFilterModel model, Func<string, FilesFilterModel, bool> func)
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
                var ids = FileHelpers.GetFilesInRootDirectory().Where(file => model.Search.IsNullOrEmpty() || file.Name.ToLower().Contains(model.Search.ToLower())).Select(file => file.Name);

                foreach (string id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

    }
}
