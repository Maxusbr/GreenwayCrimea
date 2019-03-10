using System;
using System.IO;
using System.Web.Hosting;
using AdvantShop.Diagnostics;

namespace AdvantShop.App.Landing.Handlers.Pictures
{
    public class RemovePictureHandler
    {
        private string _picture;

        public RemovePictureHandler(string picture)
        {
            _picture = picture;
        }

        public bool Execute()
        {
            try
            {
                var path = HostingEnvironment.MapPath("~/" + _picture.TrimStart('/'));
                
                if (path != null && File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("LandingPage module, RemovePictureHandler", ex);
                return false;
            }

            return true;
        }
    }
}
