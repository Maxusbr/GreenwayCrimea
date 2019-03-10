using AdvantShop.Configuration;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Tasks
{
    public class LoadSaveTasksSettings
    {
        private TasksSettingsModel _model;

        public LoadSaveTasksSettings(TasksSettingsModel model = null)
        {
            _model = model;
        }

        public TasksSettingsModel Load()
        {
            _model = new TasksSettingsModel()
            {
                DefaultTaskGroupId = SettingsTasks.DefaultTaskGroup,
                WebNotificationInNewTab = SettingsNotifications.WebNotificationInNewTab
            };

            return _model;
        }

        public void Save()
        {
            SettingsTasks.DefaultTaskGroup = _model.DefaultTaskGroupId;
            SettingsNotifications.WebNotificationInNewTab = _model.WebNotificationInNewTab;
        }
    }
}
