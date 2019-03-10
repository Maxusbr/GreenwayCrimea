using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Configuration;
using AdvantShop.Design;

namespace AdvantShop.Web.Infrastructure.Templates
{
    public class RazorThemeViewEngine : RazorViewEngine
    {
        // format is ":ViewCacheEntry:{prefix}:{name}:{controllerName}:{areaName}:{themeName}:"
        private const string CacheKeyFormat = ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:";
        private const string CacheKeyPrefixMaster = "Master";
        private const string CacheKeyPrefixPartial = "Partial";
        private const string CacheKeyPrefixView = "View";
        private static readonly string[] _emptyLocations = new string[0];

        public RazorThemeViewEngine() : base()
        {
            base.MasterLocationFormats = new[]
            {
                "~/Templates/{2}/Views/{1}/{0}.cshtml",
                "~/Templates/{2}/Views/Shared/{0}.cshtml",

                "~/Areas/{2}/Views/{1}/_Layout.cshtml",
                "~/Areas/{2}/Views/Shared/_Layout.cshtml",

                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };
            base.ViewLocationFormats = new[]
            {
                "~/Templates/{2}/Views/{1}/{0}.cshtml",
                "~/Templates/{2}/Views/Shared/{0}.cshtml",

                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };
            base.PartialViewLocationFormats = new[]
            {
                "~/Templates/{2}/Views/{1}/{0}.cshtml",
                "~/Templates/{2}/Views/Shared/{0}.cshtml",

                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",

                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
            };
            base.AreaMasterLocationFormats = new[]
            {
                "~/Templates/{3}/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Templates/{3}/Areas/{2}//Views/Shared/{0}.cshtml",

                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml"
            };
            base.AreaViewLocationFormats = new[]
            {
                "~/Templates/{3}/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Templates/{3}/Areas/{2}/Views/Shared/{0}.cshtml",

                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml"
            };
            base.AreaPartialViewLocationFormats = new[]
            {
                "~/Templates/{3}/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Templates/{3}/Areas/{2}/Views/Shared/{0}.cshtml",

                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml"
            };
            base.FileExtensions = new[] {"cshtml"};
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");
            
            if (String.IsNullOrEmpty(viewName))
                throw new ArgumentException("viewName must be specified.", "viewName");

            var themeName = GetThemeToUse();

            if (SettingsDesign.IsMobileTemplate)
                masterName = "_Layout";

            string[] viewLocationsSearched;
            string[] masterLocationsSearched;

            var requiredString = controllerContext.RouteData.GetRequiredString("controller");

            var viewPath = GetPath(controllerContext, ViewLocationFormats, AreaViewLocationFormats, viewName, themeName, requiredString, CacheKeyPrefixView, useCache, out viewLocationsSearched); // "ViewLocationFormats"
            var masterPath = GetPath(controllerContext, MasterLocationFormats, AreaMasterLocationFormats, masterName, themeName, requiredString, CacheKeyPrefixMaster, useCache, out masterLocationsSearched);  // "MasterLocationFormats"
            
            if (!String.IsNullOrEmpty(viewPath) && (!String.IsNullOrEmpty(masterPath) || String.IsNullOrEmpty(masterName)))
            {
                return new ViewEngineResult(CreateView(controllerContext, viewPath, masterPath), this);
            }

            if (masterLocationsSearched == null)
                return new ViewEngineResult(viewLocationsSearched);

            return new ViewEngineResult(viewLocationsSearched.Union(masterLocationsSearched));
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");
            
            if (String.IsNullOrEmpty(partialViewName))
                throw new ArgumentException("partialViewName must be specified.", "partialViewName");
            
            var themeName = GetThemeToUse();

            string[] searched;
            var controllerName = controllerContext.RouteData.GetRequiredString("controller");
            var partialPath = GetPath(controllerContext, PartialViewLocationFormats, AreaPartialViewLocationFormats, partialViewName, themeName, controllerName, CacheKeyPrefixPartial, useCache, out searched); // "PartialViewLocationFormats"

            if (String.IsNullOrEmpty(partialPath))
                return new ViewEngineResult(searched);

            return new ViewEngineResult(CreatePartialView(controllerContext, partialPath), this);
        }
        
        private static string GetThemeToUse()
        {
            return SettingsDesign.Template != TemplateService.DefaultTemplateId ? SettingsDesign.Template : "";
        }

