using System;
using System.IO;
using System.Web;
using AdvantShop.Diagnostics;
using AdvantShop.ViewModel.Install;

namespace AdvantShop.Handlers.Install
{
    public class InstallTrialSelectHandler
    {
        public InstallTrialSelectModel Execute()
        {
            var model = new InstallTrialSelectModel
            {
                HasWriteAccess = CheckAppDataFolder()
            };

            if (!model.HasWriteAccess)
            {
                model.ShowExpress = false;
                model.Title = Resources.Resource.Install_UserContols_TrialSelectView_h1_NoWriteAccess;
            }
            else
            {
                model.ShowExpress = true;
                model.Title = Resources.Resource.Install_UserContols_TrialSelectView_h1;
            }

            return model;
        }

        private bool CheckAppDataFolder()
        {
            var allowWrite = false;

            try
            {
                var testFileName = HttpContext.Current.Server.MapPath("~/App_Data/testFile");

                File.Create(testFileName).Close();
                File.Delete(testFileName);
                allowWrite = true;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return allowWrite;
        }
    }
}