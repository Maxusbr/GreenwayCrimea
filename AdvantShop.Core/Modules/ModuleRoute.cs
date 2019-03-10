using System.Web.Routing;

namespace AdvantShop.Core.Modules
{
    public class ModuleRoute
    {
        public string Key { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }

        public RouteValueDictionary RouteValues { get; set; }

        public bool IsSimpleText { get; set;}

        public string Content { get; set; }
    }
}
