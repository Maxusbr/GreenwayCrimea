using System;
using System.IO;
using System.Web.Hosting;
using AdvantShop.Diagnostics;
using AdvantShop.App.Landing.Domain;

namespace AdvantShop.App.Landing.Handlers.Pictures
{
    public class RemoveBlockPicturesHandler
    {
        private int _lpId;
        private int _blockId;

        public RemoveBlockPicturesHandler(int lpId, int blockId)
        {
            _lpId = lpId;
            _blockId = blockId;
        }

        public bool Execute()
        {
            try
            {
                var landingBlockFolder = HostingEnvironment.MapPath(string.Format(LpFiles.UploadPictureFolderLandingBlock, _lpId, _blockId));

                if (Directory.Exists(landingBlockFolder))
                    Directory.Delete(landingBlockFolder, true);

            }
            catch (Exception ex)
            {
                Debug.Log.Error("LandingPage module, RemoveBlockPicturesHandler", ex);
                return false;
            }
            return true;
        }
    }
}
