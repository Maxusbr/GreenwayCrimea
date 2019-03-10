//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Scheduler.Jobs;

using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;

namespace AdvantShop.Core.Scheduler
{
    //pattern singleton 
    public class TaskManager
    {
        private static readonly TaskManager taskManager = new TaskManager();
        public readonly IScheduler Sched;
        public const string DataMap = "data";
        private const string TaskGroup = "TaskGroup";
        private const string WebConfigGrop = "WebConfigGrop";
        private const string ModuleGroup = "ModuleGroup";
        private const string LicJob = "LicJob";
        private const string SiteMapJobName = "SiteMapJob";


        private readonly ConcurrentQueue<Action> _queueAction;
        private Task _currentTask;

        private TaskManager()
        {
            _queueAction = new ConcurrentQueue<Action>();
            var properties = new NameValueCollection();
            properties["quartz.threadPool.threadCount"] = "1";
            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory(properties);
            // get a scheduler
            Sched = schedFact.GetScheduler();
        }

        public static TaskManager TaskManagerInstance()
        {
            return taskManager;
        }


        public void Init()
        {
            var config = (List<XmlNode>)ConfigurationManager.GetSection("TasksConfig");

            if (config == null) return;
            foreach (XmlNode nodeItem in config)
            {
                if (nodeItem.Attributes == null ||
                    string.IsNullOrEmpty(nodeItem.Attributes["name"].Value) ||
                    string.IsNullOrEmpty(nodeItem.Attributes["type"].Value) ||
                    string.IsNullOrEmpty(nodeItem.Attributes["cronExpression"].Value))
                {
                    return;
                }

                var jobName = nodeItem.Attributes["name"].Value;
                var jobType = nodeItem.Attributes["type"].Value;
                var cronExpression = nodeItem.Attributes["cronExpression"].Value;
                var enabled = nodeItem.Attributes["enabled"].Value.TryParseBool();

                if (!enabled || Sched.CheckExists(new JobKey(jobName, WebConfigGrop)))
                    continue;

                var taskType = Type.GetType(jobType);
                if (taskType == null) continue;

                // construct job info
                var jobDetail = new JobDetailImpl(jobName, WebConfigGrop, taskType);

                // каждый класс сам обработает хmlNode для себя
                jobDetail.JobDataMap[DataMap] = nodeItem;

                //http://www.quartz-scheduler.org/documentation/quartz-1.x/tutorials/crontrigger

                var trigger = new CronTriggerImpl(jobName, WebConfigGrop, jobName, WebConfigGrop, cronExpression);

                Sched.ScheduleJob(jobDetail, trigger);
            }

            if (!Sched.CheckExists(new JobKey(LicJob, WebConfigGrop)))
            {
                var tType = typeof(LicJob);
                var jDetail = new JobDetailImpl(LicJob, WebConfigGrop, tType);
                var trig = new CronTriggerImpl(LicJob, WebConfigGrop, LicJob, WebConfigGrop, "0 59 0 1/1 * ?");
                Sched.ScheduleJob(jDetail, trig);
            }

            if (!Sched.CheckExists(new JobKey(SiteMapJobName, WebConfigGrop)))
            {
                var timeStart = new Random().Next(0, 6 * 60);

                var tType = typeof(SiteMapJob);
                var jDetail = new JobDetailImpl(SiteMapJobName, WebConfigGrop, tType);
                var trig = new CronTriggerImpl(
                    SiteMapJobName,
                    WebConfigGrop,
                    SiteMapJobName,
                    WebConfigGrop,
                    string.Format("{0} {1} 0 1/1 * ?",  timeStart % 6, timeStart / 6));

                Sched.ScheduleJob(jDetail, trig);
            }
        }

