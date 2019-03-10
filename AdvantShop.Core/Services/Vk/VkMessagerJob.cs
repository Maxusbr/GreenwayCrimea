using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Scheduler;
using AdvantShop.Diagnostics;
using Quartz;

namespace AdvantShop.Core.Services.Vk
{
    [DisallowConcurrentExecution]
    public class VkMessagerJob : IJob
    {
        private readonly VkApiService _vkService = new VkApiService();

        public void Execute(IJobExecutionContext context)
        {
            // Job останавливает сам себя если не настроен или произошла ошибка. 
            // После перенастройки запускается снова.
            if (!CanRunJob())
            {
                StopJob();
                return;
            }

            if (VkMessagerJobState.IsRun)
                return;

            try
            {
                VkMessagerJobState.IsRun = true;

                _vkService.GetLastMessagesByApi();
            }
            catch (BlException ex)
            {
                Debug.Log.Error(ex);
                StopJob();
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            finally
            {
                VkMessagerJobState.IsRun = false;
            }
        }


        private bool CanRunJob()
        {
            return _vkService.IsVkActive();
        }

        private void StopJob()
        {
            SettingsVk.TokenGroup = null;
            SettingsVk.Group = null;

            TaskManager.TaskManagerInstance().RemoveTask("VkMessagerJob", "WebConfigGrop");
        }
    }
}
