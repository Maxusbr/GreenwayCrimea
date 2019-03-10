using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Hosting;
using AdvantShop.Diagnostics;
using AdvantShop.App.Landing.Domain;

namespace AdvantShop.App.Landing.Handlers.Pictures
{
    public class UploadPictureResult
    {
        public bool Result { get; set; }
        public string Picture { get; set; }
        public string Error { get; set; }
    }

    public class UploadPictureHandler
    {
        private int _lpId;
        private int _blockId;
        private HttpPostedFileBase _file;
        private readonly List<string> _pictureExts = new List<string>() { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };


        public UploadPictureHandler(int lpId, int blockId, HttpPostedFileBase file)
        {
            _lpId = lpId;
            _blockId = blockId;
            _file = file;
        }

        public UploadPictureResult Execute()
        {
            try
            {
                var landingBlockFolder = HostingEnvironment.MapPath(string.Format(LpFiles.UploadPictureFolderLandingBlock, _lpId, _blockId));

                if (!Directory.Exists(landingBlockFolder))
                    Directory.CreateDirectory(landingBlockFolder);

                var ext = Path.GetExtension(_file.FileName);

                if (!_pictureExts.Contains(ext))
                    return new UploadPictureResult() { Error = "wrong file", Result = false };

                var fileName = Guid.NewGuid().ToString("N") + ext;

                _file.SaveAs(landingBlockFolder + fileName);

                return new UploadPictureResult()
                {
                    Result = true,
                    Picture = string.Format(LpFiles.UploadPictureFolderLandingBlockRelative + fileName, _lpId, _blockId)
                };
            }
            catch (Exception ex)
            {
                Debug.Log.Error("LandingPage module, UploadPictureHandler", ex);
                return new UploadPictureResult() { Error = ex.Message };
            }
        }
    }
}
