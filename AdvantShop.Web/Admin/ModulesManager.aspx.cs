//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Modules;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Helpers;
using AdvantShop.Trial;
using Resources;
using System.Web.UI.WebControls;

namespace Admin
{
    public partial class ModulesManager : AdvantShopAdminPage
    {
        private const int ItemsPerPage = 100;
        private bool _isSearch;

        protected int ModulesCountAll;
        protected int ModulesCountActive;
        protected int ModulesCountPopular;
        protected int ModulesCountNew;


        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ModuleManager_Header));
            lTrialMode.Visible = TrialService.IsTrialEnabled;

            if (!string.IsNullOrEmpty(Request["installModule"]))
            {
                var moduleStringId = SQLDataHelper.GetString(Request["installModule"].ToLower());

                ModulesRepository.SetModuleNeedUpdate(moduleStringId, true);
                ModulesService.InstallModule(moduleStringId, Request["version"]);

                Response.Redirect(UrlService.GetAdminAbsoluteLink("modulesmanager.aspx"));
            }

            LoadData();
        }

        protected void lvModules_ItemCommand(object source, ListViewCommandEventArgs e)
        {
            var moduleVersion = ((HiddenField)e.Item.FindControl("hfLastVersion")).Value;
            var moduleIdOnRemoteServer = ((HiddenField)e.Item.FindControl("hfId")).Value;

            if (e.CommandName == "InstallLastVersion")
            {
                ModulesRepository.SetModuleNeedUpdate(e.CommandArgument.ToString(), true);

                var message = ModulesService.GetModuleArchiveFromRemoteServer(moduleIdOnRemoteServer);

                if (message.IsNullOrEmpty())
                {
                    ModulesService.InstallModule(SQLDataHelper.GetString(e.CommandArgument).ToLower(), moduleVersion);
                    ModulesRepository.SetModuleNeedUpdate(e.CommandArgument.ToString(), false);
                    //HttpRuntime.UnloadAppDomain();

                    //Context.ApplicationInstance.CompleteRequest();
                    //Response.Redirect(
                    //    UrlService.GetAdminAbsoluteLink("modulesmanager.aspx?installModule=" + e.CommandArgument + "&version=" + moduleVersion), false);
                }

              
                HttpRuntime.UnloadAppDomain();
            }

            if (e.CommandName == "Uninstall")
            {
                ModulesService.UninstallModule(SQLDataHelper.GetString(e.CommandArgument));
                //HttpRuntime.UnloadAppDomain();
                //Response.Redirect(Request.Url.AbsoluteUri);
            }
        }

        protected void LoadData()
        {
            var modulesBox = ModulesService.GetModules();
            var items = modulesBox.Items;


            var q = txtSearchModule.Text;
            if (_isSearch && !string.IsNullOrWhiteSpace(q) && q.Length > 2)
            {
                q = q.ToLower();
                items = items.Where(x =>
                            x.Name.ToLower().Contains(q) || x.StringId.ToLower().Contains(q) ||
                            (x.BriefDescription != null && x.BriefDescription.Contains(q))).ToList();

            }
            else if (!string.IsNullOrWhiteSpace(Request["q"]))
            {
                q = q.ToLower();
                items = items.Where(x =>
                            x.Name.ToLower().Contains(q) || x.StringId.ToLower().Contains(q) ||
                            (x.BriefDescription != null && x.BriefDescription.Contains(q))).ToList();
            }
            else
            {
                var show = Request["show"];
                if (!string.IsNullOrWhiteSpace(show))
                {
                    switch (show)
                    {
                        case "active":
                            lblHead.Text = Resource.Admin_ModulesManager_ActiveModules;
                            items = items.Where(x => x.Enabled).ToList();
                            break;

                        case "popular":
                            lblHead.Text = Resource.Admin_ModulesManager_PopularModules;
                            items = items.Where(x => x.Popular).ToList();
                            break;

                        case "new":
                            lblHead.Text = Resource.Admin_ModulesManager_NewModules;
                            items = items.Where(x => x.New).ToList();
                            break;

                        default:
                            lblHead.Text = Resource.Admin_ModulesManager_AllModules;
                            break;
                    }
                }
            }

            paging.TotalPages = (int)Math.Ceiling((double)items.Count / ItemsPerPage);

            lvModulesManager.DataSource = items.Skip((paging.CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage);
            lvModulesManager.DataBind();

            ModulesCountAll = modulesBox.Items.Count;
            ModulesCountActive = modulesBox.Items.Count(x => x.Enabled);
            ModulesCountPopular = modulesBox.Items.Count(x => x.Popular);
            ModulesCountNew = modulesBox.Items.Count(x => x.New);
        }

        protected void btnSearchModule_Click(object sender, EventArgs e)
        {
            _isSearch = true;
        }
    }
}