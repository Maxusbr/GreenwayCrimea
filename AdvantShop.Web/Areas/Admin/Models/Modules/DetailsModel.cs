using System.Collections.Generic;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.Web.Admin.Models.Modules
{
    public class DetailsModel
    {
        public Module Module { get; set; }
        public List<ModuleSettingTab> Settings { get; set; }
    }
}
