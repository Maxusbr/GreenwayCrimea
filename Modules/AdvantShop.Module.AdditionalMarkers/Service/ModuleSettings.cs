using AdvantShop.Core.Modules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.AdditionalMarkers.Service
{
    public class ModuleSettings
    {
        public static readonly int Version = 0; //версия для скриптов

        public static string ModuleID = AdditionalMarkers.ModuleStringId;

        public static bool SetDefaultSettings()
        {
            return true;
        }

        public static bool RemoveSettings()
        {
            return true;
        }
    }
}