        private string GetPath(ControllerContext controllerContext, string[] locations, string[] areaLocations,  // string locationsPropertyName
                                string name, string themeName, string controllerName, string cacheKeyPrefix, bool useCache,
                                out string[] searchedLocations)
        {
            searchedLocations = _emptyLocations;

            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            string areaName = GetAreaName(controllerContext.RouteData);
            bool usingAreas = !String.IsNullOrEmpty(areaName);
            if (usingAreas)
            {
                areaName = areaName.ToLower().Replace("adminv2", "admin");
            }
            else if (SettingsDesign.IsMobileTemplate)
            {
                areaName = "mobile";
                usingAreas = true;
            }

            List<ViewLocation> viewLocations = GetViewLocations(locations, (usingAreas) ? areaLocations : null);
            if (viewLocations.Count == 0)
            {
                throw new InvalidOperationException("Properties cannot be null or empty.");
            }

            bool nameRepresentsPath = IsSpecificPath(name);
            string cacheKey = CreateCacheKey(cacheKeyPrefix, name, nameRepresentsPath ? string.Empty : controllerName, areaName, themeName);
            
            if (useCache)
            {
                var cachedLocation = ViewLocationCache.GetViewLocation(controllerContext.HttpContext, cacheKey);
                if (cachedLocation != null)
                {
                    return cachedLocation;
                }
            }

            return !nameRepresentsPath
                    ? GetPathFromGeneralName(controllerContext, viewLocations, name, controllerName, areaName, themeName, cacheKey, ref searchedLocations)
                    : GetPathFromSpecificName(controllerContext, name, cacheKey, ref searchedLocations);
        }

        private static string GetAreaName(RouteBase route)
        {
            var routeWithArea = route as IRouteWithArea;
            if (routeWithArea != null)
            {
                return routeWithArea.Area;
            }

            var castRoute = route as Route;
            if (castRoute != null && castRoute.DataTokens != null)
            {
                return castRoute.DataTokens["area"] as string;
            }

            return null;
        }

        private static string GetAreaName(RouteData routeData)
        {
            object area;
            if (routeData.DataTokens.TryGetValue("area", out area))
            {
                return area as string;
            }

            return GetAreaName(routeData.Route);
        }

        private static List<ViewLocation> GetViewLocations(string[] viewLocationFormats, string[] areaViewLocationFormats)
        {
            var allLocations = new List<ViewLocation>();
            
            if (areaViewLocationFormats != null)
            {
                foreach (string areaViewLocationFormat in areaViewLocationFormats)
                {
                    allLocations.Add(new AreaAwareViewLocation(areaViewLocationFormat));
                }
            }

            if (viewLocationFormats != null)
            {
                foreach (string viewLocationFormat in viewLocationFormats)
                {
                    allLocations.Add(new ViewLocation(viewLocationFormat));
                }
            }

            return allLocations;
        }

        private static bool IsSpecificPath(string name)
        {
            var ch = name[0];
            return ch == '~' || ch == '/';
        }

        internal string CreateCacheKey(string prefix, string name, string controllerName, string areaName, string themeName)
        {
            return string.Format(CultureInfo.InvariantCulture, CacheKeyFormat, prefix, name, controllerName, areaName, themeName);
        }

        internal static string AppendDisplayModeToCacheKey(string cacheKey, string displayMode)
        {
            // key format is ":ViewCacheEntry:{cacheType}:{prefix}:{name}:{controllerName}:{areaName}:"
            // so append "{displayMode}:" to the key
            return cacheKey + displayMode + ":";
        }

        private string GetPathFromGeneralName(ControllerContext controllerContext, List<ViewLocation> locations, string name,
            string controllerName, string areaName, string themeName, string cacheKey, ref string[] searchedLocations)
        {
            string result = String.Empty;
            searchedLocations = new string[locations.Count];

            for (int i = 0; i < locations.Count; i++)
            {
                ViewLocation location = locations[i];
                string virtualPath = location.Format(name, controllerName, areaName, themeName);

                //if(virtualPath.Contains("Templates") && areaName != null)
                //    virtualPath = virtualPath.Replace(areaName, themeName + "/Areas/" + areaName);

                if (FileExists(controllerContext, virtualPath))
                {
                    searchedLocations = _emptyLocations;
                    result = virtualPath;
                    this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
                    return result;
                }
                
                searchedLocations[i] = virtualPath;
            }

            return result;
        }

        
        private string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey,
            ref string[] searchedLocations)
        {
            var virtualPath = name;
            if (!FileExists(controllerContext, name))
            {
                virtualPath = string.Empty;
                searchedLocations = new[] {name};
            }
            ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
            return virtualPath;
        }

        private class AreaAwareViewLocation : ViewLocation
        {
            public AreaAwareViewLocation(string virtualPathFormatString)
                : base(virtualPathFormatString)
            {
            }

            public override string Format(string viewName, string controllerName, string areaName, string themeName)
            {
                return String.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName, areaName, themeName);
            }
        }

        private class ViewLocation
        {
            protected string _virtualPathFormatString;

            public ViewLocation(string virtualPathFormatString)
            {
                _virtualPathFormatString = virtualPathFormatString;
            }

            public virtual string Format(string viewName, string controllerName, string areaName, string themeName)
            {
                return String.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName, themeName);
            }
        }
    }
}