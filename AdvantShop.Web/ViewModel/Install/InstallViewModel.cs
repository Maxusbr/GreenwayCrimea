using System.Collections.Generic;
using AdvantShop.Models.Install;

namespace AdvantShop.ViewModel.Install
{
    public class InstallViewModel
    {
        public List<InstallMenuItem> MenuItems { get; set; }

        public InstallStep CurrentStep { get; set; }
    }
}