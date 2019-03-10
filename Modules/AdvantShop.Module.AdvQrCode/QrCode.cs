using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Module.AdvQrCode
{
    public class AdvQrCode : IModule, IRenderModuleByKey
    {
        #region IModule

        public string ModuleStringId
        {
            get { return "advqrcode"; }
        }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "QR код";

                    case "en":
                        return "QR code";

                    default:
                        return "QR code";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl>(); }
        }

        public bool HasSettings
        {
            get { return false; }
        }

        public bool CheckAlive()
        {
            return ModulesRepository.IsInstallModule(ModuleStringId) &&
                   File.Exists(HttpContext.Current.Server.MapPath("~/bin/Gma.QrCodeNet.Encoding.dll"));
        }

        public bool InstallModule()
        {
            return File.Exists(HttpContext.Current.Server.MapPath("~/bin/Gma.QrCodeNet.Encoding.dll"));
        }

        public bool UpdateModule()
        {
            return true;
        }

        public bool UninstallModule()
        {
            return true;
        }

        #endregion
        
        public List<ModuleRoute> GetModuleRoutes()
        {
            return new List<ModuleRoute>()
            {
                new ModuleRoute()
                {
                    Key = "product_right",
                    ActionName = "ProductRightBlock",
                    ControllerName = "AdvQrCode"
                }
            };
        }
    }
}