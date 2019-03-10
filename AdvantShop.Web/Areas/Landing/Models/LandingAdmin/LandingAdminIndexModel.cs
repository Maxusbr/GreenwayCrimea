using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.App.Landing.Domain;

namespace AdvantShop.App.Landing.Models.LandingAdmin
{
    public class LandingAdminIndexModel
    {
        public List<Lp> Landings { get; set; }


        public IEnumerable<SelectListItem> Templates { get; set; } 
    }
}
