using System;
using System.IO;
using System.Web.Hosting;
using AdvantShop.Diagnostics;
using AdvantShop.App.Landing.Domain;

namespace AdvantShop.App.Landing.Handlers.Pictures
{
    public class RemoveLandingPicturesHandler
    {
        private int _lpId;

        public RemoveLandingPicturesHandler(int lpId)
        {
            _lpId = lpId;
        }

        public bool Execute()
        {
            try
            {
                var landingFolder = HostingEnvironment.MapPath(string.Format(LpFiles.UploadPictureFolderLanding, _lpId));

                if (Directory.Exists(landingFolder))
                    Directory.Delete(landingFolder, true);

            }
            catch (Exception ex)
            {
                Debug.Log.Error("LandingPage module, RemoveLandingPicturesHandler", ex);
                return false;
            }
            return true;
        }
    }
}
