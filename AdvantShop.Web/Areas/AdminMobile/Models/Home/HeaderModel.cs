using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvantShop.Areas.AdminMobile.Models.Home
{
    public class HeaderModel
    {
        public string ClassName { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
        public bool AnyNews { get; set; }
    }
}