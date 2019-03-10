using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;


namespace AdvantShop.Web.Infrastructure.Extensions
{
    public static class ModulesExtensions
    {
        public static List<ModuleRoute> GetModuleRoutes(string key, object routeValues = null, string area = "")
        {
            var model = new List<ModuleRoute>();

            var routeValueDictionary = new RouteValueDictionary();
            if (routeValues != null)
                routeValueDictionary = TypeHelper.ObjectToDictionary(routeValues);

            if (!routeValueDictionary.ContainsKey("area"))
                routeValueDictionary.Add("area", "");
            
            foreach (var module in AttachedModules.GetModules<IRenderModuleByKey>())
            {
                var instance = (IRenderModuleByKey)Activator.CreateInstance(module);
                var moduleRoutes = instance.GetModuleRoutes();
                if (moduleRoutes != null)
                {
                    foreach (var moduleRoute in moduleRoutes.Where(m => m.Key == key))
                    {
                        if (moduleRoute.RouteValues == null)
                            moduleRoute.RouteValues = new RouteValueDictionary();

                        foreach (var routeValue in routeValueDictionary)
                        {
                            moduleRoute.RouteValues.Add(routeValue.Key, routeValue.Value);
                        }

                        model.Add(moduleRoute);
                    }
                }
            }

            return model;
        }


        public static MvcHtmlString RenderModules(this HtmlHelper helper, string key, object routeValues = null)
        {
            return helper.Partial("_Module", GetModuleRoutes(key, routeValues));
        }

        public static string GetModuleVersion(this HtmlHelper helper, string moduleId)
        {
            var version = "";

            var module = ModulesService.GetModules().Items.FirstOrDefault(x => x.StringId == moduleId);
            if (module != null)
                version = module.CurrentVersion;

            if (version.TryParseFloat() == 0)
                version = "rnd" + new Random().Next(1000);

            return version;
        }

        public static IHtmlString GetModuleVersionHtmlString(this HtmlHelper helper, string moduleId)
        {
            return new HtmlString(GetModuleVersion(helper, moduleId));
        }
    }
}