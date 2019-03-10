using System.Collections.Generic;
using System.Web.Mvc;

namespace AdvantShop.ViewModel.Home
{
    public partial class MobileOverlapViewModel
    {
        public MobileOverlapViewModel () { }

        public string logoPath { get; set; }
        public string logoAlt { get; set; }
        public bool ToolbarEnabled { get; set; }

    }
}