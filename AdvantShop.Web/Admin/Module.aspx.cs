//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.UrlRewriter;


namespace Admin
{
    public partial class Module : AdvantShopAdminPage
    {

        protected bool InNewView { get; set; }

        protected int CurrentControlIndex
        {
            get
            {
                var intval = 0;
                int.TryParse(Request["currentcontrolindex"], out intval);
                return intval;
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request["MasterPageEmpty"] == "true")
            {
                MasterPageFile = "~/Admin/MasterPageEmpty.master";
                InNewView = true;
            }
            else
            {
                MasterPageFile = "~/Admin/MasterPageAdmin.master";
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["module"].IsNullOrEmpty())
            {
                Response.Redirect(UrlService.GetAdminAbsoluteLink(""));
            }

            var module = AttachedModules.GetModuleById(Request["module"].ToLower());
            if (module == null)
            {
                Response.Redirect(UrlService.GetAdminAbsoluteLink(""));
            }


            imgDecor.Visible = !InNewView;

            ckbActiveModule.Attributes.Add("data-modulestringid", Request["module"]);
            ckbActiveModule.Checked = ModulesRepository.IsActiveModule(Request["module"]);
            lblIsActive.Text = ckbActiveModule.Checked
                                   ? Resources.Resource.Admin_Module_Active
                                   : Resources.Resource.Admin_Module_NotActive;
            lblIsActive.ForeColor = ckbActiveModule.Checked
                                   ? System.Drawing.Color.Green
                                   : System.Drawing.Color.Red;

            var moduleObject = (IModule)Activator.CreateInstance(module, null);
            lblHead.Text = moduleObject.ModuleName;

            var moduleRemoteServerObject = ModulesService.GetModuleObjectFromRemoteServer(moduleObject.ModuleStringId);
            if (moduleRemoteServerObject != null && !string.IsNullOrEmpty(moduleRemoteServerObject.InstructionLink))
            {
                lnkInstruction.Text = string.IsNullOrEmpty(moduleRemoteServerObject.InstructionTitle)
                    ? "Инструкция"
                    : moduleRemoteServerObject.InstructionTitle;
                lnkInstruction.NavigateUrl = moduleRemoteServerObject.InstructionLink +
                                             string.Format("#version={0}+{1}?siteverison={0}&moduleversion={1}",
                                                 SettingsGeneral.SiteVersionDev,
                                                 moduleRemoteServerObject.Version);

                lnkInstruction_block.Visible = true;
            }

            if (moduleObject.ModuleControls != null)
            {
                rptTabs.DataSource = moduleObject.ModuleControls;
                rptTabs.DataBind();

                int currentControlIndex = CurrentControlIndex;

                if (currentControlIndex < 0 || currentControlIndex > (moduleObject.ModuleControls.Count - 1))
                    currentControlIndex = 0;

                if (moduleObject.ModuleControls != null && moduleObject.ModuleControls.Any())
                {

                    pnlBody.Controls.Add(LoadControl(UrlService.GetAbsoluteLink(string.Format("/Modules/{0}/{1}", moduleObject.ModuleStringId, moduleObject.ModuleControls[currentControlIndex].File))));

                    //Control c =
                    //    (this).LoadControl(
                    //        UrlService.GetAbsoluteLink(string.Format("/Modules/{0}/{1}", moduleObject.ModuleStringId,
                    //                                                 moduleObject.ModuleControls[currentControlIndex]
                    //                                                     .File)));
                    //if (c != null)
                    //{
                    //    pnlBody.Controls.Add(c);
                    //}
                    SetMeta(string.Format("{0} - {1} - {2}", SettingsMain.ShopName, moduleObject.ModuleName, moduleObject.ModuleControls[currentControlIndex].NameTab));
                }
            }
            else
            {
                tdModuleSettings.Visible = false;
                lblInfo.Text = Resources.Resource.Admin_Module_NotConfigure;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

        }
    }
}