        public void ManagedTask(TaskSettings settings)
        {
            foreach (var setting in settings)
            {
                Sched.DeleteJob(new JobKey(setting.GetUniqueName(), TaskGroup));

                if (setting.Enabled)
                {
                    if (string.IsNullOrEmpty(setting.JobType)) continue;

                    var taskType = Type.GetType(setting.JobType);
                    if (taskType == null) continue;
                    if (Sched.CheckExists(new JobKey(setting.GetUniqueName(), TaskGroup))) continue;

                    var jobDetail = new JobDetailImpl(setting.GetUniqueName(), TaskGroup, taskType);
                    jobDetail.JobDataMap[DataMap] = setting;
                    var cronExpression = GetCronString(setting);
                    if (string.IsNullOrEmpty(cronExpression)) continue;

                    var trigger = new CronTriggerImpl(setting.GetUniqueName(), TaskGroup, setting.GetUniqueName(), TaskGroup, cronExpression);
                    Sched.ScheduleJob(jobDetail, trigger);
                }
            }

            var moduleTasks = AttachedModules.GetModules<IModuleTask>();
            foreach (var moduleTask in moduleTasks)
            {
                var classInstance = (IModuleTask)Activator.CreateInstance(moduleTask);
                var tasks = classInstance.GetTasks();

                foreach (var task in tasks)
                {
                    Sched.DeleteJob(new JobKey(task.GetUniqueName(), ModuleGroup));

                    if (task.Enabled)
                    {
                        var type = Type.GetType(task.JobType);
                        if (type == null) continue;

                        var cronExpression = GetCronString(task);
                        if (string.IsNullOrEmpty(cronExpression)) continue;

                        var jobDetail = new JobDetailImpl(task.GetUniqueName(), ModuleGroup, type);
                        jobDetail.JobDataMap[DataMap] = task;

                        var trigger = new CronTriggerImpl(task.GetUniqueName(), ModuleGroup, task.JobType, ModuleGroup, cronExpression);
                        Sched.ScheduleJob(jobDetail, trigger);
                    }
                }
            }
        }

        public string GetTasks()
        {
            var res = (from jobGroupName in Sched.GetJobGroupNames()
                       from jobkey in Sched.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(jobGroupName))
                       select Sched.GetJobDetail(jobkey)).ToList();

            return res.Aggregate(string.Empty, (current, item) => current + (";" + item.JobType));
        }

        private string GetCronString(TaskSetting item)
        {
            switch (item.TimeType)
            {
                case TimeIntervalType.Days:
                    return string.Format("0 {1} {0} 1/1 * ?", item.TimeHours, item.TimeMinutes);
                case TimeIntervalType.Hours:
                    return "0 0 0/1 * * ?";
                case TimeIntervalType.Minutes:
                    return "0 0/1 * * * ?";
            }
            return string.Empty;
        }

        public void Start()
        {
            Sched.Start();
        }

        public void RemoveModuleTask(TaskSetting task)
        {
            if (Sched.CheckExists(new JobKey(task.GetUniqueName(), ModuleGroup)))
            {
                Sched.DeleteJob(new JobKey(task.GetUniqueName(), ModuleGroup));
            }
        }

        public void RemoveTask(string taskName, string group)
        {
            if (Sched.CheckExists(new JobKey(taskName, group)))
            {
                Sched.DeleteJob(new JobKey(taskName, group));
            }
        }


        public void AddTask(Action action)
        {
            if (_queueAction.Contains(action)) return;
            _queueAction.Enqueue(action);

            if ((_currentTask != null) && (_currentTask.Status != TaskStatus.Created) && (_currentTask.IsCompleted == false ||
                 _currentTask.Status == TaskStatus.Running ||
                 _currentTask.Status == TaskStatus.WaitingToRun ||
                 _currentTask.Status == TaskStatus.WaitingForActivation))
                return;

            _currentTask = Task.Factory.StartNew(DoJobs, TaskCreationOptions.LongRunning);
        }

        private void DoJobs()
        {
            Action item;
            while (_queueAction.TryDequeue(out item))
            {
                item();
            }
        }
    }
}