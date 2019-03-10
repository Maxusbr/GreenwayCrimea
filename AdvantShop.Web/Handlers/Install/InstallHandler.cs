using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Models.Install;
using AdvantShop.ViewModel.Install;
using Resources;

namespace AdvantShop.Handlers.Install
{
    public class InstallHandler
    {
        public InstallViewModel Execute()
        {
            var menuItems = new List<InstallMenuItem>
            {
                new InstallMenuItem
                {
                    MenuName = Resource.Install_Default_LeftMenu_AboutShop,
                    Step = InstallStep.Shopinfo,
                    StyleClass = "shop-info"
                },
                new InstallMenuItem
                {
                    MenuName = Resource.Install_Default_LeftMenu_Finance,
                    Step = InstallStep.Finance,
                    StyleClass = "finance"
                },
                new InstallMenuItem
                {
                    MenuName = Resource.Install_Default_LeftMenu_Payment,
                    Step = InstallStep.Payment,
                    StyleClass = "payment"
                },
                new InstallMenuItem
                {
                    MenuName = Resource.Install_Default_LeftMenu_Shipping,
                    Step = InstallStep.Shipping,
                    StyleClass = "shipping"
                },
                new InstallMenuItem
                {
                    MenuName = Resource.Install_Default_LeftMenu_Security,
                    Step = InstallStep.OpenId,
                    StyleClass = "openid paragraf"
                },
                new InstallMenuItem
                {
                    MenuName = Resource.Install_Default_LeftMenu_Notification,
                    Step = InstallStep.Notify,
                    StyleClass = "notify paragraf"
                },
                new InstallMenuItem
                {
                    MenuName = Resource.Install_Default_LeftMenu_FinalStep,
                    Step = InstallStep.Final,
                    StyleClass = "final"
                }
            };

            var currentStep = InstallStep.TrialSelect;
            var action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"] as string;

            if (!string.IsNullOrEmpty(action))
                Enum.TryParse(action, true, out currentStep);

            var model = new InstallViewModel()
            {
                CurrentStep = currentStep,
                MenuItems = menuItems
            };

            return model;
        }

        
    }
}