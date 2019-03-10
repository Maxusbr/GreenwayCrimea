using System.Linq;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Payment;

namespace AdvantShop.Web.Admin.Handlers.Tasks
{
    public class ChangeTaskSorting
    {
        private readonly int _id;
        private readonly int? _prevId;
        private readonly int? _nextId;

        public ChangeTaskSorting(int id, int? prevId, int? nextId)
        {
            _id = id;
            _prevId = prevId;
            _nextId = nextId;
        }

        public bool Execute()
        {
            var task = TaskService.GetTask(_id);
            if (task == null)
                return false;

            Task prev = null, next = null;

            if (_prevId.HasValue)
                prev = TaskService.GetTask(_prevId.Value);

            if (_nextId.HasValue)
                next = TaskService.GetTask(_nextId.Value);

            if (prev == null && next == null)
                return false;

            if (prev != null && next != null)
            {
                if (next.SortOrder - prev.SortOrder > 1)
                {
                    task.SortOrder = prev.SortOrder + 1;
                    TaskService.UpdateTask(task);
                }
                else
                {
                    UpdateSortOrderForAll(task, prev, next);
                }
            }
            else
            {
                UpdateSortOrderForAll(task, prev, next);
            }

            return true;
        }

        private void UpdateSortOrderForAll(Task task, Task prev, Task next)
        {
            var tasks = TaskService.GetAllTasks().Where(x => x.Id != task.Id).OrderBy(x => x.SortOrder).ToList();

            if (prev != null)
            {
                var index = tasks.FindIndex(x => x.Id == prev.Id);
                tasks.Insert(index + 1, task);
            }
            else if (next != null)
            {
                var index = tasks.FindIndex(x => x.Id == next.Id);
                tasks.Insert(index > 0 ? index - 1 : 0, task);
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].SortOrder = i * 10 + 10;
                TaskService.UpdateTask(tasks[i]);
            }
        }
    }
}
