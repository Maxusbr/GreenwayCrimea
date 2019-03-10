//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

namespace AdvantShop.Core.Modules
{
    public class ModuleBox
    {
        public ModuleBox()
        {
            Items = new List<Module>();
            Message = string.Empty;
        }

        public List<Module> Items { get; set; }
        public string Message { get; set; }
    }
}