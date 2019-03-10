using System.Collections.Generic;
using AdvantShop.Core.Scheduler;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IModuleTask
    {
        List<TaskSetting> GetTasks();
    }
}