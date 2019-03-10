//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Statistic;
using System.Net;
using System.Web;
using System.Web.Routing;

namespace AdvantShop.Core
{
    public static class AppServiceStartAction
    {
        public static PingDbState state;
        public static string errMessage;
        public static bool isAppNeedToReRun;
        public static bool isAppFistRun;
    }
    
    public class ApplicationService
    {
        public static void StartApplication(HttpContext current)
        {
            //PreApplicationInit.InitializeModules();

            ServicePointManager.Expect100Continue = false;
            ApplicationUptime.SetApplicationStartTime();

            SettingsGeneral.SetAbsolutePath(current.Server.MapPath("~/"));

            // loger must init ONLY after SetAbsolutePath 
            Debug.InitLogger();

            // Set "first run" flag
            AppServiceStartAction.isAppFistRun = true;

            // Try to run DB depend code
            TryToStartDbDependServices();

            // No DB depend code here!
        }

        public static void TryToStartDbDependServices()
        {
            var appStartDbRes = DataBaseService.CheckDbStates();

            AppServiceStartAction.state = appStartDbRes;

            if (AppServiceStartAction.state == PingDbState.NoError)
            {
                // Other db depend codes
                RunDbDependAppStartServices();
            }
        }

        public static void RunDbDependAppStartServices()
        {
            //Load modules
            AttachedModules.LoadModules();

            // TaskManager
            TaskManager.TaskManagerInstance().Init();
            TaskManager.TaskManagerInstance().Start();

            var settings = TaskSettings.Settings;
            var exportFeedSettings = TaskSettings.ExportFeedSettings;

            if (exportFeedSettings != null && exportFeedSettings.Count > 0)
                settings.AddRange(exportFeedSettings);

            TaskManager.TaskManagerInstance().ManagedTask(settings);

            // LogSessionRestart
            InternalServices.LogApplicationRestart(false, false);

            FoldersHelper.InitFolders();
            FoldersHelper.InitExtraCss();

            LocalizationService.GenerateJsResourcesFile();

            SettingsLic.Activate();
        }

        public static void UpdateRoutes()
        {
            var category = CategoryService.GetCategory(0);
            if (category == null)
            {
                Debug.Log.Error("Can not build route for root category, because it is null");
                return;
            }
            var route = RouteTable.Routes["CatalogRoot"] as Route;
            var routeMobile = RouteTable.Routes["Mobile_CatalogRoot"] as Route;
            if (route != null)
                route.Url = category.UrlPath;
            if (routeMobile != null)
                routeMobile.Url = category.UrlPath;
        }
    }
}