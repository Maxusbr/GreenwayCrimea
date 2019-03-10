using System;
using System.Web.Hosting;
using System.Web.Mvc;
using AdvantShop.ExportImport;

namespace AdvantShop.Areas.Api.Controllers
{
    public class ResellersController : BaseApiController
    {
        public FileResult Catalog(Guid id)
        {
            var resellerSettings = ExportFeedSettingsProvider.GetSettingsByParam("ResellerCode", id.ToString());
            if (resellerSettings == null)
                return null;

            var fileName = HostingEnvironment.MapPath(string.Format("~/{0}.{1}", resellerSettings.FileName, resellerSettings.FileExtention));
            if (!System.IO.File.Exists(fileName))
                return null;

            var fileBytes = System.IO.File.ReadAllBytes(fileName);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, resellerSettings.FileName + "." + resellerSettings.FileExtention);
        }
    }
}